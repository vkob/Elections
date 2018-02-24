using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Elections.Utility;
using Elections.XmlProcessing;

namespace Elections
{
   public class ProcessTxt
   {
      #region Fields

      public bool IsStopped;

      private Dictionary<string, ElectionCommitteeResults> dictionary = new Dictionary<string, ElectionCommitteeResults>();
      private string fileName;
      private ElectionYear electionYear;

      #endregion

      public void Start(ElectionYear electionYear)
      {
         this.electionYear = electionYear;
         
         dictionary.Clear();

         var fileForExtremeResults = electionYear.DirElectionInfo + electionYear.Year + ".xml";
         var electionInfoDir = Consts.LocalPath + @"\" + Consts.ElectionsDir + @"\" + electionYear.DirElectionInfo + @"\";

         if (!Directory.Exists(electionInfoDir)) Directory.CreateDirectory(electionInfoDir);

         fileName = electionInfoDir + fileForExtremeResults;

         var filesPattern = string.Format(Consts.PatternExtTxt, electionYear.Year);
         var stopWatch = new Stopwatch();
         stopWatch.Start();

         SearchTxtFiles(Path.Combine(Consts.ResultsPath, electionYear.Result), filesPattern, electionYear.Result);

         SaveDictionary();

         stopWatch.Stop();
         Trace.WriteLine(stopWatch.Elapsed.ToString());
      }

      private void SearchTxtFiles(string path, string filesPattern, string results)
      {
         if (IsStopped) return;
         var directoryInfo = new DirectoryInfo(path);

         var directories = directoryInfo.GetDirectories();
         if (directories.Length != 0)
         {
            foreach (var di in directories)
            {
               if (IsStopped) break;
               SearchTxtFiles(di.FullName, filesPattern, results);
            }
         }
         else
         {
            foreach (var fi in directoryInfo.GetFiles(filesPattern))
            {
               //if (!fi.FullName.Contains("ерритория за пределами")) continue;

               if (IsStopped) break;
               var idx = fi.FullName.IndexOf(results);
               var key = fi.FullName.Substring(idx + results.Length + 1);

               var electionCommitteeResults = new ElectionCommitteeResults(fi.FullName);
               electionCommitteeResults.Href = Utility.ProcessData.GetHref(fi.Directory, electionYear.Year);
               dictionary.Add(key, electionCommitteeResults);
               //IsStopped = true;
            }
         }
      }

      private void SaveDictionary()
      {
         string[][] s;

                             
                             ////new FooData() { RussianShort = "АПР", Color = "white", IsMain = false, RussianLong = "АПР" , Result =0},
                             ////new FooData() { RussianShort = "РППиПСС", Color = "white", IsMain = false, RussianLong = "РППиПСС" , Result =0},
                             ////new FooData() { RussianShort = "Яблоко", Color = "white", IsMain = false, RussianLong = "Яблоко" , Result =0},
                             ////new FooData() { RussianShort = "СПС", Color = "white", IsMain = false, RussianLong = "СПС" , Result =0},

         if (electionYear.ElectionType == ElectionType.Duma)
         {
            s = new[]
               {
                  new[] {"Единая Россия", "ER"}, 
                  new[] {"КПРФ", "KPRF"}, 
                  new[] {"Яблоко", "YA"},
                  new[] {"Родина", "Rodina"},
                  new[] {"Справедливая Россия", "SR"}, 
                  new[] {"ЛДПР", "LDPR"},
                  new[] {"АПР", "APR"},
                  new[] {"РППиПСС", "RPPiPSS"},
                  new[] {"СПС", "SPS"},
               };
         }
         else
         {
            var people = dictionary.Values.First().partiesData.Keys.ToArray();   
            s = new string[people.Length][];
            for (int i = 0; i < s.Length; i++)
            {
               s[i] = new string[2];
               s[i][0] = people[i];
               s[i][1] = TextProcessFunctions.Translit(people[i]);
            }
         }

         var elections = new List<Election>();

         var numberOfVoted = dictionary.Sum(kvp => kvp.Value.NumberOfIn + kvp.Value.NumberOfOut + kvp.Value.NumberOfEarlier);
         var all = dictionary.Sum(kvp => kvp.Value.NumberOfElectorsInList);

         var first = dictionary.First();
         foreach (var partyKvp in first.Value.partiesData)
         {
            var max = dictionary.Max(kvp => kvp.Value.partiesData[partyKvp.Key].Percent);
            Trace.WriteLine(string.Format("{0} {1}", partyKvp.Key, max));
         }

         foreach (var kvp in dictionary)
         {
            var electionCommitteeResults = kvp.Value;
            int idx = kvp.Key.LastIndexOf('\\');
            var electionCommitteeName = kvp.Key.Substring(0, idx);
            var election = new Election();
            election.ElectionCommittee = electionCommitteeName;
            election.Number = electionCommitteeResults.NumberOfLocalElectionCommitees;
            election.NumberOfElectorsInList = electionCommitteeResults.NumberOfElectorsInList;
            election.NumberOfInvalidBallot = electionCommitteeResults.NumberOfInvalidBallot;
            election.NumberOfValidBallot = electionCommitteeResults.NumberOfValidBallot;
            election.Presence = Math.Round(electionCommitteeResults.Presence, 2);
            election.Href = electionCommitteeResults.Href;
            election.AllPresences = electionCommitteeResults.AllPresences;
            election.AllNumberOfElectorsInList = electionCommitteeResults.AllNumberOfElectorsInList.ToArray();
            var uiksNumbers = electionCommitteeResults.uiks.Select(TextProcessFunctions.GetUikNumber).ToArray();
            Debug.Assert(uiksNumbers.Length == election.Number, "Wrong numbers");
            election.Uiks = string.Join(",", uiksNumbers);
            //if (electionCommitteeResults.partiesData.Count == 0) continue;//murmanks and abroad

            var foos = new List<Foo>();
            for (int i = 0; i < s.Length; i++)
            {
               var shortPartyName = s[i][1];
               string partyName = s[i][0];   
               if (!electionCommitteeResults.partiesData.ContainsKey(partyName)) continue;
               var electionCommittee = electionCommitteeResults.partiesData[partyName];
               var foo = new Foo()
                            {
                               Name = shortPartyName,
                               Max = electionCommittee.MaxPercent,
                               Min = electionCommittee.MinPercent,
                               Value = electionCommittee.Percent,
                               Number = electionCommittee.Number,
                               AllValues = electionCommittee.LocalElectionCommittees.Select(l => l.Percent).ToArray()
                            };
               foos.Add(foo);
            }
            election.Foos = foos.OrderBy(f => f.Name).ToList();
            elections.Add(election);
         }

         var xmlSerializer = new XmlSerializer(typeof(List<Election>));
         using (var sw = new StreamWriter(/*@"W:\VS2010\duma\Elections\Elections\ElectionInfoDuma\ElectionInfoDuma2007new.xml"))//*/fileName))
         {
            xmlSerializer.Serialize(sw, elections);
         }
      }
   }
}
