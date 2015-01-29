using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleCommon.FileSystem
{
    public class FileSystemFactory
    {
        private FileSystemFactory() { }

        public static FileSystemController GetNewFileSystemController()
        {
            return new FileSystemControllerImpl();
        }
    }
}
