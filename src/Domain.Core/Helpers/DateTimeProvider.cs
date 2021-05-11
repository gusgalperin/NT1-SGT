using System;

namespace Domain.Core.Helpers
{
    public interface IDateTimeProvider
    {
        DateTime Ahora();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Ahora()
        {
            return DateTime.Now;
        }
    }
}