using System.Text;

namespace Middleware.Converters
{
    public static class Converter
    {
        public static string EncriptText(this string val)
        {
            var bytes = Encoding.UTF8.GetBytes(val);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }
    }
}
