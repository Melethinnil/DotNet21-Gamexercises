using Exercise_2.Models;
using Exercise_2.Services;
using System.Timers;

internal class Attendee
{
    private int _sentMessages = 0;
    public bool Attending { get; private set; } = false;

    public string Name { get; set; }
    public int Age { get; set; }
    public ushort ID { get; private set; }
    public TicketClass Ticket { get; set; }
    public string DiscountCode { get; set; }
    public List<string> Allergies { get; set; }

    public Attendee()
    {
        Name = RandomService.FirstName() + " " + RandomService.LastName();
        Age = RandomService.Age();
        ID = RandomService.ID();
        Ticket = RandomService.Ticket();
        Allergies = RandomService.Allergies();
        DiscountCode = "";
    }

    public Attendee(ushort id)
    {
        Name = "";
        Age = 0;
        ID = id;
        Ticket = TicketClass.Silver;
        Allergies = new List<string>();
    }

    public void AddDiscountCode(string code)
    {
        DiscountCode = code;
    }

    public void SendMessage()
    {
        Message m = null;

        if (_sentMessages == 0)
        {
            Attending = true;
            m = new Message(ID, MessageType.Booking, RandomService.MessageSubject(MessageType.Booking), $"{RandomService.MessageGreeting()} {RandomService.MessageName(Name)}{RandomService.MessageAttendText()}. {RandomService.AgeText(Age)}. I want a {Ticket} ticket. {RandomService.AllergyText(Allergies)}");
        }
        else
        {
            MessageType type = RandomService.RandomMessageType();
            switch (type)
            {
                case MessageType.UpdateInfo:
                    int toChange = RandomService.random.Next(3);
                    switch (toChange)
                    {
                        case 0:
                            Name = RandomService.FirstName() + " " + RandomService.LastName();
                            m = new Message(ID, MessageType.UpdateInfo, RandomService.MessageSubject(MessageType.UpdateInfo), RandomService.MessageContents(MessageType.UpdateInfo, "name", Name));
                            break;
                        case 1:
                            Age = RandomService.Age();
                            m = new Message(ID, MessageType.UpdateInfo, RandomService.MessageSubject(MessageType.UpdateInfo), RandomService.MessageContents(MessageType.UpdateInfo, "age", Age.ToString()));
                            break;
                        case 2:
                            Allergies = RandomService.Allergies();
                            m = new Message(ID, MessageType.UpdateInfo, RandomService.MessageSubject(MessageType.UpdateInfo), RandomService.MessageContents(MessageType.UpdateInfo, "allergies", GetAllergies()));
                            break;
                    }
                    break;
                case MessageType.ChangeTicket:
                    Ticket = RandomService.Ticket(Ticket);
                    m = new Message(ID, MessageType.ChangeTicket, RandomService.MessageSubject(MessageType.ChangeTicket), RandomService.MessageContents(MessageType.ChangeTicket, "ticket", Ticket.ToString()));
                    break;
                case MessageType.Unbooking:
                    Attending = false;
                    m = new Message(ID, MessageType.Unbooking, RandomService.MessageSubject(MessageType.Unbooking), RandomService.MessageContents(MessageType.Unbooking));
                    break;
            }
        }

        if (m != null)
        {
            _sentMessages++;
            GameService.Messages.Add(m);
        }
    }

    public static Attendee Empty()
    {
        return new Attendee()
        {
            Name = "",
            Age = 0,
            ID = 0,
            Ticket = TicketClass.Silver,
            Allergies = new List<string>(),
        };
    }

    public void SetID(ushort id)
    {
        ID = id;
    }

    public string GetAllergies()
    {
        if (Allergies.Count == 0)
            return "I am not allergic to anything";

        string allergyText = "I'm allergic to ";
        for (int i = 0; i < Allergies.Count; i++)
        {
            allergyText += Allergies[i];
            if (i < Allergies.Count - 2)
                allergyText += ", ";
            else if (i < Allergies.Count - 1)
                allergyText += " and ";
        }
        return allergyText;
    }

    public void ReceiveMessage(Message m)
    {
        System.Timers.Timer replyTimer = new System.Timers.Timer(RandomService.random.Next(5000, 15000));
        replyTimer.Elapsed += (sender, e) => Reply(sender, e, m);
        replyTimer.AutoReset = false;
        replyTimer.Start();
    }

    private void Reply(object sender, ElapsedEventArgs e, Message m)
    {
        Message reply = new Message(ID, MessageType.UpdateInfo, $"RE: {m.Subject}", "Okay, here is the correct information.\n");

        if (m.requestedInfo.Count > 0)
        {
            foreach (string request in m.requestedInfo)
            {
                switch (request.ToLower())
                {
                    case "name":
                        reply.Contents += $"My name is {Name}. ";
                        break;
                    case "age":
                        reply.Contents += $"I am  {Age} years old. ";
                        break;
                    case "allergies":
                        reply.Contents += $"{GetAllergies()}. ";
                        break;
                }
            }
        }
        GameService.Messages.Add(reply);
    }

    public bool Match(Attendee other)
    {
        if(ID != other.ID)
            return false;
        if(Name != other.Name)
            return false;
        if(Age != other.Age)
            return false;
        if(Ticket != other.Ticket)
            return false;
        if(!Enumerable.SequenceEqual(Allergies.OrderBy(e => e), other.Allergies.OrderBy(e => e)))
            return false;
        return true;
    }
}