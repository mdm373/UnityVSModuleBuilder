using System;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon;

namespace UnityVSModuleBuilder.Overlay
{
    internal class EditorManagedCodeOverlay : DefinedOverlay
    {
        private const string EDITOR_DLL_FILE_LOCATION = "UnityVSModuleEditor.dll";
        private const string COMMON_DLL_FILE_LOCATION = "UnityVSModuleCommon.dll";
        private const string UNITY_EDITOR_ASSETS_LOCATION = @"UnityGame\Assets\Editor";
        
        private readonly FileSystemController fileSystem;
        
        public EditorManagedCodeOverlay(FileSystemController fileSystem)
        {
            this.fileSystem = fileSystem;
        }
        public bool Overlay(BuildProjectRequest request)
        {
            bool result = CopyManagedCode(request, EDITOR_DLL_FILE_LOCATION);
            result = result && CopyManagedCode(request, COMMON_DLL_FILE_LOCATION);
            return result;
        }

        private bool CopyManagedCode(BuildProjectRequest request, string managedCodeFileLocation)
        {
            bool result = false;
            FileEntry entry = fileSystem.GetExistingFile(managedCodeFileLocation);
            if (entry != null)
            {
                String destinationDirectory = Path.Combine(request.GetCopyLocation(), request.GetProjectName());
                destinationDirectory = Path.Combine(destinationDirectory, UNITY_EDITOR_ASSETS_LOCATION);
                fileSystem.DoFileCopy(entry, destinationDirectory);
                result = true;
            }
            else
            {
                Logger.Log("Failed to copy'" + managedCodeFileLocation + "' to unity editor assets. File Did Not Exist.");
            }
            return result;
        }
    }
}
