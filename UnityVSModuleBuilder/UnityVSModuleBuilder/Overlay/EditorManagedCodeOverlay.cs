using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityVSModuleBuilder.FileSystem;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder.Overlay
{
    public class EditorManagedCodeOverlay : DefinedOverlay
    {
        private const string EDITOR_DLL_FILE_LOCATION = "UnityVSModuleEditor.dll";
        private const string UNITY_EDITOR_ASSETS_LOCATION = @"UnityGame\Assets\Editor";

        private readonly FileSystemController fileSystem;

        
        public EditorManagedCodeOverlay(FileSystemController fileSystem)
        {
            this.fileSystem = fileSystem;
        }
        public bool Overlay(BuildProjectRequest request)
        {
            bool result = false;
            FileEntry entry = fileSystem.GetFile(EDITOR_DLL_FILE_LOCATION);
            if (entry != null)
            {
                String destinationDirectory = Path.Combine(request.GetCopyLocation(), request.GetProjectName());
                destinationDirectory = Path.Combine(destinationDirectory, UNITY_EDITOR_ASSETS_LOCATION);
                fileSystem.DoFileCopy(entry, destinationDirectory);
                result = true;
            }
            else
            {
                Logger.Log("Failed to copy'" + EDITOR_DLL_FILE_LOCATION + "' to unity editor assets. File Did Not Exist.");
            }
            return result;
        }
    }
}
