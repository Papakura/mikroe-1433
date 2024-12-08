using Iot.Device.Scd4x;

namespace Mikroe.Click.Extensions
{
    public static class Extensions
    {
        public static Scd4x Create(this UsbClickAdapter adapter)
        {
            var device = adapter.CreateI2CDevice(Scd4x.DefaultI2cAddress);
            return new Scd4x(device);
        }
    }
}
