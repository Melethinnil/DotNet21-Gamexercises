using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_2.Services
{
    public enum TicketClass
    {
        Silver,
        Gold,
        Platinum
    }

    /// <summary>
    /// Service responsible for handling event details, such as attendee information
    /// </summary>
    internal class EventService
    {
        public static int AgeLimit { get; private set; } = RandomService.random.Next(8, 21);
        public static List<Attendee> Attendees { get; private set; } = new List<Attendee>();
    }
}
