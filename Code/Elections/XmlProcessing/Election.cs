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
        public TextData TextData { get; set; }

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
            uikNumbers = uikNumbers ?? Uiks.Split(',').Select(u => Convert.ToInt32(u)).ToArray();
            return uikNumbers;
        }

        public void NormalizeElectionCommitteeName(int year)
        {
            TextData = TextProcessFunctions.NormalizeElectionCommitteeName(ElectionCommittee, year);
        }
    }
}
