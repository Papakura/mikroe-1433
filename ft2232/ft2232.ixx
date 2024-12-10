#include "pch.h"

export module ftdi;
import <string>;
export class spi {
private:
	FT_HANDLE _handle;
	std::string _serial;
public:
	~spi();
	void open();
	std::string& serial();
};

export class mikroe1433 {
private:
	spi  _spi;
public:
	void open();
	
	mikroe1433();
	~mikroe1433();
};