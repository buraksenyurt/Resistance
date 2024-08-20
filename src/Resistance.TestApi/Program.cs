using Resistance;
using Resistance.Behavior.Inconsistency;
using Resistance.Behavior.Latency;
using Resistance.Behavior.NetworkFailure;
using Resistance.Behavior.Outage;
using Resistance.Behavior.ResourceRace;
using Resistance.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ResistanceFlags>(builder.Configuration.GetSection("ResistanceFlags"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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

app.MapGet("/getSalary", () =>
{
    var response = "Current Salary is 1903 Coins";
    return Results.Json(response);
})
.WithName("GetCustomerSalary")
.WithOpenApi();

app.Run();
