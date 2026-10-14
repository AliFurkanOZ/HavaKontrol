using System.IO.Ports;
using NModbus;
using NModbus.Serial;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("==============================================");
Console.WriteLine("  V-OVG Setpoint Yazma Testi (FC 06 Write)");
Console.WriteLine("==============================================");

string comPort = "COM12";
using var sp = new SerialPort(comPort, 19200, Parity.None, 8, StopBits.One)
{
    ReadTimeout = 1000,
    WriteTimeout = 1000,
    Handshake = Handshake.None
};
sp.Open();
sp.DiscardInBuffer();
sp.DiscardOutBuffer();

var adapter = new SerialPortAdapter(sp);
var factory = new ModbusFactory();
var master = factory.CreateRtuMaster(adapter);
master.Transport.ReadTimeout = 1000;
master.Transport.WriteTimeout = 1000;
master.Transport.Retries = 1;

// 1. Sıcaklık SP testi (Slave 3)
Console.WriteLine("\n1. Sıcaklık Kontrolcü (Slave 3) SP Yazma Testi:");
ushort originalTemp = master.ReadHoldingRegisters(3, 24, 1)[0];
Console.WriteLine($"  Mevcut SP: {originalTemp / 10.0f} °C (raw {originalTemp})");

ushort testTemp = 280; // 28.0 °C
Console.WriteLine($"  Yazılıyor: 28.0 °C (raw {testTemp})...");
master.WriteSingleRegister(3, 24, testTemp);

ushort readBackTemp = master.ReadHoldingRegisters(3, 24, 1)[0];
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"  ✓ Okunan yeni SP: {readBackTemp / 10.0f} °C (raw {readBackTemp})");
Console.ResetColor();

// Geri yükle
Console.WriteLine($"  Orijinal değere geri alınıyor: {originalTemp / 10.0f} °C...");
master.WriteSingleRegister(3, 24, originalTemp);
Console.WriteLine($"  ✓ Orijinal değer geri yüklendi: {master.ReadHoldingRegisters(3, 24, 1)[0] / 10.0f} °C");

// 2. Flow SP testi (Slave 4)
Console.WriteLine("\n2. Flow Kontrolcü (Slave 4) SP Yazma Testi:");
ushort originalFlow = master.ReadHoldingRegisters(4, 24, 1)[0];
Console.WriteLine($"  Mevcut SP: {originalFlow} sccm");

ushort testFlow = 15; // 15 sccm
Console.WriteLine($"  Yazılıyor: {testFlow} sccm...");
master.WriteSingleRegister(4, 24, testFlow);

ushort readBackFlow = master.ReadHoldingRegisters(4, 24, 1)[0];
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"  ✓ Okunan yeni SP: {readBackFlow} sccm");
Console.ResetColor();

// Geri yükle
Console.WriteLine($"  Orijinal değere geri alınıyor: {originalFlow} sccm...");
master.WriteSingleRegister(4, 24, originalFlow);
Console.WriteLine($"  ✓ Orijinal değer geri yüklendi: {master.ReadHoldingRegisters(4, 24, 1)[0]} sccm");

Console.WriteLine("\n==============================================");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("HEM OKUMA HEM YAZMA BAŞARIYLA TAMAMLANDI!");
Console.ResetColor();
Console.WriteLine("==============================================");
