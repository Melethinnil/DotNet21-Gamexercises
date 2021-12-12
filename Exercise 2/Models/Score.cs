using Exercise_2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_2.Models
{
    internal class Score
    {
        public static int NonAddedAttendees { get; private set; } = 0;
        public static int IncorrectInformation { get; private set; } = 0;
        public static int UnderageAttendees { get; private set; } = 0;
        public static int NonRemovedAttendees { get; private set; } = 0;
        public static int MissingDiscount { get; private set; } = 0;
        public static int TotalScore { get; private set; } = 0;

        public static void Calculate()
        {
            List<Attendee> attendingCustomers = GameService.Customers.Where(c => c.Attending).ToList();
            //Calculate the number of people who wanted to be added but weren't
            foreach (Attendee c in attendingCustomers)
            {
                if (EventService.Attendees.Find(a => a.ID == c.ID) == null)
                    NonAddedAttendees++;
            }

            //Calculate the number of attendees listed with incorrect or missing information, underage attendees, attendees without discount code, and attendees who weren't removed after they requested to be
            foreach(Attendee a in EventService.Attendees)
            {
                bool correct = false;
                foreach(Attendee b in attendingCustomers)
                {
                    if (a.Match(b))
                    {
                        correct = true;
                        break;
                    }
                }
                if (!correct)
                    IncorrectInformation++;
                if (a.Age < EventService.AgeLimit)
                    UnderageAttendees++;
                if (a.DiscountCode == "")
                    MissingDiscount++;
                if (!GameService.Customers.Find(c => c.ID == a.ID).Attending)
                    NonRemovedAttendees++;
            }

            //Calculate the total score
            TotalScore = EventService.Attendees.Count - NonAddedAttendees - IncorrectInformation - UnderageAttendees - NonRemovedAttendees - MissingDiscount;
        }
    }
}
