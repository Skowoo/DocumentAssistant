using System.Text;
using System.Security.Cryptography;

namespace DocumentAssistantLibrary.Classes
{
    /// <summary>
    /// Class holds methods to generate passwords
    /// </summary>
    public static class PassGenerator
    {
        /// <summary>
        /// Method computes hash from input strings
        /// </summary>
        /// <param name="passInput">Password</param>
        /// <param name="salt">Salt - string which have to be stored alongside password</param>
        /// <returns>128 bytes long hash created from input strings</returns>
        public static string ComputeHash(string passInput, string salt)
        {
            byte[] saltArray = Encoding.ASCII.GetBytes(salt);
            string pepper = "P1eprzykDoHaselka:)";
            var byteResult = new Rfc2898DeriveBytes(passInput + pepper, saltArray, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(128));
        }

        /// <summary>
        /// Method to generate Salt
        /// </summary>
        /// <returns>Random 32 bytes long string</returns>
        public static string GenerateSalt()
        {
            var bytes = new byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
