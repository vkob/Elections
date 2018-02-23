using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Data.Places
{
    public class District : IElectItem
    {
        public int NumberOfVoters { get; set; }
        public int NumberOfEarlier { get; set; }
        public int NumberOfInside { get; set; }
        public int NumberOfOutside { get; set; }
        public int Portable { get; set; }
        public int Stationary { get; set; }
        public int Valid { get; set; }
        public int InValid { get; set; }
        public string Name { get; set; }

        public List<Tik> Tiks { get; set; }

        public List<Pair> Pairs { get; set; }

        public void Check()
        {
            PlaceUtil.Check(this, Tiks.Cast<IElectItem>().ToList());
        }

        public void Add(Tik tik)
        {
            NumberOfVoters += tik.NumberOfVoters;
            NumberOfEarlier += tik.NumberOfEarlier;
            NumberOfInside += tik.NumberOfInside;
            NumberOfOutside += tik.NumberOfOutside;
            Portable += tik.Portable;
            Stationary += tik.Stationary;
            Valid += tik.Valid;
            InValid += tik.InValid;
        }
    }
}
