using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder.FileSystem
{
    public class FileSystemControllerImpl : FileSystemController
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
    }
}
