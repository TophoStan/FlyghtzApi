using DataExtractor.Wikipedia;
using Domain;

namespace DataExtractor.Wikipedia;


public class TableScraper : IScraper
{
    private HttpClient _client;

    public TableScraper(HttpClient httpClient)
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

        var tables = GetTablesFromPage(content);
        var rows = GetRowsFromAllTables(tables);
        return rows.Select(ConvertHtmlRowsIntoAirportData).ToList();
    }

    private List<string> GetTablesFromPage(string content)
    {
        var htmlDoc = new HtmlAgilityPack.HtmlDocument();
        htmlDoc.LoadHtml(content);

        var tables = htmlDoc.DocumentNode.SelectNodes("//table");

        var results = new List<string>();

        if (tables == null) return results;
        results.AddRange(tables.Select(table => table.InnerHtml));

        return results;
    }

    private List<string> GetRowsFromAllTables(List<string> tables)
    {
        var AllRows = new List<string>();
        foreach (var table in tables)
        {
            AllRows.AddRange(GetRowsFromTable(table));
        }

        return AllRows;
    }

    private List<string> GetRowsFromTable(string table)
    {
        var htmlDoc = new HtmlAgilityPack.HtmlDocument();
        htmlDoc.LoadHtml(table);
        var rowsHtml = htmlDoc.DocumentNode.SelectNodes("//tr").Skip(1);
        var rows = rowsHtml.Select(row => row.InnerHtml).ToList();

        var rowsWithNoNav = rows.Where(x => !x.Contains("navbox"));


        return rowsWithNoNav.ToList();
    }

    private WikiPediaAirportData ConvertHtmlRowsIntoAirportData(string row)
    {
        var AirportData = new WikiPediaAirportData();
        var htmlDoc = new HtmlAgilityPack.HtmlDocument();
        htmlDoc.LoadHtml(row);

        var cells = htmlDoc.DocumentNode.SelectNodes("//td");
        if (cells == null) return null;

        try
        {
            AirportData.ICAO = cells[0].InnerText;
            AirportData.IATA = cells[1].InnerText is null or "" ? "-" : cells[1].InnerText;
            AirportData.Name = cells[2].InnerText;
        }
        catch (Exception e)
        {
            Console.WriteLine("Row failed to convert with following values: " + row);
            throw;
        }
        

        return AirportData;
    }
}

