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
        public static string PlayerName { get; set; }
        public static List<Message> Messages { get; set; }
        public static ushort NumUnreadMessages { get; private set; }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public static void Start()
        {
            UIService.IntroScreen();

            //Display the main menu
        }
        public static ushort GenerateUniqueNumber()
        {
            //Generate a random unique number between 10,000 and 65535
            return 10000;
        }
    }
}
