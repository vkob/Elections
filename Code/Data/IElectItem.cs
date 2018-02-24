using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public interface IElectItem 
    {
        int NumberOfVoters { get; set; }

        int NumberOfEarlier { get; set; }

        int NumberOfInside { get; set; }
        int NumberOfOutside { get; set; }

        int Portable { get; set; }
        int Stationary { get; set; }

        int Valid { get; set; }
        int InValid { get; set; }

        string Name { get; set; }
        List<Pair> Pairs { get; set; }

        void Check();
    }
}
