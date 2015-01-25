using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityVSModuleEditor.UnityApis;

namespace UnityVSModuleEditor
{
    class StreamUtil
    {
        public static void CloseFileStream(FileStream fileStream, UnityApi unityApi)
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
                unityApi.LogError("Exception Closing File Stream. Resources May Be Lost.");
                unityApi.LogException(e);
            }
        }
    }
}
