﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.Logging
{
    public interface LoggingService
    {
        void Log(string message);

        void LogError(string message);

        void LogError(string message, Exception e);
    }
}