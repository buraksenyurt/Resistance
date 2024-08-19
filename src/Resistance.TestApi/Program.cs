var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/getSalary", () =>
{
    var response = "Current Salary is 1903 Coins";
    return Results.Json(response);
})
.WithName("GetCustomerSalary")
.WithOpenApi();

app.Run();
