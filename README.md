# Resistance

It is a library containing functions that create a chaotic environment for durability tests in web app environments.

In distributed systems, it is important that the whole is durable. The functions in this package can be added to an Asp.Net Runtime Middleware line. The following situations can be simulated in server or service environments.

## Simulation Scenarios

- Latency: Generating HTTP 500 statuses in service calls. For example, ensuring that a service call returns HTTP 500 with a 25% probability.
- Resource Race: Simulating receiving too many requests.
- Outage: Providing service interruption for certain periods of time. For example, a service outage of ten seconds per minute.
- Network Failure: It ensures that HTTP 500 is returned based on a certain percentage of service calls. For example, service response times are delayed randomly by 500 to 2500 milliseconds.
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
    // Produce HTTP 429 Too Many Request scenario with 3 concurrent request
    ResourceRaceUpperLimit = 3,
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

## Test App

Resistance.Test Api application can be used to test Nuget package. After referencing the package from Nuget repo, tests can be performed with Swagger interface or Postman or simple Curl command.

```bash
dotnet run

# The port address may differ
curl -X 'GET' \
  'http://localhost:5225/getSalary' \
  -H 'accept: */*'
```
