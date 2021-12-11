using Exercise_2.Services;
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
        public string Description { get; set; }
        //A list of the screens in which the command can be used
        public List<Screen> ValidIn { get; private set; }

        public Command(string value, string description, string extraValue)
        {
            Value = value;
            ExtraValue = extraValue;
            Description = description;
            ValidIn = Enum.GetValues(typeof(Screen)).Cast<Screen>().ToList();
        }
        public Command(string value, string description)
        {
            Value = value;
            ExtraValue = "";
            Description = description;
            ValidIn = Enum.GetValues(typeof(Screen)).Cast<Screen>().ToList();
        }
        public Command(string value, string description, string extraValue, List<Screen> validIn)
        {
            Value = value;
            ExtraValue = extraValue;
            Description = description;
            ValidIn = validIn;
        }
        public Command(string value, string description, List<Screen> validIn)
        {
            Value = value;
            ExtraValue = "";
            Description = description;
            ValidIn = validIn;
        }

        public string FullValue()
        {
            return Value + " " + ExtraValue;
        }
    }
}
