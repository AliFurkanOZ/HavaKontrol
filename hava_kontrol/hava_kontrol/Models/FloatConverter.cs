namespace hava_kontrol.Models
{
    /// <summary>
    /// IEEE-754 float32 ↔ Modbus register word dönüşüm.
    /// HIGH word first, big-endian byte order.
    /// 
    /// Python float_to_words / words_to_float karşılığı.
    /// </summary>
    public static class FloatConverter
    {
        /// <summary>
        /// Float değeri HIGH ve LOW word'lere dönüştürür.
        /// IEEE-754 float32, HIGH word first.
        /// </summary>
        public static (ushort High, ushort Low) FloatToWords(float value)
        {
            if (value < 0.0f)
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "MFC flow değeri negatif olamaz.");

            byte[] bytes = BitConverter.GetBytes(value);

            // .NET little-endian kullanır, big-endian'a çeviriyoruz
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            // HIGH word = ilk 2 byte, LOW word = son 2 byte
            ushort high = (ushort)((bytes[0] << 8) | bytes[1]);
            ushort low = (ushort)((bytes[2] << 8) | bytes[3]);

            return (high, low);
        }

        /// <summary>
        /// HIGH ve LOW word'lerden float değer oluşturur.
        /// IEEE-754 float32, HIGH word first.
        /// </summary>
        public static float WordsToFloat(ushort high, ushort low)
        {
            // Big-endian byte dizisi oluştur
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(high >> 8);
            bytes[1] = (byte)(high & 0xFF);
            bytes[2] = (byte)(low >> 8);
            bytes[3] = (byte)(low & 0xFF);

            // .NET little-endian bekler
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }
    }
}
