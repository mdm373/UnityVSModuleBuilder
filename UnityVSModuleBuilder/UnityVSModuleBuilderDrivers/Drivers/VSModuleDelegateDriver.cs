using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleEditor;
using UnityVSModuleEditor.MiddleTier;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleBuilder.Drivers
{
    internal class VSModuleDelegateDriver : Drivable
    {
        public int Drive(string[] args)
        {
            UnityApi unityApi = new MockUnityApi();
            VSModuleDelegate vsModuleDelegate = VSModuleFactory.GetNewDelegate(unityApi);

            VSModuleSettingsTO to = vsModuleDelegate.RetrieveModuleSettingsTO();
            //to.SetUnityInstallLocation("junk");
            //vsModuleDelegate.SaveModuleSettingsTO(to);

            vsModuleDelegate.AddModuleDependency("cn", "someOtherProject");
            return 0;
        }
    }

    internal class MockUnityApi : UnityApi 
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


        public void ExportRootAssets(string[] assetPathname, string exportFileName)
        {
            Console.Out.WriteLine("MockUnityApi.ExportRootAssets('" + assetPathname + "', '" + exportFileName + "')");
        }


        public void LogError(string p, Exception e)
        {
            
        }


        public void ImportRootAssets(string importFileName)
        {
            Console.Out.WriteLine("MockUnityApi.ImportRootAssets('" + importFileName + "')");
        }
    }
}
