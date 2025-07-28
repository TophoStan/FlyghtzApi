namespace DataExtractor.Wikipedia;

public class WikiPediaAirportData : IComparable<WikiPediaAirportData>
{
    public string Name { get; set; }
    
    public string ICAO  { get; set; }
    
    public string IATA { get; set; }


    public int CompareTo(WikiPediaAirportData? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var nameComparison = string.Compare(Name, other.Name, StringComparison.Ordinal);
        if (nameComparison != 0) return nameComparison;
        var icaoComparison = string.Compare(ICAO, other.ICAO, StringComparison.Ordinal);
        if (icaoComparison != 0) return icaoComparison;
        return string.Compare(IATA, other.IATA, StringComparison.Ordinal);
    }
}