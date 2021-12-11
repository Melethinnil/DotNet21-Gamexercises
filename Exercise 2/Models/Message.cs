using Exercise_2.Services;

namespace Exercise_2.Models
{
    public enum MessageType
    {
        Booking,
        Unbooking,
        WrongInfo,
        ChangeTicket,
        MissingInfo
    }

    internal class Message
    {
        public ushort ID { get; set; }
        public MessageType Type { get; set; }
        public string Subject { get; set; }
        public string Contents { get; set; }
        public bool HasBeenRead { get; private set; } = false;

        public Message(ushort id, MessageType type, string subject, string contents)
        {
            ID = id;
            Type = type;
            Subject = subject;
            Contents = contents;
        }

        /// <summary>
        /// Marks the message as having been read
        /// </summary>
        public void Read()
        {
            HasBeenRead = true;
        }

        /// <summary>
        /// Gets a short one-line summary of the message contents
        /// </summary>
        /// <returns></returns>
        public string Summary()
        {
            if (Contents.Length > 30)
                return Contents.Substring(0, 27) + "...";
            else
                return Contents;
        }

        public string ShortSubject()
        {
            if (Subject.Length > 20)
                return Subject.Substring(0, 17) + "...";
            else
                return Subject;
        }
    } 
}