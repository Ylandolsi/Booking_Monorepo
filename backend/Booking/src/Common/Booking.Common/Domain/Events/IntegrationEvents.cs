using System.ComponentModel.DataAnnotations;

namespace Booking.Common.Domain.Events;

public class IntegrationEvent
{
    public long Id { get; set; }
        
    [Required]
    [MaxLength(100)]
    public string EventType { get; set; }
        
    [Required]
    public string EventData { get; set; }
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    public DateTime? ProcessedAt { get; set; }
        
    public DateTime? ScheduledAt { get; set; }
        
    public int RetryCount { get; set; } = 0;
        
    public int MaxRetries { get; set; } = 5; // Increased for exponential backoff
        
    [MaxLength(20)]
    public string Status { get; set; } = "pending";
        
    public string? ErrorMessage { get; set; }
        
    [MaxLength(100)]
    public string? CorrelationId { get; set; }
        
    // Hangfire job tracking
    [MaxLength(100)]
    public string? HangfireJobId { get; set; }
        
    // Retry delay for exponential backoff
    public TimeSpan? NextRetryDelay { get; set; }
}