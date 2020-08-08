using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMBA
{
    public class IniReader
    {
        public string[] iniText { get; private set; }
        private string Path2Ini { get; set; }

        public IniReader(string path)
        {
            iniText = File.ReadAllLines(path);
            Path2Ini = path;
        }

        public string GetValuebyParam(string paramName)
        {
            iniText = File.ReadAllLines(Path2Ini);

            foreach(var i in iniText)
            {
                if (i.StartsWith(paramName + "=array"))
                    continue;
                if (i.StartsWith(paramName + "="))
                    return i.Remove(0, paramName.Length + 1);
            }
            return null;
        }

        public string[] GetValuebyParam(string paramName, bool isArray)
        {
            iniText = File.ReadAllLines(Path2Ini);

            if(isArray)
            {
                string[] a;

                for(int i = 0; i<iniText.Length; i++)
                {
                    if(iniText[i].StartsWith(paramName + "=array"))
                    {
                        int b = 0;
                        a = new string[iniText.Skip(i + 2).TakeWhile(x => x != "}").Count()];
                        int c = i + 2 + a.Length;
                        for(int j = i+2; j < c; j++)
                        {
                            a[b] = new Regex(@".*:").Replace(iniText[j], "");
                            b++;
                        }
                        return a;
                    }
                }
                return null;
            }
            else
            {
                return new string[] { GetValuebyParam(paramName) };
            }
        }

        public string[] GetParams()
        {
            return iniText.Select(x => new Regex(@".*=")
                .Match(x)
                .Value
                .Replace("=",""))
                .Where(x => x.Replace(" ", "") != "")
                .ToArray();
        }
    }
}
