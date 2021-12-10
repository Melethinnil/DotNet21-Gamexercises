using Exercise_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_2.Services
{
    /// <summary>
    /// Service responsible for handling gameplay-related things, such as score and customer satisfaction.
    /// </summary>
    internal class GameService
    {
        private static int _timeLimit = 10 * 60;    //The time limit of the game in seconds
        private static DateTime _startTime;
        public static string PlayerName { get; set; } = "you";
        public static List<Message> Messages { get; set; } = new List<Message>();
        public static ushort NumUnreadMessages { get; private set; }
        public static Command[] ValidCommands { get; private set; } =
        {
                new Command("messages", "Show a list of received messages"),
                new Command("attendees", "Show a list of all current attendees"),
                new Command("read message", "Read the message with the specified number", "[number]"),
                new Command("send message", "Send a message to the specified customer ID", "[id]"),
                new Command("view attendee", "View attendee details for the specified customer ID", "[id]"),
                new Command("next", "Go to the next page of the current screen"),
                new Command("prev", "Go to the previous page of the current screen"),
                new Command("menu", "return to the main menu")
        };

        /// <summary>
        /// Starts the game.
        /// </summary>
        public static void Start()
        {
            UIService.IntroScreen();

            _startTime = DateTime.Now;

            Timer statusUpdater = new Timer(UIService.UpdateStatus, new AutoResetEvent(false), 1000, 1000);

            TestSetup();

            MainLoop();
        }

        /// <summary>
        /// Adds some placeholder messages and attendees for testing purposes
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void TestSetup()
        {
            Messages.Add(new Message(GenerateUniqueNumber(), "Remove reservation", "Hi! I want to remove my reservation for the event."));
            Messages[0].Read();
            Messages.Add(new Message(GenerateUniqueNumber(), "i sent the wrong information sorry", "i accidentally sent the wrong information. my last name is supposed to be haraldsson, can you please change it?"));
        }

        /// <summary>
        /// The main gameplay loop of the game.
        /// </summary>
        private static void MainLoop()
        {
            string command = UIService.MainScreen().ToLower();

            while (TimeRemaining() > 0)
            {
                if (command == "messages")
                    command = UIService.MessageListScreen();
                else if (command == "attendees")
                    command = UIService.AttendeeListScreen();
                else if (command == "menu")
                    command = UIService.MainScreen();
            }
        }

        public static ushort GenerateUniqueNumber()
        {
            //NOT YET IMPLEMENTED
            //Generate a random unique number between 10,000 and 65535
            return 10000;
        }

        /// <summary>
        /// Calculates the time remaining based on when the game was started.
        /// </summary>
        /// <returns>The remaining time in whole seconds.</returns>
        public static int TimeRemaining()
        {
            return (int)Math.Ceiling((_startTime.AddSeconds((double)_timeLimit) - DateTime.Now).TotalSeconds);
        }
    }
}
