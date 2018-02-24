using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elections.Utility
{
   public class Triple<T1, T2, T3>
   {
      public Triple(T1 t1, T2 t2, T3 t3)
      {
         First = t1;
         Second = t2;
         Third = t3;
      }

      public T1 First { get; set; }
      public T2 Second { get; set; }
      public T3 Third { get; set; }
   }
}
