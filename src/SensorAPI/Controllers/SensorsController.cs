using Microsoft.AspNetCore.Mvc;
using SensorAPI.Models;

namespace SensorAPI.Controllers;

[ApiController]
[Route("sensors")]
public class SensorsController : ControllerBase
{
    private static readonly List<Sensor> sensors = new();
    private static readonly List<SensorReading> readings = new();
    private static readonly object sync = new();

    [HttpPost]
    public IActionResult CreateSensor(Sensor sensor)
    {
        lock (sync)
        {
            sensor.Id = sensors.Count + 1;
            sensors.Add(sensor);
            return Ok(sensor);
        }
    }

    [HttpGet]
    public IActionResult GetSensors()
    {
        lock (sync)
        {
            return Ok(sensors.ToList());
        }
    }

    [HttpPost("{id}/readings")]
    public IActionResult AddReading(int id, SensorReading reading)
    {
        lock (sync)
        {
            var sensor = sensors.Find(s => s.Id == id);

            if (sensor == null)
                return NotFound("Khong tim thay cam bien");

            reading.Id = readings.Count + 1;
            reading.SensorId = id;
            reading.Timestamp = DateTime.UtcNow;
            reading.IsAlert = reading.Value > sensor.Threshold;

            readings.Add(reading);

            return Ok(reading);
        }
    }

    [HttpGet("{id}/readings/latest")]
    public IActionResult GetLatest(int id)
    {
        lock (sync)
        {
            var reading = readings
                .Where(r => r.SensorId == id)
                .OrderByDescending(r => r.Timestamp)
                .FirstOrDefault();

            if (reading == null)
                return NotFound();

            return Ok(reading);
        }
    }

    [HttpGet("{id}/readings")]
    public IActionResult GetReadings(
        int id, DateTime? from, DateTime? to)
    {
        lock (sync)
        {
            if (!sensors.Any(s => s.Id == id))
                return NotFound();

            var result = readings
                .Where(r => r.SensorId == id
                    && (!from.HasValue || r.Timestamp >= from.Value)
                    && (!to.HasValue || r.Timestamp <= to.Value))
                .OrderByDescending(r => r.Timestamp)
                .ToList();

            return Ok(result);
        }
    }
}