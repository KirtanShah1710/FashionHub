using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;
using ClothStore_Backend.Application.IService;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;

namespace ClothStore_Backend.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Name = dto.Name,
                Age = dto.Age
            };

            bool created = await _userRepository.CreateUserAsync(user, dto.Password);
            if (!created)
                return null;

            var token = await _tokenService.CreateToken(user);
            //return new AuthResponseDto { Email = user.Email!, Name = user.Name!, Token = token };
            return new AuthResponseDto { Token = token };

        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !await _userRepository.CheckPasswordAsync(user, dto.Password))
                return null;

            var token = await _tokenService.CreateToken(user);
            //return new AuthResponseDto { Email = user.Email!, Name = user.Name!, Token = token };
            return new AuthResponseDto { Token = token };

        }

    }
}
