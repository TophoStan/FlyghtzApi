using System.Text.RegularExpressions;

namespace DataExtractor.Wikipedia;

public class ListScraper : IScraper
{
    private HttpClient _client;


    public ListScraper(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task<List<WikiPediaAirportData>> Scrape(string letter)
    {
        var wikipediaBase = $"https://en.wikipedia.org/wiki/List_of_airports_by_ICAO_code:_{letter}";

        // HTTP Get to
        var response = await _client.GetAsync(wikipediaBase);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var bulletpoints = GetBulletPointsFromPage(content).Where(x => !x.Contains("ICAO"));


        return bulletpoints.Select(ConvertBulletPointToAirportData).ToList();
    }

    private List<string> GetBulletPointsFromPage(string page)
    {
        var htmlDoc = new HtmlAgilityPack.HtmlDocument();
        htmlDoc.LoadHtml(page);

        var list = htmlDoc.DocumentNode.SelectNodes("//li");
        var listItems = list.Select(n => n.InnerText.Trim()).ToList();

        // The first four letters should be capital letters and should contain a space after the fourth for example "ABCD "
        var filtered = listItems.Where(x => Regex.IsMatch(x, @"^[A-Z]{4}\s")).ToList();

        return filtered;
    }

    public WikiPediaAirportData ConvertBulletPointToAirportData(string bulletPoint)
    {
        
        var airportData = new WikiPediaAirportData();
        try
        {
            
            // Decode HTML entities like &nbsp;
            // Split using regex to handle hyphen or em dash, with optional whitespace
            bulletPoint = bulletPoint
                .Replace('\u00A0', ' ')   // non-breaking space → space
                .Replace('\u2014', '-')   // em dash → hyphen
                .Replace('\u2013', '-');  // en dash → hyphen (just in case)
            var parts = bulletPoint.Split("-");
            if (parts.Length < 2)
                throw new FormatException("Unexpected format: " + bulletPoint);

            // Assign name (can be at index 1 or later depending on structure)
            airportData.Name = parts[1].Trim();

            // Extract ICAO and IATA
            var codePart = parts[0].Trim(); // e.g., "FAAB (ALJ)"
            var match = Regex.Match(codePart, @"^(?<icao>[A-Z]{4})(?:\s*\((?<iata>[A-Z]{3})\))?");

            if (match.Success)
            {
                airportData.ICAO = match.Groups["icao"].Value.Trim();
                airportData.IATA = match.Groups["iata"].Success ? match.Groups["iata"].Value.Trim() : "-";
            }
            else
            {
                throw new FormatException("Could not extract ICAO/IATA from: " + codePart);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Bulletpoint failed to convert: " + bulletPoint);
            airportData.Name = bulletPoint;
            airportData.ICAO = "-";
            airportData.IATA = "-";
        }

        return airportData;
    }
}