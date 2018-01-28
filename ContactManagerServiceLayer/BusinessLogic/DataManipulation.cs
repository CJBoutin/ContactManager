﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ContactManagerServiceLayer.BusinessLogic
{
    public class DataManipulation
    {
        public static string FormatForSql(string str)
        {
            int a = -1;
            if(str == "")
            {
                str = "null";
            }
            else if(int.TryParse(str, out a))
            {
                return str;
            }
            else
            {
                str = string.Format("'{0}'", str);
            }

            return str;
        }
    }
}