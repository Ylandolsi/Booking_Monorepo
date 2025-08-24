using Booking.Common.Domain.DomainEvent;
using Booking.Modules.Mentorships.Domain.Entities.Payments;
using Booking.Modules.Mentorships.Features.GoogleCalendar;
using Booking.Modules.Mentorships.Persistence;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payment;

public class PaymentCompletedDomainEventHandler(
    MentorshipsDbContext context,
    IBackgroundJobClient backgroundJobClient,
    GoogleCalendarService googleCalendarService,
    ILogger<PaymentCompletedDomainEventHandler> logger) : IDomainEventHandler<PaymentCompletedDomainEvent>
{
    // create meeting link 
    // send to escrow 
    // send link to mentor and mentee for confirmation 
    public Task Handle(PaymentCompletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling PaymentCompletedDomainEvent for menteeId:{mentee} , mentorId:{mentor} ,sessionId:{session} with price : {Price} ",
            domainEvent.MenteeId,
            domainEvent.MentorId,
            domainEvent.SessionId,
            domainEvent.Price);

        throw new NotImplementedException();


        /*backgroundJobClient.Enqueue<SendingPasswordResetToken>(
            job => job.SendAsync(command.Email, resetUrl, null));*/
    }
}