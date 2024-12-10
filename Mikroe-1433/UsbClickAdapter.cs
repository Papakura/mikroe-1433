using Ardalis.Result;
using Iot.Device.Adc;
using Iot.Device.Ft2232H;
using Iot.Device.FtCommon;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Spi;
namespace Mikroe.Click
{
    public class UsbClickAdapter
    {
        Ft2232HDevice? _channelA;
        Ft2232HDevice? _channelB;
        private GpioController? _channelAController;
        private GpioController? _channelBController;
        private GpioPin? _led;
        public Result Open()
        {
            var devices = Ft2232HDevice.GetFt2232H();
            if (devices == null)
            {
                return Result.NotFound();
            }
            _channelA = devices.FirstOrDefault(static d => d.Channel == FtChannel.A);
            _channelAController = _channelA?.CreateGpioController();
            _channelB = devices.FirstOrDefault(static d => d.Channel == FtChannel.B);
            _channelBController = _channelB?.CreateGpioController();
            int gpio = Ft2232HDevice.GetPinNumberFromString($"{_channelA?.Channel}CBUS4");
            _led = _channelAController?.OpenPin(gpio);
            _led?.Write(PinValue.High);
            _led?.SetPinMode(PinMode.Output);
            _led?.Write(PinValue.Low);
            _led?.Write(PinValue.High);
            return Result.Success();

        }

        public Result<Mcp3204?> CreateMcp4304()
        {
            var spiChannel = _channelB;
            SpiConnectionSettings settings = new(0, 3) { ClockFrequency = 1_000_000, ChipSelectLine = 4, DataBitLength = 8, ChipSelectLineActiveState = PinValue.Low };
            var spi = spiChannel?.CreateSpiDevice(settings, [0, 1, 2, 4], PinNumberingScheme.Logical);
            if (spi != null)
            {
                Mcp3204 sensor = new(spi);
                return sensor;
            }
            return Result.Error("SPI could not be created");
        }

        public Result<SpiDevice?> CreateSpi()
        {
            var spiChannel = _channelB;
            SpiConnectionSettings settings = new(0, 3) { ClockFrequency = 1_000_000, DataBitLength = 8, ChipSelectLineActiveState = PinValue.Low };
            var spi = spiChannel?.CreateSpiDevice(settings);
            if (spi != null)
            {
                return spi;
            }
            return Result.Error("SPI could not be created");
        }

        public Result<I2cDevice> CreateI2CDevice(byte address)
        {
            var i2cDevice = _channelA?.CreateI2cDevice(new I2cConnectionSettings(0, address));
            if (i2cDevice != null)
            {
                return i2cDevice;
            }
            return Result.Error("The I2C device could not be created");
        }
        public Result<T> CreateI2CDevice<T>(byte address, Func<I2cDevice,T> factory)
        {
            var i2cDevice = _channelA?.CreateI2cDevice(new I2cConnectionSettings(0, address));
            if (i2cDevice != null)
            {
                return Result<T>.Created(factory(i2cDevice));
            }
            return Result.Error("The I2C device could not be created");
        }

        public GpioPin SetLed(bool led) 
        {
            _led.Write(led ? PinValue.Low :  PinValue.High);
            return _led;
        }

        public GpioPin? GetResetPin()
        {
            var controller = _channelB?.CreateGpioController();
            int gpio = Ft2232HDevice.GetPinNumberFromString($"{_channelB?.Channel}CBUS1");
            var pin = controller?.OpenPin(gpio);
            pin?.SetPinMode(PinMode.Output);
            return pin;
        }

        public GpioPin? GetInterrupt(PinMode mode, Action<object, PinValueChangedEventArgs> onInterrupt)
        {
            var controller = _channelB?.CreateGpioController();
            int gpio = Ft2232HDevice.GetPinNumberFromString($"{_channelB?.Channel}CBUS2");
            var pin = controller?.OpenPin(gpio);
            pin?.SetPinMode(mode);
            pin!.ValueChanged += (s,e) => onInterrupt(s,e);
            return pin;
        }

        public GpioPin? GetPwm()
        {
            var controller = _channelB?.CreateGpioController();
            int gpio = Ft2232HDevice.GetPinNumberFromString($"{_channelB?.Channel}CBUS3");
            var pin = controller?.OpenPin(gpio);
            pin?.SetPinMode(PinMode.Output);
            return pin;
        }


    }
}
