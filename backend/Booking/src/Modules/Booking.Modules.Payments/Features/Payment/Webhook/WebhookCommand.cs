
using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Payment;

public record WebhookCommand (string PaymentRef) : ICommand ;