using System;

namespace PokemonCardTraderBot.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationNameAttribute : Attribute
    {
        public string Name { get; set; }
        public ConfigurationNameAttribute(string name)
        {
            Name = name;
        }
    }
}