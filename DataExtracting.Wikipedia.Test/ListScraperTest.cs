using DataExtractor;
using DataExtractor.Wikipedia;
using Domain;

namespace DataExtracting.Test;

public class ListScraperTest
{
    [Fact]
    public void WhenConvertBulletPointToAirportDataRecievesInputGetAirportData()
    {
        var testInput = "FAAB (ALJ) – Kortdoorn Airport – Alexander Bay";

        var listScraper = new ListScraper(new HttpClient());
        
        var result = listScraper.ConvertBulletPointToAirportData(testInput);
        
        var airportData = new WikiPediaAirportData()
        {
            IATA = "ALJ",
            ICAO = "FAAB",
            Name = "Kortdoorn Airport",
        };
        
        Assert.Equal(airportData, result);
        
        
    }
}