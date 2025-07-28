using Domain;

namespace DataExtracting.Contracts;

public interface IDataExtractionController
{
    public Task<List<AirportData>> GatherData();
}