using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityVSModuleCommon;

namespace UnityVSModuleBuilder.GUI
{
    class LoggingFileService : LoggingService
    {
        private string LOG_FILE = Assembly.GetExecutingAssembly().GetName().Name + ".log";
        private StreamWriter file;
        

        public LoggingFileService()
        {
            file = new StreamWriter(LOG_FILE);
            Log("Logging Started @" + System.DateTime.Now.ToLongTimeString());
        }

        public void CloseLog()
        {
            Log("Logging Stopped @" + System.DateTime.Now.ToLongTimeString());
            file.Close();
        }

        public void Log(string message)
        {
            WriteText("INFO: " + message);
        }

        private void WriteText(string p)
        {
            try
            {
                file.WriteLine(p);
            }
            catch (Exception)
            {
                //Consumed... can't even log this T_T
            }
        }

        public void LogError(string message)
        {
            WriteText("ERROR: " + message);
        }

        public void LogError(string message, Exception e)
        {
            WriteText("ERROR: " + message);
            WriteText("EXCEPTION: " + e.ToString());
        }
    }
}
