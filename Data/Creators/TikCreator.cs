using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Data.Core;
using Data.Places;

namespace Data
{
    public class TikCreator
    {
        public ElectionFoo GetFoo(string fileName)
        {
            ElectionFoo electionFoo = null;

            if (fileName.EndsWith(Consts.Ending2003Txt))
            {
                electionFoo = ElectionFoo.Duma2003;
            }
            else
            if (fileName.EndsWith(Consts.Ending2007Txt))
            {
                electionFoo = ElectionFoo.Duma2007;
            }
            else
            if (fileName.EndsWith(Consts.Ending2011Txt))
            {
                electionFoo = ElectionFoo.Duma2011;
            }
            else
            if (fileName.EndsWith(Consts.Ending2004Txt))
            {
                electionFoo = ElectionFoo.President2004;
            }
            else
            if (fileName.EndsWith(Consts.Ending2008Txt))
            {
                electionFoo = ElectionFoo.President2008;
            }
            else
            if (fileName.EndsWith(Consts.Ending2012Txt))
            {
                electionFoo = ElectionFoo.President2012;
            }

            return electionFoo;
        }

        const int captionIndex = 1;

        public Tik Create(string fileName)
        {
            var electionFoo = GetFoo(fileName);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var lineCounter = 0;

            var firstTime = true;

            var tik = new Tik(){Name = StringUtils.GetTikName(fileName)};

            using (var sr = new StreamReader(fileName, Encoding.GetEncoding(1251)))
            {
                while (!sr.EndOfStream)
                {
                    lineCounter++;
                    var line = sr.ReadLine();

                    if (lineCounter == electionFoo.NumberOfElectorsInList.LineNumber)
                    {
                        Set(tik, line, electionFoo.NumberOfElectorsInList, (x, v) => x.NumberOfVoters = v);
                    }
                    if (lineCounter == electionFoo.NumberOfEarlier.LineNumber)
                    {
                        Set(tik, line, electionFoo.NumberOfEarlier, (x, v) => x.NumberOfEarlier = v);
                    }
                    if (lineCounter == electionFoo.NumberOfIn.LineNumber)
                    {
                        Set(tik, line, electionFoo.NumberOfIn, (x, v) => x.NumberOfInside = v);
                    }
                    if (lineCounter == electionFoo.NumberOfOut.LineNumber)
                    {
                        Set(tik, line, electionFoo.NumberOfOut, (x, v) => x.NumberOfOutside = v);
                    }
                    if (lineCounter == electionFoo.NumberOfValidBallot.LineNumber)
                    {
                        Set(tik, line, electionFoo.NumberOfValidBallot, (x, v) => x.Valid = v);
                    }
                    if (lineCounter == electionFoo.NumberOfInvalidBallot.LineNumber)
                    {
                        Set(tik, line, electionFoo.NumberOfInvalidBallot, (x, v) => x.InValid = v);
                    }
                    if (lineCounter == electionFoo.Stationary.LineNumber)
                    {
                        Set(tik, line, electionFoo.Stationary, (x, v) => x.Stationary = v);
                    }
                    if (lineCounter == electionFoo.Portable.LineNumber)
                    {
                        Set(tik, line, electionFoo.Portable, (x, v) => x.Portable = v);
                    }

                    if (lineCounter == electionFoo.RowLocalElectionCommittee)
                    {
                        tik.Uiks = GetUiks(line);
                    }
                    if (lineCounter >= electionFoo.MinRowNumberForFactions)
                    {
                        //if (firstTime)
                        //{
                        //    CheckZero();
                        //    firstTime = false;
                        //}
                        //if (!GetNewPartyData(line, sr, fileName)) break;
                    }
                }
            }

            //Presence = 100 * ((double)(NumberOfEarlier + NumberOfIn + NumberOfOut)) / NumberOfElectorsInList;
            //AllPresences = Enumerable
            //   .Range(0, allNumberOfEarlier.Count)
            //   .Select(
            //      i =>
            //      Math.Round(
            //         100.0 * (allNumberOfEarlier[i] + allNumberOfIn[i] + allNumberOfOut[i]) /
            //         AllNumberOfElectorsInList[i], 2))
            //   .ToArray();

            //for (int i = 0; i < AllPresences.Length; i++)
            //{
            //    if (double.IsNaN(AllPresences[i]))
            //    {
            //        foreach (var electionCommittee in partiesData)
            //        {
            //            electionCommittee.Value.LocalElectionCommittees[i].Percent = double.NaN;
            //        }
            //        //Trace.WriteLine(uiks[i]);
            //    }
            //}

            stopWatch.Stop();
             //Trace.WriteLine(stopWatch.Elapsed);

            return tik;
        }

        private void Set(Tik tik, string line, Label label, Action<IElectItem, int> setFunc)
        {
            var parts = line.Split(new[] { Consts.Tab }, StringSplitOptions.RemoveEmptyEntries);
            Utils.AssertCaption(parts[captionIndex], label.Caption);

            setFunc(tik, Convert.ToInt32(parts[captionIndex + 1]));

            var res = Enumerable.Range(captionIndex + 2, parts.Length - captionIndex - 2)
                .Select(i => Convert.ToInt32(parts[i]))
                .ToList();

            foreach (var i in Enumerable.Range(0, res.Count))
            {
                setFunc(tik.Uiks[i], res[i]);
            }
        }

        private List<Uik> GetUiks(string line)
        {
            var parts = line.Split(new[] { Consts.Tab }, StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert(parts[0] == ElectionFoo.Flag, "Sum was not found");
            var uiksNames = Enumerable.Range(1, parts.Length - 1).Select(i => parts[i]).ToArray();
            var uiks = new List<Uik>(uiksNames.Length);
            for (int i = 0; i < uiksNames.Length; i++)
            {
                uiks.Add(new Uik {Name = uiksNames[i]});
            }

            return uiks;
        }
    }
}
