
namespace UnityVSModuleEditor.MiddleTier
{
    internal class VSModuleConstants
    {
        public const string CONFIG_FILE_ASSET_LOCATION = @"..\UVSModule\ModuleConfig.xml";
        public const string DEPENDENCY_FILE_LOCATION = @"..\UVSModule\ModuleDependencies.xml";
        public const string UNITYPACKAGE_EXTENSION = ".unitypackage";
        public const string ASSET_FOLDER_NAME = "Assets";
        public const string EDITOR_FOLDER_NAME = "Editor";
        public const string MANAGED_CODE_FOLDER_NAME = "ManagedCode";
        public const string PLUGIN_FOLDER_NAME = "Plugins";
        public const string VISUAL_STUDIO_FOLDER = @"..\..\VisualStudio\";
        public const string UNITY_ROOT_PATTERN = @"<UnityRoot>.*</UnityRoot>";
        public const string UNITYROOT_OPEN_TAG = @"<UnityRoot>";
        public const string UNITYROOT_CLOSE_TAG = @"</UnityRoot>";
        public const string DEPENDENCY_ITEM_EDITOR_FORMAT =
@"
<Reference Include='{0}Editor'>
    <HintPath>$(SolutionDir)$(PluginsEditorRoot)\{0}Editor.dll</HintPath>
</Reference>";
        public const string DEPENDENCY_ITEM_FORMAT =
@"
<Reference Include='{0}'>
    <HintPath>$(SolutionDir)$(PluginsRoot)\{0}.dll</HintPath>
</Reference>";
        public const string ITEM_GROUP_HINT_MATCH = "<!--DepRefsGroup--><ItemGroup>";
        public const string DEPS_START_TAG = "<!--DepRefsGroup-->";
        public const string DEPS_END_TAG = "<!--EndDepRefsGroup-->";
        public const string ANDRIOD_ID_FORMAT = "com.{0}.{1}";
    }
}
