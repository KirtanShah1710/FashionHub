using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.IService;
using ClothStore_Backend.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst("sub") ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) throw new UnauthorizedAccessException("UserId not found in token");
        return int.Parse(claim.Value);
    }

    // ================= Admin =================
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders([FromQuery] OrderStatus? status)
    {
        var orders = await _orderService.GetAllAsync();

        if (status.HasValue)
            orders = orders.Where(o => o.Status == status.Value.ToString()).ToList();

        return Ok(orders);
    }
    [HttpPut("admin/{orderId}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrderStatus(
    int orderId,
    [FromBody] UpdateOrderStatusDto dto)
    {
        var result = await _orderService.UpdateStatusAsync(orderId, dto.Status);

        if (!result)
            return NotFound(new { message = "Order not found" });

        return Ok(new { message = "Order status updated successfully" });
    }

    // ================= Customer =================
    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders()
    {
        int userId = GetUserId();
        var orders = await _orderService.GetMyOrdersAsync(userId);
        return Ok(orders);
    }
    [HttpPut("{orderId}/cancel")]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        int userId = GetUserId();

        var result = await _orderService.CancelOrderAsync(orderId, userId);

        if (!result)
            return BadRequest("Unable to cancel order");

        return Ok(new { message = "Order cancelled successfully" });
    }

    // ================= START PAYMENT =================
    [HttpPost("payment/start")]
    public async Task<IActionResult> StartPayment([FromBody] CreatePaymentOrderDto dto)
    {
        int userId = GetUserId();
        var order = await _orderService.CreatePaymentOrderAsync(userId, dto);
        return Ok(order);
    }

    // ================= VERIFY PAYMENT =================
    [HttpPost("payment/verify")]
    public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentDto dto)
    {
        int userId = GetUserId();
        bool success = await _orderService.VerifyPaymentAsync(userId, dto);

        if (!success) {
            return BadRequest("Payment verification failed");
        }
        //return Ok("Payment successful");
        return Ok(new { message= "Payment successful" });
    }
}
