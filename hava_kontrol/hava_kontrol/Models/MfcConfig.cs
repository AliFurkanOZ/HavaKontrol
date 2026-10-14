namespace hava_kontrol.Models
{
    /// <summary>
    /// MFC konfigürasyon sabitleri.
    /// LINE TECH MD30C Mass Flow Controller.
    /// 
    /// MFC'ler PLC COM2 RS485 üzerinden bağlıdır.
    /// PC artık doğrudan MFC ile konuşmaz.
    /// Tüm iletişim PLC D register'ları aracılığıyla yapılır.
    /// </summary>
    public static class MfcConfig
    {
        // Setpoint limitleri (SCCM)
        public const float MinSetpoint = 0.0f;
        public const float MaxSetpoint = 2000.0f;

        // MFC Slave ID'leri (LINE TECH MD30C, RS485 üzerinde tanımlı)
        // ID 1: 40001 - 40020 (Hex Ofset: 16#0000)
        public const int Mfc1DefaultSlaveId = 1;
        // ID 2: 40021 - 40040 (Hex Ofset: 16#0014)
        public const int Mfc2DefaultSlaveId = 2;
        // ID 3: 40041 - 40060 (Hex Ofset: 16#0028)
        public const int Mfc3DefaultSlaveId = 3;
        // ID 6: 40101 - 40120 (Hex Ofset: 16#0064)
        public const int Mfc4DefaultSlaveId = 6;
    }
}