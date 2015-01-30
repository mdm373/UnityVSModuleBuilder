﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleCommon.FileSystem
{
    public class Path
    {
        private Path() { }
        public static String Combine(String first, String second)
        {
            return System.IO.Path.Combine(first, second);
        }

        public static char DirectorySeparatorChar {
            get
            {
                return System.IO.Path.DirectorySeparatorChar;
            }

        }
    }
}
