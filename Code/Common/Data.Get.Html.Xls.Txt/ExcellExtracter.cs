using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Data.Core;
using Microsoft.Office.Interop.Excel;

namespace Data.Get.Html.Xls.Txt
{
    public class ExcellExtracter : IDisposable
    {
        private readonly ApplicationClass _app;

        public ExcellExtracter()
        {
            _app = new ApplicationClass();
        }

        public void ExportXls(string years)
        {
            var yearsSplitted = years.Split(',').Select(y => int.Parse(y.Trim()));

            foreach (var year in yearsSplitted)
            {
                var item = Items.ElectionItems.FirstOrDefault(i => i.Year == year);
                ExportXls(Path.Combine(Consts.ResultsPath, item.Result), $"*{year}.xls");
            }
        }

        public void ExportXls(string path, string filePattern)
        {
            var directoryInfo = new DirectoryInfo(path);
            var directoryInfos = directoryInfo.GetDirectories();
            foreach (var di in directoryInfos)
            {
                if (di.FullName.EndsWith(Consts.LocalCommittee))
                {
                    ProcessFiles(di, filePattern);
                }
                ExportXls(di.FullName, filePattern);
            }
        }

        public void SaveXlsToTxt(FileInfo fi)
        {
            var fileName = string.Format(@"{0}\{1}.txt", fi.DirectoryName, Path.GetFileNameWithoutExtension(fi.FullName));
            if (File.Exists(fileName)) return;

            Console.WriteLine(fileName);

            object misValue = System.Reflection.Missing.Value;

            var workbooks = _app.Workbooks;
            var workBook = workbooks.Open(fi.FullName, 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            workBook.SaveAs(fileName, XlFileFormat.xlTextWindows, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            workBook.Close(true, misValue, misValue);

            Marshal.ReleaseComObject(workBook);
            Marshal.ReleaseComObject(workbooks);
        }

        public void Dispose()
        {
            _app.Quit();
            Marshal.ReleaseComObject(_app);
        }

        private void ProcessFiles(DirectoryInfo di, string pattern)
        {
            foreach (var fi in di.GetFiles(pattern))
            {
                try
                {

                    SaveXlsToTxt(fi);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{fi.FullName}: {ex.Message}");
                }
            }
        }
    }
}
