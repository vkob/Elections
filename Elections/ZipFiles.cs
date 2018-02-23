using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Elections.Utility;
using ICSharpCode.SharpZipLib.Zip;

namespace Elections
{
   public class ZipFiles
   {
      public void StartCopying(ElectionYear[] electionYears)
      {
         foreach (var electionYear in electionYears/*.Where(e => e.Year == 2003)*/)
         {
            var destJpg = Path.Combine(Consts.ToZipPath, string.Format(Consts.JpgZip, electionYear.Year, SortByDelta.GetAdditional(electionYear.ElectionType)));
            var destJpgByIrr = Path.Combine(Consts.ToZipPath, string.Format(Consts.JpgZipIrr, electionYear.Year, SortByDelta.GetAdditional(electionYear.ElectionType)));
            var destTxt = Path.Combine(Consts.ToZipPath, string.Format(Consts.TxtZip, electionYear.Year, SortByDelta.GetAdditional(electionYear.ElectionType)));
            var destXls = Path.Combine(Consts.ToZipPath, string.Format(Consts.XlsZip, electionYear.Year, SortByDelta.GetAdditional(electionYear.ElectionType)));
            Directory.CreateDirectory(destJpg);
            Directory.CreateDirectory(destJpgByIrr);
            Directory.CreateDirectory(destTxt);
            Directory.CreateDirectory(destXls);

            string what = electionYear.DirElectionInfo;
            var resultsLast = Consts.LocalPath + @"\" + Consts.ElectionsDir + @"\" + what + @"\" + what + electionYear.Year + ".xml";

            var elections = ProcessData.GetElectionDataWithNormalizedPlace(resultsLast, electionYear.Year, SortByDelta.GetAdditional(electionYear.ElectionType));

            FindFiles(elections, new DirectoryInfo(Path.Combine(Consts.ResultsPath, electionYear.Result)), electionYear, destJpg, destJpgByIrr, destTxt, destXls);
         }
      }

      public void StartZipping(ElectionYear[] electionYears)
      {
         foreach (var electionYear in electionYears/*.Where(e => e.Year == 2003)*/)
         {
            var jpgName = string.Format(Consts.JpgZip, electionYear.Year, SortByDelta.GetAdditional(electionYear.ElectionType));
            var jpgByIrr = string.Format(Consts.JpgZipIrr, electionYear.Year, SortByDelta.GetAdditional(electionYear.ElectionType));
            var txtName = string.Format(Consts.TxtZip, electionYear.Year, SortByDelta.GetAdditional(electionYear.ElectionType));
            var xlsName = string.Format(Consts.XlsZip, electionYear.Year, SortByDelta.GetAdditional(electionYear.ElectionType));
            
            var sourceJpg = Path.Combine(Consts.ToZipPath, jpgName);
            var sourceJpgByIrr = Path.Combine(Consts.ToZipPath, jpgByIrr);
            var sourceTxt = Path.Combine(Consts.ToZipPath, txtName);
            var sourceXls = Path.Combine(Consts.ToZipPath, xlsName);

            var txtZip = Path.Combine(Consts.ToZipPath, txtName + ".zip");
            var jpgZip = Path.Combine(Consts.ToZipPath, jpgName + ".zip");
            var jpgByIrrZip = Path.Combine(Consts.ToZipPath, jpgByIrr + ".zip");
            var xlsZip = Path.Combine(Consts.ToZipPath, xlsName + ".zip");
            ZipConstants.DefaultCodePage = 866;
            var fz = new FastZip();
            if (!File.Exists(txtZip)) fz.CreateZip(txtZip, sourceTxt, true, "");
            if (!File.Exists(jpgZip)) fz.CreateZip(jpgZip, sourceJpg, true, "");
            if (!File.Exists(jpgByIrrZip)) fz.CreateZip(jpgByIrrZip, sourceJpgByIrr, true, "");
            if (!File.Exists(xlsZip)) fz.CreateZip(xlsZip, sourceXls, true, "");
         }
      }

      public void CopyZipFiles()
      {
         Directory.CreateDirectory(Consts.ZipPath);
         foreach (var fi in new DirectoryInfo(Consts.ToZipPath).GetFiles())
         {
            fi.CopyTo(Path.Combine(Consts.ZipPath, fi.Name));
         }
      }

      public void FindFiles(Dictionary<string, XmlProcessing.Election> elections, DirectoryInfo directoryInfo, ElectionYear electionYear, string destJpg, string destJpgByIrr, string destTxt, string destXls)
      {
         foreach (var di in directoryInfo.GetDirectories())
         {
            FindFiles(elections, di, electionYear, destJpg, destJpgByIrr, destTxt, destXls);
         }
         var jpgPattern = string.Format("*{0}.jpg", electionYear.Year);
         foreach (var jpg in directoryInfo.GetFiles(jpgPattern))
         {
            var electionCommitteeName = TextProcessFunctions.GetNormalizedPlace(TextProcessFunctions.GetElectionCommitteeName(electionYear, jpg.FullName));
            var mainFoo = electionYear.FooData.First(f => f.IsMain);
            var irregularity = elections[electionCommitteeName].GetFoo(mainFoo.EnglishShort).Irregularity;
            var destFile = Path.Combine(destJpg, electionCommitteeName + ".jpg");
            if (!File.Exists(destFile))
               jpg.CopyTo(destFile);
            var destFileIrr = Path.Combine(destJpgByIrr, string.Format("{0:00}", irregularity) + "_" + electionCommitteeName + ".jpg");
            if (!File.Exists(destFileIrr))
               jpg.CopyTo(destFileIrr);
         }
         var txtPattern = string.Format("*{0}.txt", electionYear.Year);
         foreach (var txt in directoryInfo.GetFiles(txtPattern))
         {
            var electionCommitteeName = TextProcessFunctions.GetNormalizedPlace(TextProcessFunctions.GetElectionCommitteeName(electionYear, txt.FullName));
            var translit = TextProcessFunctions.Translit(electionCommitteeName);
            var destFile = Path.Combine(destTxt, electionCommitteeName + ".txt");
            if (!File.Exists(destFile))
               txt.CopyTo(destFile);
         }
         var csvPattern = string.Format("*{0}.xls", electionYear.Year);
         foreach (var xls in directoryInfo.GetFiles(csvPattern))
         {
            var electionCommitteeName = TextProcessFunctions.GetNormalizedPlace(TextProcessFunctions.GetElectionCommitteeName(electionYear, xls.FullName));
            var translit = TextProcessFunctions.Translit(electionCommitteeName);
            var destFile = Path.Combine(destXls, electionCommitteeName + ".xls");
            if (!File.Exists(destFile))
               xls.CopyTo(destFile);
         }
      }
   }
}
