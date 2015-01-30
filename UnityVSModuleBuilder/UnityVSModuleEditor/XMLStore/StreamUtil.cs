using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityVSModuleCommon.Logging;
using UnityVSModuleEditor.UnityApis;

namespace UnityVSModuleEditor
{
    internal class StreamUtil
    {
        private const string ERROR_MESSAGE = "Exception Closing File Stream. Resources May Be Lost.";


        public static void CloseFileStream(FileStream fileStream, UnityApi unityApi = null)
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
                if (unityApi != null)
                {
                    unityApi.LogError(ERROR_MESSAGE, e);
                }
                else
                {
                    Logger.LogError(ERROR_MESSAGE, e);
                }
                
            }
        }

    }
}
