using System.Text.Json.Serialization;

namespace FlightsApi.Dto;

public class AirportDataDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("icao")]
    public required  string ICAO { get; set; }

    [JsonPropertyName("iata")]
    public required  string IATA { get; set; }

    [JsonPropertyName("city")]
    public required  string City { get; set; }

    [JsonPropertyName("latitude")]
    public required  double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public required  double Longitude { get; set; }
}
