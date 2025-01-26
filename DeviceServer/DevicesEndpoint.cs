using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace DeviceServer;

public class DevicesEndpoint : EndpointWithoutRequest<IEnumerable<string>>
{
    public override void Configure()
    {
        Get("api/devices");
        Description(b =>b.Produces<string[]>(StatusCodes.Status200OK));
        AllowAnonymous();
    }

    public override async Task HandleAsync( CancellationToken ct)
    {
        await SendAsync(["rest"],StatusCodes.Status200OK,ct);
        
    }
}