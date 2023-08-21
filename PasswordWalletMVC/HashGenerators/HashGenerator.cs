using System.Security.Cryptography;
using System.Text;
using System;
using System.Security.Policy;

namespace PasswordWalletMVC.HashGenerators
{
    public static class HashGenerator
    {
        public static string GenerateHmac(string password, string salt)
        {

            salt = salt ?? "";

            using (var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(salt)))
            {
                var hash = hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }


        }

        public static string GenerateSha(string password, string salt)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] hashValue = sha512.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return Convert.ToHexString(hashValue);
            }
        }

    }
}
