using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleCommon;

namespace UnityVSModuleBuilder.Drivers
{
    class RegistryDriver : Drivable
    {
        private const string REG_LOCATION = @"Software\UnityVSmoduleBuilder";
        private const string INSTALL_LOC_NAME = "install-location";

        public int Drive(string[] args)
        {
            RegistryController reg = new RegistryControllerImpl();
            String value = reg.GetRegistryKey<String>(RegKeyType.HK_LOCAL_MACHINE, REG_LOCATION, INSTALL_LOC_NAME);
            Console.Out.WriteLine(value);
            Console.In.ReadLine();
            return 0;
        }
    }
}
