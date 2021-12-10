using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_2.Services
{
    /// <summary>
    /// Service responsible for handling event details, such as attendee information
    /// </summary>
    internal class EventService
    {
        public static List<Attendee> Attendees { get; private set; } = new List<Attendee>();
    }
}
