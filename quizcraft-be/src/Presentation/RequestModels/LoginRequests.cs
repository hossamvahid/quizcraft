using System.ComponentModel.DataAnnotations;

namespace src.Presentation.RequestModels
{
    public class LoginRequests
    {
        [EmailAddress]
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
