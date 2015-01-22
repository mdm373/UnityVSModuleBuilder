using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityVSModuleBuilder.Drivers;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder
{
    
    public class Driver
    {
        /*
        * Swap out provided driver class to run alternate drivers
        */
        private static Drivable GetDriverinstance()
        {
            //return new FileSystemDriver();
            return new TemplateProjectBuilderDriver();
        }

        public static int Main(string[] args)
        {
            int returnCode = 0;
            try
            {
                returnCode = GetDriverinstance().Drive(args);
            }
            catch (Exception e)
            {
                Logger.LogError("Exception running driver.", e);
                returnCode = 1337;
            }
            Console.Out.WriteLine("Driver Finished. Press Any Key To Continue.");
            Console.In.ReadLine();
            return returnCode;
        }
    }
}
