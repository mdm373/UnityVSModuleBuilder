using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleCommon.FileSystem
{
    public interface FileSystemController
    {
        void DoFullDirectoryCopy(string from, string to);
        void DoFileCopy(FileEntry origional, string destinationDirectory);
        List<FileEntry>.Enumerator GetFilesForLocationRecursive(String location);
        void DeleteFile(string fileLocation);
        FileEntry GetExistingFile(string fileLocation);
        FileEntry GetExistingOrNewlyCreatedFile(string fileLocation);
        FileEntry CreateNotPresentFile(string fileLocation);
        bool DoCreateDirectory(string moduleRepoLocation);
    }
}
