using src.Domain.Enums;
using System.Security.Claims;

namespace src.Application.Utils
{
    public static class AdminHelper
    {
        public static string? VerifyAdmin(string email, string password)
        {
            if (email == Environment.GetEnvironmentVariable("ADMIN_EMAIL"))
            {
                if (password != Environment.GetEnvironmentVariable("ADMIN_PASS"))
                {
                    return null;
                }

                var adminToken = TokenHelper.GenerateJwtToken(email,Role.ADMIN);
                return adminToken.ToString();
            }
            return null;
        }
    }
}
