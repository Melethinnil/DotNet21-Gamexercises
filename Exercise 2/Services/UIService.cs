using Exercise_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_2.Services
{
    public enum Screen
    {
        MainScreen,
        MessageListScreen,
        MessageScreen,
        SendMessageScreen,
        AttendeeListScreen,
        AttendeeScreen
    }

    /// <summary>
    /// Service responsible for displaying things to the player and read commands
    /// </summary>
    internal class UIService
    {
        private static byte _maxMessagesPerScreen = 8;
        private static byte _maxAttendeesPerScreen = 8;
        private static byte _page = 0;
        public static int ScreenWidth { get; private set; } = 80;
        public static int ScreenHeight { get; private set; } = 26;
        public static Screen CurrentScreen { get; set; } = Screen.MainScreen;

        /// <summary>
        /// The intro before the game, presenting the player with instructions and some story.
        /// </summary>
        public static void IntroScreen()
        {
            //Set the console window's text color
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            //Get the player's name and show them a short story text that sets the scene and tells them what they're supposed to do
            Console.WriteLine(
                "Hello and welcome to Eventum, the #1 event planning company!\n" +
                "You must be our newest employee, right? Would you please tell\n" +
                "me your name?");

            ResetCursor();
            Console.Write("\nSure, my name is ");
            string name = Console.ReadLine();
            if (name != "")
                GameService.PlayerName = name;

            Console.Clear();
            Console.WriteLine(
                $"Ah yes, {GameService.PlayerName}, I remember now.\n" +
                "You were the one who applied for the prestigeous position of\n" +
                "Coordination Helper for Urgent Management. Or CHUM, for short.\n" +
                "The only one who applied, as a matter of fact. Fascinating.");
            ResetCursor();
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine(
                "Anyway, your job will be to register attendees for the various\n" +
                "events we manage and make sure all the information is correct.\n" +
                "You will also need to make sure that the attendees are satisfied,\n" +
                "by for example adjusting any already-entered information if they\n" +
                "want to make any changes.");
            ResetCursor();
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine(
                "For each attendee, we require their first and last names, email\n" +
                "adress, which class of ticket they want, and possibly special\n" +
                "notes such as allergies. They are also eligible for a personal\n" +
                "discount code depending on their ticket class, so you'll need\n" +
                "to create those codes as well. And don't forget to enter the\n" +
                "customer ID from the message they send you, otherwise there\n" +
                "will be issues with linking it all together in the system.");
            Console.WriteLine("\n\nYour first assignment starts right now. Good luck.");
            ResetCursor();
            Console.ReadLine();

            LogoScreen();
        }

        /// <summary>
        /// A screen that shows the game's logo, before moving on the main screen
        /// </summary>
        static void LogoScreen()
        {
            Console.Clear();
            //Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"

                                                                     ,&@       
                                                      .*     @@@@@@@@@@@       
                                            .(    *@@@@@@@@  @@@@@@@@@@@       
                                .(@   @@@@@@@@   @@@@@@@@@@@ @@@#@@@@          
                    ,%@@@   @@@@@@  .@@@@@@@@@   @@@@& @@@@@     @@@@          
          &@@@@@@@ @@@@@@@  @@@@@@  @@@@@@&     .@@@@* (@@@@     @@@@          
       #@@@@@@@@@@  @@@@@@@*@@@@@   @@@@@@#@@@   @@@@% .@@@@     @@@@          
      %@@@@@@@@@@@   @@@@@@@@@@@(   &@@@@@@@@@   @@@@@  @@@@     @@@@.         
      @@@@@@@         @@@@@@@@@@    *@@@@@@%     @@@@@  @@@@(    @@@@/         
      @@@@@@@@@@@@     @@@@@@@@,     @@@@@ *@@@  @@@@@  @@@@&    @%            
      (@@@@@@@@@@@*     @@@@@@@      @@@@@@@@@@  @@@@@  @@                     
       @@@@@@@@          @@@@@        @@@@@@@@@  *                             
       @@@@@@, (@@@       &@@ ____  __     __   __ _  __ _  ____  ____         
       @@@@@@@@@@@@,       %@(  _ \(  )   / _\ (  ( \(  ( \(  __)(  _ \        
        @@@@@@@@@@@&        * ) __// (_/\/    \/    //    / ) _)  )   /        
          &@@@@(             (__)  \____/\_/\_/\_)__)\_)__)(____)(__\_)        

                                                                                            ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n\n                                   START GAME");
            Console.ReadLine();
        }

        /// <summary>
        /// The main screen of the game, which shows basic overview information about the event and the commands the player can enter here.
        /// </summary>
        public static string MainScreen()
        {
            Console.Clear();

            StatusBar();

            CurrentScreen = Screen.MainScreen;

            Console.ForegroundColor = ConsoleColor.DarkYellow;

            //Show a list of available commands
            ListCommands(Screen.MainScreen);

            //Wait for player command and return the command if valid, otherwise loop back to the start of the screen
            ResetCursor();
            return Console.ReadLine();
        }

        /// <summary>
        /// A screen that shows all received messages, with unread messages highlighted.
        /// </summary>
        public static string MessageListScreen()
        {
            Console.Clear();

            StatusBar();

            CurrentScreen = Screen.MessageListScreen;

            Console.ForegroundColor = ConsoleColor.DarkCyan;

            //Show a list of messages (newest first), with unread messages highlighted
            //Console.WriteLine("\nMessages:\n");
            Console.WriteLine();
            Console.WriteLine("#".PadRight(5) + "ID".PadRight(10) + "Subject".PadRight(25) + "Message");
            int startIndex = GameService.Messages.Count - 1 - _maxMessagesPerScreen * _page;
            for (int i = startIndex; i >= startIndex - _maxMessagesPerScreen && i >= 0; i--)
            {
                Message message = GameService.Messages[i];
                if (!message.HasBeenRead)
                    Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{GameService.Messages.IndexOf(message).ToString().PadRight(5)}{message.ID.ToString().PadRight(10)}{message.ShortSubject().PadRight(25)}{message.Summary()}");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }

            //Show a list of available commands
            ListCommands(Screen.MessageListScreen);

            //Wait for player command and return the command if valid, otherwise loop back to the start of the screen
            ResetCursor();
            return Console.ReadLine();
        }

        /// <summary>
        /// A screen that shows the text of a specified message.
        /// </summary>
        /// <param name="m">The message to show</param>
        public static string MessageScreen(Message m)
        {
            Console.Clear();

            StatusBar();

            CurrentScreen = Screen.MessageScreen;
            //Show the information contained in the selected message

            //Show a list of available commands

            //Wait for player command and return the command if valid, otherwise loop back to the start of the screen
            ResetCursor();
            return Console.ReadLine();
        }

        /// <summary>
        /// A screen that allows you to send a message to a customer.
        /// </summary>
        /// <param name="id">The id of the customer you want to send a message to</param>
        public static string SendMessageScreen(int id)
        {
            Console.Clear();

            StatusBar();

            CurrentScreen = Screen.SendMessageScreen;
            //Show the information in the message to be sent

            //Show a list of available message subjects

            //Show a list of available commands

            //Wait for player command and return the command if valid, otherwise loop back to the start of the screen
            ResetCursor();
            return Console.ReadLine();
        }

        /// <summary>
        /// A screen that shows a list of all current attendees.
        /// </summary>
        public static string AttendeeListScreen()
        {
            Console.Clear();

            StatusBar();

            CurrentScreen = Screen.AttendeeListScreen;
            //Show a list of attendees with basic details like ID, full name, email adress and discount code

            //Show a list of available commands

            //Wait for player command and return the command if valid, otherwise loop back to the start of the screen
            ResetCursor();
            return Console.ReadLine();
        }

        /// <summary>
        /// A screen that shows all info about a specific attendee, including their message history.
        /// </summary>
        /// <param name="a">The attendee to show information about</param>
        /// <param name="editMode">True if editing an existing attendee, false if adding a new one</param>
        public static string AttendeeScreen(Attendee a, bool editMode)
        {
            Console.Clear();

            StatusBar();

            CurrentScreen = Screen.AttendeeScreen;
            //Show extended details about the selected attendee

            //Show a list of messages sent by the attendee

            //Show a list of available commands

            //Wait for player command and return the command if valid, otherwise loop back to the start of the screen
            ResetCursor();
            return Console.ReadLine();
        }

        /// <summary>
        /// A screen shown at the end of the game, where the player sees their stats, score and customer reviews.
        /// </summary>
        public static void EndScreen()
        {
        }

        /// <summary>
        /// A status bar that shows useful at-a-glance information
        /// </summary>
        private static void StatusBar()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("|:".PadRight(ScreenWidth - 2) + ":|");

            UpdateStatus(null);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();
            Console.WriteLine("|:".PadRight(ScreenWidth - 2, '_') + ":|");
        }

        public static void UpdateStatus(Object stateInfo)
        {
            //Save the current console cursor then set it to top left
            int[] savedCursosPos = { Console.CursorLeft, Console.CursorTop };
            Console.SetCursorPosition(0, 1);

            //Save the current foreground color
            ConsoleColor tempColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;

            string nameString = $"|: Welcome, {GameService.PlayerName}.";
            int timeLeft = GameService.TimeRemaining();
            string timeString = $"Time Left: {Math.Floor(timeLeft / 60d)}:{timeLeft % 60}";
            string messageString = $"Messages: {GameService.NumUnreadMessages} ";

            int nameLength = (ScreenWidth - timeString.Length) / 2;
            int messageLength = ScreenWidth - timeString.Length - nameLength - 2;

            Console.Write($"{nameString.PadRight(nameLength)}{timeString}");
            if (GameService.NumUnreadMessages > 0)
                Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{messageString.PadLeft(messageLength)}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(":|");

            //Restore the cursor position and foreground color
            Console.SetCursorPosition(savedCursosPos[0], savedCursosPos[1]);
            Console.ForegroundColor = tempColor;
        }

        /// <summary>
        /// Lists all the available commands for a given screen.
        /// </summary>
        /// <param name="screen">The current screen</param>
        private static void ListCommands(Screen screen)
        {
            //ConsoleColor tempColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            List<Command> validCommands = GetValidCommands(screen);
            Console.SetCursorPosition(0, ScreenHeight - 12);
            Console.WriteLine("\nAvailable Commands:");
            foreach (Command command in validCommands)
                Console.WriteLine($"{command.FullValue().PadRight(ScreenWidth / 3)}{command.Description}");

            //Console.ForegroundColor = tempColor;
        }

        private static List<Command> GetValidCommands(Screen screen)
        {
            return GameService.ValidCommands.Where(c => c.ValidIn.Contains(screen)).ToList();
        }

        /// <summary>
        /// Resets the console cursor to the default input location.
        /// </summary>
        private static void ResetCursor()
        {
            Console.SetCursorPosition(0, ScreenHeight - 1);
        }

        /// <summary>
        /// Moves to the next page, depending on the contents of the given screen.
        /// </summary>
        /// <param name="screen">The screen to use for limiting the page numbers</param>
        public static void NextPage()
        {
            switch (CurrentScreen)
            {
                case Screen.MessageListScreen:
                    if (GameService.Messages.Count - _maxMessagesPerScreen * (_page + 1) > 0)
                        _page++;
                    break;
                case Screen.AttendeeListScreen:
                    if (EventService.Attendees.Count - _maxAttendeesPerScreen * (_page + 1) > 0)
                        _page++;
                    break;
            }
        }

        /// <summary>
        /// Moves to the previous page.
        /// </summary>
        public static void PrevPage()
        {
            _page = (byte)Math.Max(_page - 1, 0);
        }

        /// <summary>
        /// Sets the current page to the first page.
        /// </summary>
        public static void FirstPage()
        {
            _page = 0;
        }

        /// <summary>
        /// Redraws the current screen.
        /// </summary>
        internal static string Redraw()
        {
            switch(CurrentScreen)
            {
                case Screen.MessageListScreen:
                    return MessageListScreen();
                case Screen.AttendeeListScreen:
                    return AttendeeListScreen();
                default:
                    return "menu";
            }
        }

        /// <summary>
        /// Displays an error message above the input line.
        /// </summary>
        public static void ErrorMessage(string message)
        {
            //Save the current console cursor then set it to one row above the input line
            int[] savedCursosPos = { Console.CursorLeft, Console.CursorTop };
            Console.SetCursorPosition(0, ScreenHeight - 2);

            //Save the current foreground color
            ConsoleColor tempColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.WriteLine(message);

            //Restore the cursor position and foreground color
            Console.SetCursorPosition(savedCursosPos[0], savedCursosPos[1]);
            Console.ForegroundColor = tempColor;
        }
    }
}
