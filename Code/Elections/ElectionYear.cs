using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Elections
{
    public class ElectionYear
    {
        #region Private

        private const string DirElectionInfoDuma = "ElectionInfoDuma";
        private const string DirElectionInfoPresident = "ElectionInfoPresident";

        public const string CaptionDiagramDuma = "Результаты выборов в Гос. Думу, {0} год.\n{1}";
        public const string CaptionDiagramPresident = "Результаты выборов Президента, {0} год.\n{1}";

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
            PatternExt = String.Format("*{0}.txt", year);
        }

        #endregion

        #region Properties

        public string CaptionDiagram { get; private set; }
        public int Year { get; private set; }
        public ElectionType ElectionType { get; private set; }
        public double Presence { get; private set; }
        public string DirElectionInfo { get; private set; }
        public string Result { get; private set; }

        public string FullPath => Path.Combine(Data.Core.Consts.ResultsPath, Result);

        public string PatternExt { get; private set; }
        public string MainTitle { get; private set; }

        public FooData[] FooData { get; set; }

        #endregion

        public string ElectionCaption()
        {
            var caption = (ElectionType == ElectionType.Duma)
                ? "В Думу"
                : "Президента";

            return caption;
        }
        public static ElectionYear GetElectionYear(string year)
        {
            switch (year)
            {
                case "2003":
                    return Consts.ElectionYear2003;
                case "2007":
                    return Consts.ElectionYear2007;
                case "2011":
                    return Consts.ElectionYear2011;
                case "2016":
                    return Consts.ElectionYear2016;
                case "2004":
                    return Consts.ElectionYear2004;
                case "2008":
                    return Consts.ElectionYear2008;
                case "2012":
                    return Consts.ElectionYear2012;
                case "2018":
                    return Consts.ElectionYear2018;
            }

            return null;
        }
    }
}
