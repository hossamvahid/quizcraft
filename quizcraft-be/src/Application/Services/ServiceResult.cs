using System.ComponentModel;

namespace src.Application.Services
{
    public enum ServiceResult
    {
        [Description("Request was empty")]
        EMPTY_REQUEST,

        [Description("It already exists")]
        ALREADY_EXISTS,

        [Description("Status is ok")]
        OK,

        [Description("Email does not exists")]
        INVALID_EMAIL,

        [Description("Passwords does not match")]
        INVALID_PASSWORD
    }
}
