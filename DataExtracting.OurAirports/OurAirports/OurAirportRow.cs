namespace DataExtractor.OurAirports;

public class OurAirportRow
{
    public int Id { get; set; }
    public string Ident { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public double Latitude_Deg { get; set; }
    public double Longitude_Deg { get; set; }
    public int? Elevation_Ft { get; set; }
    public string Continent { get; set; }
    public string Iso_Country { get; set; }
    public string Iso_Region { get; set; }
    public string Municipality { get; set; }
    public string Scheduled_Service { get; set; }
    public string Icao_Code { get; set; }
    public string Iata_Code { get; set; }
    public string Gps_Code { get; set; }
    public string Local_Code { get; set; }
    public string Home_Link { get; set; }
    public string Wikipedia_Link { get; set; }
    public string Keywords { get; set; }
}