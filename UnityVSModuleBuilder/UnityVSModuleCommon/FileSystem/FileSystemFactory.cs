
namespace UnityVSModuleCommon.FileSystem
{
    public class FileSystemFactory
    {
        private FileSystemFactory() { }

        public static FileSystemController GetNewFileSystemController()
        {
            return new FileSystemControllerImpl();
        }
    }
}
