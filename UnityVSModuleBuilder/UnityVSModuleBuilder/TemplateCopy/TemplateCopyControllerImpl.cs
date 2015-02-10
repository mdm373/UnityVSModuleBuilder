using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon;

namespace UnityVSModuleBuilder.TemplateCopy
{
    internal class TemplateCopyControllerImpl : TemplateCopyController
    {
        private const string TEMPLATE_LOCATION = @"ProjectTemplate";
        private const string REM_TAG_FILE_NAME = "[[REM_TAG]]";

        private readonly FileSystemController fileSystem;
        

        public TemplateCopyControllerImpl(FileSystemController fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public bool CopyAndCleanTemplate(string copyLocation, string projectName)
        {
            bool isSuccess = true;
            try
            {
                String fullCopyLocation = Path.Combine(copyLocation, projectName);
                fileSystem.DoCreateDirectory(fullCopyLocation);
                fileSystem.DoFullDirectoryCopy(TEMPLATE_LOCATION, fullCopyLocation);
                List<FileEntry>.Enumerator files = fileSystem.GetFilesForLocationRecursive(fullCopyLocation);
                while (files.MoveNext())
                {
                    String fileName = files.Current.GetFileName();
                    if(REM_TAG_FILE_NAME.Equals(fileName)){
                        fileSystem.DeleteFile(files.Current.GetFilePath());
                    }
                }
            }
            catch (Exception e)
            {
                isSuccess = false;
                Logger.LogError("Exception performing template project copy.", e);
            }
            return isSuccess;
        }
    }
}
