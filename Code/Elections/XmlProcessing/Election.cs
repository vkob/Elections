using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Elections.Utility;

namespace Elections.XmlProcessing
{
   public class Election
   {
      #region Fields

      private List<Foo> foos;

      private Dictionary<string, Foo> foosDictionary;

      private int[] uikNumbers;

      #endregion

      #region Properties

      public string ElectionCommittee { get; set; }
      public string Href { get; set; }
      public string Uiks { get; set; }
      public int NumberOfElectorsInList { get; set; }
      public int NumberOfValidBallot { get; set; }
      public int NumberOfInvalidBallot { get; set; }

      public int Number { get; set; }
      public double Presence { get; set; }

      public double[] AllPresences { get; set; }
      public int[] AllNumberOfElectorsInList { get; set; }

      [XmlIgnore]
      public string ElectionCommitteeName { get; private set; }

      [XmlIgnore]
      public string Region { get; private set; }

      [XmlIgnore]
      public string Translit { get; private set; }

      [XmlIgnore]
      public string HrefHtmlFile { get; private set; }

      #endregion

      public List<Foo> Foos
      {
         get { return foos; }
         set
         {
            foos = value;            
         }
      }

      public Foo GetFoo(string name)
      {
         if (foosDictionary == null)
         {
            foosDictionary = foos.ToDictionary(f => f.Name, f => f, StringComparer.CurrentCultureIgnoreCase);
         }
         return foosDictionary[name];
      }

      public int[] GetUikNumbers()
      {
         uikNumbers = uikNumbers ??  Uiks.Split(',').Select(u => Convert.ToInt32(u)).ToArray();
         return uikNumbers;
      }

      public void NormalizeElectionCommitteeName(int year, string additional)
      {
         ElectionCommitteeName = TextProcessFunctions.GetNormalizedPlace(TextProcessFunctions.GetElectionCommitteeName(ElectionCommittee, year));
         Region = TextProcessFunctions.GetRegion(ElectionCommittee);

         Translit = TextProcessFunctions.Translit(ElectionCommitteeName);
         HrefHtmlFile = string.Format("<a href=\"../{0}{1}/{2}.html\">{3}</a>", Consts.Files, year + additional, Translit, ElectionCommitteeName);
      }
   }
}
