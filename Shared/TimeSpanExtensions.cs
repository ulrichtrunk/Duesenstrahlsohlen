using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Get the short string.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns></returns>
        public static string ToShortString(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"d\d\:h\h\:m\m\:s\s", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
