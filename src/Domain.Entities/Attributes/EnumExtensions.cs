using System;

namespace Domain.Entities
{
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum value)
            where T : Attribute
        {
            return (value.GetType()
                .GetMember(Enum.GetName(value.GetType(), value))[0]
                .GetCustomAttributes(typeof(T), inherit: false)[0] as T);
        }
    }
}