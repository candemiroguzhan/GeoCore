namespace GeoCore.Infrastructure.Resilience;

public sealed record GeoCoreResilienceOptions(
    int RetryCount,
    TimeSpan RetryDelay,
    TimeSpan Timeout,
    double CircuitBreakerFailureRatio,
    int CircuitBreakerMinimumThroughput,
    TimeSpan CircuitBreakerBreakDuration);
