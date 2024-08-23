# Resistance

It is a library containing functions that create a chaotic environment for durability tests in web app environments.

In distributed systems, it is important that the whole is durable. The functions in this package can be added to an Asp.Net Runtime Middleware line. The following situations can be simulated in server or service environments.

## Simulation Scenarios

- Latency: Generating delays in service response times. For example delaying the response by 500 to 2500 milliseconds.
- Resource Race: Simulating receiving too many requests.
- Outage: Providing service interruption for certain periods of time. For example, a service outage of ten seconds in every minute.
- Network Failure: It ensures that HTTP 500 is returned based on a certain percentage of service calls.
- Data Inconsistency: Data corruption on the response body. For example, adding text-based information to the body of every second response.

## Usage

By default, all simulation behaviors are disabled. These values ​​can be enabled via appSettings. For more detailed settings, the Middleware function is used.

```json
{
  "ResistanceFlags": {
    "NetworkFailureIsActive": true,
    "LatencyIsActive": false,
    "ResourceRaceIsActive": true,
    "OutageIsActive": false,
    "DataInconsistencyIsActive": false
  }
}
```

Code example;

```csharp
// For appSettings based options
builder.Services.Configure<ResistanceFlags>(builder.Configuration.GetSection("ResistanceFlags"));

app.UseResistance(new ResistanceOptions
{
    // Network Failure (HTTP 500 Internal Service Error with %25 probility)
    NetworkFailureProbability = NetworkFailureProbability.Percent25,
    // For 5 requests coming from the same IP every 10 seconds, the HTTP 429 Too Many Requests scenario is generated.
    ResourceRacePeriod = new ResourceRacePeriod
    {
        DueTime = TimeSpan.FromSeconds(5),
        Period = TimeSpan.FromSeconds(5),
        RequestLimit = 5
    },
    // Manipulating response data with %50 probability
    DataInconsistencyProbability = DataInconsistencyProbability.Percent20,
    // Produce HTTP 503 Service Unavailable 10 seconds per minute
    OutagePeriod = new OutagePeriod
    {
        Duration = TimeSpan.FromSeconds(10),
        Frequency = TimeSpan.FromMinutes(1)
    },
    // Latency 500 millisecdons - 2500 milliseconds
    LatencyPeriod = new LatencyPeriod
    {
        MinDelayMs = TimeSpan.FromMilliseconds(500),
        MaxDelayMs = TimeSpan.FromMilliseconds(2500)
    }
});
```
