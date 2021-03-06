﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Data.Core;

namespace Data.Get.Html.Xls.Txt
{
    public class Download
    {
        #region Consts

        private const string Vybory = @"http://www.[a-z]*\.?vybory.izbirkom.ru";
        private const string RegionIzbirkom = "/region/[0-9a-z&=;?/_]+";
        private const string Name = @"(?<name>([а-яёa-z\s\(\)\-№N0-9,._]+))";

        private const string NameSpecial = @"^[0-9]{1,3}\s" + Name;

        public const string Ending2003Html = "2003.html";
        public const string Ending2007Html = "2007.html";
        public const string Ending2011Html = "2011.html";
        public const string Ending2016Html = "2016.html";
        public const string Ending2018Html = "2018.html";

        public const string Ending2004Html = "2004.html";
        public const string Ending2008Html = "2008.html";
        public const string Ending2012Html = "2012.html";

        public const string Ending2003Xls = "2003.xls";
        public const string Ending2007Xls = "2007.xls";
        public const string Ending2011Xls = "2011.xls";
        public const string Ending2016Xls = "2016.xls";
        public const string Ending2018Xls = "2018.xls";

        public const string Ending2004Xls = "2004.xls";
        public const string Ending2008Xls = "2008.xls";
        public const string Ending2012Xls = "2012.xls";

        public const string ExtHtml = ".html";

        public const string MainHtmlFileName = "all";

        public const int ElectionYear2003 = 2003;

        public const string LocalCommitteeLong = "сайт избирательной комиссии субъекта Российской Федерации";

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

        public void GetHtmlFiles(string years)
        {
            var yearsSplitted = years.Split(',').Select(y => int.Parse(y.Trim()));

            foreach (var year in yearsSplitted)
            {
                var item = Items.ElectionItems.FirstOrDefault(i => i.Year == year);
                Start(item.Year, Path.Combine(Consts.ResultsPath, item.Result), MainHtmlFileName, item.Link);
            }
        }

        public void GetXlsFiles(string years)
        {
            var yearsSplitted = years.Split(',').Select(y => int.Parse(y.Trim()));

            foreach (var year in yearsSplitted)
            {
                var item = Items.ElectionItems.FirstOrDefault(i => i.Year == year);
                FindFileForXlsExtraction(Path.Combine(Consts.ResultsPath, item.Result), item.Year);
            }
        }

        public static void FindFileForXlsExtraction(string path, int year)
        {
            var directoryInfo = new DirectoryInfo(path);
            var directoryInfos = directoryInfo.GetDirectories();

            var fileFilter = $"*{year}.html";
            foreach (var di in directoryInfos)
            {
                if (di.FullName.EndsWith(Consts.LocalCommittee))
                {
                    foreach (var fi in di.GetFiles(fileFilter))
                    {
                        var xlsFileName = GetXlsName(fi.FullName);
                        var xlsFullFileName = di.FullName + @"\" + xlsFileName;
                        if (File.Exists(xlsFullFileName)) continue;
                        using (var sr = new StreamReader(fi.FullName))
                        {
                            var text = sr.ReadToEnd();
                            var xlsHRef = FindRegionXlsHRef(text);
                            DownloadFile(xlsHRef, xlsFullFileName);
                        }
                    }
                }
                else
                {
                    FindFileForXlsExtraction(di.FullName, year);
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
            if (year != ElectionYear2003) return name;

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

            var localFile = path + @"\" + fileName + " " + year + ExtHtml;

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

            var regex = (year == ElectionYear2003) ? regexSpecial : regexMain;

            var match = regex.Match(source);
            if (match.Success)
            {
                while (match.Success)
                {
                    var tag = match.Groups["tag"].Value;
                    var name = GetSpecialName(match.Groups["name"].Value.Trim(), year);
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
                    if (string.Compare(name, LocalCommitteeLong) == 0)
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
                    Console.WriteLine(localFile);
                    client.DownloadFile(url, localFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
                case Ending2003Html:
                    ending = Ending2003Xls;
                    break;
                case Ending2004Html:
                    ending = Ending2004Xls;
                    break;
                case Ending2007Html:
                    ending = Ending2007Xls;
                    break;
                case Ending2008Html:
                    ending = Ending2008Xls;
                    break;
                case Ending2011Html:
                    ending = Ending2011Xls;
                    break;
                case Ending2012Html:
                    ending = Ending2012Xls;
                    break;
                case Ending2016Html:
                    ending = Ending2016Xls;
                    break;
                case Ending2018Html:
                    ending = Ending2018Xls;
                    break;
                default:
                    throw new Exception("Add new extension to constants");
            }

            return string.Format("{0} {1}", name, ending);
        }

        #endregion
    }
}
