using Exercise_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_2.Services
{
    /// <summary>
    /// A service for generating various random things
    /// </summary>
    internal class RandomService
    {
        public static Random random = new Random(DateTime.Now.Hour+DateTime.Now.Minute+DateTime.Now.Second);
        private static List<ushort> _uniqueIDs = new();
        private static List<string> _firstNames = new()
        {
            "Paula",
            "Shelley",
            "Stephanie",
            "Mabel",
            "Jana",
            "Rachel",
            "Theresa",
            "Jessie",
            "Amanda",
            "Jody",
            "Iris",
            "Renee",
            "Brenda",
            "Mandy",
            "Ellen",
            "Annie",
            "Naomi",
            "Valerie",
            "Arlene",
            "Antoinette",
            "Terry",
            "Gail",
            "Constance",
            "Katrina",
            "Hannah",
            "Amanda",
            "Virginia",
            "Patty",
            "Colleen",
            "Joy",
            "Olga",
            "Kristi",
            "Lynn",
            "Inez",
            "Candice",
            "Della",
            "Blanche",
            "Debra",
            "Jennie",
            "Sandra",
            "Joyce",
            "Kay",
            "Suzanne",
            "Marsha",
            "Thelma",
            "Bridget",
            "Denise",
            "Emma",
            "Lynne",
            "Minnie",
            "Frank",
            "Marcus",
            "Clifford",
            "Don",
            "Lee",
            "Rickey",
            "Michael",
            "Arthur",
            "Marcos",
            "Ed",
            "Harold",
            "Arnold",
            "Gustavo",
            "James",
            "Edmund",
            "Fredrick",
            "Nathaniel",
            "Marlon",
            "Fred",
            "Johnny",
            "Ruben",
            "Donald",
            "Duane",
            "Devin",
            "Phil",
            "Luther",
            "Nicolas",
            "Hubert",
            "Allan",
            "Nelson",
            "Ted",
            "Eugene",
            "Julio",
            "Terrence",
            "Lyle",
            "Oscar",
            "Jake",
            "William",
            "Kurt",
            "Kyle",
            "Myron",
            "Pete",
            "Boyd",
            "Sidney",
            "Andre",
            "Thomas",
            "Alfredo",
            "Wilbur",
            "Scott",
            "Pedro"
        };
        private static List<string> _lastNames = new()
        {
            "Fletcher",
            "Stokes",
            "Collier",
            "Taylor",
            "Wells",
            "Maldonado",
            "Barber",
            "Drake",
            "Wilson",
            "Walsh",
            "Dunn",
            "Duncan",
            "Simmons",
            "Holloway",
            "Moreno",
            "Chandler",
            "Mclaughlin",
            "Ingram",
            "Ward",
            "Little",
            "Malone",
            "Singleton",
            "Fields",
            "Young",
            "Mathis",
            "Thompson",
            "Todd",
            "Hamilton",
            "Mcgee",
            "Bryant",
            "Sims",
            "Williams",
            "Mcdaniel",
            "Griffin",
            "Ballard",
            "Hoffman",
            "Wise",
            "Zimmerman",
            "Clarke",
            "Harris",
            "Hale",
            "Garner",
            "Mack",
            "Stephens",
            "Mason",
            "Holland",
            "Turner",
            "Wilkins",
            "Barnes",
            "Casey",
            "Day",
            "Kelley",
            "Bailey",
            "Hudson",
            "Moreno",
            "Rowe",
            "Arnold",
            "Floyd",
            "Manning",
            "Sims",
            "Banks",
            "Joseph",
            "Wolfe",
            "Jimenez",
            "Rice",
            "Newton",
            "Shaw",
            "Wood",
            "Knight",
            "Palmer",
            "Russell",
            "Hammond",
            "Stone",
            "Collier",
            "Morgan",
            "Hawkins",
            "Guerrero",
            "Harris",
            "Hunter",
            "Goodwin",
            "Roy",
            "Nunez",
            "Franklin",
            "Phillips",
            "Vega",
            "Garcia",
            "Cannon",
            "Tyler",
            "Mitchell",
            "Alvarado",
            "Fuller",
            "Daniels",
            "Garza",
            "Tucker",
            "Mclaughlin",
            "Stokes",
            "Meyer",
            "Gonzales",
            "Hill",
            "Parker"
        };
        private static List<string> _emailDomains = new()
        {
            "domain.com",
            "yourmail.com",
            "workplace.org",
            "university.edu"
        };
        private static List<string> _specialNotesPrefixes = new()
        {
            "I am allergic to",
            "I don't want",
            "No",
            "I can't stand",
            "I get sick from"
        };
        private static List<string> _specialNotesSubjects = new()
        {
            "peanuts",
            "olives",
            "nuts",
            "tomato",
            "gluten",
            "dairy",
            "lactose",
            "eggs",
            "fish",
            "shellfish",
            "crab",
            "corn",
            "meat"
        };
        private static List<string> _subjectsReservation = new()
        {
            "Reservation",
            "Attend",
            "Attend the event",
            "I want to attend",
            "Booking",
            "Ticket reservation",
            "Ticket booking"
        };

        public static ushort ID()
        {
            //Populate the list of unique IDs if it's empty
            if (_uniqueIDs.Count == 0)
            {
                for (ushort i = 12345; i <= 59999; i++)
                    _uniqueIDs.Add(i);
            }

            int index = random.Next(_uniqueIDs.Count);
            ushort id = _uniqueIDs[index];
            _uniqueIDs.RemoveAt(index);
            return id;
        }

        internal static string SpecialNotes()
        {
            bool generateNotes = random.Next(100) >= 50;
            if (generateNotes)
            {
                string notes = _specialNotesPrefixes[random.Next(_specialNotesPrefixes.Count)] + " ";
                int numNotes = random.Next(1, 4);
                List<string> subjects = new List<string>(_specialNotesSubjects);
                for(int i = 0; i < numNotes; i++)
                {
                    int index = random.Next(0, subjects.Count);
                    notes += subjects[index];
                    subjects.RemoveAt(index);
                    if (i < numNotes - 2)
                        notes += ", ";
                    else if (i < numNotes - 1)
                        notes += " and ";
                    else
                        notes += ".";
                }
                return notes;
            }
            else
                return "";
        }

        internal static TicketClass Ticket()
        {
            Array values = Enum.GetValues(typeof(TicketClass));
            return (TicketClass)values.GetValue(random.Next(values.Length));
        }

        internal static int Age()
        {
            return random.Next(14, 100);
        }

        internal static string Email(string firstName, string lastName)
        {
            int type = random.Next(0, 6);
            string domain = _emailDomains[random.Next(_emailDomains.Count)];
            switch (type)
            {
                case 0:
                    return $"{firstName.ToLower()}.{lastName.ToLower()}@{domain}";
                case 1:
                    return $"{firstName.ToLower()}.{lastName.ToLower()[0]}@{domain}";
                case 2:
                    return $"{firstName.ToLower()[0]}{lastName.ToLower()}@{domain}";
                case 3:
                    return $"{firstName.ToLower()}_{lastName.ToLower()}@{domain}";
                case 4:
                    return $"{firstName.ToLower().Substring(0, Math.Min(firstName.Length, 3))}{lastName.ToLower().Substring(0, Math.Min(lastName.Length, 3))}{random.Next(10, 100)}@{domain}";
                default:
                    return $"{firstName.ToLower()}_{lastName.ToLower()[0]}@{domain}";
            }
        }

        internal static string LastName()
        {
            int index = random.Next(_lastNames.Count);
            return _lastNames[index];
        }

        internal static string FirstName()
        {
            int index = random.Next(_firstNames.Count);
            return _firstNames[index];
        }

        internal static MessageType RandomMessageType()
        {
            Array types = Enum.GetValues(typeof(MessageType));
            return (MessageType)types.GetValue(random.Next(types.Length));
        }

        internal static string MessageSubject(MessageType type)
        {
            switch(type)
            {
                case MessageType.Booking:
                    return _subjectsReservation[random.Next(_subjectsReservation.Count)];
                case MessageType.Unbooking:
                    return "";
                case MessageType.MissingInfo:
                    return "";
                case MessageType.WrongInfo:
                    return "";
                case MessageType.ChangeTicket:
                    return "";
                default:
                    return "";
            }
        }

        internal static string MessageAttendText()
        {
            return " and I want to attend the event";
        }

        internal static string MessageName()
        {
            return "My name is";
        }

        internal static string MessageGreeting()
        {
            return "Hi!";
        }
    }
}
