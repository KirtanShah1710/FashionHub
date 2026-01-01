using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;
using ClothStore_Backend.Application.IService;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.Enums;
using ClothStore_Backend.Data.IRepository;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;
using EntityOrder = ClothStore_Backend.Data.Entities.Order;

namespace ClothStore_Backend.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductVariantRepository _variantRepo;
        private readonly IConfiguration _config;

        public OrderService(
            IOrderRepository orderRepo,
            IProductVariantRepository variantRepo,
            IConfiguration config)
        {
            _orderRepo = orderRepo;
            _variantRepo = variantRepo;
            _config = config;
        }

        // ================= BUY NOW =================
        public async Task<OrderResponseDto> CreateAsync(int userId, CreateOrderDto dto)
        {
            if (dto.ProductVariantId <= 0)
                throw new Exception("Product variant not selected");

            var variant = await _variantRepo.GetByIdAsync(dto.ProductVariantId)
                          ?? throw new Exception("Product variant not found");

            if (variant.Stock < dto.Quantity)
                throw new Exception("Insufficient stock");

            var order = new EntityOrder
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = variant.Price * dto.Quantity,
                Status = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductVariantId = variant.ProductVariantId,
                        Quantity = dto.Quantity,
                        Price = variant.Price
                    }
                }
            };

            // Reduce stock immediately for COD
            variant.Stock -= dto.Quantity;

            await _orderRepo.CreateAsync(order);
            return MapToDto(order);
        }

        // ================= CREATE PAYMENT ORDER =================
        public async Task<OrderResponseDto> CreatePaymentOrderAsync(int userId, CreatePaymentOrderDto dto)
        {
            if (dto.ProductVariantId <= 0)
                throw new Exception("Product variant not selected");

            var variant = await _variantRepo.GetByIdAsync(dto.ProductVariantId)
                          ?? throw new Exception("Product variant not found");

            if (variant.Stock < dto.Quantity)
                throw new Exception("Insufficient stock");

            decimal amount = variant.Price * dto.Quantity;

            var order = new EntityOrder
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = amount,
                Status = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductVariantId = variant.ProductVariantId,
                        Quantity = dto.Quantity,
                        Price = variant.Price
                    }
                }
            };

            await _orderRepo.CreateAsync(order);

            // Razorpay order
            string keyId = _config["Razorpay:KeyId"] ?? throw new Exception("Razorpay KeyId not configured");
            string keySecret = _config["Razorpay:KeySecret"] ?? throw new Exception("Razorpay KeySecret not configured");

            var client = new RazorpayClient(keyId, keySecret);
            var options = new Dictionary<string, object>
            {
                { "amount", (int)(amount * 100) }, // amount in paisa
                { "currency", "INR" },
                { "receipt", order.OrderId.ToString() }
            };

            var razorpayOrder = client.Order.Create(options);
            order.RazorpayOrderId = razorpayOrder["id"].ToString();
            await _orderRepo.UpdateAsync(order);

            return MapToDto(order);
        }

        // ================= VERIFY PAYMENT =================
        public async Task<bool> VerifyPaymentAsync(int userId, VerifyPaymentDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(dto.OrderId)
                        ?? throw new Exception("Order not found");

            if (order.UserId != userId)
                throw new Exception("User not authorized");

            if (order.PaymentStatus == PaymentStatus.Paid)
                return true;

            if (order.RazorpayOrderId != dto.RazorpayOrderId)
                throw new Exception("Order ID mismatch");

            string keySecret = _config["Razorpay:KeySecret"] ?? throw new Exception("Razorpay KeySecret not configured");
            var generatedSignature = GenerateSignature($"{dto.RazorpayOrderId}|{dto.RazorpayPaymentId}", keySecret);

            // Debug logging
            Console.WriteLine($"Generated Signature: {generatedSignature}");
            Console.WriteLine($"Received Signature: {dto.RazorpaySignature}");

            if (generatedSignature != dto.RazorpaySignature)
                throw new Exception("Invalid payment signature");

            // Update order & stock
            order.PaymentStatus = PaymentStatus.Paid;
            order.Status = OrderStatus.Confirmed;
            order.RazorpayPaymentId = dto.RazorpayPaymentId;

            foreach (var item in order.OrderItems!)
            {
                var variant = await _variantRepo.GetByIdAsync(item.ProductVariantId)
                              ?? throw new Exception("Variant missing");

                if (variant.Stock < item.Quantity)
                    throw new Exception("Stock mismatch");

                variant.Stock -= item.Quantity;
            }

            await _orderRepo.UpdateAsync(order);
            return true;
        }

        // ================= CANCEL ORDER =================
        public async Task<bool> CancelOrderAsync(int orderId, int userId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null || order.UserId != userId)
                return false;

            if (order.Status != OrderStatus.Pending &&
                order.Status != OrderStatus.Confirmed)
                throw new Exception("Order cannot be cancelled at this stage");

            order.Status = OrderStatus.Cancelled;

            foreach (var item in order.OrderItems!)
            {
                var variant = await _variantRepo.GetByIdAsync(item.ProductVariantId);
                if (variant != null)
                    variant.Stock += item.Quantity;
            }

            await _orderRepo.UpdateAsync(order);
            return true;
        }


        // ================= UPDATE STATUS =================
        public async Task<bool> UpdateStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
                return false;

            order.Status = status;
            await _orderRepo.UpdateAsync(order);
            return true;
        }


        // ================= GET MY ORDERS =================
        public async Task<List<OrderResponseDto>> GetMyOrdersAsync(int userId)
        {
            var orders = await _orderRepo.GetByUserIdAsync(userId);
            return orders.Select(MapToDto).ToList();
        }

        // ================= GET ALL ORDERS =================
        public async Task<List<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            return orders.Select(MapToDto).ToList();
        }

        // ================= HELPER METHODS =================
        private static string GenerateSignature(string payload, string secret)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload ?? ""));

            // Convert to lowercase HEX (required by Razorpay)
            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        private static OrderResponseDto MapToDto(EntityOrder order)
        {
            return new OrderResponseDto
            {
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount,
                RazorpayOrderId = order.RazorpayOrderId,
                Status = order.Status.ToString(),
                OrderDate = order.OrderDate,
                PaymentStatus = order.PaymentStatus.ToString()
            };
        }
    }
}
