/*using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payment.Create;

public class CreatePaymentCommandHandler(
    KonnectService konnectService,
    MentorshipsDbContext context,
    ILogger<CreatePaymentCommandHandler> logger) : ICommandHandler<CreatePaymentCommand>
{
    // TODO : transaction
    public async Task<Result> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Create Payment endpoint triggered for mentee with {id} to mentro with {mentorId} ( UsdPrice = {Price} ) ",
            command.MenteeId, command.MentorId, command.UsdAmount);
        
        Transaction transaction = new 
    }
}*/