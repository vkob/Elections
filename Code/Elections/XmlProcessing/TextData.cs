using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Elections.XmlProcessing
{
    public class TextData
    {
        public string ElectionCommitteeName { get; set; }

        public string Region { get; set; }

        public string Translit { get; set; }

        public string HrefHtmlFile { get; set; }
    }
}
