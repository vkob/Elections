using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Core;
using Elections.Utility;

namespace Elections
{
    public static partial class ExcelConsts
    {
        public static ElectionFoo Astrahan2009;
        public static ElectionFoo Astrahan2012;

        static ExcelConsts()
        {
            Astrahan2009 = new ElectionFoo()
            {
                RowLocalElectionCommittee = 8,
                NumberOfElectorsInList = new Label("Число избирателей, внесенных в списки", 9),
                NumberOfEarlier = new Label("Число бюллетеней, выданных избирателям, проголосовавшим досрочно, всего", 11),
                NumberOfIn = new Label("Число бюллетеней, выданных избирателям на избирательных участках", 13),
                NumberOfOut = new Label("Число бюллетеней, выданных избирателям, проголосовавшим вне помещения для голосования", 14),
                NumberOfInvalidBallot = new Label("Число недействительных бюллетеней", 18),
                NumberOfValidBallot = new Label("Число действительных бюллетеней", 19),
                MinRowNumberForFactions = 23
            };

            Astrahan2012 = new ElectionFoo()
            {
                RowLocalElectionCommittee = 8,
                NumberOfElectorsInList = new Label("Число избирателей, внесенных в списки", 9),
                NumberOfEarlier = new Label("Число бюллетеней, выданных избирателям, проголосовавшим досрочно", 11),
                NumberOfIn = new Label("Число бюллетеней, выданных избирателям на избирательных участках", 12),
                NumberOfOut = new Label("Число бюллетеней, выданных избирателям, проголосовавшим вне помещения для голосования", 13),
                NumberOfInvalidBallot = new Label("Число недействительных бюллетеней", 17),
                NumberOfValidBallot = new Label("Число действительных бюллетеней", 18),
                MinRowNumberForFactions = 22
            };
        }
    }
}
