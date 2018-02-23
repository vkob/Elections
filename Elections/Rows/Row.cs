using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elections.XmlProcessing;

namespace Elections.Rows
{
   public class Row
   {
      public const int NoDelta = -200;
      private const int NoResult = -1;

      #region Fields

      public double Irregularity;
     
      private double deltaValue = NoDelta;
      private double deltaPresence = NoDelta;

      private double resultPrevious = NoResult;
      private double resultLast = NoResult;

      public double presencePrevious = NoResult;
      public double presenceLast = NoResult;

      public HashSet<string> MaxValues = new HashSet<string>();

      #endregion


      #region Constructor

      public Row(string main, Dictionary<string, FooData> others, string href, string regionName, string electionCommitteeName, Election electionLast, Election electionPrevious)
      {
         ElectionCommitteeName = electionCommitteeName;
         Irregularity = electionLast.GetFoo(main).Irregularity;
         Main = main;
         Others = others;
         Href = href;
         RegionName = regionName;

         ElectionLast = electionLast;
         ElectionPrevious = electionPrevious;

         resultLast = ElectionLast.GetFoo(main).Value;
         presenceLast = ElectionLast.Presence;

         double maxValue = -1;
         var foosAllowed = electionLast.Foos.Where(foo => foo.Name == main || others.ContainsKey(foo.Name)).ToList();
         foosAllowed.ForEach(foo => { if (foo.Value > maxValue) maxValue = foo.Value; });
         var foos = foosAllowed.Where(foo => Math.Abs(foo.Value - maxValue) < 0.001).ToList();

         foreach (var foo in foos)
         {
            MaxValues.Add(foo.Name);
         }
         
         if (electionPrevious != null)
         {
            deltaPresence = electionLast.Presence - electionPrevious.Presence;
            resultPrevious = ElectionPrevious.GetFoo(main).Value;
            deltaValue = resultLast - resultPrevious;
            presencePrevious = ElectionPrevious.Presence;
         }
      }

      #endregion

      #region Properties

      public double ResultLast { get { return resultLast; } }
      public double ResultPrevious { get { return resultPrevious; } }

      public double PresenceLast { get { return presenceLast; } }
      public double PresencePrevious { get { return presencePrevious; } }

      public string Main { get; private set; }
      public Dictionary<string, FooData> Others { get; private set; }

      public double Delta { get { return deltaValue; } }
      public double PresenceDelta { get { return deltaPresence; } }

      public string Href { get; private set; }
      public string RegionName { get; private set; }
      public string ElectionCommitteeName { get; private set; }

      public Election ElectionPrevious { get; private set; }
      public Election ElectionLast { get; private set; }

      #endregion

      #region Methods

      public double GetOtherValue(string key)
      {
         return ElectionLast.GetFoo(key).Value;
      }

      #endregion
   }
}
