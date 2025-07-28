using DataExtracting.Contracts;
using FastEndpoints;
using FlightsApi.Dto;

namespace FlightsApi.Endpoints.AirportData;

public class GetAirportDataEndpoint : Endpoint<GetAirportDataRequest, GetAirportDataResponse>
{
    private IDataExtractionController _dataExtractionController;

    public GetAirportDataEndpoint(IDataExtractionController dataExtractionController)
    {
        _dataExtractionController = dataExtractionController;
    }

    public override void Configure()
    {
        Get("api/airports");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAirportDataRequest req, CancellationToken ct)
    {
        var data = await _dataExtractionController.GatherData();
        var dataAsDto = data.Select(x => x.ToDto());
        await SendAsync(new()
        {
            AirportData = dataAsDto
        }, cancellation: ct);
    }
}