using Exercise_2.Models;
using Exercise_2.Services;

internal class Attendee
{
    private int _sentMessages = 0;
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

    public Message SendMessage()
    {
        Message m;

        if (_sentMessages == 0)
            m = new Message(ID, MessageType.Booking, RandomService.MessageSubject(MessageType.Booking), $"{RandomService.MessageGreeting()} {RandomService.MessageName()} {Name}{RandomService.MessageAttendText()}. I'm {Age} years old. I want a {Ticket} ticket. {RandomService.AllergyText(Allergies)}");
        else
            m = new Message(ID, MessageType.UpdateInfo, "Random Subject", "Random contents");

        _sentMessages++;
        return m;
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

    public void ReceiveMessage(Message m)
    {
        Message reply = new Message(ID, MessageType.UpdateInfo, $"RE: {m.Subject}", "Okay, here is the correct information.\n");

        if (m.requestedInfo.Count > 0)
        {
            foreach(string request in m.requestedInfo)
            {
                switch(request.ToLower())
                {
                    case "name":
                        reply.Contents += $"My name is {Name}. ";
                        break;
                    case "age":
                        reply.Contents += $"I am  {Age} years old. ";
                        break;
                    case "allergies":
                        if (Allergies.Count == 0)
                            reply.Contents += $"I am not allergic to anything. ";
                        else
                            reply.Contents += RandomService.AllergyText(Allergies) + " ";
                        break;
                }
            }
        }
        GameService.Messages.Add(Reply(m));
    }

    private Message Reply(Message m)
    {
        return new Message(ID, MessageType.UpdateInfo, $"RE: {m.Subject}", "I received your message, here is my reply.");
    }
}