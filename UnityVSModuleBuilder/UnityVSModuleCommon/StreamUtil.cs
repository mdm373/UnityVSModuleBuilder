using System;
using System.IO;

namespace UnityVSModuleCommon
{
    public class StreamUtil
    {
        private const string ERROR_MESSAGE = "Exception Closing File Stream. Resources May Be Lost.";


        public static void CloseFileStream(FileStream fileStream)
        {
            try
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
            catch (Exception e)
            {
                Logger.LogError(ERROR_MESSAGE, e);
            }
        }

    }
}
