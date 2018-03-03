using System;
using System.Collections.Generic;

namespace Data.Core
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
