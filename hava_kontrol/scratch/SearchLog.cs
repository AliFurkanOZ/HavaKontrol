using System;
using System.IO;

class LogSearch
{
    static void Main()
    {
        string[] lines = File.ReadAllLines(@"c:\Users\onure\OneDrive\Desktop\HavaKontrol\hava_kontrol\hava_kontrol\build_diag.log");
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("MarkupCompilePass2") || lines[i].Contains("App.xaml") || lines[i].Contains("MainWindow.xaml"))
            {
                if (lines[i].Contains("Target \"") || lines[i].Contains("Task \"") || lines[i].Contains("skipping") || lines[i].Contains("Skipping"))
                {
                    Console.WriteLine(string.Format("{0}: {1}", i, lines[i]));
                }
            }
        }
    }
}
