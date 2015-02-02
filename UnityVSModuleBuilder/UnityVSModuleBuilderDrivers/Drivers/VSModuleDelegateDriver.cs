using System;
using System.Collections.Generic;
using UnityVSModuleEditor.MiddleTier;

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

            vsModuleDelegate.AddModuleDependency("cn", "DriverProject");
            vsModuleDelegate.AddModuleDependency("cn", "DriverProject0");

            VSModuleDependencyItem removeItem = new VSModuleDependencyItem("cn", "DriverProject");
            List<VSModuleDependencyItem> removeItems = new List<VSModuleDependencyItem>();
            removeItems.Add(removeItem);
            vsModuleDelegate.RemoveDependencies(removeItems.GetEnumerator());
            VSModuleDependencyTO updatedDeps = vsModuleDelegate.RetrieveModuleDependenciesTO();
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
            return @"F:\Projects\unity\UnityVSModuleBuilder\UnityVSModuleBuilder\UnityVSModuleBuilderDrivers\bin\Debug\GeneratedDriverProject\DriverProject2\UnityGame\Assets";
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


        public void UpdateProjectName(string projectName)
        {
            Console.Out.WriteLine("MockUnityApi.UpdateProjectName('" + projectName + "')");
        }

        public void UpdateCompanyName(string companyName)
        {
            Console.Out.WriteLine("MockUnityApi.UpdateCompanyName('" + companyName + "')");
        }

        public void UpdateAndriodBundleIdentifier(string andriodIdentifier)
        {
            Console.Out.WriteLine("MockUnityApi.UpdateAndriodBundleIdentifier('" + andriodIdentifier + "')");
        }
    }
}
