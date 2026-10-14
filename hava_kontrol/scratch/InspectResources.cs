using System;
using System.Collections;
using System.Reflection;
using System.Resources;

public class Program
{
    public static void Main()
    {
        Assembly asm = Assembly.LoadFrom(@"c:\Users\onure\OneDrive\Desktop\HavaKontrol\hava_kontrol\hava_kontrol\bin\Debug\net8.0-windows\hava_kontrol.dll");
        using (System.IO.Stream stream = asm.GetManifestResourceStream("hava_kontrol.g.resources"))
        {
            if (stream == null)
            {
                Console.WriteLine("Stream is null!");
                return;
            }
            using (ResourceReader reader = new ResourceReader(stream))
            {
                foreach (DictionaryEntry item in reader)
                {
                    Console.WriteLine(item.Key);
                }
            }
        }
    }
}
