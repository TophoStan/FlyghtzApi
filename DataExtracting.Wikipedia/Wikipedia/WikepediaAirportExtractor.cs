using System.Runtime.InteropServices.JavaScript;
using DataExtractor.Wikipedia;
using Domain;
using HtmlAgilityPack;
using Spire.Xls;

namespace DataExtractor.Wikipedia;

public class WikepediaAirportExtractor
{
    private TableScraper _tableScraper;
    private ListScraper _listScraper;

    public WikepediaAirportExtractor(TableScraper tableScraper, ListScraper listScraper)
    {
        _tableScraper = tableScraper;
        _listScraper = listScraper;
    }


    public async Task<List<WikiPediaAirportData>> Execute(bool onlyAirportsWithIata = true)
    {
        var fileName = $"{DateTime.Now:yyyy-MM-dd} airports.xlsx";
        var solutionRoot = Environment.CurrentDirectory;
        var filePath = Path.Combine(solutionRoot, fileName);

        // Check if the Excel file already exists
        if (File.Exists(filePath))
        {
            // Read the Excel file or return, as needed
            return await ReadExistingExcel(fileName);
        }

        // Add dynamic detection of page type
        var pagesToScrapeWithTables =
            new[]
            {
                "A", "B", "C", "D", "E", "V"
            };

        var pagesToScrapeWithLists = new[]
        {
            "F", "G", "H",
            "K", "L", "M",
            "N", "O", "P",
            "R", "S", "T",
            "U", "W", "Y", "Z"
        };

        var airportList = new List<WikiPediaAirportData>();
        foreach (var letter in pagesToScrapeWithTables)
        {
            airportList.AddRange(await _tableScraper.Scrape(letter));
        }

        foreach (var letter in pagesToScrapeWithLists)
        {
            airportList.AddRange(await _listScraper.Scrape(letter));
        }


        WriteToExcel(airportList,
            filename: fileName);
        if (onlyAirportsWithIata)
        {
            return airportList.Where(x => x.IATA is not "-").ToList();
        }

        return airportList;
    }


    private Task<List<WikiPediaAirportData>> ReadExistingExcel(string fileName)
    {
        return Task.Run(() =>
        {
            var airportList = new List<WikiPediaAirportData>();
            var workbook = new Workbook();
            workbook.LoadFromFile(fileName);

            var sheet = workbook.Worksheets.FirstOrDefault(s => s.Name == "Airports");
            if (sheet == null)
                return airportList;

            int totalRows = sheet.Rows.Length;

            // Define the range from row 2 to the last row, columns 1 to 3
            var range = sheet.Range[2, 1, totalRows, 3];

            for (int i = 0; i < range.Count; i++)
            {
                var name = range[i+1, 1]?.Text?.Trim();
                var icao = range[i+1, 2]?.Text?.Trim();
                var iata = range[i+1, 3]?.Text?.Trim();

                if (string.IsNullOrWhiteSpace(name) &&
                    string.IsNullOrWhiteSpace(icao) &&
                    string.IsNullOrWhiteSpace(iata))
                    continue;

                airportList.Add(new WikiPediaAirportData
                {
                    Name = name ?? "",
                    ICAO = icao ?? "",
                    IATA = iata ?? ""
                });
            }

            return airportList;
        });
    }



    private string WriteToExcel(List<WikiPediaAirportData> airports, string filename)
    {
        var workbook = new Workbook();
        var sheet = workbook.Worksheets.Add("Airports");

        // Write headers
        sheet.Range[1, 1].Text = "Name";
        sheet.Range[1, 2].Text = "ICAO";
        sheet.Range[1, 3].Text = "IATA";

        // Write airport data starting from row 2
        for (int i = 0; i < airports.Count; i++)
        {
            var airport = airports[i];
            int row = i + 2;

            sheet.Range[row, 1].Text = airport.Name;
            sheet.Range[row, 2].Text = airport.ICAO;
            sheet.Range[row, 3].Text = airport.IATA;
        }

        sheet.AllocatedRange.AutoFitColumns();

        // Save the file
        workbook.SaveToFile(filename, ExcelVersion.Version2016);

        return filename;
    }
}