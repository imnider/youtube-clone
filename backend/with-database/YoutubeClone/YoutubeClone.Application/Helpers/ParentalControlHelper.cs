namespace YoutubeClone.Application.Helpers
{
    public static class ParentalControlHelper
    {
        public static bool hasMinimumAge(DateTime Birthday, int mininumAge)
        {
            var age = DateTime.Today.Year - Birthday.Year;
            if (Birthday > DateTime.Today.AddYears(-age))
            {
                age--;
            }
            if (age < mininumAge)
            {
                return false;
            }
            return true;
        }
    }
}
