using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Elections.XmlProcessing;
using Microsoft.Office.Interop.Excel;

namespace Elections.Utility
{
   public class ProcessData
   {
      public const char Tab = '	';

       private const string Vybory = @"http://www.[a-z]*\.?vybory.izbirkom.ru";
       private const string RegionIzbirkom = "/region/[0-9a-z&=;?/_]+";
       private const string Name = @"(?<name>([а-яёa-z\s\(\)\-№N0-9,._]+))";

        public static readonly Regex regexFinish =
           new Regex("(?<tag>(<a href=\"(?<href>(" + Vybory + RegionIzbirkom + "))\">" + Name + "</a>))", RegexOptions.IgnoreCase);

        public static List<Election> ReadSavedData(string path)
      {

         var xmlSerializer = new XmlSerializer(typeof(List<Election>));
         using (var sr = new StreamReader(path))
         {
            return (List<Election>)xmlSerializer.Deserialize(sr);
         }
      }

      public static string GetHref(DirectoryInfo di, int year)
      {
         var parent = di.Parent;
         var pattern = string.Format(Consts.PatternExtCommonHtml, year);
         var files = parent.GetFiles(pattern);

         if (files.Length == 0) throw new Exception("Bad");

         string source = "";
         using (var sr = new StreamReader(files[0].FullName, Encoding.GetEncoding(1251)))
         {
            source = sr.ReadToEnd();
         }

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
            return href;
         }
         return null;
      }

      public static Dictionary<string, Election> GetElectionDataWithNormalizedPlace(string fileName, int year, string additional)
      {
         var xmlSerializer = new XmlSerializer(typeof(List<Election>));
         using (var sr = new StreamReader(fileName))
         {
            var list = ((List<Election>) xmlSerializer.Deserialize(sr));
            list.ForEach(e => e.NormalizeElectionCommitteeName(year, additional));
            //var check = list.GroupBy(l => l.ElectionCommitteeName).ToDictionary(g => g.Key, g => g.ToList()).Where(e => e.Value.Count > 1).ToList();
            var dict = list.
               ToDictionary(election => election.ElectionCommitteeName, election => election, StringComparer.CurrentCultureIgnoreCase);

            return dict;
         }
      }
   }
}
