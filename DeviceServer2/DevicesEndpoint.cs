using FastEndpoints;

namespace DeviceServer;

public class DevicesEndpoint : EndpointWithoutRequest<IEnumerable<string>>
{
    public override void Configure()
    {
        Get("api/devices");
        Description(b =>
        {
            b.Produces<string[]>(StatusCodes.Status200OK)
                .WithTags("Devices");
        });
        Summary(s =>
        {
            s.Summary = "Get all devices";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync( CancellationToken ct)
    {
        await SendAsync(["rest"],StatusCodes.Status200OK,ct);
        
    }
}