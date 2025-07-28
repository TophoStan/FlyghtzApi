namespace FlightsApi.Dto;

public record GetAirportDataResponse
{
    public required IEnumerable<AirportDataDto> AirportData { get; init; }
};