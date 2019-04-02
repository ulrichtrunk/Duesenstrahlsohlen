using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Code
{
    public class Argon2PasswordHasher : Liphsoft.Crypto.Argon2.PasswordHasher, IPasswordHasher
    {
        public Argon2PasswordHasher()
            : base(timeCost: 50, memoryCost: 32384, parallelism: 2, hashLength: 32)
        {

        }


        /// <summary>
        /// Hash a password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            return Hash(password);
        }


        /// <summary>
        /// Verify that a password matches the hashed password
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="providedPassword"></param>
        /// <returns></returns>
        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (Verify(hashedPassword, providedPassword))
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }
    }
}