using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.FileSystem
{
    public enum FileType
    {
        UNKNNOWN, FILE, DIRECTORY,
    }
    public interface FileEntry
    {
        void ReplaceContents(string EXPECTED_MATCH, string EXPECTED_PROJECT_NAME);
        String GetFileName();
        void RenameFile(string newName);
        FileType GetFileType();

        String GetFilePath();
    }
}
