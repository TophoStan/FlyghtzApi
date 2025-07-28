using DataExtracting.Contracts;
using DataExtractor.Extensions;
using DataExtractor.OurAirports;
using DataExtractor.Wikipedia;
using Domain;

namespace DataExtractor;

public class DataExtractionController : IDataExtractionController
{
    private readonly WikepediaAirportExtractor _wikipedia;

    public DataExtractionController(WikepediaAirportExtractor wikipedia)
    {
        _wikipedia = wikipedia;
    }


    public async Task<List<AirportData>> GatherData()
    {
        var wikiPediaAirportDataList = await _wikipedia.Execute();

        var filePath = "C:\\Users\\stant\\RiderProjects\\WebApplication1\\FlightsApi\\airports.csv";
        var ourAirportData = await OurAirportsCsvReader.ReadAirportsFromExcel(filePath);

        var generalAirportDataList = wikiPediaAirportDataList.AddProperties(ourAirportData);

        return generalAirportDataList;
    }
}