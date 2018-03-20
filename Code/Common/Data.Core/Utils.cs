using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Data.Core
{
    public class Utils
    {
        public static void AssertCaption(string textFromFile, string caption)
        {
            Debug.Assert(textFromFile == caption || $"\"{textFromFile}\"" == caption || $"\"{caption}\"" == textFromFile, caption);   //todo      
        }
    }
}
