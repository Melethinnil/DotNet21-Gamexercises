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
                new Command("read message", "Read the message with the specified number", "[number]", new List<Screen>(){Screen.MessageListScreen}),
                new Command("send message", "Send a message to the specified customer ID", "[id]", Enum.GetValues(typeof(Screen)).Cast<Screen>().Where(screen => screen != Screen.SendMessageScreen).ToList()),
                new Command("view attendee", "View attendee details for the specified customer ID", "[id]", Enum.GetValues(typeof(Screen)).Cast<Screen>().Where(screen => screen != Screen.AttendeeScreen).ToList()),
                new Command("add attendee", "Add an attendee with the specified ID to the event", "[id]", Enum.GetValues(typeof(Screen)).Cast<Screen>().Where(screen => screen != Screen.AttendeeScreen).ToList()),
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
            for (int i = 0; i < 100; i++)
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
                if (ValidCommands[0].Match(command))    //messages
                {
                    UIService.FirstPage();
                    command = UIService.MessageListScreen();
                }
                else if (ValidCommands[1].Match(command))   //attendees
                {
                    UIService.FirstPage();
                    command = UIService.AttendeeListScreen();
                }
                else if (ValidCommands[2].Match(command))    //read message
                {
                    int index = int.Parse(command.Split(' ', StringSplitOptions.RemoveEmptyEntries)[2]);
                    if (index >= Messages.Count)
                        command = UIService.Redraw("That message doesn't exist. Try again.");
                    else
                        command = UIService.MessageScreen(Messages[index]);
                }
                else if (ValidCommands[3].Match(command))    //send message
                {
                    ushort id = ushort.Parse(command.Split(' ', StringSplitOptions.RemoveEmptyEntries)[2]);
                    if (id < 12345 || id > 59999)
                        command = UIService.Redraw("Invalid ID. Try again.");
                    else
                        command = UIService.SendMessageScreen(id);
                }
                else if (ValidCommands[4].Match(command))    //view attendee
                {
                    ushort id = ushort.Parse(command.Split(' ', StringSplitOptions.RemoveEmptyEntries)[2]);
                    if (id < 12345 || id > 59999)
                        command = UIService.Redraw("Invalid ID. Try again.");
                    else
                        command = UIService.AttendeeScreen(id, true);
                }
                else if (ValidCommands[5].Match(command))    //add attendee
                {
                    ushort id = ushort.Parse(command.Split(' ', StringSplitOptions.RemoveEmptyEntries)[2]);
                    if (id < 12345 || id > 59999)
                        command = UIService.Redraw("Invalid ID. Try again.");
                    else
                        command = UIService.AttendeeScreen(id, false);
                }
                else if (ValidCommands[6].Match(command))   //next
                {
                    UIService.NextPage();
                    command = UIService.Redraw();
                }
                else if (ValidCommands[7].Match(command))   //previous
                {
                    UIService.PrevPage();
                    command = UIService.Redraw();
                }
                else if (ValidCommands[8].Match(command))   //menu
                {
                    command = UIService.MainScreen();
                }
            }

            UIService.EndScreen();
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
