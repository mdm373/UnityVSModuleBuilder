using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.Implement
{
    class BuildProjectResponseImpl : BuildProjectResponse
    {
        private readonly bool isSuccess;

        private BuildProjectResponseImpl(bool isSuccess)
        {
            this.isSuccess = isSuccess;
        }

        public static BuildProjectResponse GetInstance(bool isSuccess)
        {
            return new BuildProjectResponseImpl(isSuccess);
        }

        public bool IsSuccess()
        {
            return isSuccess;
        }
    }
}
