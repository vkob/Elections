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
            Tiks.Add(tik);

            PlaceUtil.Add(this, tik);
        }
    }
}
