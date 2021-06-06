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
    }
}