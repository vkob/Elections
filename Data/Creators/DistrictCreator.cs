using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Data.Places;

namespace Data.Creators
{
    public class DistrictCreator
    {
        public District Create(List<string> pathes)
        {
            var tikCreator = new TikCreator();

            var district = new District {Tiks = new List<Tik>(), Name = StringUtils.GetDistrictName(pathes[0])};

            foreach (var path in pathes)
            {
                district.Add(tikCreator.Create(path));
            }
            
            return district;
        }

     
    }
}
