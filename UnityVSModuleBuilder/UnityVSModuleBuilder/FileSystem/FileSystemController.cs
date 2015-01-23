using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.FileSystem
{
    public interface FileSystemController
    {
        void DoFullDirectoryCopy(string from, string to);
        List<FileEntry>.Enumerator GetFilesForLocationRecursive(String location);


        
    }
}
