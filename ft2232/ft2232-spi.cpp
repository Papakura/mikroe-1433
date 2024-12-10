module;
#include "pch.h"
#include "libmpsse_spi.h"
import <string>;
module ftdi;
std::string& spi::serial()
{
	return _serial;
}

spi::~spi()
{
	SPI_CloseChannel(_handle);
}

void spi::open()
{
	DWORD channels;
	SPI_GetNumChannels(&channels);
	for (int index = 0; index < channels; index++)
	{
		FT_DEVICE_LIST_INFO_NODE info;
		SPI_GetChannelInfo(index, &info);
		if (std::string(info.SerialNumber) == std::string("B"))
		{
			_serial = info.SerialNumber;
			SPI_OpenChannel(index, &_handle);
			SPI_ChangeCS(_handle, 0b00100);
		}
	}
}
