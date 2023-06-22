using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DocumentAssistantLibrary.Classes
{
    public static class PassGenerator
    {
        public static string ComputeHash(string passInput, string salt)
        {
            byte[] saltArray = Encoding.ASCII.GetBytes(salt);
            string pepper = "P1eprzykDoHaselka:)";
            var byteResult = new Rfc2898DeriveBytes(passInput + pepper, saltArray, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(128));
        }

        public static string GenerateSalt()
        {
            var bytes = new byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
