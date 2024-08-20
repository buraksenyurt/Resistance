Console.WriteLine("Press any key to start test");
Console.ReadLine();
Console.WriteLine("Running...");
Console.WriteLine("To stop CTRL + C");

using HttpClient client = new() { BaseAddress = new Uri("http://localhost:5225/") };

while (true)
{
    var response = await client.GetAsync("getSalary");
    Console.WriteLine($"{response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
    Task.Delay(500).Wait();
}