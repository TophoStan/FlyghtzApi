using DataExtractor.OurAirports;
using DataExtractor.Wikipedia;
using Domain;

namespace DataExtractor.Extensions;

public static class AirportDataExtension
{
    public static List<AirportData> AddProperties(this List<WikiPediaAirportData> wikiData, List<OurAirportRow> rowData)
    {
        return wikiData
            .Where(w => !string.IsNullOrWhiteSpace(w.ICAO)) 
            .Select(wiki =>
            {
                var row = rowData.FirstOrDefault(r => r.Icao_Code == wiki.ICAO);

                if (row == null) return null;

                try
                {
                    return new AirportData
                    {
                        Name = wiki.Name,
                        IATA = wiki.IATA,
                        ICAO = wiki.ICAO,
                        City = row.Municipality ?? "-",
                        Longitude = row.Longitude_Deg,
                        Latitude = row.Latitude_Deg
                    };
                }
                catch (Exception)
                {
                    Console.WriteLine(
                        $"Failed for row: {wiki.ICAO} - {wiki.Name} - {wiki.IATA}");
                    throw;
                }
            })
            .Where(a => a != null && a.City != "-" && !string.IsNullOrWhiteSpace(a.IATA))
            .ToList()!;
    }
}