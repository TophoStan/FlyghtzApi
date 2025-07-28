using Microsoft.VisualBasic.FileIO;

namespace DataExtractor.OurAirports;

public class OurAirportsCsvReader
{
    public static async Task<List<OurAirportRow>> ReadAirportsFromExcel(string filePath)
    {

        var list = new List<OurAirportRow>();

        using (var parser = new TextFieldParser(filePath))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;

           var headers = parser.ReadFields();
            
            while (!parser.EndOfData)
            {
                var values = parser.ReadFields();
                try
                {
                    var airport = new OurAirportRow()
                    {
                        Id = int.Parse(values[0]),
                        Ident = values[1],
                        Type = values[2],
                        Name = values[3],
                        Latitude_Deg = double.Parse(values[4]),
                        Longitude_Deg = double.Parse(values[5]),
                        Elevation_Ft = string.IsNullOrEmpty(values[6]) ? 0 : int.Parse(values[6]),
                        Continent = values[7],
                        Iso_Country = values[8],
                        Iso_Region = values[9],
                        Municipality = values[10],
                        Scheduled_Service = values[11],
                        Icao_Code = values[12],
                        Iata_Code = values[13],
                        Gps_Code = values[14],
                        Local_Code = values[15],
                        Home_Link = values[16],
                        Wikipedia_Link = values[17],
                        Keywords = values[18]
                    };
                    
                    if(airport.Iata_Code.Equals("")){ continue;}
                    if(airport.Icao_Code.Equals("")){continue;}
                    
                    list.Add(airport);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error in row: {string.Join(",", values)}");
                    throw;
                }
            }
        }


        
        return list;
    }
}