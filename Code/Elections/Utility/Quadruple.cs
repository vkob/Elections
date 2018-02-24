using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elections.Utility
{
   public class Quadruple<T1, T2, T3, T4>
   {
      public Quadruple(T1 t1, T2 t2, T3 t3, T4 t4)
      {
         First = t1;
         Second = t2;
         Third = t3;
         Fourth = t4;
      }

      public T1 First { get; set; }
      public T2 Second { get; set; }
      public T3 Third { get; set; }
      public T4 Fourth { get; set; }
   }
}
