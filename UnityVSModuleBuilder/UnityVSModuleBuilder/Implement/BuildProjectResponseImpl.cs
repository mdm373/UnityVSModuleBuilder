
namespace UnityVSModuleBuilder.Implement
{
    internal class BuildProjectResponseImpl : BuildProjectResponse
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
