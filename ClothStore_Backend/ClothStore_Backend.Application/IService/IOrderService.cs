using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;
using ClothStore_Backend.Data.Enums;

namespace ClothStore_Backend.Application.IService
{
    public interface IOrderService
    {
        // ================= ORDERS =================

        /// Create a normal order (without payment gateway)
        Task<OrderResponseDto> CreateAsync(int userId, CreateOrderDto dto);

        /// Cancel an order by the user
        Task<bool> CancelOrderAsync(int orderId, int userId);

        /// Update order status (Admin)
        Task<bool> UpdateStatusAsync(int orderId, OrderStatus status);

        /// Get orders of a specific user
        Task<List<OrderResponseDto>> GetMyOrdersAsync(int userId);

        /// Get all orders (Admin)
        Task<List<OrderResponseDto>> GetAllAsync();


        // ================= PAYMENT ORDERS =================

        /// Create a payment order using Razorpay
        Task<OrderResponseDto> CreatePaymentOrderAsync(int userId, CreatePaymentOrderDto dto);

        /// Verify payment after Razorpay payment is completed
        Task<bool> VerifyPaymentAsync(int userId, VerifyPaymentDto dto);
    }
}
