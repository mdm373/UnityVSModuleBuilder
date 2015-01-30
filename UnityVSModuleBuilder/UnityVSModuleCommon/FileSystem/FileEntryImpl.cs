using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UnityVSModuleCommon.FileSystem
{
    internal class FileEntryImpl : FileEntry
    {
        private FileSystemInfo info;

        public FileEntryImpl(FileSystemInfo fileInfo)
        {
            this.info = fileInfo;
        }

        public void ReplaceContents(string match, string replace)
        {
            if (File.Exists(info.FullName))
            {
                String text = File.ReadAllText(info.FullName);
                String textReplaced = text.Replace(match, replace);
                if (!text.Equals(textReplaced))
                {
                    File.WriteAllText(info.FullName, textReplaced);
                }
            }
            else
            {
                throw new Exception("Cannot write contents of file a'" + info.FullName + "'. File does not exist.");
            }
            
        }

        public string GetFileName()
        {
            return info.Name;
        }

        public void RenameFile(string newName)
        {
            if(Directory.Exists(info.FullName))
            {
                DirectoryInfo existingDirInfo = new DirectoryInfo(info.FullName);
                String newDirName = Path.Combine(existingDirInfo.Parent.FullName, newName);
                Directory.Move(existingDirInfo.FullName, newDirName);
                info = new DirectoryInfo(newDirName);
            }
            else if(File.Exists(info.FullName))
            {
                FileInfo existingFileInfo = new FileInfo(info.FullName);
                String newFileName = Path.Combine(existingFileInfo.Directory.FullName, newName);
                File.Move(info.FullName, newFileName);
                info = new FileInfo(newFileName);
            }
            else
            {
                throw new Exception("Cannot rename file or dir at'" + info.FullName + "'. File or dir does not exist.");
            }
            
        }


        public FileType GetFileType()
        {
            FileType type = FileType.UNKNNOWN;
            if(Directory.Exists(info.FullName)){
                type = FileType.DIRECTORY;
            } else if(File.Exists(info.FullName)){
                type = FileType.FILE;
            }
            return type;
        }


        public String GetFilePath()
        {
            return info.FullName;
        }


        public bool IsPresent()
        {
            return info.Exists;
        }
    }
}
