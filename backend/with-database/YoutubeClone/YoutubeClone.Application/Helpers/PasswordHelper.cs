using System.Text.RegularExpressions;

namespace YoutubeClone.Application.Helpers
{
    public static class PasswordHelper
    {
        public static bool isValid(string password)
        {
            string patronPassword = @"^(?=.*[A-Z])(?=.*[\W_]).+$";
            bool validPassword = Regex.IsMatch(password, patronPassword);

            return validPassword;
        }
    }
}
