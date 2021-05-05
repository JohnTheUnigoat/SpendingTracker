using SpendingTracker.Models.User.Response;

namespace SpendingTracker.Models.Auth.Response
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }

        public UserResponse User { get; set; }
    }
}
