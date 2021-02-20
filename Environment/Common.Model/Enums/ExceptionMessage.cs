using System.ComponentModel;

namespace Common.Model.Enums
{
    public enum ExceptionMessage
    {
        [Description("Invalid Credentails")]
        InvalidCredentials,

        [Description("Expired Refresh Token")]
        ExpiredRefreshToken,

        [Description("User Already Exists")]
        UserAlreadyExists
    }
}