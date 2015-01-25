using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityVSModuleEditor;
using UnityVSModuleEditor.UnityApis;

namespace UnityVSModuleBuilder.Drivers
{
    class VSModuleDelegateDriver : Drivable
    {
        public int Drive(string[] args)
        {
            VSModuleDelegate vsModuleDelegate = new VSModuleDelegate(new MockUnityApi());
            VSModuleSettingsTO to =  vsModuleDelegate.RetrieveModuleSettingsTO();
            to.SetUnityInstallLocation(@"C:\Temp\SomethingElse");
            vsModuleDelegate.SaveModuleSettingsTO(to);
            return 0;
        }
    }

    public class MockUnityApi : UnityApi 
    {
        public void Log(string message)
        {
            Console.Out.WriteLine(message);
        }

        public void LogError(string message)
        {
            Console.Error.WriteLine(message);
        }

        public void LogException(Exception e)
        {
            Console.Out.WriteLine(e.Message);
        }

        public void LogWarning(string message)
        {
            Console.Out.WriteLine(message);
        }

        public string GetAssetFolder()
        {
            return @"F:\Projects\unity\UnityVSModuleBuilder\UnityVSModuleBuilder\UnityVSModuleBuilderDrivers\bin\Debug\GeneratedDriverProject\DriverProject\UnityGame\Assets";
        }
    }
}
