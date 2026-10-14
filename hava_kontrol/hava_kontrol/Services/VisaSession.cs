using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace hava_kontrol.Services
{
    public class VisaSession : IDisposable
    {
        private const string VisaDll = "visa64.dll";
        public const int VI_SUCCESS = 0;
        public const int VI_SUCCESS_EVENT_ENQUEUED = 0x3FFF0002;
        public const int VI_SUCCESS_MAX_CNT = 0x3FFF0005;
        public const int VI_SUCCESS_TERM_CHAR = 0x3FFF0006;

        public const int VI_ERROR_INV_OBJECT = unchecked((int)0xBFFF000E);
        public const int VI_ERROR_RSRC_NFOUND = unchecked((int)0xBFFF0011);
        public const int VI_ERROR_TMO = unchecked((int)0xBFFF0015);
        public const int VI_ERROR_BERR = unchecked((int)0xBFFF0028);
        public const int VI_ERROR_SYSTEM_ERROR = unchecked((int)0xBFFF0000);
        public const int VI_ERROR_CONN_LOST = unchecked((int)0xBFFF00A6);

        public const uint VI_ATTR_TMO_VALUE = 0x3FFF001A;
        public const uint VI_ATTR_TERMCHAR_EN = 0x3FFF0038;
        public const uint VI_ATTR_TERMCHAR = 0x3FFF0018;

        [DllImport(VisaDll, EntryPoint = "viOpenDefaultRM")]
        private static extern int viOpenDefaultRM(out IntPtr sesn);

        [DllImport(VisaDll, EntryPoint = "viFindRsrc", CharSet = CharSet.Ansi)]
        private static extern int viFindRsrc(IntPtr sesn, string expr, out IntPtr viList, out int retCnt, StringBuilder desc);

        [DllImport(VisaDll, EntryPoint = "viFindNext", CharSet = CharSet.Ansi)]
        private static extern int viFindNext(IntPtr viList, StringBuilder desc);

        [DllImport(VisaDll, EntryPoint = "viOpen", CharSet = CharSet.Ansi)]
        private static extern int viOpen(IntPtr sesn, string rsrcName, int accessMode, int timeout, out IntPtr vi);

        [DllImport(VisaDll, EntryPoint = "viSetAttribute")]
        private static extern int viSetAttribute(IntPtr vi, uint attrName, ulong attrValue);

        [DllImport(VisaDll, EntryPoint = "viWrite")]
        private static extern int viWrite(IntPtr vi, byte[] buf, int count, out int retCount);

        [DllImport(VisaDll, EntryPoint = "viRead")]
        private static extern int viRead(IntPtr vi, byte[] buf, int count, out int retCount);

        [DllImport(VisaDll, EntryPoint = "viClear")]
        private static extern int viClear(IntPtr vi);

        [DllImport(VisaDll, EntryPoint = "viClose")]
        private static extern int viClose(IntPtr vi);

        private IntPtr _defaultRm = IntPtr.Zero;
        private IntPtr _instrument = IntPtr.Zero;
        private bool _disposed;

        public bool IsOpen => _instrument != IntPtr.Zero;
        public string ResourceName { get; private set; } = string.Empty;

        /// <summary>
        /// VISA durum kodunun ölümcül bir kopma hatası olup olmadığını denetler.
        /// USB kablosu çekildiğinde VI_ERROR_CONN_LOST veya VI_ERROR_INV_OBJECT/BERR döner.
        /// </summary>
        public static bool IsFatalDisconnect(int status)
        {
            if (status >= 0) return false;
            return status == VI_ERROR_CONN_LOST ||
                   status == VI_ERROR_INV_OBJECT ||
                   status == VI_ERROR_BERR ||
                   status == VI_ERROR_SYSTEM_ERROR;
        }

        public static List<string> FindUsbInstruments()
        {
            var list = new List<string>();
            try
            {
                int st = viOpenDefaultRM(out IntPtr rm);
                if (st != VI_SUCCESS) return list;

                try
                {
                    StringBuilder desc = new StringBuilder(512);
                    st = viFindRsrc(rm, "USB?*INSTR", out IntPtr viList, out int count, desc);
                    if (st == VI_SUCCESS && count > 0)
                    {
                        list.Add(desc.ToString());
                        for (int i = 1; i < count; i++)
                        {
                            StringBuilder nextDesc = new StringBuilder(512);
                            if (viFindNext(viList, nextDesc) == VI_SUCCESS)
                            {
                                list.Add(nextDesc.ToString());
                            }
                        }
                        viClose(viList);
                    }
                }
                finally
                {
                    viClose(rm);
                }
            }
            catch
            {
                // VISA DLL bulunamadı veya hata
            }
            return list;
        }

        public bool Open(string resourceName, int timeoutMs = 2000)
        {
            Close();

            try
            {
                int st = viOpenDefaultRM(out _defaultRm);
                if (st != VI_SUCCESS) return false;

                st = viOpen(_defaultRm, resourceName, 0, timeoutMs, out _instrument);
                if (st != VI_SUCCESS)
                {
                    Close();
                    return false;
                }

                viSetAttribute(_instrument, VI_ATTR_TMO_VALUE, (ulong)timeoutMs);
                viSetAttribute(_instrument, VI_ATTR_TERMCHAR_EN, 1);
                viSetAttribute(_instrument, VI_ATTR_TERMCHAR, 0x0A);
                ResourceName = resourceName;
                return true;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public bool Write(string command)
        {
            if (_instrument == IntPtr.Zero) return false;

            string cmd = command.TrimEnd() + "\n";
            byte[] bytes = Encoding.ASCII.GetBytes(cmd);
            int st = viWrite(_instrument, bytes, bytes.Length, out _);
            if (st < 0)
            {
                if (IsFatalDisconnect(st))
                {
                    Close();
                }
                return false;
            }
            return true;
        }

        public string Query(string query, int maxBytes = 1024)
        {
            if (_instrument == IntPtr.Zero) return string.Empty;

            if (!Write(query)) return string.Empty;

            byte[] buf = new byte[maxBytes];
            int st = viRead(_instrument, buf, buf.Length, out int retCount);

            // VISA standardında st >= 0 durumu (VI_SUCCESS, VI_SUCCESS_TERM_CHAR, VI_SUCCESS_MAX_CNT vb.) başarılıdır.
            if (st >= 0)
            {
                if (retCount <= 0) return string.Empty;
                return Encoding.ASCII.GetString(buf, 0, retCount).Trim();
            }

            // st < 0 durumunda hata oluşmuştur:
            if (IsFatalDisconnect(st))
            {
                Close();
            }
            else if (st == VI_ERROR_TMO)
            {
                // Geçici zaman aşımında arabelleği temizle, oturumu hemen kapatma
                Clear();
            }

            return string.Empty;
        }

        /// <summary>
        /// Cihazın I/O arabelleğini temizler (Selected Device Clear).
        /// </summary>
        public bool Clear()
        {
            if (_instrument == IntPtr.Zero) return false;
            try
            {
                int st = viClear(_instrument);
                if (IsFatalDisconnect(st))
                {
                    Close();
                    return false;
                }
                return st >= 0;
            }
            catch
            {
                Close();
                return false;
            }
        }

        public void Close()
        {
            if (_instrument != IntPtr.Zero)
            {
                try { viClose(_instrument); } catch { }
                _instrument = IntPtr.Zero;
            }
            if (_defaultRm != IntPtr.Zero)
            {
                try { viClose(_defaultRm); } catch { }
                _defaultRm = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Close();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
