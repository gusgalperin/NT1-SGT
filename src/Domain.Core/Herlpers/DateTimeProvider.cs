using System;

namespace Domain.Core.Herlpers
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