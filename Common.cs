using System;
using System.Text;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Tile
{
    static class Common
    {
        public const string LOG_FILE = "tile.log";
        public const string CONFIG_FILE = "tile.xml";

        static public void AñadeALog(string log, string adicionalInfo = "")
        {
            StreamWriter sw = new StreamWriter(LOG_FILE, true, Encoding.UTF8);
            sw.WriteLine($"\n___________________________________________ Usuario: {Environment.UserName} { DateTime.Now} -> {adicionalInfo}\n");
            sw.WriteLine(log);
            sw.Close();
        }

        static public bool IAmOnWindows()
        {
            OperatingSystem os = Environment.OSVersion;
            switch (os.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return true;
                default:
                    return false;
            }
        }

        static public bool IAmOnLinux()
        {
            OperatingSystem os = Environment.OSVersion;
            switch (os.Platform)
            {
                case PlatformID.Unix:
                    return true;
                default:
                    return false;
            }
        }

        static public Color FromRGBString(string rgb)
        {
            int R, G, B;

            string patronColor = @"^#?(?<R>[0-9a-fA-F]{2})(?<G>[0-9a-fA-F]{2})(?<B>[0-9a-fA-F]{2})$";
            Match m = Regex.Match(rgb, patronColor);

            if (m.Success)
            {
                R = int.Parse(m.Groups["R"].Value, NumberStyles.AllowHexSpecifier);
                G = int.Parse(m.Groups["G"].Value, NumberStyles.AllowHexSpecifier);
                B = int.Parse(m.Groups["B"].Value, NumberStyles.AllowHexSpecifier);
            }
            else
                throw new ArgumentException($"Se esperaba un color en formato #RGB Ej: #FF0000 y se recibió {rgb}.");

            return Color.FromArgb(R, G, B);
        }
    }
}
