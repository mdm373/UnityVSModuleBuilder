using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityVSModuleBuilder.FileSystem;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder.Overlay
{
    public abstract class DefinedOverlayImpl : DefinedOverlay
    {
        
        private readonly FileSystemController fileSystem;
        

        public DefinedOverlayImpl(FileSystem.FileSystemController fileSystem)
        {
            this.fileSystem = fileSystem;
        }
        public bool Overlay(BuildProjectRequest request)
        {
            String definedTag = GetDefinedTag();
            String definedValue = GetDefinedValue(request);
            bool isSuccessful = true;
            try {
                List<FileEntry>.Enumerator files = fileSystem.GetFilesForLocationRecursive(request.GetCopyLocation());
                while (files.MoveNext())
                {
                    if (files.Current.GetFileType() == FileType.FILE)
                    {
                        files.Current.ReplaceContents(definedTag, definedValue);
                    }
                }

                files = fileSystem.GetFilesForLocationRecursive(request.GetCopyLocation());
                while (files.MoveNext())
                {
                    String fileName = files.Current.GetFileName();
                    if (fileName.Contains(definedTag))
                    {
                        files.Current.RenameFile(fileName.Replace(definedTag, definedValue));
                        files = fileSystem.GetFilesForLocationRecursive(request.GetCopyLocation());
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Unexpected Exception overlaying defined tag '" + definedTag + "' in generated project.", e);
                isSuccessful = false;
            }
            return isSuccessful;
        }

        public abstract string GetDefinedValue(BuildProjectRequest request);

        public abstract string GetDefinedTag();
    }
}
