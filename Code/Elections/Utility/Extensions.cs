using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elections.Utility
{
   public static class Extensions
   {
      public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
      {
         foreach (var element in self)
         {
            action(element);
         }
      }
   }
}
