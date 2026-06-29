using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace GeoCore.Infrastructure.Resilience;

public sealed class GeoCoreResiliencePipelineFactory
{
    public ResiliencePipeline CreateExternalGeospatialApiPipeline()
    {
        return Create(new GeoCoreResilienceOptions(3, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(20), 0.5, 10, TimeSpan.FromSeconds(30)));
    }

    public ResiliencePipeline CreateOtbCliPipeline()
    {
        return Create(new GeoCoreResilienceOptions(2, TimeSpan.FromSeconds(3), TimeSpan.FromMinutes(10), 0.7, 5, TimeSpan.FromMinutes(1)));
    }

    public ResiliencePipeline CreateRasterProcessingPipeline()
    {
        return Create(new GeoCoreResilienceOptions(2, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30), 0.7, 5, TimeSpan.FromMinutes(2)));
    }

    public ResiliencePipeline CreateRemoteServicePipeline()
    {
        return Create(new GeoCoreResilienceOptions(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), 0.5, 10, TimeSpan.FromSeconds(30)));
    }

    private static ResiliencePipeline Create(GeoCoreResilienceOptions options)
    {
        return new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = options.RetryCount,
                Delay = options.RetryDelay,
                BackoffType = DelayBackoffType.Exponential,
                ShouldHandle = new PredicateBuilder().Handle<Exception>()
            })
            .AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = options.Timeout
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = options.CircuitBreakerFailureRatio,
                MinimumThroughput = options.CircuitBreakerMinimumThroughput,
                BreakDuration = options.CircuitBreakerBreakDuration,
                ShouldHandle = new PredicateBuilder().Handle<Exception>()
            })
            .Build();
    }
}
