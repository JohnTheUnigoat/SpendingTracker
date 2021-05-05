using SpendingTracker.Models.Auth.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpendingTracker.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> GoogleSignIn(string googleIdToken);
    }
}
