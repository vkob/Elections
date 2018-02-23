using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Data.Places
{
    class PlaceUtil
    {
        public static void Check(IElectItem item, IList<IElectItem> items)
        {
            Debug.Assert(item.NumberOfVoters == items.Sum(u => u.NumberOfVoters), "NumberOfVoters");
            Debug.Assert(item.NumberOfEarlier == items.Sum(u => u.NumberOfEarlier), "NumberOfEarlier");
            Debug.Assert(item.NumberOfInside == items.Sum(u => u.NumberOfInside), "NumberOfInside");
            Debug.Assert(item.NumberOfOutside == items.Sum(u => u.NumberOfOutside), "NumberOfOutside");
            Debug.Assert(item.Portable == items.Sum(u => u.Portable), "Portable");
            Debug.Assert(item.Stationary == items.Sum(u => u.Stationary), "Stationary");
            Debug.Assert(item.Valid == items.Sum(u => u.Valid), "Valid");
            Debug.Assert(item.InValid == items.Sum(u => u.InValid), "InValid");
        }
        
        public static void Add(IElectItem parent, IElectItem child)
        {
            parent.NumberOfVoters += child.NumberOfVoters;
            parent.NumberOfEarlier += child.NumberOfEarlier;
            parent.NumberOfInside += child.NumberOfInside;
            parent.NumberOfOutside += child.NumberOfOutside;
            parent.Portable += child.Portable;
            parent.Stationary += child.Stationary;
            parent.Valid += child.Valid;
            parent.InValid += child.InValid;
        }
    }
}
