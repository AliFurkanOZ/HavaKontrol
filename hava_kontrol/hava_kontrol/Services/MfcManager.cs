using hava_kontrol.Models;

namespace hava_kontrol.Services
{
    /// <summary>
    /// 4 adet LINE TECH MD30C MFC yönetimi — PLC üzerinden.
    /// 
    /// Mimari:
    ///   PC → Modbus TCP → PLC (AS228P) → COM2 RS485 → MFC'ler
    ///   PC artık MFC'lerle doğrudan konuşmaz.
    ///   Tüm komutlar PlcService aracılığıyla PLC D register'larına yazılır.
    ///   PLC MODRWE state machine ile MFC'lere iletir.
    /// 
    /// Güvenlik:
    ///   - Setpoint: 0..2000 SCCM
    ///   - Genel güç OFF ise FLOW ON engellenir (PLC tarafında da korunur)
    /// </summary>
    public class MfcManager : IDisposable
    {
        private readonly PlcService _plcService;
        private bool _disposed;

        // Genel MFC gücünün yazılım durumu (D300 üzerinden PLC'ye de yazılır).
        // Varsayılan: true — şu an fiziksel röle yok, MFC'ler direkt güç alıyor.
        // Röle bağlandığında false yapılıp kullanıcıdan toggle ile açması istenebilir.
        private bool _generalPower = true;

        public MfcManager(PlcService plcService)
        {
            _plcService = plcService ?? throw new ArgumentNullException(nameof(plcService));
        }

        // ============================================================
        // BAĞLANTI (PlcService üzerinden)
        // ============================================================

        /// <summary>
        /// MFC bağlantısı artık PLC bağlantısına bağlıdır.
        /// PlcService bağlıysa MFC'lere erişilebilir.
        /// </summary>
        public bool IsConnected => _plcService.IsConnected;

        /// <summary>
        /// PLC bağlantısını başlatır ve tüm MFC'ler için
        /// dijital setpoint source modunu ayarlar.
        /// </summary>
        public bool Connect()
        {
            if (!_plcService.IsConnected)
            {
                if (!_plcService.Connect())
                    return false;
            }

            // Tüm MFC'ler için dijital setpoint source modu ayarla
            InitializeMfcs();

            return true;
        }

        /// <summary>
        /// Bağlantı sonrası tüm MFC'ler için dijital setpoint source (1) yazar.
        /// Hata olursa yutulur — MFC offline olabilir, bağlantı süreci kesilmez.
        /// </summary>
        private void InitializeMfcs()
        {
            for (int i = 1; i <= PlcConfig.MfcCount; i++)
            {
                try
                {
                    _plcService.MfcSetSetpointSource(i, 1);
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"MFC-{i} dijital mod ayarlanamadı: {exc.Message}");
                }
            }
        }

        // ============================================================
        // GENEL GÜÇ
        // ============================================================

        /// <summary>
        /// Genel güç komutunu hem yazılımda hem PLC'de günceller.
        /// PLC D300'e yazar. PLC programı bu değere göre:
        ///   - D400 durumunu günceller
        ///   - 0 ise tüm MFC flow'larını kapatır (Section 5)
        /// </summary>
        public void SetGeneralPower(bool enabled)
        {
            _generalPower = enabled;

            if (_plcService.IsConnected)
            {
                try
                {
                    _plcService.WriteGeneralPower(enabled);
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"Genel güç komutu yazılamadı: {exc.Message}");
                }
            }
        }

        public bool IsGeneralPowerOn => _generalPower;

        // ============================================================
        // SETPOINT
        // ============================================================

        /// <summary>
        /// MFC setpoint yazar. Float → 2 word olarak PLC D3x2+D3x3'e yazılır.
        /// PLC MODRWE ile MFC'nin 40013-40014 register'larına iletir.
        /// </summary>
        public void SetSetpoint(int mfcNumber, float sccm)
        {
            ValidateMfcNumber(mfcNumber);

            if (sccm < MfcConfig.MinSetpoint)
                throw new ArgumentOutOfRangeException(
                    nameof(sccm),
                    "Setpoint 0 SCCM'den küçük olamaz.");

            if (sccm > MfcConfig.MaxSetpoint)
                throw new ArgumentOutOfRangeException(
                    nameof(sccm),
                    "Setpoint 2000 SCCM'den büyük olamaz.");

            _plcService.MfcSetSetpointSource(mfcNumber, 1);
            _plcService.MfcWriteSetpoint(mfcNumber, sccm);
        }

        // ============================================================
        // FLOW ON / OFF
        // ============================================================

        /// <summary>
        /// MFC flow ON/OFF komutu. PLC D3x1'e yazılır.
        /// PLC MODRWE ile MFC'nin 40011 register'ına iletir.
        /// Flow ON için genel güç şarttır.
        /// </summary>
        public void SetFlow(int mfcNumber, bool enabled)
        {
            ValidateMfcNumber(mfcNumber);

            // FLOW OFF her durumda izinli.
            if (!enabled)
            {
                _plcService.MfcSetFlowOnOff(mfcNumber, false);
                return;
            }

            // FLOW ON için genel güç şart.
            if (!_generalPower)
                throw new InvalidOperationException(
                    "MFC genel gücü OFF. " +
                    "Genel güç açılmadan FLOW ON yapılamaz.");

            // Flow açmadan önce MFC'nin dijital modda (D3x0 = 1) olduğundan emin ol
            _plcService.MfcSetSetpointSource(mfcNumber, 1);
            _plcService.MfcSetFlowOnOff(mfcNumber, true);
        }

        public void Start(int mfcNumber)
        {
            SetFlow(mfcNumber, true);
        }

        public void Stop(int mfcNumber)
        {
            SetFlow(mfcNumber, false);
        }

        // ============================================================
        // OKUMA
        // ============================================================

        /// <summary>
        /// MFC actual flow okur. PLC D4x4+D4x5'ten IEEE-754 float32 oluşturur.
        /// PLC bu değerleri MFC'nin 40001-40002'den okuyarak günceller.
        /// </summary>
        public float ReadActualFlow(int mfcNumber)
        {
            ValidateMfcNumber(mfcNumber);
            return _plcService.MfcReadActualFlow(mfcNumber);
        }

        /// <summary>
        /// MFC setpoint'i PLC komut register'larından okur. D3x2+D3x3.
        /// </summary>
        public float ReadSetpoint(int mfcNumber)
        {
            ValidateMfcNumber(mfcNumber);
            return _plcService.MfcReadSetpoint(mfcNumber);
        }

        /// <summary>
        /// Tek MFC için durum bilgisi okur.
        /// PLC register'larından actual flow, setpoint, flow on/off,
        /// iletişim durumu ve hata kodu okunur.
        /// </summary>
        public Dictionary<string, object> ReadStatus(int mfcNumber)
        {
            ValidateMfcNumber(mfcNumber);

            var status = new Dictionary<string, object>();

            try
            {
                bool commOk = _plcService.MfcReadCommStatus(mfcNumber);
                ushort errorCode = _plcService.MfcReadErrorCode(mfcNumber);

                status["online"] = commOk;
                status["error_code"] = (int)errorCode;

                // Comm durumundan bağımsız olarak TÜM verileri oku.
                // D410 (comm flag) PLC state machine zamanlamasına bağlı —
                // henüz set edilmemiş olabilir ama D4x4-D4x5'te veri olabilir.
                float actualFlow = _plcService.MfcReadActualFlow(mfcNumber);
                float setpoint = _plcService.MfcReadSetpoint(mfcNumber);
                bool flowOn = _plcService.MfcReadFlowOnOff(mfcNumber);

                status["actual_flow_sccm"] = actualFlow;
                status["setpoint_sccm"] = setpoint;
                status["flow_on"] = flowOn;
            }
            catch (Exception exc)
            {
                status["online"] = false;
                status["error"] = exc.Message;
            }

            return status;
        }

        // ============================================================
        // TÜM MFC'LERİ OKU
        // ============================================================

        /// <summary>
        /// 4 MFC'nin tamamı için durum bilgisi okur.
        /// Her MFC için ayrı ayrı PLC register'ları okunur.
        /// </summary>
        public Dictionary<int, Dictionary<string, object>> ReadAllStatus()
        {
            var result = new Dictionary<int, Dictionary<string, object>>();

            for (int i = 1; i <= PlcConfig.MfcCount; i++)
            {
                result[i] = ReadStatus(i);
            }

            return result;
        }

        // ============================================================
        // TÜM MFC'LERİ DURDUR
        // ============================================================

        /// <summary>
        /// Tüm MFC'ler için flow OFF yazar.
        /// Hata olan MFC'ler toplanır ve topluca raporlanır.
        /// </summary>
        public void StopAll()
        {
            var errors = new List<string>();

            for (int i = 1; i <= PlcConfig.MfcCount; i++)
            {
                try
                {
                    _plcService.MfcSetFlowOnOff(i, false);
                }
                catch (Exception exc)
                {
                    errors.Add($"MFC-{i}: {exc.Message}");
                }
            }

            if (errors.Count > 0)
                throw new InvalidOperationException(
                    "Bazı MFC'ler OFF yapılamadı:\n" +
                    string.Join("\n", errors));
        }

        // ============================================================
        // HEARTBEAT
        // ============================================================

        /// <summary>
        /// PLC'ye heartbeat gönderir. Timer'dan her çevrimde çağrılmalıdır.
        /// PLC watchdog bu değerin değişip değişmediğini izler.
        /// </summary>
        public void SendHeartbeat()
        {
            if (_plcService.IsConnected)
            {
                try
                {
                    _plcService.SendHeartbeat();
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"Heartbeat gönderilemedi: {exc.Message}");
                }
            }
        }

        // ============================================================
        // YARDIMCI
        // ============================================================

        private static void ValidateMfcNumber(int mfcNumber)
        {
            if (mfcNumber < 1 || mfcNumber > PlcConfig.MfcCount)
                throw new ArgumentOutOfRangeException(
                    nameof(mfcNumber),
                    "MFC numarası 1, 2, 3 veya 4 olmalıdır.");
        }

        // ============================================================
        // DISPOSE
        // ============================================================

        public void Dispose()
        {
            if (!_disposed)
            {
                // PlcService'in yaşam döngüsü dışarıdan yönetilir,
                // burada sadece kendi kaynaklarımızı temizleriz.
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}