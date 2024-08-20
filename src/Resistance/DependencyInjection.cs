using Microsoft.AspNetCore.Builder;
using Resistance.Configuration;
using Resistance.Behavior.Latency;
using Resistance.Behavior.NetworkFailure;
using Resistance.Behavior.Outage;
using Resistance.Behavior.ResourceRace;
using Resistance.Behavior.Inconsistency;

namespace Resistance;

public static class DependencyInjection
{
    public static IApplicationBuilder UseResistance(this IApplicationBuilder app, ResistanceOptions options)
    {
        app.UseMiddleware<NetworkFailureBehavior>(options.NetworkFailureProbability);
        app.UseMiddleware<LatencyBehavior>(options.LatencyPeriod);
        app.UseMiddleware<ResourceRaceBehavior>(options.ResourceRaceUpperLimit);
        app.UseMiddleware<OutageBehavior>(options.OutagePeriod);
        app.UseMiddleware<DataInconsistencyBehavior>(options.DataInconsistencyProbability);

        return app;
    }
}
