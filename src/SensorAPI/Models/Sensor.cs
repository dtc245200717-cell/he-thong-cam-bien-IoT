namespace SensorAPI.Models;

public class Sensor
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public double Threshold { get; set; }
}