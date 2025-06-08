using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Services
{
    public class IniFile
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filepath);

        public static string READ(string szFile, string szSection, string szKey)
        {
            StringBuilder tmp = new StringBuilder(255);
            long i = GetPrivateProfileString(szSection, szKey, "", tmp, 255, szFile);
            return tmp.ToString();
        }
        public static void WRITE(string szFile, string szSection, string szKey, string szData)
        {
            WritePrivateProfileString(szSection, szKey, szData, szFile);
        }
        public static List<string> ReadSectionRaw(string szFile, string szSection)
        {
            List<string> lines = new List<string>();
            bool inSection = false;
            foreach (string rawLine in File.ReadAllLines(szFile))
            {
                string line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";") || line.StartsWith("#"))
                    continue;

                if(line.StartsWith("[") && line.EndsWith("]"))
                {
                    inSection = (line.Substring(1, line.Length - 2).Trim() == szSection);
                    continue;
                }

                if(inSection && line.Contains('='))
                {
                    lines.Add(line);
                }

                if(inSection && line.StartsWith("[") && line.EndsWith("]"))
                {
                    break;
                }
            }
            return lines;
        }
        public static List<string> ReadSectionRawValue(string szFile, string szSection)
        {
            List<string> lines = new List<string>();
            bool inSection = false;
            foreach (string rawLine in File.ReadAllLines(szFile))
            {
                string line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";") || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    inSection = (line.Substring(1, line.Length - 2).Trim() == szSection);
                    continue;
                }

                if (inSection && line.Contains('='))
                {
                    var parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        lines.Add(parts[1].Trim());
                    }
                }

                if (inSection && line.StartsWith("[") && line.EndsWith("]"))
                {
                    break;
                }
            }
            return lines;
        }
    }
}
