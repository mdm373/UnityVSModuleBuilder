using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleBuilder.Drivers;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder
{
    
    public class Driver
    {
        /*
        * Swap out provided driver class to run alternate drivers
        */
        private static List<Drivable> GetDriverinstances()
        {
            List<Drivable> drivers= new List<Drivable>();
            //drivers.Add(new FileSystemDriver());
            drivers.Add(new TemplateProjectBuilderDriver());
            //drivers.Add(new VSModuleDelegateDriver());
            return drivers;
        }

        public static int Main(string[] args)
        {
            int returnCode = 0;
            try
            {
                foreach(Drivable driver in GetDriverinstances()){
                    returnCode = driver.Drive(args);
                }   
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
