using ClothStore_Backend.Data.Entities;

namespace ClothStore_Backend.Application.IService
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
    }
}
