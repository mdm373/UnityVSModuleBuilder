using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityVSModuleBuilder;
using UnityVSModuleBuilder.FileSystem;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder.Drivers
{
    class FileSystemDriver : Drivable
    {

        public int Drive(string[] args)
        {
            FileSystemController fsController = new FileSystemControllerImpl();
            fsController.DoFullDirectoryCopy(@"C:\Temp\DriverTest\From", @"C:\Temp\DriverTest\To");
            return 0;
        }
    }
}
