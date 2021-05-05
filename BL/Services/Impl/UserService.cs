using BL.Mappers;
using BL.Model.User;
using DAL_EF;
using DAL_EF.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        /// <summary>
        /// Gets user id but google id. Return 0 if no such user exists
        /// </summary>
        public async Task<int> GetUserIdByGoogleId(string googleId)
        {
            int userId = await _dbContext.Users
                .Where(u => u.GoogleId == googleId)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            return userId;
        }

        public async Task<UserDomain> UpsertUser(UserDomain model)
        {
            User user = model.ToEntity();

            if (user.Id == 0)
            {
                _dbContext.Users.Add(user);
            }
            else
            {
                var entry = _dbContext.Users.Attach(user);
                entry.State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();

            return user.ToDomain();
        }
    }
}
