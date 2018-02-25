using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Elections.Utility
{
   public class ElectionDataRenamer
   {
      #region Consts

      public const string SiteNew = @"\СИЗКСРФ";
      public const string NewResults = "ResultsNew";

      private const string Old = "2010.";
      private const string New = "2011.";

      #endregion

      #region Fields

      private string pathOld;

      #endregion

      #region Constructor

      public ElectionDataRenamer(string path)
      {
         pathOld = path;
         var idx = pathOld.IndexOf(Consts.ResultsDuma);
      }

      #endregion

      public void Start()
      {
         //Remove necessary comment

         //Copy(pathOld);
         FilesRename(pathOld);
      }

      #region Private Methods

      private void Copy(string path)
      {
         var diretoryInfo = new DirectoryInfo(path);
         var dirNew = diretoryInfo.FullName.Replace(Consts.ResultsDuma, NewResults);

         if (dirNew.Contains(Consts.LocalCommittee))
         {
            dirNew = dirNew.Replace(Consts.LocalCommittee, SiteNew);
         }

         if (!Directory.Exists(dirNew)) Directory.CreateDirectory(dirNew);

         foreach (var di in diretoryInfo.GetDirectories())
         {
            Copy(di.FullName);
         }

         foreach (var fi in diretoryInfo.GetFiles())
         {
            var filePathNew = fi.FullName.Replace(Consts.ResultsDuma, NewResults);
            if (filePathNew.Contains(Consts.LocalCommittee))
            {
               filePathNew = filePathNew.Replace(Consts.LocalCommittee, SiteNew);
            }
            if (!File.Exists(filePathNew)) fi.CopyTo(filePathNew);
         }
      }

      private static void FilesRename(string path)
      {
         var diretoryInfo = new DirectoryInfo(path);

         foreach (var di in diretoryInfo.GetDirectories())
         {
            FilesRename(di.FullName);
         }

         foreach (var fi in diretoryInfo.GetFiles())
         {
            if (fi.Name.Contains(Old))
            {
               var filePathNew = fi.FullName.Replace(Old, New);
               fi.MoveTo(filePathNew);               
            }
         }
      }

      #endregion
   }
}
