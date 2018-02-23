using System.Collections.Generic;

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
    }
}
