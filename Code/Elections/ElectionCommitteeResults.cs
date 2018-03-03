using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Data.Core;

namespace Elections
{
   public enum ElectionType 
   {
      Duma,
      President,
      Astrahan
   }

   public class ElectionCommitteeResults
   {
      #region Consts

       #endregion Consts

      #region Fields
      
      public List<string> uiks = new List<string>();

      public Dictionary<string, ElectionCommittee> partiesData = new Dictionary<string, ElectionCommittee>();

      public int NumberOfElectorsInList;
      public List<int> AllNumberOfElectorsInList;

      public int NumberOfEarlier;
      private List<int> allNumberOfEarlier;

      public int NumberOfIn;
      private List<int> allNumberOfIn;

      public int NumberOfOut;
      private List<int> allNumberOfOut;

      public int NumberOfValidBallot;
      public int NumberOfInvalidBallot;

      public int NumberOfLocalElectionCommitees;
      public double Presence;
      public double[] AllPresences;

      public string Href;

      private List<int> exclusionIndexes = new List<int>();

      #endregion

      #region Constructors

      public ElectionCommitteeResults(string fileName)
      {
         ElectionFoo electionFoo = ElectionFoo.GetFoo(fileName);

         const int captionIndex = 1;
         var stopWatch = new Stopwatch();
         stopWatch.Start();

         var lineCouner = 0;

         var firstTime = true;

         using (var sr = new StreamReader(fileName, Encoding.GetEncoding(1251)))
         {
            while (!sr.EndOfStream)
            {
               lineCouner++;
               var line = sr.ReadLine();
               var parts = line.Split(new[] {Data.Core.Consts.Tab }, StringSplitOptions.RemoveEmptyEntries);

               Func<int> getTotal = () => Convert.ToInt32(parts[captionIndex + 1]);
               Func<List<int>> getAll = () => 
                  Enumerable.Range(captionIndex + 2, parts.Length - captionIndex - 2)
                            .Select(i => Convert.ToInt32(parts[i]))
                            .ToList();

               if (lineCouner == electionFoo.NumberOfElectorsInList.LineNumber)
               {
                  AssertCaption(parts[captionIndex], electionFoo.NumberOfElectorsInList.Caption);
                  NumberOfElectorsInList = getTotal();

                  AllNumberOfElectorsInList = getAll();
               }
               if (lineCouner == electionFoo.NumberOfEarlier.LineNumber)
               {
                  AssertCaption(parts[captionIndex], electionFoo.NumberOfEarlier.Caption);
                  NumberOfEarlier = getTotal();

                  allNumberOfEarlier = getAll();
               }
               if (lineCouner == electionFoo.NumberOfIn.LineNumber)
               {
                  AssertCaption(parts[captionIndex], electionFoo.NumberOfIn.Caption);
                  NumberOfIn = getTotal();

                  allNumberOfIn = getAll();
               }
               if (lineCouner == electionFoo.NumberOfOut.LineNumber)
               {
                  AssertCaption(parts[captionIndex], electionFoo.NumberOfOut.Caption);
                  NumberOfOut = getTotal();
               
                  allNumberOfOut = getAll();
               }
               if (lineCouner == electionFoo.NumberOfValidBallot.LineNumber)
               {
                  AssertCaption(parts[captionIndex], electionFoo.NumberOfValidBallot.Caption);
                  NumberOfValidBallot = getTotal();
               }
               if (lineCouner == electionFoo.NumberOfInvalidBallot.LineNumber)
               {
                  AssertCaption(parts[captionIndex], electionFoo.NumberOfInvalidBallot.Caption);
                  NumberOfInvalidBallot = getTotal();
               }

               if (lineCouner == electionFoo.RowLocalElectionCommittee)
               {
                  GetUiks(line);
               }
               if (lineCouner >= electionFoo.MinRowNumberForFactions)
               {
                  if (firstTime)
                  {
                     CheckZero();
                     firstTime = false;
                  }
                  if (!GetNewPartyData(line, sr, fileName)) break;
               }
            }
         }

         Presence = 100 * ((double) (NumberOfEarlier + NumberOfIn + NumberOfOut)) / NumberOfElectorsInList;
         AllPresences = Enumerable
            .Range(0, allNumberOfEarlier.Count)
            .Select(
               i =>
               Math.Round(
                  100.0*(allNumberOfEarlier[i] + allNumberOfIn[i] + allNumberOfOut[i])/
                  AllNumberOfElectorsInList[i], 2))
            .ToArray();

         for (int i = 0; i < AllPresences.Length; i++)
         {
            if (double.IsNaN(AllPresences[i]))
            {
               foreach (var electionCommittee in partiesData)
               {
                  electionCommittee.Value.LocalElectionCommittees[i].Percent = double.NaN;
               }
               //Trace.WriteLine(uiks[i]);
            }
         }
            
         stopWatch.Stop();
         //Trace.WriteLine(stopWatch.Elapsed);
      }

      #endregion Constructors

      #region Private Methods

      public static void AssertCaption(string textFromFile, string caption)
      {
         Debug.Assert(textFromFile == caption || string.Format("\"{0}\"", textFromFile) == caption, caption);         
      }

      private void CheckZero()
      {
         for (int i = 0; i < allNumberOfEarlier.Count; i++)
         {
            bool exclusionExists = allNumberOfEarlier[i] == 0 && allNumberOfIn[i] == 0 && allNumberOfOut[i] == 0;
            if (exclusionExists)
            {
               exclusionIndexes.Add(i);     
            }
         }
      }

      private void GetUiks(string line)
      {
         var parts = line.Split(new[] {Data.Core.Consts.Tab }, StringSplitOptions.RemoveEmptyEntries);
         Debug.Assert(parts[0] == ElectionFoo.Flag, "hello");
         uiks.AddRange(Enumerable.Range(1, parts.Length - 1).Select(i => parts[i]));
         NumberOfLocalElectionCommitees = uiks.Count;
         //Trace.WriteLine(line);
      }

      private bool GetNewPartyData(string line, StreamReader sr, string fileName)
      {
         //Trace.WriteLine("-------------------------------------------");
         var numbersData = line.Split(new[] {Data.Core.Consts.Tab }, StringSplitOptions.RemoveEmptyEntries);
         if (numbersData.Length == 0) return false;
         
         const int captionIndex = 1;
         var partyName = numbersData[captionIndex];

         if (partyName.StartsWith("\"") && partyName.EndsWith("\""))
         {
            partyName = partyName.Substring(1, partyName.Length - 2);
         }

         if (partyName.StartsWith(". "))
         {
            partyName = partyName.Substring(". ".Length);
         }

         partyName = partyName.Replace("\"\"", "\"");

         var numbers = Enumerable.Range(captionIndex + 1, numbersData.Length - captionIndex - 1).Select(i => numbersData[i]).ToArray();
         
         var linePercents = sr.ReadLine();

         if (!ProcessExcel.Parties.ContainsKey(partyName)) return true;

         var shortPartyName = ProcessExcel.Parties[partyName];

         var percents = linePercents.Split(new[] {Data.Core.Consts.Tab }, StringSplitOptions.RemoveEmptyEntries);

         var electionCommittee = new ElectionCommittee(exclusionIndexes)
         {
            LocalElectionCommittees =  
            Enumerable
            .Range(1, numbers.Length - 1)
            .Select(i => new LocalElectionCommittee()
                            {
                               Number = Convert.ToInt32(numbers[i]), 
                               Percent = GetPercent(percents[i]),
                            })
            .ToList(),
            
            Number = Convert.ToInt32(numbers[0]),
            Percent = GetPercent(percents[0])
         };
         
         if (!electionCommittee.Check())
         {
            Trace.WriteLine(string.Format("{0} {1}", fileName, shortPartyName));
         }

         partiesData[shortPartyName] = electionCommittee;

         //Trace.WriteLine(line);
         //Trace.WriteLine("----");
         //Trace.WriteLine(linePercents);
         return true;
      }

      private static double GetPercent(string text)
      {
         text = text.Replace("%", "").Replace(".", ",");
         var value = Convert.ToDouble(text);
         return value;
      }

      #endregion

      #region Nested Types

      public class LocalElectionCommittee
      {
         public int Number;
         public double Percent;
      }

      public class ElectionCommittee
      {
         #region Consts

         private const double UnrealMax = 101;
         private const double UnrealMin = -1;

         #endregion

         #region Fields

         public int Number;
         public double Percent;

         private List<int> exclusionIndexes;
         private List<LocalElectionCommittee> localElectionCommittees;
         private double minPercent = UnrealMax;
         private double maxPercent = UnrealMin;

         #endregion

         #region Constructor

         public ElectionCommittee(List<int> exclusions)
         {
            exclusionIndexes = exclusions;
         }

         #endregion

         public List<LocalElectionCommittee> LocalElectionCommittees
         {
            get { return localElectionCommittees; }
            set 
            { 
               localElectionCommittees = value; 
               CalcMin();
               CalcMax();
            }
         }

         public double MinPercent
         {
            get { return minPercent; }
         }

         public double MaxPercent
         {
            get { return maxPercent; }
         }

         public bool Check()
         {
            var sum = LocalElectionCommittees.Sum(l => l.Number);
            return sum == Number;
         }

         private void CalcMin()
         {
            minPercent = Enumerable
               .Range(0, LocalElectionCommittees.Count)
               .Where(i => !exclusionIndexes.Contains(i))
               .Select(i => LocalElectionCommittees[i])
               .Min(l => l.Percent);
         }

         private void CalcMax()
         {
            maxPercent = Enumerable
               .Range(0, LocalElectionCommittees.Count)
               .Where(i => !exclusionIndexes.Contains(i))
               .Select(i => LocalElectionCommittees[i])
               .Max(l => l.Percent);
         }
      }

      #endregion
   }
}
