
namespace UnityVSModuleBuilder
{
    public interface TemplateProjectBuilder
    {
        BuildProjectResponse DoBuild(BuildProjectRequest request);
    }
}
