using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityVSModuleBuilder.FileSystem;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder.Overlay
{
    public class ProjectNameOverlayImpl : ProjectNameOverlay
    {
        private const string PROJECT_NAME_REPLACEMENT = @"[[PROJECT_NAME]]";

        private readonly FileSystemController fileSystem;
        

        public ProjectNameOverlayImpl(FileSystem.FileSystemController fileSystem)
        {
            this.fileSystem = fileSystem;
        }
        public bool Overlay(string projectName, string projectLocation)
        {
            bool isSuccessful = true;
            try {
                List<FileEntry>.Enumerator files = fileSystem.GetFilesForLocationRecursive(projectLocation);
                while (files.MoveNext())
                {
                    if (files.Current.GetFileType() == FileType.FILE)
                    {
                        files.Current.ReplaceContents(PROJECT_NAME_REPLACEMENT, projectName);
                    }
                }

                files = fileSystem.GetFilesForLocationRecursive(projectLocation);
                while (files.MoveNext())
                {
                    String fileName = files.Current.GetFileName();
                    if (fileName.Contains(PROJECT_NAME_REPLACEMENT))
                    {
                        files.Current.RenameFile(fileName.Replace(PROJECT_NAME_REPLACEMENT, projectName));
                        files = fileSystem.GetFilesForLocationRecursive(projectLocation);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Unexpected Exception overlaying project name in generated project.", e);
                isSuccessful = false;
            }
            return isSuccessful;
        }
    }
}
