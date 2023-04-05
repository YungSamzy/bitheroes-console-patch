using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BitHeroesDecrypt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.Title = "BitHeroes Decryption";
                Console.WriteLine(@"@@@@@@@@@@@@@@@@@@*****@@@@@@@@@@@@@@@@@
@@@@@@@@@@**,,,,........,,,,,**@@@@@@@@@
@@@@@@/,,,.....................,,*@@@@@@
@@@@*,,......               ......,,*@@@
@@*,,......                   ......,,*@
@*,,......                     ......,,*
#*,,,......                   ......,,,*
**,,,,.......               .......,,,,*
@*,,,,,,#@@@@@@@@@.....@@@@@@@@@*,,,,,,*
@&***,,&&&&&&&&&@@....,@@&&&&&&&&&,,***@
@@@***,%%%%%%%%%&&,,,,,&&%%%%%%%%%,***@@
@@@@***%##(###%%,*******,%%###(###***@@@
@@@@**,,,*******,*,@@@,*********,****@@@
@@@@@@*******,***,&#,#&***********/@@@@@
@@@@@@@@@@@****,,,******,,****@@@@@@@@@@
@@@@@@@@@@@@//****,,,,*****//@@@@@@@@@@@
@@@@@@@@@@@@***,,/,***,/,,/**@@@@@@@@@@@
@@@@@@@@@@@@@@/..,  /  ,.,*@@@@@@@@@@@@@
");
                Console.WriteLine("[1] Encrypt String");
                Console.WriteLine("[2] Decrypt String");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        try
                        {
                            Console.WriteLine("String: ");
                            string text = Console.ReadLine();
                            Console.WriteLine("Output: " + Encrypt(text));
                            Console.ReadKey();
                        }
                        catch
                        {
                            Console.WriteLine("Error! Could not encrypt!");
                        }
                        break;
                    case "2":
                        try
                        {
                            Console.WriteLine("String: ");
                            string text = Console.ReadLine();
                            Console.WriteLine("Output: " + Decrypt(text));
                            Console.ReadKey();
                        }
                        catch
                        {
                            Console.WriteLine("Error! Could not decrypt!");
                        }
                        break;
                    default:
                        Console.WriteLine("That is not an option!");
                        Console.ReadKey();
                        break;
                }
            }
        }
        public static string Encrypt(string clearText)
        {
            string password = key;
            byte[] bytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, new byte[]
                {
                    73,
                    118,
                    97,
                    110,
                    32,
                    77,
                    101,
                    100,
                    118,
                    101,
                    100,
                    101,
                    118
                });
                aes.Key = rfc2898DeriveBytes.GetBytes(32);
                aes.IV = rfc2898DeriveBytes.GetBytes(16);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                        cryptoStream.Close();
                    }
                    clearText = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string result;
            try
            {
                string password = key;
                cipherText = cipherText.Replace(" ", "+");
                byte[] array = Convert.FromBase64String(cipherText);
                using (Aes aes = Aes.Create())
                {
                    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, new byte[]
                    {
                        73,
                        118,
                        97,
                        110,
                        32,
                        77,
                        101,
                        100,
                        118,
                        101,
                        100,
                        101,
                        118
                    });
                    aes.Key = rfc2898DeriveBytes.GetBytes(32);
                    aes.IV = rfc2898DeriveBytes.GetBytes(16);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(array, 0, array.Length);
                            cryptoStream.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(memoryStream.ToArray());
                    }
                }
                result = cipherText;
            }
            catch (FormatException)
            {
                result = null;
            }
            return result;
        }
        private static string key = "jdY9[{`wC*Gz6/'Vga=ZH~N{cC/*]nwT";
    }
}
