using BL.Model.User;
using DAL_EF.Entity;

namespace BL.Mappers
{
    public static class UserMappers
    {
        public static UserDomain ToDomain(this User entity) => new UserDomain
        {
            Id = entity.Id,
            GoogleId = entity.GoogleId,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            PictureUrl = entity.PictureUrl
        };

        public static User ToEntity(this UserDomain domain) => new User
        {
            Id = domain.Id,
            GoogleId = domain.GoogleId,
            Email = domain.Email,
            FirstName = domain.FirstName,
            LastName = domain.LastName,
            PictureUrl = domain.PictureUrl
        };
    }
}
