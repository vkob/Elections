using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Elections
{
   public class Download
   {
      #region Consts

      private const string Vybory = @"http://www.[a-z]*\.?vybory.izbirkom.ru";
      private const string RegionIzbirkom = "/region/[0-9a-z&=;?/_]+";
      private const string Name = @"(?<name>([а-яa-z\s\(\)\-№N0-9,.]+))";

      private const string NameSpecial = @"^[0-9]{1,3}\s" + Name;

      #endregion

      #region Fields

      private readonly Regex regexSpecialName = new Regex(NameSpecial, RegexOptions.IgnoreCase);

      private readonly Regex regexSpecial =
         new Regex("(?<tag>(<option value=\"(?<href>(" + Vybory + RegionIzbirkom + "))\">" + Name + "</option>))", RegexOptions.IgnoreCase);

      private readonly Regex regexMain =
         new Regex("(?<tag>(<a style=\"TEXT-DECORATION: none\" href=\"(?<href>(" + Vybory + "/region" + RegionIzbirkom + "))\">" + Name + "</a>))", RegexOptions.IgnoreCase);
      
      public static readonly Regex regexFinish = 
         new Regex("(?<tag>(<a href=\"(?<href>(" + Vybory + RegionIzbirkom + "))\">" + Name + "</a>))", RegexOptions.IgnoreCase);

      #endregion

      #region Public Methods

      public void Start(ElectionYear electionYear)
      {
         Start(electionYear.Year, Path.Combine(Consts.ResultsPath, electionYear.Result), Consts.MainHtmlFileName, electionYear.Link);
      }

      public static void FindFileForXlsExtraction(string path)
      {
         var directoryInfo = new DirectoryInfo(path);
         var directoryInfos = directoryInfo.GetDirectories();
         foreach (var di in directoryInfos)
         {
            if (di.FullName.EndsWith(Consts.LocalCommittee))
            {
               foreach (var fi in di.GetFiles(Consts.PatthernExtHtml))
               {
                  //Trace.WriteLine(fi.FullName);
                  var xlsFileName = GetXlsName(fi.FullName);
                  using (var sr = new StreamReader(fi.FullName))
                  {
                     var text = sr.ReadToEnd();
                     var xlsHRef = FindRegionXlsHRef(text);
                     DownloadFile(xlsHRef, di.FullName + @"\" + xlsFileName);
                  }
               }
            }
            else
            {
               FindFileForXlsExtraction(di.FullName);
            }
         }
      }

      public static string FindRegionXlsHRef(string text)
      {
         var regex = new Regex("window.location.assign\\(\"(?<href>(http://www.[a-z_\\-\\.]+.vybory.izbirkom.ru/servlet/ExcelReportVersion\\?[0-9a-z&=;/_\\t\\n\\+\"\\s]+))\"\\+sortorder\\);", RegexOptions.IgnoreCase);

         var match = regex.Match(text);
         if (match.Success)
         {
            var res = match.Groups["href"].Value;
            res = res.Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\"+\"", "") + "0";
            return res;
         }
         return null;
      }

      public string GetName1(string text)
      {
         var match = regexMain.Match(text);
         var tag = match.Groups["tag"].Value;
         var name = match.Groups["name"].Value.Trim();
         var href = match.Groups["href"].Value;
         href = href.Replace("&amp;", "&");
         return name;
      }

      public string GetName2(string text)
      {
         var match = regexFinish.Match(text);
         var tag = match.Groups["tag"].Value;
         var name = match.Groups["name"].Value.Trim();
         var href = match.Groups["href"].Value;
         href = href.Replace("&amp;", "&");
         return name;
      }

      public string GetName3(string text)
      {
         var match = regexSpecial.Match(text);
         var tag = match.Groups["tag"].Value;
         var name = match.Groups["name"].Value.Trim();
         var href = match.Groups["href"].Value;
         href = href.Replace("&amp;", "&");
         return name;
      }

      public string GetSpecialName(string name, int year)
      {
         if (year != Consts.ElectionYear2003.Year) return name;

         var match = regexSpecialName.Match(name);
         if (match.Success)
         {
            return match.Groups["name"].Value;
         }
         return name;
      }

      #endregion

      #region Private Methods

      private void Start(int year, string path, string fileName, string url)
      {
         path = path.Replace("Астрахнь, Ленинская", "Астрахань, Ленинская");
         fileName = fileName.Replace("Астрахнь, Ленинская", "Астрахань, Ленинская");

         var localFile = path + @"\" + fileName + " " + year + Consts.ExtHtml;

         string result;
         if (!Directory.Exists(path)) Directory.CreateDirectory(path);

         DownloadFile(url, localFile);

         using (var sr = new StreamReader(localFile, Encoding.GetEncoding(1251)))
         {
            result = sr.ReadToEnd();
         }
         FindRegionHRef(year, path, result);
      }

      private void FindRegionHRef(int year, string path, string source)
      {
         //Trace.WriteLine(source);  
         //Trace.WriteLine("---------");

         int count = 0;

         var regex = (year == Consts.ElectionYear2003.Year) ? regexSpecial : regexMain;

         var match = regex.Match(source);
         if (match.Success)
         {
            while (match.Success)
            {
               var tag = match.Groups["tag"].Value;
               var name = GetSpecialName(match.Groups["name"].Value.Trim() ,year);
               var href = match.Groups["href"].Value;
               //Trace.WriteLine(tag);
               //Trace.WriteLine(name);
               //Trace.WriteLine(href);
               count++;
               match = match.NextMatch();
               href = href.Replace("&amp;", "&");
               Start(year, path + @"\" + name, name, href);
               //break;
            }
         }
         else
         {
            //Trace.WriteLine(source);
            var matchFinish = regexFinish.Match(source);

            if (matchFinish.Success)
            {
               var tag = matchFinish.Groups["tag"].Value;
               var name = matchFinish.Groups["name"].Value.Trim();
               var href = matchFinish.Groups["href"].Value;
               //Trace.WriteLine(tag);
               //Trace.WriteLine(name);
               //Trace.WriteLine(href);
               href = href.Replace("&amp;", "&");
               if (string.Compare(name, Consts.LocalCommitteeLong) == 0)
               {
                  name = Consts.LocalCommittee;
               }
               Start(year, path + @"\" + name, name, href);
            }
         }
         //Trace.WriteLine(count);
      }

      private static void DownloadFile(string url, string localFile)
      {
         if (File.Exists(localFile)) return;
         
         using (var client = new WebClient())
         {
            try
            {
               Trace.WriteLine(localFile);
               client.DownloadFile(url, localFile);
            }
            catch (Exception ex)
            {
               Trace.WriteLine(ex.Message);
            }
         }         
      }

      private static string GetXlsName(string fileForExtraction)
      {
         var idx2 = fileForExtraction.IndexOf(Consts.LocalCommittee);
         var idx1 = fileForExtraction.LastIndexOf(@"\", idx2 - 2);
         var name = fileForExtraction.Substring(idx1 + 1, idx2 - idx1 - 2);

         var fileEnd = fileForExtraction.Substring(fileForExtraction.Length - 9);
         string ending;
         switch (fileEnd)
         {
            case Consts.Ending2003Html:
               ending = Consts.Ending2003Xls;
               break;
            case Consts.Ending2004Html:
               ending = Consts.Ending2004Xls;
               break;
            case Consts.Ending2007Html:
               ending = Consts.Ending2007Xls;
               break;
            case Consts.Ending2008Html:
               ending = Consts.Ending2008Xls;
               break;
            case Consts.Ending2011Html:
               ending = Consts.Ending2011Xls;
               break;
            case Consts.Ending2012Html:
               ending = Consts.Ending2012Xls;
               break;
            default:
               throw new Exception("Add new extension to constants");
         }

         return string.Format("{0} {1}", name, ending);
      }
   
      #endregion
   }
}
