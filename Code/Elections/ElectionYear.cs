using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elections
{
   public class ElectionYear
   {
      #region Private

      private const string DirElectionInfoDuma = "ElectionInfoDuma";
      private const string DirElectionInfoPresident = "ElectionInfoPresident";
      private const string DirElectionInfoAstrahan = "ElectionInfoAstrahan";

      private const string CaptionDiagramDuma = "Результаты выборов в Гос. Думу, {0} год.\n{1}";
      private const string CaptionDiagramPresident = "Результаты выборов Президента, {0} год.\n{1}";

      private const string MainTitleDuma = "в Государственную Думу";
      private const string MainTitlePresident = "Президента";

      #endregion

      #region Constructor

      public ElectionYear(ElectionType electionType, int year, double presence, FooData[] fooData)
      {
         FooData = fooData;
         ElectionType = electionType;
         Year = year;
         Presence = presence;

         switch (electionType)
         {
            case ElectionType.Duma:
               DirElectionInfo = DirElectionInfoDuma;
               Result = Data.Core.Consts.ResultsDuma;
               CaptionDiagram = CaptionDiagramDuma;
               MainTitle = MainTitleDuma;
               break;
            case ElectionType.President:
               DirElectionInfo = DirElectionInfoPresident;
               Result = Data.Core.Consts.ResultsPresident;
               CaptionDiagram = CaptionDiagramPresident;
               MainTitle = MainTitlePresident;
               break;
            default:
               throw new Exception("Unknown type");
         }
         PatternExt = string.Format("*{0}.txt", year);
      }

      #endregion

      #region Properties

      public string CaptionDiagram { get; private set; }
      public int Year { get; private set; }
      public ElectionType ElectionType { get; private set; }
      public double Presence { get; private set; }
      public string DirElectionInfo { get; private set; }
      public string Result { get; private set; }
      public string PatternExt { get; private set; }
      public string MainTitle { get; private set; }

      public FooData[] FooData { get; set; }

      #endregion
   }
}
