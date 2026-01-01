using ClothStore_Backend.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothStore_Backend.Application.IService
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
    }
}
