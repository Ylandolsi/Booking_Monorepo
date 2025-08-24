using Booking.Modules.Mentorships.Domain.Entities.Sessions;

namespace Booking.Modules.Mentorships.Domain.Entities;

public class Dispute
{
    public int Id { get; private set; }
    public int SessionId { get; private set; } 
    public RaisedBy RaisedBy { get; private set; }
    public string Reason { get; private set; } = string.Empty; 
    public DisputeStatus Status { get; private set; }

    public Dispute(int sessionId, RaisedBy raisedBy, string reason)
    {
        SessionId = sessionId;
        RaisedBy = raisedBy;
        Reason = reason ?? "";
        Status = DisputeStatus.Pending; 
    }

    public static Dispute MentorRaiseDispute( int sessionId , string reason)
    {
        return new Dispute(sessionId, RaisedBy.Mentor, reason ?? "");
    }
    public static Dispute MenteeRaiseDispute( int sessionId , string reason)
    {
        return new Dispute(sessionId, RaisedBy.Mentee, reason ?? "");
    }

    public void Resolve()
    {
        Status = DisputeStatus.Resolved;
    }
    
    public Session Session { get; private set;  } =default!;
}


public enum DisputeStatus
{
    Resolved,
    Pending
}

public enum RaisedBy
{
    Mentor,
    Mentee
}