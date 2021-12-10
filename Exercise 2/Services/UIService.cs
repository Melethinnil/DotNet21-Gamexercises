using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_2.Services
{
    /// <summary>
    /// Service responsible for displaying things to the player and read commands
    /// </summary>
    internal class UIService
    {
        public static int ScreenWidth { get; private set; } = 80;
        public static int ScreenHeight { get; private set; } = 25;

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

            Console.Write("\nSure, my name is "); GameService.PlayerName = Console.ReadLine();
            //Set a default "name" if not entering a name
            if (GameService.PlayerName == "")
                GameService.PlayerName = "you";

            Console.Clear();
            Console.WriteLine(
                $"Ah yes, {GameService.PlayerName}, I remember now.\n" +
                "You were the one who applied for the prestigeous position of\n" +
                "Coordination Helper for Urgent Management. Or CHUM, for short.\n" +
                "The only one who applied, as a matter of fact. Fascinating.");
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine(
                "Anyway, your job will be to register attendees for the various\n" +
                "events we manage and make sure all the information is correct.\n" +
                "You will also need to make sure that the attendees are satisfied,\n" +
                "by for example adjusting any already-entered information if they\n" +
                "want to make any changes.");
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
            MainScreen();
        }

        /// <summary>
        /// The main screen of the game, which shows basic overview information about the event and unread messages.
        /// </summary>
        static void MainScreen()
        {
            Console.Clear();

            StatusBar();

            Console.ForegroundColor = ConsoleColor.DarkYellow;

            //Show a list of available commands

            //Wait for player command
        }

        /// <summary>
        /// A screen that shows all received messages, with unread messages highlighted.
        /// </summary>
        static void MessageListScreen()
        {
            //Show basic event details (age limit, number of attendees, time left, etc) and notification icon

            //Show a list of messages, with unread messages highlighted

            //Show a list of available commands

            //Wait for player command
        }

        /// <summary>
        /// A screen that shows the text of a specified message.
        /// </summary>
        /// <param name="m">The message to show</param>
        static void MessageScreen(Message m)
        {
            //Show basic event details (age limit, number of attendees, time left, etc) and notification icon

            //Show the information contained in the selected message

            //Show a list of available commands

            //Wait for player command
        }

        /// <summary>
        /// A screen that allows you to send a message to a customer.
        /// </summary>
        /// <param name="id">The id of the customer you want to send a message to</param>
        static void SendMessageScreen(int id)
        {
            //Show basic event details (age limit, number of attendees, time left, etc) and notification icon

            //Show the information in the message to be sent

            //Show a list of available message subjects

            //Show a list of available commands

            //Wait for player command
        }

        /// <summary>
        /// A screen that shows a list of all current attendees.
        /// </summary>
        static void AttendeeListScreen()
        {
            //Show basic event details (age limit, number of attendees, time left, etc) and notification icon

            //Show a list of attendees with basic details like ID, full name, email adress and discount code

            //Show a list of available commands

            //Wait for player command
        }

        /// <summary>
        /// A screen that shows all info about a specific attendee, including their message history.
        /// </summary>
        /// <param name="a">The attendee to show information about</param>
        /// <param name="editMode">True if editing an existing attendee, false if adding a new one</param>
        static void AttendeeScreen(Attendee a, bool editMode)
        {
            //Show basic event details (age limit, number of attendees, time left, etc) and notification icon

            //Show extended details about the selected attendee

            //Show a list of messages sent by the attendee

            //Show a list of available commands

            //Wait for player command
        }

        /// <summary>
        /// A screen shown at the end of the game, where the player sees their stats, score and customer reviews.
        /// </summary>
        static void EndScreen()
        {
        }

        /// <summary>
        /// A status bar that shows information basic event information, such as number of attendees and an unread message notification
        /// </summary>
        static void StatusBar()
        {
            //Write out a status bar with a left-aligned name, center-aligned remaining time, and right-aligned message notification
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("|:");
            for (int i = 0; i < ScreenWidth - 4; i++)
                Console.Write(" ");
            Console.Write(":|");

            string nameString = $"|: Welcome, {GameService.PlayerName}.";
            string attendeeString = $"Total attendees: {EventService.Attendees.Count}";
            string messageString = $"\u2709 {GameService.NumUnreadMessages} :|";
            Console.Write($"{nameString} {attendeeString} {messageString}");
        }
    }
}
