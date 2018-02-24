using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Elections.XmlProcessing
{
   public class Foo
   {
      #region Fields

      private double? irregularity;

      #endregion

      [XmlAttribute]
      public string Name { get; set; }
      public double Min { get; set; }//percentage
      public double Max { get; set; }//percentage
      public double Value { get; set; }//percentage
      public int Number { get; set; }

      public double[] AllValues { get; set; }

      [XmlIgnore]
      public double Irregularity
      {
         get
         {
            if (!irregularity.HasValue) irregularity = Math.Round(GetIrregularity(), 2);
            return irregularity.Value;
         }
      }

      private double GetIrregularity()
      {
         var avgValue = AllValues.Where(v => !double.IsNaN(v)).Average();
         var sumOfDiffs = AllValues.Where(v => !double.IsNaN(v)).Sum(v => Math.Abs(avgValue - v));
         var sum = AllValues.Where(v => !double.IsNaN(v)).Sum();
         return 100.0 * sumOfDiffs / sum;
      }
   }
}
