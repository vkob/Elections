namespace Data.Core
{
    public class ElectionFoo
    {
        public const int MinColUik = 4;
        public const string Flag = "Сумма";

        private const string ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВНЕСЕННЫХ_В_СПИСКИ              = "\"Число избирателей, внесенных в списки\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВНЕСЕННЫХ_В_СПИСКИ_ИЗБИРАТЕЛЕЙ  = "\"Число избирателей, внесенных в списки избирателей\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВНЕСЕННЫХ_В_СПИСОК_ИЗБИРАТЕЛЕЙ  = "\"Число избирателей, внесенных в список избирателей\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВНЕСЕННЫХ_В_СПИСОК              = "\"Число избирателей, внесенных в список\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВКЛЮЧЕННЫХ_В_СПИСКИ_ИЗБИРАТЕЛЕЙ = "\"Число избирателей, включенных в списки избирателей\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВКЛЮЧЕННЫХ_В_СПИСОК_ИЗБИРАТЕЛЕЙ =   "Число избирателей, включенных в список избирателей";

        private const string ЧИСЛО_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ПРОГОЛОСОВАВШИМ_ДОСРОЧНО 
            = "\"Число бюллетеней, выданных избирателям, проголосовавшим досрочно\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ПРОГОЛОСОВАВШИМ_ДОСРОЧНО 
            = "\"Число избирательных бюллетеней, выданных избирателям, проголосовавшим досрочно\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ДОСРОЧНО 
            = "\"Число избирательных бюллетеней, выданных  досрочно\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ПРОГОЛОСОВАВШИМ_ДОСРОЧНО1 
            =   "Число избирательных бюллетеней, выданных избирателям, проголосовавшим досрочно";

        private const string ЧИСЛО_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_НА_ИЗБИРАТЕЛЬНОМ_УЧАСТКЕ 
            = "\"Число бюллетеней, выданных избирателям на избирательном участке\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_В_ПОМЕЩЕНИЯХ_ДЛЯ_ГОЛОСОВАНИЯ 
            = "\"Число избирательных бюллетеней, выданных избирателям в помещениях для голосования\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_В_ПОМЕЩЕНИИ_ДЛЯ_ГОЛОСОВАНИЯ
            = "\"Число избирательных бюллетеней, выданных избирателям в помещении для голосования\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_В_ДЕНЬ_ГОЛОСОВАНИЯ 
            = "\"Число избирательных бюллетеней, выданных в день голосования\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_В_ПОМЕЩЕНИЯХ_ДЛЯ_ГОЛОСОВАНИЯ_В_ДЕНЬ_ГОЛОСОВАНИЯ 
            = "\"Число избирательных бюллетеней, выданных в помещениях для голосования в день голосования\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_В_ПОМЕЩЕНИИ_ДЛЯ_ГОЛОСОВАНИЯ_В_ДЕНЬ_ГОЛОСОВАНИЯ 
              = "Число избирательных бюллетеней, выданных в помещении для голосования в день голосования";


        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ВНЕ_ПОМЕЩЕНИЙ_ДЛЯ_ГОЛОСОВАНИЯ_В_ДЕНЬ_ГОЛОСОВАНИЯ 
            = "\"Число избирательных бюллетеней, выданных вне помещений для голосования в день голосования\"";
        private const string ЧИСЛО_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ПРОГОЛОСОВАВШИМ_ВНЕ_ПОМЕЩЕНИЯ_ДЛЯ_ГОЛОСОВАНИЯ 
            = "\"Число бюллетеней, выданных избирателям, проголосовавшим вне помещения для голосования\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ВНЕ_ПОМЕЩЕНИЙ_ДЛЯ_ГОЛОСОВАНИЯ 
            = "\"Число избирательных бюллетеней, выданных избирателям вне помещений для голосования\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ВНЕ_ПОМЕЩЕНИЯ_ДЛЯ_ГОЛОСОВАНИЯ 
            = "\"Число избирательных бюллетеней, выданных избирателям вне помещения для голосования\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ВНЕ_ПОМЕЩЕНИЯ 
            = "\"Число избирательных бюллетеней, выданных вне помещения\"";
        private const string ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ВНЕ_ПОМЕЩЕНИЯ_ДЛЯ_ГОЛОСОВАНИЯ_В_ДЕНЬ_ГОЛОСОВАНИЯ 
            =   "Число избирательных бюллетеней, выданных вне помещения для голосования в день голосования";

        private const string ЧИСЛО_НЕДЕЙСТВИТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ               = "Число недействительных бюллетеней";
        private const string ЧИСЛО_НЕДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ = "Число недействительных избирательных бюллетеней";

        private const string ЧИСЛО_ДЕЙСТВИТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ               = "Число действительных бюллетеней";
        private const string ЧИСЛО_ДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ = "Число действительных избирательных бюллетеней";


        public static ElectionFoo Duma2003;
        public static ElectionFoo Duma2007;
        public static ElectionFoo Duma2011;
        public static ElectionFoo Duma2016;

        public static ElectionFoo President2004;
        public static ElectionFoo President2008;
        public static ElectionFoo President2012;

        static ElectionFoo()
        {
            Duma2003 = new ElectionFoo()
            {
                RowLocalElectionCommittee = 8,
                NumberOfElectorsInList = new Label(ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВНЕСЕННЫХ_В_СПИСКИ, 9),
                NumberOfEarlier = new Label(ЧИСЛО_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ПРОГОЛОСОВАВШИМ_ДОСРОЧНО, 11),
                NumberOfIn = new Label(ЧИСЛО_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_НА_ИЗБИРАТЕЛЬНОМ_УЧАСТКЕ, 12),
                NumberOfOut = new Label(ЧИСЛО_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ПРОГОЛОСОВАВШИМ_ВНЕ_ПОМЕЩЕНИЯ_ДЛЯ_ГОЛОСОВАНИЯ, 13),
                NumberOfInvalidBallot = new Label(ЧИСЛО_НЕДЕЙСТВИТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 17),
                NumberOfValidBallot = new Label(ЧИСЛО_ДЕЙСТВИТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 18),
                MinRowNumberForFactions = 27,
            };

            Duma2007 = new ElectionFoo()
            {
                RowLocalElectionCommittee = 8,
                NumberOfElectorsInList = new Label(ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВНЕСЕННЫХ_В_СПИСКИ_ИЗБИРАТЕЛЕЙ, 9),
                NumberOfEarlier = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ПРОГОЛОСОВАВШИМ_ДОСРОЧНО, 11),
                NumberOfIn = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_В_ПОМЕЩЕНИЯХ_ДЛЯ_ГОЛОСОВАНИЯ, 12),
                NumberOfOut = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ВНЕ_ПОМЕЩЕНИЙ_ДЛЯ_ГОЛОСОВАНИЯ, 13),
                NumberOfInvalidBallot = new Label(ЧИСЛО_НЕДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 17),
                NumberOfValidBallot = new Label(ЧИСЛО_ДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 18),
                MinRowNumberForFactions = 29,
            };

            Duma2011 = new ElectionFoo()
            {
                RowLocalElectionCommittee = 8,
                NumberOfElectorsInList = new Label(ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВНЕСЕННЫХ_В_СПИСОК_ИЗБИРАТЕЛЕЙ, 9),
                NumberOfEarlier = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ПРОГОЛОСОВАВШИМ_ДОСРОЧНО, 11),
                NumberOfIn = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_В_ПОМЕЩЕНИИ_ДЛЯ_ГОЛОСОВАНИЯ, 12),
                NumberOfOut = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ВНЕ_ПОМЕЩЕНИЯ_ДЛЯ_ГОЛОСОВАНИЯ, 13),
                NumberOfInvalidBallot = new Label(ЧИСЛО_НЕДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 17),
                NumberOfValidBallot = new Label(ЧИСЛО_ДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 18),
                MinRowNumberForFactions = 28
            };

            Duma2016 = new ElectionFoo()
            {
                RowLocalElectionCommittee = 8,
                NumberOfElectorsInList = new Label("\"Число избирателей, внесенных в список избирателей на момент окончания голосования\"", 9),
                NumberOfEarlier = new Label("\"Число избирательных бюллетеней, выданных избирателям, проголосовавшим досрочно\"", 11),
                NumberOfIn = new Label("\"Число избирательных бюллетеней, выданных в помещении для голосования в день голосования\"", 12),
                NumberOfOut = new Label("\"Число избирательных бюллетеней, выданных вне помещения для голосования в день голосования\"", 13),
                NumberOfInvalidBallot = new Label("\"Число недействительных избирательных бюллетеней\"", 17),
                NumberOfValidBallot = new Label("\"Число действительных избирательных бюллетеней\"", 18),
                Portable = new Label("\"Число избирательных бюллетеней, содержащихся в переносных ящиках для голосования\"", 15),
                Stationary = new Label("\"Число избирательных бюллетеней, содержащихся в стационарных ящиках для голосования\"", 16),
                MinRowNumberForFactions = 28
            };

            President2004 = new ElectionFoo()
            {
                RowLocalElectionCommittee = 7,
                NumberOfElectorsInList = new Label(ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВНЕСЕННЫХ_В_СПИСОК, 8),
                NumberOfEarlier = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ДОСРОЧНО, 10),
                NumberOfIn = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_В_ДЕНЬ_ГОЛОСОВАНИЯ, 11),
                NumberOfOut = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ВНЕ_ПОМЕЩЕНИЯ, 12),
                NumberOfInvalidBallot = new Label(ЧИСЛО_НЕДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 16),
                NumberOfValidBallot = new Label(ЧИСЛО_ДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 17),
                MinRowNumberForFactions = 26
            };

            President2008 = new ElectionFoo()
            {
                RowLocalElectionCommittee = 11,
                NumberOfElectorsInList = new Label(ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВКЛЮЧЕННЫХ_В_СПИСКИ_ИЗБИРАТЕЛЕЙ, 12),
                NumberOfEarlier = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ПРОГОЛОСОВАВШИМ_ДОСРОЧНО, 14),
                NumberOfIn = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_В_ПОМЕЩЕНИЯХ_ДЛЯ_ГОЛОСОВАНИЯ_В_ДЕНЬ_ГОЛОСОВАНИЯ, 15),
                NumberOfOut = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ВНЕ_ПОМЕЩЕНИЙ_ДЛЯ_ГОЛОСОВАНИЯ_В_ДЕНЬ_ГОЛОСОВАНИЯ, 16),
                NumberOfInvalidBallot = new Label(ЧИСЛО_НЕДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 20),
                NumberOfValidBallot = new Label(ЧИСЛО_ДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 21),
                MinRowNumberForFactions = 32
            };

            President2012 = new ElectionFoo()
            {
                RowLocalElectionCommittee = 11,
                NumberOfElectorsInList = new Label(ЧИСЛО_ИЗБИРАТЕЛЕЙ_ВКЛЮЧЕННЫХ_В_СПИСОК_ИЗБИРАТЕЛЕЙ, 12),
                NumberOfEarlier = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ИЗБИРАТЕЛЯМ_ПРОГОЛОСОВАВШИМ_ДОСРОЧНО1, 14),
                NumberOfIn = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_В_ПОМЕЩЕНИИ_ДЛЯ_ГОЛОСОВАНИЯ_В_ДЕНЬ_ГОЛОСОВАНИЯ, 15),
                NumberOfOut = new Label(ЧИСЛО_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ_ВЫДАННЫХ_ВНЕ_ПОМЕЩЕНИЯ_ДЛЯ_ГОЛОСОВАНИЯ_В_ДЕНЬ_ГОЛОСОВАНИЯ, 16),
                NumberOfInvalidBallot = new Label(ЧИСЛО_НЕДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 20),
                NumberOfValidBallot = new Label(ЧИСЛО_ДЕЙСТВИТЕЛЬНЫХ_ИЗБИРАТЕЛЬНЫХ_БЮЛЛЕТЕНЕЙ, 21),
                Portable = new Label("Число избирательных бюллетеней в переносных ящиках для голосования", 18),
                Stationary = new Label("Число бюллетеней в стационарных ящиках для голосования", 19),
                MinRowNumberForFactions = 31
            };
        }

        public int RowLocalElectionCommittee;

        public Label NumberOfElectorsInList;
        public Label NumberOfEarlier;
        public Label NumberOfIn;
        public Label NumberOfOut;
        public Label NumberOfValidBallot;
        public Label NumberOfInvalidBallot;
        public Label Portable;
        public Label Stationary;

        public int MinRowNumberForFactions;

        public static ElectionFoo GetFoo(string fileName)
        {
            ElectionFoo electionFoo = null;

            if (fileName.EndsWith(Consts.Ending2003Txt))
            {
                electionFoo = Duma2003;
            }
            else
            if (fileName.EndsWith(Consts.Ending2007Txt))
            {
                electionFoo = Duma2007;
            }
            else
            if (fileName.EndsWith(Consts.Ending2011Txt))
            {
                electionFoo = Duma2011;
            }
            else
            if (fileName.EndsWith(Consts.Ending2004Txt))
            {
                electionFoo = President2004;
            }
            else
            if (fileName.EndsWith(Consts.Ending2008Txt))
            {
                electionFoo = President2008;
            }
            else
            if (fileName.EndsWith(Consts.Ending2012Txt))
            {
                electionFoo = President2012;
            }
            else
            if (fileName.EndsWith(Consts.Ending2016Txt))
            {
                electionFoo = Duma2016;
            }

            return electionFoo;
        }
    }
}
