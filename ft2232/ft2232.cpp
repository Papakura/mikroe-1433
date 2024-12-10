module;
#include "pch.h"
#include "libmpsse_spi.h"
module ftdi;

mikroe1433::mikroe1433()
{
	Init_libMPSSE();
}
mikroe1433::~mikroe1433()
{
	Cleanup_libMPSSE();
}
void mikroe1433::open()
{
	_spi.open();
}