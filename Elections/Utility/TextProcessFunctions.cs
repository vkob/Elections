using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Elections.Utility
{
   public class TextProcessFunctions
   {
      private static Regex regex = new Regex("(?<number>([0-9]+))");
      private static Regex regexNormalizePlace = new Regex(@"^(?<region>([А-Яа-я\s\.\-]+))\s\-\s[А-Я][а-я]+(?<part>, (.+))$");
      private static Regex regexNormalizeRegion = new Regex(@"^(?<region>([А-Яа-я\s\.\-]+))\s\-\s[А-Я][а-я]+$");

      public const string SIZKSRF = "СИЗКСРФ";

      public static string[] GetLocation(ElectionYear electionYear, string fileName)
      {
         var path = Path.GetDirectoryName(fileName);
         var what = electionYear.Result;

         var idx = path.IndexOf(what);
         var text = path.Substring(idx + what.Length + 1);
         var parts = text.Split('\\');
         return parts;
      }

      public static string GetElectionCommitteeName(ElectionYear electionYear, string fileName)
      {
         var parts = GetLocation(electionYear, fileName);
         return GetElectionCommitteeName(parts, electionYear.Year);
      } 

      public static string GetElectionCommitteeName(string text, int year)
      {
         var parts = text.Split('\\');
         return GetElectionCommitteeName(parts, year);
      }

      private static string GetElectionCommitteeName(string[] parts, int year)
      {
         string first = parts[0];
         string second = parts[1];
         if (year == Consts.ElectionYear2003.Year && parts.Length == 4)
         {
            if (second == "Пушкинский" || second == "Нижневартовский")
            {
               second = "(" + parts[1] + ") " + parts[2];
            }
            else
            {
               second = parts[2];
            }
         }
         var location = (second == SIZKSRF) ? first : string.Format("{0}, {1}", first, second);
         return location;
      }

      public static string GetRegion(string electionCommittee)
      {
         var idx = electionCommittee.IndexOf('\\');
         var region = electionCommittee.Substring(0, idx);

         if (!region.Contains(" - ")) return region;

         var match = regexNormalizeRegion.Match(region);
         if (match.Success)
         {
            var normalizedRegion = match.Groups["region"].Value;
            return normalizedRegion;
         }
         return region;
      }

      public static string GetNormalizedPlace(string place)
      {
         if (place.Contains("Санкт-Петербург"))
         {
            place = place.Replace("Территориальная избирательная комиссия №30", "Центральная №30");
            place = place.Replace("Территориальная избирательная комиссия №29", "Фрунзенская №29");
            place = place.Replace("Территориальная избирательная комиссия №28", "Приморская №28");
            place = place.Replace("Территориальная избирательная комиссия №27", "Московская №27");
            place = place.Replace("Территориальная избирательная комиссия №26", "Красносельская №26");
            place = place.Replace("Территориальная избирательная комиссия №25", "Красногвардейская №25");
            place = place.Replace("Территориальная избирательная комиссия №24", "Невская №24");
            place = place.Replace("Территориальная избирательная комиссия №23", "Фрунзенская №23");
            //place = place.Replace("Территориальная избирательная комиссия №22", "");
            place = place.Replace("Территориальная избирательная комиссия №21", "Колпинская №21");
            place = place.Replace("Территориальная избирательная комиссия №20", "Пушкинская №20");
            place = place.Replace("Территориальная избирательная комиссия №19", "Московская №19");
            place = place.Replace("Территориальная избирательная комиссия №18", "Петроградская №18");
            place = place.Replace("Территориальная избирательная комиссия №17", "Калининская №17");
            place = place.Replace("Территориальная избирательная комиссия №16", "Центральная №16");
            place = place.Replace("Территориальная избирательная комиссия №15", "Кронштадтская №15");
            place = place.Replace("Территориальная избирательная комиссия №14", "Выборгская №14");
            place = place.Replace("Территориальная избирательная комиссия №13", "Курортная №13");
            place = place.Replace("Территориальная избирательная комиссия №12", "Приморская №12");
            place = place.Replace("Территориальная избирательная комиссия №11", "Калининская №11");
            place = place.Replace("Территориальная избирательная комиссия №10", "Выборгская №10");
            place = place.Replace("Территориальная избирательная комиссия №9", "Ломоносовская №9");
            place = place.Replace("Территориальная избирательная комиссия №8", "Петродворцовая №8");
            place = place.Replace("Территориальная избирательная комиссия №7", "Кировская №7");
            place = place.Replace("Территориальная избирательная комиссия №6", "Красносельская №6");
            place = place.Replace("Территориальная избирательная комиссия №5", "Невская №5");
            place = place.Replace("Территориальная избирательная комиссия №4", "Красногвардейская №4");
            place = place.Replace("Территориальная избирательная комиссия №3", "Кировская №3");
            place = place.Replace("Территориальная избирательная комиссия №2", "Василеостровская №2");
            place = place.Replace("Территориальная избирательная комиссия №1", "Адмиралтейская №1");
         }
         if (place.Contains("N") || place.Contains("N "))
         {
            place = place.Replace("N ", "№");
            place = place.Replace("N", "№");
            //Trace.WriteLine(place);
         }
         place = place.Replace("№ ", "№");
         place = place.Replace(" .", ".");
         place = place.Replace(".", ". ");
         place = place.Replace("  ", " ");

         var match = regexNormalizePlace.Match(place);

         if (match.Success)
         {
            var region = match.Groups["region"].Value;
            var part = match.Groups["part"].Value;
            return region + part;
         }
         //Trace.WriteLine(place);
         //place = place.Replace("Читинская область, Забайкальский край")
         return place;
      }

      public static string GetUikNumber(string uikYext)
      {
         var match = regex.Match(uikYext);
         if (match.Success)
         {
            return match.Groups["number"].Value;
         }
         return uikYext;
      }

      public static string Translit(string text)
      {
         var dict = new Dictionary<char, string>();
         dict.Add('а', "a"); dict.Add('б', "b"); dict.Add('в', "v");
         dict.Add('г', "g"); dict.Add('д', "d"); dict.Add('е', "e");
         dict.Add('ё', "yo"); dict.Add('ж', "g"); dict.Add('з', "z");
         dict.Add('и', "i"); dict.Add('й', "j"); dict.Add('к', "k");
         dict.Add('л', "l"); dict.Add('м', "m"); dict.Add('н', "n");
         dict.Add('о', "o"); dict.Add('п', "p"); dict.Add('р', "r");
         dict.Add('с', "s"); dict.Add('т', "t"); dict.Add('у', "u");
         dict.Add('ф', "f"); dict.Add('х', "h"); dict.Add('ц', "ts");
         dict.Add('ч', "ch"); dict.Add('ш', "sh"); dict.Add('щ', "sh");
         dict.Add('ь', ""); dict.Add('ъ', ""); dict.Add('э', "e");
         dict.Add('ы', "i"); dict.Add('ю', "yu"); dict.Add('я', "ya");
         dict.Add(' ', "_");

         var sb = new StringBuilder();
         foreach (var c in text)
         {
            var isUpper = char.IsUpper(c);
            var cLower = char.ToLower(c);
            var res = dict.ContainsKey(cLower) ? dict[cLower] : char.IsDigit(c) ? c.ToString() : "";
            if (isUpper)
            {
               if (res.Length == 1)
                  res = res.ToUpper();
               else
                  if (res.Length == 2) res = string.Format("{0}{1}", char.ToUpper(res[0]), res[1]);
            }
            sb.Append(res);
         }
         return sb.ToString();
      }
   }
}
