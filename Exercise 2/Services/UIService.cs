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
        private static string _errorMessage = "";
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
                "For each attendee, we require their first and last names, age,\n" +
                "which class of ticket they want, and whether or not they have\n" +
                "any allergies. They are also eligible for a personal discount\n" +
                "code depending on their ticket class, so you'll need to create\n" +
                "those codes as well. And don't forget to enter the customer ID\n" +
                "from the message they send you, otherwise there will be issues\n" +
                "with linking it all together in the system.");
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
            while (true)
            {
                Console.Clear();

                StatusBar();

                CurrentScreen = Screen.MainScreen;

                Console.ForegroundColor = ConsoleColor.DarkYellow;

                //Show event details, such as age limit, number of attendees, etc
                Console.WriteLine($"\n\nMinimum age of admission: {EventService.AgeLimit}");
                Console.WriteLine($"Number of attendees:      {EventService.Attendees.Count}");

                //Show a list of available commands
                ListCommands(CurrentScreen);

                //Show error message, if any
                if (_errorMessage != "")
                {
                    ErrorMessage(_errorMessage);
                    _errorMessage = "";
                }

                //Wait for player command and return the command if valid, otherwise loop back to the start of the screen with an error message
                ResetCursor();
                string command = Console.ReadLine();
                if (IsValidCommand(command, CurrentScreen))
                    return command;
                else if (command == "")
                    return "menu";
                else
                    _errorMessage = "Command not recognized. Please try again.";
            }
        }

        /// <summary>
        /// A screen that shows all received messages, with unread messages highlighted.
        /// </summary>
        public static string MessageListScreen()
        {
            while (true)
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
                ListCommands(CurrentScreen);

                //Show error message, if any
                if (_errorMessage != "")
                {
                    ErrorMessage(_errorMessage);
                    _errorMessage = "";
                }

                //Wait for player command and return the command if valid, otherwise loop back to the start of the screen with an error message
                ResetCursor();
                string command = Console.ReadLine();
                if (IsValidCommand(command, CurrentScreen))
                    return command;
                else if (command == "")
                    return "menu";
                else
                    _errorMessage = "Command not recognized. Please try again.";
            }
        }

        /// <summary>
        /// A screen that shows the text of a specified message.
        /// </summary>
        /// <param name="m">The message to show</param>
        public static string MessageScreen(Message m)
        {
            while (true)
            {
                Console.Clear();

                StatusBar();

                CurrentScreen = Screen.MessageScreen;
                //Show the information contained in the selected message and mark it as read
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nSubject: {m.Subject}\nCustomer ID: {m.ID}\n\n{m.Contents}");
                m.Read();

                //Show a list of available commands
                ListCommands(CurrentScreen);

                //Show error message, if any
                if (_errorMessage != "")
                {
                    ErrorMessage(_errorMessage);
                    _errorMessage = "";
                }

                //Wait for player command and return the command if valid, otherwise loop back to the start of the screen with an error message
                ResetCursor();
                string command = Console.ReadLine();
                if (IsValidCommand(command, CurrentScreen))
                    return command;
                else if (command == "")
                    return "menu";
                else
                    _errorMessage = "Command not recognized. Please try again.";
            }
        }

        /// <summary>
        /// A screen that allows you to send a message to a customer.
        /// </summary>
        /// <param name="id">The id of the customer you want to send a message to</param>
        public static string SendMessageScreen(ushort id)
        {
            Attendee customer = GameService.Customers.Find(a => a.ID == id);
            if (customer == null)
            {
                _errorMessage = "ID not found. Try again.";
                return "menu";
            }

            List<string> requests = new List<string>();

            while (true)
            {
                Console.Clear();

                StatusBar();

                CurrentScreen = Screen.SendMessageScreen;

                Console.ForegroundColor = ConsoleColor.Cyan;

                //Show the information in the message to be sent
                Message message = new Message(id, MessageType.UpdateInfo, "Missing/wrong info", $"Hello!\n" +
                    $"Your request to attend the event contains some missing or erroneous information.\n" +
                    $"Please reply with the following information: {string.Join(", ", requests)}\n\n" +
                    $"Best regards, {GameService.PlayerName}", requests);
                Console.WriteLine($"\nTo: {customer.Name} ({customer.ID})");
                Console.WriteLine($"\nSubject: {message.Subject}");
                Console.WriteLine($"\n{message.Contents}");

                //Show a list of available commands
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(0, ScreenHeight - 12);
                Console.WriteLine("\nAvailable Commands:");
                Console.WriteLine($"{"request [info]".PadRight(ScreenWidth / 3)}Request the specified information.");
                Console.WriteLine($"{"unrequest [info]".PadRight(ScreenWidth / 3)}Remove the specified information from the request.");
                Console.WriteLine($"{"send".PadRight(ScreenWidth / 3)}Send the message and return to the menu.");
                Console.WriteLine($"{"discard".PadRight(ScreenWidth / 3)}Discard the message and return to the menu.");

                //Show error message, if any
                if (_errorMessage != "")
                {
                    ErrorMessage(_errorMessage);
                    _errorMessage = "";
                }

                //Wait for player command and return the command if valid, otherwise loop back to the start of the screen with an error message
                ResetCursor();
                string command = Console.ReadLine();
                if (command.StartsWith("request") && command.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length > 1)   //Entered a command to request certain info
                {
                    string request = command.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1].ToLower();
                    requests.Add(request);
                }
                else if (command.StartsWith("unrequest") && command.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length > 1)   //Entered a command to unrequest certain info
                {
                    string request = command.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1].ToLower();
                    requests.Remove(request);
                }
                else if (command == "send")
                {
                    customer.ReceiveMessage(message);
                    return "menu";
                }
                else if (command == "discard")
                    return "menu";
                else if (command == "")
                    return "menu";
                else
                    _errorMessage = "Command not recognized. Please try again.";
            }
        }

        /// <summary>
        /// A screen that shows a list of all current attendees.
        /// </summary>
        public static string AttendeeListScreen()
        {
            while (true)
            {
                Console.Clear();

                StatusBar();

                CurrentScreen = Screen.AttendeeListScreen;

                Console.ForegroundColor = ConsoleColor.DarkGreen;

                //Show a list of attendees with basic details like ID, full name, discount code
                Console.WriteLine();
                Console.WriteLine("ID".PadRight(10) + "Name".PadRight(20) + "Age".PadRight(5) + "Allergies".PadRight(25) + "Discount Code");
                int startIndex = EventService.Attendees.Count - 1 - _maxAttendeesPerScreen * _page;
                for (int i = startIndex; i >= startIndex - _maxAttendeesPerScreen && i >= 0; i--)
                {
                    Attendee attendee = EventService.Attendees[i];
                    Console.WriteLine($"{attendee.ID.ToString().PadRight(10)}{attendee.Name.PadRight(20)}{attendee.Age.ToString().PadRight(5)}{string.Join(", ", attendee.Allergies).PadRight(25)}{attendee.DiscountCode}");
                }

                //Show a list of available commands
                ListCommands(CurrentScreen);

                //Show error message, if any
                if (_errorMessage != "")
                {
                    ErrorMessage(_errorMessage);
                    _errorMessage = "";
                }

                //Wait for player command and return the command if valid, otherwise loop back to the start of the screen with an error message
                ResetCursor();
                string command = Console.ReadLine();
                if (IsValidCommand(command, CurrentScreen))
                    return command;
                else if (command == "")
                    return "menu";
                else
                    _errorMessage = "Command not recognized. Please try again.";
            }
        }

        /// <summary>
        /// A screen that shows all info about a specific attendee, including their message history.
        /// </summary>
        /// <param name="a">The attendee to show information about</param>
        /// <param name="editMode">True if editing an existing attendee, false if adding a new one</param>
        public static string AttendeeScreen(ushort id, bool editMode)
        {
            Attendee attendee = Attendee.Empty();

            if (!editMode)
            {
                //return to the menu with an error message if trying to add the same attendee twice
                if (EventService.Attendees.Find(a => a.ID == id) != null)
                {
                    _errorMessage = "Attendee already exists. Try again.";
                    return "menu";
                }
                EventService.AddAttendee(attendee);
                attendee.SetID(id);
            }
            else
            {
                attendee = EventService.Attendees.Find(a => a.ID == id);
                if (attendee == null)
                {
                    _errorMessage = "Attendee not found. Try again.";
                    return "menu";
                }
            }

            while (true)
            {
                Console.Clear();

                StatusBar();

                CurrentScreen = Screen.AttendeeScreen;

                Console.ForegroundColor = ConsoleColor.Green;

                //Show extended details about the selected attendee
                Console.WriteLine($"\nID: {attendee.ID}");
                Console.WriteLine($"\nName: {attendee.Name}");
                Console.WriteLine($"Age: {attendee.Age}");
                Console.WriteLine($"Ticket: {attendee.Ticket}");
                Console.WriteLine($"Allergies: {string.Join(", ", attendee.Allergies)}");
                Console.WriteLine($"Discount: {attendee.DiscountCode}");

                //Show a list of available commands
                ListCommands(CurrentScreen);
                Console.WriteLine($"{"set [field] [value]".PadRight(ScreenWidth / 3)}Set the specified field to the specified value");
                Console.WriteLine($"{"add allergy [allergy]".PadRight(ScreenWidth / 3)}Adds the specified allergy");
                Console.WriteLine($"{"remove allergy [allergy]".PadRight(ScreenWidth / 3)}Removes the specified allergy");

                //Show error message, if any
                if (_errorMessage != "")
                {
                    ErrorMessage(_errorMessage);
                    _errorMessage = "";
                }

                //Wait for player command and return the command if valid, otherwise loop back to the start of the screen with an error message
                ResetCursor();
                string command = Console.ReadLine();
                if(command.StartsWith("set") && command.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length > 2)   //Entered a command to change attendee values
                {
                    string field = command.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1].ToLower();
                    string value = string.Join(" ", command.Split(" ", StringSplitOptions.RemoveEmptyEntries)[2..]);
                    switch (field)
                    {
                        case "name":
                            attendee.Name = value;
                            break;
                        case "age":
                            attendee.Age = int.Parse(value);
                            break;
                        case "ticket":
                            switch(value.ToLower())
                            {
                                case "silver":
                                    attendee.Ticket = TicketClass.Silver;
                                    break;
                                case "gold":
                                    attendee.Ticket = TicketClass.Gold;
                                    break;
                                case "platinum":
                                    attendee.Ticket = TicketClass.Platinum;
                                    break;
                                default:
                                    _errorMessage = "Command not recognized. Please try again.";
                                    break;
                            }
                            break;
                        case "discount":
                            string code = $"EVENT{DateTime.Now.Year.ToString()[2..]}-{value.Replace(" ", "").ToUpper()[..4]}";
                            if (EventService.Attendees.Find(a => a.DiscountCode == code) == null)
                                attendee.DiscountCode = code;
                            else
                                _errorMessage = "Code already exists. Please try again.";
                            break;
                        default:
                            _errorMessage = "Command not recognized. Please try again.";
                            break;


                    }
                }
                else if (command.StartsWith("add allergy") && command.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length > 2)
                {
                    string allergy = command.Split(" ", StringSplitOptions.RemoveEmptyEntries)[2];
                    attendee.Allergies.Add(allergy.ToLower());
                }
                else if (command.StartsWith("remove allergy") && command.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length > 2)
                {
                    string allergy = command.Split(" ", StringSplitOptions.RemoveEmptyEntries)[2];
                    attendee.Allergies.Remove(allergy.ToLower());
                }
                else if (IsValidCommand(command, CurrentScreen))
                    return command;
                else if (command == "")
                    return "menu";
                else
                    _errorMessage = "Command not recognized. Please try again.";
            }
        }

        /// <summary>
        /// A screen shown at the end of the game, where the player sees their stats, score and customer reviews.
        /// </summary>
        public static void EndScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"           _____          __  __ ______    ______      ________ _____  
          / ____|   /\   |  \/  |  ____|  / __ \ \    / /  ____|  __ \ 
         | |  __   /  \  | \  / | |__    | |  | \ \  / /| |__  | |__) |
         | | |_ | / /\ \ | |\/| |  __|   | |  | |\ \/ / |  __| |  _  / 
         | |__| |/ ____ \| |  | | |____  | |__| | \  /  | |____| | \ \ 
          \_____/_/    \_\_|  |_|______|  \____/   \/   |______|_|  \_\");

            //Show the total number of attendees for the event at the end
            Console.WriteLine($"\n\n\nCongratulations! You added {EventService.Attendees.Count} attendees to the event! However...\n");
            //Show the total number of attendees that wanted to attend but weren't added
            Console.WriteLine($"\n{Score.NonAddedAttendees} people who wanted to attend the event weren't added.");
            //Show the number of attendees that had missing or incorrect information at the end
            Console.WriteLine($"\n{Score.IncorrectInformation} attendees were registered with missing or incorrect information.");
            //Show the number of underage attendees in the event
            Console.WriteLine($"\n{Score.UnderageAttendees} attendees were added despite being below the age limit.");
            //Show the number of attendees that should have been removed but are still in the list
            Console.WriteLine($"\n{Score.NonRemovedAttendees} asked to be removed but weren't.");
            //Show the number of attendees that didn't receive a discount code
            Console.WriteLine($"\n{Score.MissingDiscount} attendees didn't receive their discount code.");

            //Calculate and show the final score
            Console.WriteLine($"\n\nWhich brings your total score to {Score.TotalScore}!");
            Console.ReadLine();
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
            string timeString = $"{Math.Floor(timeLeft / 60d).ToString().PadLeft(2,'0')}:{(timeLeft % 60).ToString().PadLeft(2, '0')}";
            string messageString = $"Messages: {GameService.CountUnreadMessages()} ";

            int nameLength = (ScreenWidth - timeString.Length) / 2;
            int messageLength = ScreenWidth - timeString.Length - nameLength - 2;

            Console.Write(nameString.PadRight(nameLength));
            if (GameService.TimeRemaining() <= 60 || GameService.TimeRemaining() == 120 || GameService.TimeRemaining() == 90)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(timeString);
            if (GameService.CountUnreadMessages() > 0)
                Console.ForegroundColor = ConsoleColor.Gray;
            else
                Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(messageString.PadLeft(messageLength));
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

        private static bool IsValidCommand(string command, Screen screen)
        {
            List<Command> validCommands = GetValidCommands(screen);
            foreach (Command command2 in validCommands)
                if (command2.Match(command))
                    return true;
            return false;
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
        /// Redraws the current screen, optionally with an error message
        /// </summary>
        internal static string Redraw(string error = "")
        {
            _errorMessage = error;
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
