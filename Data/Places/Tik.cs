using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Data.Places
{
    public class Tik : IElectItem 
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

        public List<Uik> Uiks { get; set; }

        public List<Pair> Pairs { get; set; }

        public void Check()
        {
            Debug.Assert(NumberOfVoters == Uiks.Sum(u => u.NumberOfVoters), "NumberOfVoters");
            Debug.Assert(NumberOfEarlier == Uiks.Sum(u => u.NumberOfEarlier), "NumberOfEarlier");
            Debug.Assert(NumberOfInside == Uiks.Sum(u => u.NumberOfInside), "NumberOfInside");
            Debug.Assert(NumberOfOutside == Uiks.Sum(u => u.NumberOfOutside), "NumberOfOutside");
            Debug.Assert(Portable == Uiks.Sum(u => u.Portable), "Portable");
            Debug.Assert(Stationary == Uiks.Sum(u => u.Stationary), "Stationary");
            Debug.Assert(Valid == Uiks.Sum(u => u.Valid), "Valid");
            Debug.Assert(InValid == Uiks.Sum(u => u.InValid), "InValid");
        }
    }
}
