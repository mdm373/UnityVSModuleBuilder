﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleEditor.MiddleTier;

namespace UnityVSModuleEditor.UI
{
    internal class UnityApiFactory
    {
        private UnityApiFactory()
        {

        }

        public static UnityApi GetUnityApi()
        {
            return new UnityApiImpl();
        }
    }
}
