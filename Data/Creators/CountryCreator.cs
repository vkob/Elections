using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Data.Places;

namespace Data.Creators
{
    public class CountryCreator
    {
        public Country Create(string countryPath, string filePattern)
        {
            var tikPaths = new DirectoryInfo(countryPath).GetDirectories();

            var country = new Country();

            foreach (var tikPath in tikPaths)
            {
                var district = new DistrictCreator().Create(tikPath.FullName, filePattern);
                
                if (district !=null) country.Add(district);
            }

            return country;
        }
    }
}
