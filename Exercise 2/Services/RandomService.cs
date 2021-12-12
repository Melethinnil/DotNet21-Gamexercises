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
            "Manda",
            "Ellen",
            "Annie",
            "Naomi",
            "Valerie",
            "Theres",
            "Stephnie",
            "Terry",
            "Gail",
            "Hanna",
            "Katrina",
            "Hannah",
            "Shelly",
            "Therese",
            "Patty",
            "Colleen",
            "Joy",
            "Jenny",
            "Kristi",
            "Lynn",
            "Inez",
            "Debora",
            "Della",
            "Blanche",
            "Debra",
            "Jennie",
            "Sandra",
            "Joyce",
            "Susanne",
            "Suzanne",
            "Marsha",
            "Thelma",
            "Bridget",
            "Denise",
            "Emma",
            "Minny",
            "Minnie",
            "Frank",
            "Marcus",
            "Markus",
            "Don",
            "Lee",
            "Rickey",
            "Michael",
            "Arthur",
            "Marcos",
            "Ed",
            "Harold",
            "Arnold",
            "Ricky",
            "James",
            "Edmund",
            "Fredrick",
            "Frederick",
            "Marlon",
            "Fred",
            "Johnny",
            "Ruben",
            "Donald",
            "Jonny",
            "Devin",
            "Phil",
            "Luther",
            "Tedd",
            "Hubert",
            "Allan",
            "Nelson",
            "Ted",
            "Eugene",
            "Julio",
            "Oskar",
            "Lyle",
            "Oscar",
            "Jake",
            "William",
            "Kurt",
            "Kyle",
            "Myron",
            "Pete",
            "Boyd",
            "Peter",
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
            "Yung",
            "Hamilton",
            "McGee",
            "Bryant",
            "Thomson",
            "Williams",
            "Clark",
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
            "Wolffe",
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
            "McLaughlin",
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
        private static List<string> _allergyPrefixes = new()
        {
            "I am allergic to",
            "I don't want",
            "No",
            "I can't stand",
            "I get sick from"
        };
        private static List<string> _allergies = new()
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
        private static List<string> _subjectsUnReservation = new()
        {
            "Changed my mind",
            "Unbooking",
            "Remove reservation",
            "I no longer want to attend",
            "Delete ticket"
        };
        private static List<string> _subjectsWrongInfo = new()
        {
            "Oops",
            "Wrong info",
            "Forgot something",
            "Additional info",
            "Correction"
        };
        private static List<string> _subjectsChangeTicket = new()
        {
            "Changed my mind",
            "Change ticket",
            "I want to change my ticket level",
            "Different ticket"
        };
        private static List<string> _greetings = new()
        {
            "Hi!",
            "Hello!",
            "Greetings.",
            "Good day."
        };
        private static List<string> _introductions = new()
        {
            "I am",
            "My name is",
            "I'm"
        };
        private static List<string> _attendTexts = new()
        {
            " and I want to attend the event",
            " and I'd like a ticket for the event",
            ". I want to book a ticket",
            ". I'd like to attend the event",
            " and I would like to make a ticket reservation for the event"
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

        internal static List<string> Allergies()
        {
            bool generateNotes = random.Next(100) >= 50;
            if (generateNotes)
            {
                List<string> notes = new List<string>();
                int numNotes = random.Next(1, 4);
                List<string> subjects = new List<string>(_allergies);
                for (int i = 0; i < numNotes; i++)
                {
                    int index = random.Next(0, subjects.Count);
                    notes.Add(subjects[index]);
                    subjects.RemoveAt(index);
                }
                return notes;
            }
            else
                return new List<string>();
        }

        internal static string AllergyText(List<string> allergies)
        {
            bool omitAllergies = random.Next(100) <= 30;

            if (!omitAllergies)
            {
                if (allergies.Count == 0)
                    return "I am not allergic to anything.";

                string allergyText = _allergyPrefixes[random.Next(_allergyPrefixes.Count)] + " ";
                for (int i = 0; i < allergies.Count; i++)
                {
                    allergyText += allergies[i];
                    if (i < allergies.Count - 2)
                        allergyText += ", ";
                    else if (i < allergies.Count - 1)
                        allergyText += " and ";
                    else
                        allergyText += ".";
                }
                return allergyText; 
            }
            return "";
        }

        internal static TicketClass Ticket()
        {
            Array values = Enum.GetValues(typeof(TicketClass));
            return (TicketClass)values.GetValue(random.Next(values.Length));
        }

        internal static TicketClass Ticket(TicketClass except)
        {
            List<TicketClass> values = ((TicketClass[])Enum.GetValues(typeof(TicketClass))).ToList();
            values.Remove(except);
            return values[random.Next(values.Count)];
        }

        internal static int Age()
        {
            //return a random age, weighted towards younger ages
            bool older = random.Next(100) <= 30;
            if(older)
                return random.Next(40, 100);
            else
                return random.Next(14, 40);

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
            MessageType[] types = { MessageType.Unbooking, MessageType.ChangeTicket, MessageType.UpdateInfo };
            return (MessageType)types.GetValue(random.Next(types.Length));
        }

        internal static string MessageSubject(MessageType type)
        {
            switch(type)
            {
                case MessageType.Booking:
                    return _subjectsReservation[random.Next(_subjectsReservation.Count)];
                case MessageType.Unbooking:
                    return _subjectsUnReservation[random.Next(_subjectsUnReservation.Count)];
                case MessageType.UpdateInfo:
                    return _subjectsWrongInfo[random.Next(_subjectsWrongInfo.Count)];
                case MessageType.ChangeTicket:
                    return _subjectsChangeTicket[random.Next(_subjectsChangeTicket.Count)];
                default:
                    return "";
            }
        }

        internal static string MessageAttendText()
        {
            return _attendTexts[random.Next(_attendTexts.Count)];
        }

        internal static string MessageName(string name)
        {
            bool misSpelled = random.Next(100) <= 20;
            if(misSpelled)
            {
                char[] temp = name.ToCharArray();
                temp[random.Next(temp.Length)] = RandomChar();
                name = new string(temp);
            }
            else
            {
                bool skipLast = random.Next(100) <= 30;
                if (skipLast)
                    name = name.Split(' ')[0].ToString();
            }
            return _introductions[random.Next(_introductions.Count)] + " " + name;
        }

        private static char RandomChar()
        {
            return (char)random.Next('a', 'z');
        }

        internal static string MessageGreeting()
        {
            return _greetings[random.Next(_greetings.Count)];
        }

        internal static string MessageContents(MessageType type, string changedInfo = "", string changeValue = "")
        {
            switch(type)
            {
                case MessageType.UpdateInfo:
                    switch(changedInfo)
                    {
                        case "name":
                            return $"I accidentally gave you the wrong name. My actual name is {changeValue}.";
                        case "age":
                            return $"I accidentally gave you the wrong age. I'm actually {changeValue} years old.";
                        case "allergies":
                            return $"I accidentally gave you the wrong list of allergies. I'm actually allergic to {changeValue}.";
                    }
                    break;
                case MessageType.ChangeTicket:
                    return $"I want to change to a {changeValue} ticket.";
                case MessageType.Unbooking:
                    return "I no longer want to attend the event.";
            }

            return "";
        }

        internal static string AgeText(int age)
        {
            int style = random.Next(3);
            switch (style)
            {
                case 0:
                    return $"I am {age} years old";
                case 1:
                    return $"I was born in {DateTime.Now.Year - age}";
                case 2:
                    bool error = random.Next(100) <= 20;
                    if (error)
                    {
                        bool wrongAge = random.Next(100) <= 50;
                        if (wrongAge)
                            return $"I was born in {DateTime.Now.Year - age}, so I'm {age + random.Next(-8, 11)}";
                        else
                            return $"I was born in {DateTime.Now.Year - age + random.Next(-8, 11)}, so I'm {age}";
                    }
                    else
                        return $"I was born in {DateTime.Now.Year - age}, so I'm {age}";
                default:
                    return $"I'm {age}";
            }
        }
    }
}
