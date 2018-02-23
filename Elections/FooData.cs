using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elections.Utility;

namespace Elections
{
   public class FooData
   {
      private string russianShort;

      public string EnglishShort { get; set; }

      public string RussianShort 
      { 
         get
         {
            return russianShort;
         }
         set 
         { 
            russianShort = value;
            EnglishShort = TextProcessFunctions.Translit(russianShort);
         }
      }

      public string RussianLong { get; set; }

      public bool IsMain { get; set; }

      public bool IsHiddenForIks { get; set; }

      public string Color { get; set; }

      public double Result { get;  set; }
   }
}
