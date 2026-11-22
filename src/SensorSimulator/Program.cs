using System.Text.Json;

var sensorId = Environment.GetEnvironmentVariable("SENSOR_ID") ?? "sensor-01";
var intervalMs = int.Parse(Environment.GetEnvironmentVariable("INTERVAL_MS") ?? "2000");

var rnd = new Random();
double temperature = 28.0;
double humidity = 65.0;

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

Console.WriteLine($"[{DateTime.UtcNow:O}] [INFO] {sensorId} started, interval {intervalMs} ms");

while (!cts.IsCancellationRequested)
{
    // dao dong nhe quanh gia tri hien tai de du lieu trong giong that
    temperature = Math.Round(temperature + (rnd.NextDouble() - 0.5), 2);
    humidity = Math.Round(Math.Clamp(humidity + (rnd.NextDouble() - 0.5) * 2, 30, 95), 2);

    var reading = new
    {
        sensorId,
        temperature,
        humidity,
        timestamp = DateTime.UtcNow
    };

    Console.WriteLine($"[{DateTime.UtcNow:O}] [INFO] {JsonSerializer.Serialize(reading)}");

    try { await Task.Delay(intervalMs, cts.Token); }
    catch (TaskCanceledException) { break; }
}

Console.WriteLine($"[{DateTime.UtcNow:O}] [INFO] {sensorId} stopped");