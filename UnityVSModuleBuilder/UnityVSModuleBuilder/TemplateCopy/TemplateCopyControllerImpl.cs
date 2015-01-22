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

        private readonly FileSystemController fileSystem;
        

        public TemplateCopyControllerImpl(FileSystem.FileSystemController fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public bool CopyTemplate(string copyLocation)
        {
            bool isSuccess = true;
            try
            {
                fileSystem.DoFullDirectoryCopy(TEMPLATE_LOCATION, copyLocation);
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
