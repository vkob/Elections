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
        public District Create(string districtPath, string filePattern)
        {
            List<string> pathes = new FileList(districtPath, filePattern).GetList();
            var tikCreator = new TikCreator();

            if (pathes.Count == 0) return null;

            var district = new District {Tiks = new List<Tik>(), Name = StringUtils.GetDistrictName(pathes[0])};

            foreach (var tikPath in pathes)
            {
                district.Add(tikCreator.Create(tikPath));
            }
            
            return district;
        }
    }
}
