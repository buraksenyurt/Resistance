using Resistance.Behavior.Inconsistency;
using Resistance.Behavior.Latency;
using Resistance.Behavior.NetworkFailure;
using Resistance.Behavior.Outage;
using Resistance.Behavior.ResourceRace;

namespace Resistance.Configuration;

public class ResistanceOptions
{
    public NetworkFailureProbability NetworkFailureProbability { get; set; } = NetworkFailureProbability.Percent10;
    public LatencyPeriod LatencyPeriod { get; set; } = new LatencyPeriod();
    public ResourceRacePeriod ResourceRacePeriod { get; set; } = new ResourceRacePeriod();
    public OutagePeriod OutagePeriod { get; set; } = new OutagePeriod();
    public DataInconsistencyProbability DataInconsistencyProbability { get; set; } = DataInconsistencyProbability.Percent20;
}