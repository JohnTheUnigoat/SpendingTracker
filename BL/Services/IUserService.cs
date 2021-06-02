using BL.Model.User;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Gets user id but google id. Return 0 if no such user exists
        /// </summary>
        Task<int> GetUserIdByGoogleId(string googleId);

        Task<UserDomain> GetUser(int userId);

        Task<UserDomain> UpsertUser(UserDomain model);
    }
}
