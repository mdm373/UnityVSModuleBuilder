using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleBuilder.FileSystem;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder.TemplateCopy
{
    public class TemplateCopyControllerImpl : TemplateCopyController
    {
        private const string TEMPLATE_LOCATION = @"ProjectTemplate";
        private const string REM_TAG_FILE_NAME = "[[REM_TAG]]";

        private readonly FileSystemController fileSystem;
        

        public TemplateCopyControllerImpl(FileSystem.FileSystemController fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public bool CopyAndCleanTemplate(string copyLocation)
        {
            bool isSuccess = true;
            try
            {
                fileSystem.DoFullDirectoryCopy(TEMPLATE_LOCATION, copyLocation);
                List<FileEntry>.Enumerator files = fileSystem.GetFilesForLocationRecursive(copyLocation);
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
