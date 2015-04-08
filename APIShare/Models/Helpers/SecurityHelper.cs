using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Security;

namespace APIShare.Models.Helpers
{
    public static class SecurityHelper
    {
        public static string GetSalt()
        {
            //Create and populate random byte array
            byte[] randomArray = new byte[20];
            string randomString;

            //Create random salt and convert to string
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomArray);
            randomString = Convert.ToBase64String(randomArray);
            return randomString;
        }

        public static string GetHash(string password, string salt)
        {
            using (SHA512CryptoServiceProvider sha512 = new SHA512CryptoServiceProvider())
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(password + salt);
                data = sha512.ComputeHash(data);
                string SHA512Hash = System.Text.Encoding.ASCII.GetString(data);

                return SHA512Hash;
            }
        }
    }
}