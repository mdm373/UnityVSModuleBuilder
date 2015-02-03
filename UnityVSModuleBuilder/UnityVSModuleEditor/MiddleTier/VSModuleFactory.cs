using UnityVSModuleCommon;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor.MiddleTier
{
    internal class VSModuleFactory
    {
        public static VSModuleDelegate GetNewDelegate(UnityApi unityApi)
        {
            FileSystemController fsController = FileSystemFactory.GetNewFileSystemController();
            XmlSerializerWrapper serializer = new XmlSerializerWrapperImpl();
            VSModuleSettingsManagerImpl vsSettingsManager = new VSModuleSettingsManagerImpl(unityApi, serializer, fsController);
            VSModuleProjectManagerImpl vsProjectManager = new VSModuleProjectManagerImpl(unityApi, fsController);
            VSModuleDependencyManagerImpl vsDependencyManager = new VSModuleDependencyManagerImpl(unityApi, serializer, fsController);
            VSModuleImportExportManagerImpl vsImportManager = new VSModuleImportExportManagerImpl(unityApi, fsController);
            VSModuleUnityManager vsUnityManager = new VSModuleUnityManagerImpl(unityApi);
            return new VSModuleDelegateImpl(vsSettingsManager, vsProjectManager, vsImportManager, vsDependencyManager, vsUnityManager);
        }
    }
}
