using System.Collections.Concurrent;

namespace BookServer.Extensions;

public class RateLimitingMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private static readonly ConcurrentDictionary<string,
        (int currentRequests, DateTime lastTime)> RequestTimes = new();

    private readonly TimeSpan _timeSpan = TimeSpan.FromSeconds(
        int.Parse(configuration["IpRateLimit:PeriodInSeconds"] ??
        throw new NullReferenceException("IpRateLimit:PeriodInSeconds not found")));

    private readonly int _maxRequests = int.Parse(configuration["IpRateLimit:Limit"] ??
        throw new NullReferenceException("IpRateLimit:Limit not found"));

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        if (clientIp == null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Unable to determine client IP.");
            return;
        }


        if (!RequestTimes.TryGetValue(clientIp, out (int currentRequests, DateTime lastTime) value))
        {
            RequestTimes[clientIp] = (1, DateTime.UtcNow.Add(_timeSpan));
        }
        else
        {
            if (value.lastTime > DateTime.UtcNow)
            {
                RequestTimes[clientIp] = (value.currentRequests + 1, value.lastTime);
                if (RequestTimes[clientIp].currentRequests > _maxRequests)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Too many requests. Please try again later.");
                    return;
                }
            }
            else
            {
                RequestTimes[clientIp] = (1, DateTime.UtcNow.Add(_timeSpan));
            }
        }

        await next(context);
    }
}
