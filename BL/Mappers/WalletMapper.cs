using BL.Model.Wallet;
using DAL_EF.Entity;
using System.Linq;

namespace BL.Mappers
{
    public static class WalletMapper
    {
        public static Wallet ToEntity(this AddWalletDto dto) => new Wallet
        {
            Name = dto.Name,
            Categories = dto.Categories
                .Select(name => new Category
                {
                    Name = name
                })
                .ToList(),
            UserId = dto.UserId
        };
    }
}
