﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleEditor.UnityApis;
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
            VSModuleProjectManagerImpl vsProjectManager = new VSModuleProjectManagerImpl(unityApi);
            VSModuleDependencyManagerImpl vsDependencyManager = new VSModuleDependencyManagerImpl(unityApi, serializer, fsController);
            VSModuleImportExportManagerImpl vsImportManager = new VSModuleImportExportManagerImpl(unityApi, fsController);
            return new VSModuleDelegateImpl(vsSettingsManager, vsProjectManager, vsImportManager, vsDependencyManager);
        }
    }
}
