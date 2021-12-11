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
        //A list of the attendees currently registered for the event
        public static List<Attendee> Attendees { get; private set; } = new List<Attendee>();
        //A list of all the possible people who can send messages and be added to the attendee list
        public static List<Attendee> Customers { get; private set; } = new List<Attendee>();
        public static List<Message> Messages { get; set; } = new List<Message>();
        public static Command[] ValidCommands { get; private set; } =
        {
                new Command("messages", "Show a list of received messages", Enum.GetValues(typeof(Screen)).Cast<Screen>().Where(screen => screen != Screen.MessageListScreen).ToList()),
                new Command("attendees", "Show a list of all current attendees", Enum.GetValues(typeof(Screen)).Cast<Screen>().Where(screen => screen != Screen.AttendeeListScreen).ToList()),
                new Command("read message", "Read the message with the specified number", "[number]"),
                new Command("send message", "Send a message to the specified customer ID", "[id]"),
                new Command("view attendee", "View attendee details for the specified customer ID", "[id]"),
                new Command("add attendee", "Add an attendee with the specified ID to the event", "[id]"),
                new Command("next", "Go to the next page of the current screen", new List<Screen>(){Screen.MessageListScreen, Screen.AttendeeListScreen}),
                new Command("prev", "Go to the previous page of the current screen", new List<Screen>(){Screen.MessageListScreen, Screen.AttendeeListScreen}),
                new Command("menu", "return to the main menu", Enum.GetValues(typeof(Screen)).Cast<Screen>().Where(screen => screen != Screen.MainScreen).ToList())
        };

        /// <summary>
        /// Starts the game.
        /// </summary>
        public static void Start()
        {
            UIService.IntroScreen();

            _startTime = DateTime.Now;

            Timer statusUpdater = new Timer(UIService.UpdateStatus, new AutoResetEvent(false), 1000, 1000);

            GenerateCustomers();

            TestSetup();

            MainLoop();
        }

        private static void GenerateCustomers()
        {
            for (int i = 0; i < 150; i++)
            {
                Customers.Add(new Attendee());
            }
        }

        /// <summary>
        /// Adds some placeholder messages and attendees for testing purposes
        /// </summary>
        private static void TestSetup()
        {
            for (int i = 0; i < 15; i++)
            {
                Messages.Add(Customers[RandomService.random.Next(Customers.Count)].SendMessage());
            }
        }

        public static ushort CountUnreadMessages()
        {
            return (ushort)Messages.Where(m => !m.HasBeenRead).Count();
        }

        /// <summary>
        /// The main gameplay loop of the game.
        /// </summary>
        private static void MainLoop()
        {
            string command = UIService.MainScreen();

            while (TimeRemaining() > 0)
            {
                command = command.ToLower();
                if (command == "messages")
                {
                    UIService.FirstPage();
                    command = UIService.MessageListScreen();
                }
                else if (command == "attendees")
                {
                    UIService.FirstPage();
                    command = UIService.AttendeeListScreen();
                }
                else if (command == "menu")
                {
                    command = UIService.MainScreen();
                }
                else if (command == "next")
                {
                    UIService.NextPage();
                    command = UIService.Redraw();
                }
                else if (command == "prev")
                {
                    UIService.PrevPage();
                    command = UIService.Redraw();
                }
                else if(command.StartsWith("read message"))
                {
                    int index = int.Parse(command.Substring(13).Trim());
                    command = UIService.MessageScreen(Messages[index]);
                }
            }
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
