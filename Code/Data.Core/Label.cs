using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Core
{
    public class Label
    {
        public Label(string caption, int lineNumber)
        {
            Caption = caption;
            LineNumber = lineNumber;
        }

        public string Caption { get; set; } 

        public int LineNumber { get; set; }
    }
}
