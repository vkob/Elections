using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Get.Html.Xls.Txt
{
    public class Normalizer2016
    {
        private Dictionary<string, string> list = new Dictionary<string, string>();

        public Normalizer2016()
        {

        }

        public Dictionary<string, string> GetDictionary(DirectoryInfo dir)
        {
            Start(dir);
            return list;
        }

        private void Start(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories("ОИК №*"))
            {
                Start(subDir);
            }

            foreach (var fileInfo in dir.GetFiles("ОИК №*"))
            {
                list.Add(fileInfo.Directory.Name, GetName(fileInfo.FullName));
            }
        }

        private string GetName(string fullName)
        {
            using (var sr = new StreamReader(fullName, Encoding.GetEncoding(1251)))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (line.Contains("><b>Наименование избирательной комиссии</b>"))
                    {
                        return TextProcessFunctions.GetElectionCommitteeName(line);
                    }
                }
            }

            throw new Exception("Nothing found");
        }
    }
}
