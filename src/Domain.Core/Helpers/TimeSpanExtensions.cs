using System;

namespace Domain.Core.Helpers
{
    public static class TimeSpanExtensions
    {
        public static string ToLegibleString(this TimeSpan t)
            => t.ToString("hh\\:mm");
    }
}