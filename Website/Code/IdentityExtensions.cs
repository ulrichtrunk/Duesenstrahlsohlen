using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Website.Code
{
    public static class IdentityExtensions
    {
        public const string ClaimIsLocal = "IsLocal";


        /// <summary>
        /// Determines whether this instance is local.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns>
        ///   <c>true</c> if the specified identity is local; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLocal(this IIdentity identity)
        {
            if (identity == null)
            {
                return false;
            }

            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimIsLocal);

            return claim != null ? Convert.ToBoolean(claim.Value) : false;
        }
    }
}