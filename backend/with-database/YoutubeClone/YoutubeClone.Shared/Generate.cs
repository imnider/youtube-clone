using System.Text;

namespace YoutubeClone.Shared
{
    public class Generate
    {
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static string RandomText(int length = 50)
        {
            var value = new StringBuilder();
            var rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                value.Append(Characters[rnd.Next(0, Characters.Length - 1)]);
            }

            return value.ToString();
        }
    }
}
