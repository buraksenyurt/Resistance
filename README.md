# Resistance

It is a library containing functions that create a chaotic environment for durability tests in web app environments.

In distributed systems, it is important that the whole is durable. The functions in this package can be added to an Asp.Net Runtime Middleware line. The following situations can be simulated in server or service environments.

## Simulation Scenarios

- Latency: Generating delays in service response times. For example delaying the response by 500 to 2500 milliseconds.
- Resource Race: Simulating receiving too many requests.
- Outage: Providing service interruption for certain periods of time. For example, a service outage of ten seconds in every minute.
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

## Test App

Resistance.Test Api application can be used to test Nuget package. After referencing the package from Nuget repo, tests can be performed with Swagger interface or Postman, simple Curl command or Resistance.TestClient console application(Suggested).

```bash
dotnet run

# The port address may differ
curl -X 'GET' \
  'http://localhost:5225/getSalary' \
  -H 'accept: */*'
```

For example, when the Network Failure behavior is enabled, one of the expected results should be as follows.

![Postman Sample](https://github.com/user-attachments/assets/d03f2f1b-1f7a-4c6e-9e74-2af4be0b88b8)

The tests performed with the Console application are as follows.

### Outage Test

The service is down for 10 seconds every minute.

![Outage sample](https://github.com/user-attachments/assets/14758868-68d4-4f3d-81e3-76a4c91a1475)

### Network Failure Test

HTTP 500 Internal Service Error error is returned with a 25% probability.

![Network Failure Sample](https://github.com/user-attachments/assets/dc31002c-7bb4-4034-aaec-84aec943ab0e)

### Latency Test

There will be a delay in service response times.

![Latency Sample](https://github.com/user-attachments/assets/bb75d6dd-9810-47c6-9358-aa704c579f95)

### Data Inconsistency Test

Service response data is manipulated with a certain probability value.

![Data Inconsistency Sample](https://github.com/user-attachments/assets/377163ae-e416-4c4e-add6-f044762524e7)

### Resource Race Test

The service returns HTTP code 429, indicating that it has received too many requests. For 5 requests coming from the same IP every 10 seconds, the HTTP 429 Too Many Requests scenario is generated.

![Resource Race Sample](https://github.com/user-attachments/assets/8e0a271a-44b3-4745-98ad-8ade9dfac5d8)
