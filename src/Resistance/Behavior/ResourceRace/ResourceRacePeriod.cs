namespace Resistance.Behavior.ResourceRace;

public class ResourceRacePeriod
{
    public TimeSpan DueTime { get; set; } = TimeSpan.FromSeconds(10);
    public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(10);
    public short RequestLimit { get; set; } = 5;
}
