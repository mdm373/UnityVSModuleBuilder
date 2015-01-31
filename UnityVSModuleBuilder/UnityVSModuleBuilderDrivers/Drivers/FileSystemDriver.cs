using System.Collections.Generic;
using System.IO;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon;

namespace UnityVSModuleBuilder.Drivers
{
    internal class FileSystemDriver : Drivable
    {

        public int Drive(string[] args)
        {
            Directory.Delete(@"C:\Temp\DriverTest\To", true); //Clean-up from last run

            FileSystemController fsController =  FileSystemFactory.GetNewFileSystemController();
            fsController.DoFullDirectoryCopy(@"C:\Temp\DriverTest\FileReplaceExample", @"C:\Temp\DriverTest\To");

            List<FileEntry>.Enumerator files = fsController.GetFilesForLocationRecursive(@"C:\Temp\DriverTest\To");
            while (files.MoveNext())
            {
                Logger.Log(files.Current.GetFileName() + ", " + files.Current.GetFileType());
                if (files.Current.GetFileType() == FileType.FILE)
                {
                    files.Current.ReplaceContents("[[SOMETHING]]", "Replacement");
                }
                files.Current.RenameFile(files.Current.GetFileName() + "-Renamed");
            }
            return 0;
        }
    }
}
