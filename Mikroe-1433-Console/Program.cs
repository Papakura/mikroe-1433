using System.Device.Gpio;
using Iot.Device.Scd4x;
using Mikroe.Click;
using Mikroe.Click.Extensions;

Console.WriteLine("Mikroe-1433 Test");
var adapter = new UsbClickAdapter();
var result = adapter.Open();
if(result.IsSuccess)
{
    var sensor = adapter.CreateMcp4304();
    if (sensor.IsSuccess)
    {
        var value = sensor.Value.Read(0);
    }
    sensor.Value.Dispose();
    var pin = adapter.SetLed(true);
    var scd4x = adapter.Create();
    scd4x.StartPeriodicMeasurements();
    Thread.Sleep(Scd4x.MeasurementPeriod);
    var measurements = scd4x.ReadPeriodicMeasurement();
    adapter.SetLed(false);
}
