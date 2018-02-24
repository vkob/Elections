using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Data
{
    public class FileList
    {
        private readonly string _path;
        private readonly string _filePattern;

        public FileList(string path, string filePattern)
        {
            _path = path;
            _filePattern = filePattern;
        }

        public List<string> GetList()
        {
            List<string> list = new List<string>();
            FillList(_path, ref list);

            return list;
        }

        public void FillList(string path, ref List<string> list)
        {
            var directory = new DirectoryInfo(path);

            foreach (var dirChild in directory.GetDirectories())
            {
                FillList(dirChild.FullName, ref list);
            }

            foreach (var fileInfo in directory.GetFiles(_filePattern))
            {
                list.Add(fileInfo.FullName);
            }
        }
    }
}
