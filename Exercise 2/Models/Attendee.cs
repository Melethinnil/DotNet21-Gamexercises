using Exercise_2.Models;
using Exercise_2.Services;

internal class Attendee
{
    private int _sentMessages = 0;
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public int Age { get; private set; }
    public ushort ID { get; private set; }
    public TicketClass Ticket { get; private set; }
    public string DiscountCode { get; private set; }
    public string SpecialNotes { get; private set; }

    public Attendee()
    {
        FirstName = RandomService.FirstName();
        LastName = RandomService.LastName();
        Email = RandomService.Email(FirstName, LastName);
        Age = RandomService.Age();
        ID = RandomService.ID();
        Ticket = RandomService.Ticket();
        SpecialNotes = RandomService.SpecialNotes();
        DiscountCode = "";
    }

    public void AddDiscountCode(string code)
    {
        DiscountCode = code;
    }

    public Message SendMessage()
    {
        Message m;

        if (_sentMessages == 0)
            m = new Message(ID, MessageType.Booking, RandomService.MessageSubject(MessageType.Booking), $"{RandomService.MessageGreeting()} {RandomService.MessageName()} {FirstName} {LastName}{RandomService.MessageAttendText()}. I'm {Age} years old. I want a {Ticket} ticket. {SpecialNotes}");
        else
            m = new Message(ID, MessageType.WrongInfo, "Random Subject", "Random contents");

        _sentMessages++;
        return m;
    }
}