using System;

namespace UnityVSModuleCommon.FileSystem
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
        bool IsPresent();
        string ReadAllText();
        void WriteAllText(string text);
    }
}
