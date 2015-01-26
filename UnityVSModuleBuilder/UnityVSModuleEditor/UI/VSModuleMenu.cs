using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityVSModuleEditor.UI
{
    
    public class VSModuleMenu : MonoBehaviour
    {
        private const string VS_MENU_ITEM = "VSModule Config";
        private const string WINDOW_MENU_ITEM = "Window/";


        [MenuItem(WINDOW_MENU_ITEM + VS_MENU_ITEM)]
        public static void LaunchVSModuleWindow()
        {
            EditorWindow.GetWindow(typeof(UnityVSModuleConfigWindow));
        }
    }
}
