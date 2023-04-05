using DevConsolePatcher.Properties;
using System;
using System.IO;

namespace DevConsolePatcher
{
    internal class Program
    {
        private static string gamedirectory = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Bit Heroes";
        static void Main(string[] args)
        {
            Console.Title = "BitHeroes Quest Developer Console Patch | SamzyDev";
            Console.WriteLine("[NOTE] There is also a decryptor for their encryption in this folder...");
            if (!Directory.Exists(gamedirectory))
            {
                Console.WriteLine("[ERROR] Could not find game directory, please paste it in.");
                Console.WriteLine("Game Directory: ");
                gamedirectory = Console.ReadLine();
            }
            Console.WriteLine("[+] Applying Patch");
            try
            {
                File.Delete(gamedirectory + "\\Bit Heroes_Data\\Managed\\Assembly-CSharp.dll");
                File.WriteAllBytes(gamedirectory + "\\Bit Heroes_Data\\Managed\\Assembly-CSharp.dll", Resources.Assembly_CSharp);
                File.Delete(gamedirectory + "\\Bit Heroes_Data\\Managed\\UnityEngine.CoreModule.dll");
                File.WriteAllBytes(gamedirectory + "\\Bit Heroes_Data\\Managed\\UnityEngine.CoreModule.dll", Resources.UnityEngine_CoreModule);
            }
            catch
            {
                Console.WriteLine("[ERROR] Make sure you ran this as admin and try again!");
                return;
            }
            Console.WriteLine("[+] Patch done!");
            Console.WriteLine("[+] Press enter to close");
            Console.ReadKey();
        }
    }
}
