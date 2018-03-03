using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Elections.Utility;
using Microsoft.Office.Interop.Excel;

namespace Elections
{
    public class ExcellExtracter : IDisposable
    {
        private readonly ApplicationClass app;

        public ExcellExtracter()
        {
            app = new ApplicationClass();
        }

        public void ExportXls(string path, string years)
        {
            var filePatterns = years.Split(',').Select(y => $"*{y}.xls").ToArray();
            ExportXls(path, filePatterns);
        }

        public void ExportXls(string path, string[] filePatterns)
        {
            var directoryInfo = new DirectoryInfo(path);
            var directoryInfos = directoryInfo.GetDirectories();
            foreach (var di in directoryInfos)
            {
                if (di.FullName.EndsWith(Data.Core.Consts.LocalCommittee))
                {
                    filePatterns.ForEach(pattern => ProcessFiles(di, pattern));
                }
                ExportXls(di.FullName, filePatterns);
            }
        }

        public void SaveXlsToTxt(FileInfo fi)
        {
            var fileName = string.Format(@"{0}\{1}.txt", fi.DirectoryName, Path.GetFileNameWithoutExtension(fi.FullName));
            if (File.Exists(fileName)) return;

            Trace.WriteLine(fileName);

            object misValue = System.Reflection.Missing.Value;

            var workbooks = app.Workbooks;
            var workBook = workbooks.Open(fi.FullName, 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            var worksheets = workBook.Worksheets;
            var workSheet = (Worksheet)worksheets[1];

            workBook.SaveAs(fileName, XlFileFormat.xlTextWindows, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            workBook.Close(true, misValue, misValue);

            Marshal.ReleaseComObject(workSheet);
            Marshal.ReleaseComObject(worksheets);
            Marshal.ReleaseComObject(workBook);
        }

        public void Dispose()
        {
            app.Quit();
            Marshal.ReleaseComObject(app);
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
                    Trace.WriteLine(string.Format("{0}: {1}", fi.FullName, ex.Message));
                }
            }
        }
    }
}
