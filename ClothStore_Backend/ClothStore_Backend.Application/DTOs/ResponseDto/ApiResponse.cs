namespace ClothStore_Backend.Application.DTOs.ResponseDto
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        // Generic data Payload
        public T? Data { get; set; }

        // Extra information (pagination, counts, etc.)
        public object? Meta { get; set; }

    }
}
