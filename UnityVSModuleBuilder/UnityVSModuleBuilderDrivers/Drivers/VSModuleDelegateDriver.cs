using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleEditor;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleBuilder.Drivers
{
    class VSModuleDelegateDriver : Drivable
    {
        public int Drive(string[] args)
        {
            UnityApi unityApi = new MockUnityApi();
            VSModuleDelegate vsModuleDelegate = new VSModuleDelegate(unityApi, new VSModuleXmlSerializer(unityApi));

            VSModuleSettingsTO to = vsModuleDelegate.RetrieveModuleSettingsTO();
            to.SetUnityInstallLocation("junk");
            vsModuleDelegate.SaveModuleSettingsTO(to);

            //vsModuleDelegate.ExportModuleToRepository();
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


        public void ExportRootAssets(string assetPathname, string exportFileName)
        {
            Console.Out.WriteLine("MockUnityApi.ExportRootAssets('" + assetPathname + "', '" + exportFileName + "')");
        }
    }
}
