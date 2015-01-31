using System;
using System.Collections.Generic;
using System.IO;
using UnityVSModuleCommon;

namespace UnityVSModuleCommon.FileSystem
{
    internal class FileSystemControllerImpl : FileSystemController
    {
        public void DoFullDirectoryCopy(string from, string to)
        {
            Logger.Log("Performing full directory copy'" + from + "' to '" + to + '\'');
            DirectoryInfo fromInfo = new DirectoryInfo(from);
            DirectoryInfo toInfo = new DirectoryInfo(to);
            if (!fromInfo.Exists)
            {
                throw new DirectoryNotFoundException("Source dir'" + from + "' does not exist");
            }
            if (toInfo.Exists)
            {
                throw new DirectoryNotFoundException("Destination dir'" + to + "' already exists");
            }
            else
            {
                Directory.CreateDirectory(to);
            }

            FileInfo[] files = fromInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                string toFilePath = Path.Combine(to, file.Name);
                FileInfo toFileInfo = new FileInfo(toFilePath);
                if (toFileInfo.Exists)
                {
                    Logger.Log("Warning: Destination file '" + toFilePath + "' already exists during directory copy.");
                }
                file.CopyTo(toFilePath, true);

            }

            DirectoryInfo[] subDirs = fromInfo.GetDirectories();
            foreach(DirectoryInfo subDir in subDirs)
            {
                string toFilePath = Path.Combine(to, subDir.Name);
                DoFullDirectoryCopy(subDir.FullName, toFilePath);
            }

        }

        public List<FileEntry>.Enumerator GetFilesForLocationRecursive(String location)
        {
            List<FileEntry> files = new List<FileEntry>();
            DirectoryInfo info = new DirectoryInfo(location);
            if (!info.Exists)
            {
                Logger.Log("Warning: requested dirtory at '" + location + "' does not exist. Cannot list files recursive");
            }
            else
            {
                FileInfo[] directFiles = info.GetFiles();
                foreach(FileInfo directFile in directFiles){
                    FileEntry entry = new FileEntryImpl(directFile);
                    files.Add(entry);
                }
                
                DirectoryInfo[] dirs = info.GetDirectories();
                foreach(DirectoryInfo dir in dirs){
                    files.Add(new FileEntryImpl(dir));
                    List<FileEntry>.Enumerator nestedFiles = GetFilesForLocationRecursive(dir.FullName);
                    while (nestedFiles.MoveNext())
                    {
                        files.Add(nestedFiles.Current);
                    }
                }
            }
            return files.GetEnumerator();
        }


        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }


        public void DoFileCopy(FileEntry origional, string destinationDirectory)
        {
            String name = origional.GetFileName();
            String newLocation = Path.Combine(destinationDirectory, name);
            File.Copy(origional.GetFilePath(), newLocation, true);
        }

        public FileEntry GetExistingFile(string fileLocation)
        {
            FileEntry entry = null;
            FileInfo info = new FileInfo(fileLocation);
            if (info.Exists)
            {
                entry = new FileEntryImpl(info);
            }
            return entry;
        }

        public FileEntry CreateNotPresentFile(string fileLocation)
        {
            FileEntry file = null;
            bool isPresent = Directory.Exists(fileLocation);
            isPresent = isPresent || File.Exists(fileLocation);
            FileStream stream = null;
            try{
                if(!isPresent){
                    stream = File.Create(fileLocation);
                    file = GetExistingFile(fileLocation);
                }
            } catch(Exception e){
                Logger.LogError("Exception creating File at '" + fileLocation + "'.", e);
            } finally{
                stream.Close();
            }
            return file;
        }

        public FileEntry GetExistingOrNewlyCreatedFile(string fileLocation)
        {
            FileEntry entry = GetExistingFile(fileLocation);
            if (entry == null)
            {
                entry = CreateNotPresentFile(fileLocation);
            }
            return entry;
        }


        public bool DoCreateDirectory(string directoryLocation)
        {
            bool isCreated = false;
            try
            {
                DirectoryInfo dir = Directory.CreateDirectory(directoryLocation);
                isCreated = dir.Exists;
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to create directory'" + directoryLocation +"' see logged error for details.", e);
            }
            
            return isCreated;
        }
    }
}
