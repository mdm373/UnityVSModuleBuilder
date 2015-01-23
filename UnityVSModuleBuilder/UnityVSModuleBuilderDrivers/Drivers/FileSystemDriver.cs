using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityVSModuleBuilder;
using UnityVSModuleBuilder.FileSystem;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder.Drivers
{
    class FileSystemDriver : Drivable
    {

        public int Drive(string[] args)
        {
            Directory.Delete(@"C:\Temp\DriverTest\To", true); //Clean-up from last run

            FileSystemController fsController = new FileSystemControllerImpl();
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
