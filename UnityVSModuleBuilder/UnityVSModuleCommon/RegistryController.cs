using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UnityVSModuleCommon
{
    public enum RegKeyType
    {
        HK_LOCAL_MACHINE
    }

    public sealed class RegistryFactory
    {
        private RegistryFactory() { }
        public static RegistryController GetRegistryController()
        {
            return new RegistryControllerImpl();
        }
    }

    public interface RegistryController
    {
        KeyType GetRegistryKey<KeyType>(RegKeyType regKeyType, string location, string name);
    }

    public class RegistryControllerImpl : RegistryController
    {

        public KeyType GetRegistryKey<KeyType>(RegKeyType regKeyType, string location, string name)
        {
            KeyType value = default(KeyType);
            RegistryKey root = null;
            try
            {
                root = GetRootKeyValue(regKeyType);
                value = GetRegistrySubKey<KeyType>(root, location, name);
            }
            catch (Exception){ }
            finally
            {
                if (root != null)
                {
                    root.Close();
                }
            }
            return value;
        }

        public KeyType GetRegistrySubKey<KeyType>(RegistryKey root, string location, string name)
        {
            KeyType value = default(KeyType);
            RegistryKey regSubKey = null;
            try
            {
                regSubKey = root.OpenSubKey(location);
                object regValue = regSubKey.GetValue(name);
                value = (KeyType)regValue;
            }
            catch (Exception) { }
            finally
            {
                if (regSubKey != null)
                {
                    regSubKey.Close();
                }
            }
            return value;
        }

        private RegistryKey GetRootKeyValue(RegKeyType type)
        {
            RegistryKey root = null;
            if (type == RegKeyType.HK_LOCAL_MACHINE)
            {
                root = Registry.LocalMachine;
            }
            return root;
        }
    }
}
