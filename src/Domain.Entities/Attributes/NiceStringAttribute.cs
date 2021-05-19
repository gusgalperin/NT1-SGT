using System;

namespace Domain.Entities
{
    public class NiceStringAttribute : Attribute
    {
        public string Description { get; }

        public NiceStringAttribute(string description)
        {
            Description = description;
        }
    }

    public static class NiceStringAttributeExtensions
    {
        public static string ToNiceString(this TurnoAccion turnoAccion)
        {
            return turnoAccion.GetAttribute<NiceStringAttribute>()?.Description 
                ?? turnoAccion.ToString();
        }

        private static T GetAttribute<T>(this Enum value)
            where T : Attribute
        {
            return (value.GetType()
                .GetMember(Enum.GetName(value.GetType(), value))[0]
                .GetCustomAttributes(typeof(T), inherit: false)[0] as T);
        }
    }
}