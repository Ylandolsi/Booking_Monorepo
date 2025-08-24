
using Booking.Common.Domain.Entity;

namespace Booking.Modules.Payments.Domain;

public class Transaction : Entity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int EscrowId { get; private set; }
    public decimal Price { get; private set; }
    public TransactionStatus Status { get; private set; }
    
    private Transaction(){}

    public Transaction(int userId, decimal price, TransactionStatus status ,int escrowId)
    {
        EscrowId = escrowId;
        UserId = userId;
        Price = price;
        Status = status;
    }
}

public enum TransactionStatus
{
    Deduction,
    Refund,
    Release
}