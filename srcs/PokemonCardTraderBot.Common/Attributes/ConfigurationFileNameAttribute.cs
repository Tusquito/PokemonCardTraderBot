using System;

namespace PokemonCardTraderBot.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationFileNameAttribute : Attribute
    {
        public string Name { get; set; }
        public ConfigurationFileNameAttribute(string name)
        {
            Name = name;
        }
    }
}