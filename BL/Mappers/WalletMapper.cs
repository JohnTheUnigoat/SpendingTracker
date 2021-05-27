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
            UserId = dto.UserId
        };
    }
}
