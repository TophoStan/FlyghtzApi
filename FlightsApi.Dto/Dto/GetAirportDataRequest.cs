namespace FlightsApi.Dto;

public record GetAirportDataRequest
{
    public required bool WillScrapeWikipedia { get; set; }
};