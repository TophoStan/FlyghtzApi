using Domain;

namespace FlightsApi.Dto;

public static class GetAirportDataExtension
{
    public static AirportDataDto ToDto(this AirportData data)
    {
        return new AirportDataDto
        {
            Name = data.Name,
            ICAO = data.ICAO,
            IATA = data.IATA,
            City = data.City,
            Latitude = data.Latitude,
            Longitude = data.Longitude
        };
    }
}