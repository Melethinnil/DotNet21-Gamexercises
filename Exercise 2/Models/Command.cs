using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_2.Models
{
    internal class Command
    {
        //The value that the player must enter to execute the command
        public string Value { get; set; }
        //A text that indicated any extra values the player can enter as part of the command
        public string ExtraValue { get; set; }
        //A text that describes what the command does
        public string Description  { get; set; }

        public Command(string value, string description, string extraValue)
        {
            Value = value;
            ExtraValue = extraValue;
            Description = description;
        }
        public Command(string value, string description)
        {
            Value = value;
            ExtraValue = "";
            Description = description;
        }

        public string FullValue()
        {
            return Value + " " + ExtraValue;
        }
    }
}
