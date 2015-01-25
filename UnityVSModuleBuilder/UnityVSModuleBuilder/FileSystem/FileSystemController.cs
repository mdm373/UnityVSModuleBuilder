using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.FileSystem
{
    public interface FileSystemController
    {
        void DoFullDirectoryCopy(string from, string to);
        void DoFileCopy(FileEntry origional, string destinationDirectory);
        List<FileEntry>.Enumerator GetFilesForLocationRecursive(String location);
        void DeleteFile(string fileLocation);
        FileEntry GetFile(string fileLocation);

        
    }
}
