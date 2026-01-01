using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;

namespace ClothStore_Backend.Application.IService
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);

    }
}
