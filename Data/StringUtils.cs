﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class StringUtils
    {
        public static string GetTikName(string fileName)
        {
            var split = fileName.Split('\\');
            return split[split.Length - 3];
        }
    }
}