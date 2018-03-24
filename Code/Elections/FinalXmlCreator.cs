using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Elections.Utility;
using Elections.XmlProcessing;

namespace Elections
{
    public class FinalXmlCreator
    {
        #region Fields

        private readonly Dictionary<string, ElectionCommitteeResults> _dictionary = new Dictionary<string, ElectionCommitteeResults>();
        private string _fileName;
        private ElectionYear _electionYear;

        #endregion

        public void Start(string years)
        {
            var yearsSplitted = years.Split(',').Select(y => y.Trim());

            foreach (var year in yearsSplitted)
            {
                _electionYear = ElectionYear.GetElectionYear(year);

                _dictionary.Clear();

                var electionInfoDir = Consts.LocalPath + @"\" + Consts.ElectionsDir + @"\" +
                                      _electionYear.DirElectionInfo + @"\";

                if (!Directory.Exists(electionInfoDir)) Directory.CreateDirectory(electionInfoDir);

                _fileName = electionInfoDir + _electionYear.DirElectionInfo + _electionYear.Year + ".xml";

                var filesPattern = string.Format(Consts.PatternExtTxt, _electionYear.Year);
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var path = Path.Combine(Data.Core.Consts.ResultsPath, _electionYear.Result);
                SearchTxtFiles(path, filesPattern, _electionYear.Result);

                SaveDictionary();

                stopWatch.Stop();
                Trace.WriteLine(stopWatch.Elapsed.ToString());
            }
        }

        private void SearchTxtFiles(string path, string filesPattern, string results)
        {
            var directoryInfo = new DirectoryInfo(path);

            var directories = directoryInfo.GetDirectories();
            if (directories.Length != 0)
            {
                foreach (var di in directories)
                {
                    SearchTxtFiles(di.FullName, filesPattern, results);
                }
            }
            else
            {
                foreach (var fi in directoryInfo.GetFiles(filesPattern))
                {
                    //if (!fi.FullName.Contains("ерритория за пределами")) continue;

                    var idx = fi.FullName.IndexOf(results);
                    var key = fi.FullName.Substring(idx + results.Length + 1);

                    var electionCommitteeResults = new ElectionCommitteeResults(fi.FullName);
                    electionCommitteeResults.Href = Utility.ProcessData.GetHref(fi.Directory, _electionYear.Year);
                    _dictionary.Add(key, electionCommitteeResults);
                }
            }
        }

        private void SaveDictionary()
        {
            string[][] translitted;


            ////new FooData() { RussianShort = "АПР", Color = "white", IsMain = false, RussianLong = "АПР" , Result =0},
            ////new FooData() { RussianShort = "РППиПСС", Color = "white", IsMain = false, RussianLong = "РППиПСС" , Result =0},
            ////new FooData() { RussianShort = "Яблоко", Color = "white", IsMain = false, RussianLong = "Яблоко" , Result =0},
            ////new FooData() { RussianShort = "СПС", Color = "white", IsMain = false, RussianLong = "СПС" , Result =0},

            if (_electionYear.ElectionType == ElectionType.Duma)
            {
                translitted = new[]
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
                var people = _dictionary.Values.First().partiesData.Keys.ToArray();
                translitted = new string[people.Length][];
                for (int i = 0; i < translitted.Length; i++)
                {
                    translitted[i] = new string[2];
                    translitted[i][0] = people[i];
                    translitted[i][1] = TextProcessFunctions.Translit(people[i]);
                }
            }

            var elections = new List<Election>();

            foreach (var kvp in _dictionary)
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

                var foos = new List<Foo>();
                for (int i = 0; i < translitted.Length; i++)
                {
                    var shortPartyName = translitted[i][1];
                    string partyName = translitted[i][0];
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
            using (var sw = new StreamWriter(_fileName))
            {
                xmlSerializer.Serialize(sw, elections);
            }
        }
    }
}
