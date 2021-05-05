using BL.Model.User;
using SpendingTracker.Models.User.Response;

namespace SpendingTracker.Mappers
{
    public static class UserMappers
    {
        public static UserResponse ToResponse(this UserDomain domain) => new UserResponse
        {
            Email = domain.Email,
            FirstName = domain.FirstName,
            LastName = domain.LastName,
            PictureUrl = domain.PictureUrl
        };
    }
}
