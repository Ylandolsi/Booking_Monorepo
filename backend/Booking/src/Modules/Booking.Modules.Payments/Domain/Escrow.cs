using Booking.Common.Domain.Entity;
using Booking.Modules.Mentorships.Domain.Entities.Sessions;

namespace Booking.Modules.Mentorships.Domain.Entities;

public class Escrow : Entity
{
    public int Id { get; private set; }
    
    public decimal Price { get; private set; }
    public EscrowState State { get; private set; }
    
    public int SessionId { get; private set; }
    public Session Session { get; private set; }
    
    public Escrow(decimal price, int sessionId)
    {
        Price = price;
        SessionId = sessionId;
    }

    
    public void SetRefunded()
    {
        State = EscrowState.Refunded;
    }

    public void Realese()
    {
        State = EscrowState.Released; 
    }
}

public enum EscrowState
{
    Held , 
    Released ,
    Refunded , 
}
