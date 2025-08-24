/*using Booking.Common.Authentication;
using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Booking.Modules.Mentorships.Features.Payment.Create;

public class CreatePayment : IEndpoint
{
    public record Request(int UsdAmount, int TndAmount, int MentorId, int MenteeId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(MentorshipEndpoints.Payment.Create,
                async (
                    [FromBody] Request request,
                    UserContext userContext,
                    ICommandHandler<CreatePaymentCommand> handler,
                    CancellationToken cancellationToken
                ) =>
                {
                    int userId = userContext.UserId;

                    var command = new CreatePaymentCommand(
                        request.UsdAmount,
                        request.TndAmount,
                        request.MentorId,
                        request.MenteeId);

                    Result result = await handler.Handle(command, cancellationToken);
                })
            .RequireAuthorization();
    }
}*/