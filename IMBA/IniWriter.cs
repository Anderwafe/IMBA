using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMBA
{
    public class IniWriter
    {
        public List<string> iniText { get; private set; }
        private string Path2Ini { get; set; }

        public IniWriter(string path)
        {
            if (!File.Exists(path))
                File.Create(path).Close();
            iniText = File.ReadAllLines(path).ToList();
            Path2Ini = path;
        }

        public bool WriteParam(string paramName, string paramValue)
        {
            try
            {
                for (int i = 0; i < iniText.Count; i++)
                {
                    if (iniText[i].StartsWith(paramName + "="))
                    {
                        iniText[i] = paramName + "=" + paramValue;
                        WriteChanges();
                        return true;
                    }
                }
                iniText.Add(paramName + "=" + paramValue);
                WriteChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WriteParam(string paramName, string[] paramValues)
        {
            try
            {
                for (int i = 0; i < paramValues.Length; i++)
                    paramValues[i] = $@"{i}:{paramValues[i]}";
                for(int i = 0; i < iniText.Count; i++)
                {
                    if(iniText[i].StartsWith(paramName + "=array"))
                    {
                        iniText.RemoveRange(i, iniText.Skip(i + 2).TakeWhile(x => x != "}").Count() + 3);

                        List<string> abs = paramValues.ToList();
                        abs.Add("}");
                        abs.Insert(0, "{");
                        abs.Insert(0, $"{paramName}=array");
                        iniText.InsertRange(i, abs);
                        WriteChanges();
                        return true;
                    }
                }
                List<string> list = paramValues.ToList();
                list.Add("}");
                list.Insert(0, "{");
                list.Insert(0, $"{paramName}=array");
                iniText.AddRange(list);
                WriteChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void WriteChanges()
        {
            File.WriteAllLines(Path2Ini, iniText);
        }
    }
}
