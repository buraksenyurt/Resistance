using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resistance.Configuration;
using System.Collections.Concurrent;
using System.Net;

namespace Resistance.Behavior.ResourceRace;
public class ResourceRaceBehavior
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResourceRaceBehavior> _logger;
    private readonly IOptionsMonitor<ResistanceFlags> _optionsMonitor;
    private readonly ConcurrentDictionary<string, int> _requestCounts = new();
    private static Timer? _resetTimer;
    private readonly ResourceRacePeriod _period;

    public ResourceRaceBehavior(RequestDelegate next, IOptionsMonitor<ResistanceFlags> optionsMonitor, ILogger<ResourceRaceBehavior> logger, ResourceRacePeriod period)
    {
        _next = next;
        _logger = logger;
        _optionsMonitor = optionsMonitor;
        _period = period;
        _resetTimer = new Timer(ResetCounts, null, period.DueTime, period.Period);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_optionsMonitor.CurrentValue.ResourceRaceIsActive)
        {
            await _next(context);
            return;
        }

        var key = context.Connection.RemoteIpAddress.ToString();
        _requestCounts.AddOrUpdate(key, 1, (k, v) => v + 1);

        if (_requestCounts[key] > _period.RequestLimit)
        {
            _logger.LogError("Simulated TooManyRequest(HTTP 429)");
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await context.Response.WriteAsync("Simulated resource contention.");
            return;
        }
        await _next(context);
    }

    private void ResetCounts(object? state)
    {
        _requestCounts.Clear();
    }
}
