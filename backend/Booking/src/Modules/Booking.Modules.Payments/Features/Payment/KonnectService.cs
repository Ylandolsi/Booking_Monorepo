using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Options;
using Microsoft.Extensions.Options;

namespace Booking.Modules.Mentorships.Features.Payment;

public static class PaymentErrors
{
    public static Error FailedToCreatePayment(int amount, string firstName, string lastName) =>
        Error.Failure("Create.Payment.Failed",
            $"Konnect failed to create payment with amount {amount} for {firstName} {lastName}");

    public static Error FailedToFetchPaymentDetails(string paymentRef) =>
        Error.Failure("Fetch.PaymentDetails.Failed",
            $"Konnect failed to fetch paymentDetails with ref  {paymentRef}");
}

public class KonnectService(
    IHttpClientFactory httpClientFactory,
    IOptions<KonnectOptions> options)
{
    private readonly KonnectOptions KonnectOptions = options.Value;

    public record PaymentResponse(string PaymentRef, string PayUrl);

    public record PaymentInfo
    {
        public string Id { get; init; } = null!;
        public string Status { get; init; } = null!;
        public int AmountDue { get; init; }
        public int ReachedAmount { get; init; }
        public int Amount { get; init; }
        public string Token { get; init; } = null!;
        public int ConvertedAmount { get; init; }
        public int ExchangeRate { get; init; }
        public string ExpirationDate { get; init; } = null!;
        public string ShortId { get; init; } = null!;
        public string Link { get; init; } = null!;
        public string Webhook { get; init; } = null!;
        public string SuccessUrl { get; init; } = null!;
        public string FailUrl { get; init; } = null!;
        public string OrderId { get; init; } = null!;
        public string Type { get; init; } = null!;
        public string Details { get; init; } = null!;
        public string[] AcceptedPaymentMethods { get; init; } = null!;
        public WalletInfo ReceiverWallet { get; init; } = null!;
        public TransactionInfo[] Transactions { get; init; } = null!;
    }

    public record WalletInfo
    {
    }

    public record TransactionInfo
    {
        public string Status { get; init; } = null!;
    }

    public async Task<Result<PaymentResponse>> CreatePayment(int amount, int paymentId, string firstName,
        string lastName, string email,
        string phone)
    {
        var httpClient = httpClientFactory.CreateClient("KonnectClient");
        var paymentInfo = new
        {
            receiverWalletId = KonnectOptions.WalletKey,
            token = "USD",
            amount = amount,
            type = "immediate",
            description = "Pay for session ",
            acceptedPaymentMethods = new[] { "wallet", "bank_card", "e-DINAR" },
            lifespan = KonnectOptions.PaymentLifespan,
            checkoutForm = false,
            addPaymentFeesToAmount = true,
            firstName = firstName,
            lastName = lastName,
            phoneNumber = phone,
            email = email,
            orderId = paymentId, // 
            webhook = KonnectOptions.Webhook,
            silentWebhook = true,
            successUrl = KonnectOptions.SuccessUrl,
            failUrl = KonnectOptions.FailureUrl,
            theme = "light" // theme= req.body.theme ? req.body.theme : "light",
        };

        var response = await httpClient.PostAsJsonAsync(
            $"{KonnectOptions.ApiUrl}/payments/init-payment",
            paymentInfo,
            new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token
        );


        if (response.IsSuccessStatusCode)
        {
            var responseDate = await response.Content.ReadFromJsonAsync<PaymentResponse>();
            return Result.Success(responseDate!);
        }

        return Result.Failure<PaymentResponse>(PaymentErrors.FailedToCreatePayment(amount, firstName, lastName));


        /*
         const response = await axios.post(`${process.env.KONNECT_API_URL}/payments/init-payment`, paymentInfo, {
            headers: {
                "x-api-key": process.env.KONNECT_API_KEY,
            },
            timeout: 10000, // Timeout in milliseconds (e.g., 10000ms = 10 seconds)
        });

        if (response.data.success || response.data.payUrl) {
            payment.paymentRef = response.data.paymentRef;
            payment.paymentUrl = response.data.payUrl;
            await payment.save();
            return res.status(201).json({ success: true, data: response.data.payUrl });
        } else {
            // remove the payment
            await Payment.findByIdAndDelete(paymentId);
            return res.status(400).json({ success: false, message: response.data });
        }
        */
    }

    public async Task<Result<PaymentInfo>> GetPaymentDetails(string paymentRef)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("KonnectClient");
            var response = await httpClient.GetAsync($"payments/{paymentRef}");

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var paymentDetails = await response.Content.ReadFromJsonAsync<PaymentInfo>();
                    if (paymentDetails == null)
                    {
                        return Result.Failure<PaymentInfo>(PaymentErrors.FailedToFetchPaymentDetails(paymentRef));
                    }

                    if (IsPaymentExpired(paymentDetails))
                    {
                        return Result.Failure<PaymentInfo>(Error.Failure(
                            "Payment.Expired",
                            $"Payment with ref {paymentRef} has expired (Expiration: {paymentDetails.ExpirationDate})"));
                    }

                    if (paymentDetails.Status != "completed")
                    {
                        return Result.Failure<PaymentInfo>(Error.Failure(
                            "Payment.NotCompleted",
                            $"Payment with ref {paymentRef} is not completed (Status: {paymentDetails.Status})"));
                    }

                    if (paymentDetails.ReachedAmount < paymentDetails.AmountDue)
                    {
                        return Result.Failure<PaymentInfo>(Error.Failure(
                            "Payment.Partial",
                            $"Payment with ref {paymentRef} is partially paid (Reached: {paymentDetails.ReachedAmount}, Due: {paymentDetails.AmountDue})"));
                    }

                    return Result.Success(paymentDetails);

                case HttpStatusCode.NotFound:
                    return Result.Failure<PaymentInfo>(Error.NotFound(
                        "Payment.NotFound",
                        $"Payment with ref {paymentRef} not found"));

                case HttpStatusCode.Unauthorized:
                    return Result.Failure<PaymentInfo>(Error.Unauthorized(
                        "Payment.Unauthorized",
                        "Invalid or missing API key"));

                case HttpStatusCode.TooManyRequests:
                    return Result.Failure<PaymentInfo>(Error.Failure(
                        "Payment.RateLimit",
                        "Rate limit exceeded for payment API"));

                default:
                    return Result.Failure<PaymentInfo>(PaymentErrors.FailedToFetchPaymentDetails(paymentRef));
            }
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<PaymentInfo>(Error.Failure(
                "Payment.NetworkError",
                $"Network error while fetching payment details for ref {paymentRef}: {ex.Message}"));
        }
        catch (JsonException ex)
        {
            return Result.Failure<PaymentInfo>(Error.Failure(
                "Payment.JsonError",
                $"Failed to deserialize payment details for ref {paymentRef}: {ex.Message}"));
        }
    }

    private bool IsPaymentExpired(PaymentInfo paymentDetails)
    {
        if (DateTime.TryParse(paymentDetails.ExpirationDate, out var expirationDate))
        {
            return expirationDate < DateTime.UtcNow;
        }

        return false; // Assume not expired if date parsing fails
    }
}