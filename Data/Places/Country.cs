using System.Collections.Generic;
using System.Linq;

namespace Data.Places
{
    public class Country : IElectItem
    {
        public List<District> Districts { get; set; }

        public List<Pair> Pairs { get; set; }

        public int NumberOfVoters { get; set; }
        public int NumberOfEarlier { get; set; }
        public int NumberOfInside { get; set; }
        public int NumberOfOutside { get; set; }
        public int Portable { get; set; }
        public int Stationary { get; set; }
        public int Valid { get; set; }
        public int InValid { get; set; }
        public string Name { get; set; }
        
        public void Check()
        {
            PlaceUtil.Check(this, Districts.Cast<IElectItem>().ToList());
        }

        public void Add(District district)
        {
            NumberOfVoters += district.NumberOfVoters;
            NumberOfEarlier += district.NumberOfEarlier;
            NumberOfInside += district.NumberOfInside;
            NumberOfOutside += district.NumberOfOutside;
            Portable += district.Portable;
            Stationary += district.Stationary;
            Valid += district.Valid;
            InValid += district.InValid;
        }
    }
}
