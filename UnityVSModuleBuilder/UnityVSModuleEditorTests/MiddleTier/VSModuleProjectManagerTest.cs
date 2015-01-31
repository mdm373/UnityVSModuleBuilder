using NUnit.Framework;
using NSubstitute;
using UnityVSModuleEditor.MiddleTier;
using UnityVSModuleCommon.FileSystem;
using System.Collections.Generic;
using System;

namespace UnityVSModuleEditor.MiddleTier
{
    [TestFixture]
    class VSModuleProjectManagerTest
    {
        private const string ORIGINAL_UNITY_INSTALL_LOCATION = "ORIGINAL_UNITY_INSTALL_LOCATION";
        private const string MODIFIED_UNITY_INSTALL_LOCATION = "MODIFIED_UNITY_INSTALL_LOCATION";
        private const string PROJECT_NAME = "PROJECT_NAME";
        private const string PROJECT_FILE_LOCATION = @"ASSET_FOLDER\..\..\VisualStudio\PROJECT_NAME\PROJECT_NAME.csproj";
        private const string EDITOR_FILE_LOCATION = @"ASSET_FOLDER\..\..\VisualStudio\PROJECT_NAMEEditor\PROJECT_NAMEEditor.csproj";
        private const string PROJECT_FILE_TEXT_WITH_UNITY_ROOT = "<SomeProject><UnityRoot>OldRootEntry</UnityRoot></SomeProject>";
        private const string EDITOR_FILE_TEXT_WITH_UNITY_ROOT = "<SomeEditorProject><UnityRoot>OldRootEntry</UnityRoot></SomeProject>";
        private const string UPDATED_PROJECT_TEXT = "<SomeProject><UnityRoot>MODIFIED_UNITY_INSTALL_LOCATION</UnityRoot></SomeProject>";
        private const string UPDATED_EDITOR_TEXT = "<SomeEditorProject><UnityRoot>MODIFIED_UNITY_INSTALL_LOCATION</UnityRoot></SomeProject>";
        private const string ASSET_FOLDER = "ASSET_FOLDER";
        private const string DEP_1_CN = "CN1";
        private const string DEP_1_PN = "PN1";
        private const string DEP_2_CN = "CN2";
        private const string DEP_2_PN = "PN2";
        private const string UPDATED_PROJECT_TEXT_WITH_NEW_DEPENDENCIES =
@"
<SomeProject>
    <ItemGroup>
<!--DepRefsGroup-->
<Reference Include='PN1'>
    <HintPath>$(SolutionDir)$(PluginsRoot)\PN1.dll</HintPath>
</Reference>
<Reference Include='PN2'>
    <HintPath>$(SolutionDir)$(PluginsRoot)\PN2.dll</HintPath>
</Reference>
<!--EndDepRefsGroup-->
    </ItemGroup>
</SomeProject> 
";
        private const string UPDATED_EDITOR_TEXT_WITH_NEW_DEPENDENCIES =
@"
<SomeProject>
    <ItemGroup>
<!--DepRefsGroup-->
<Reference Include='PN1'>
    <HintPath>$(SolutionDir)$(PluginsRoot)\PN1.dll</HintPath>
</Reference>
<Reference Include='PN1Editor'>
    <HintPath>$(SolutionDir)$(PluginsEditorRoot)\PN1Editor.dll</HintPath>
</Reference>
<Reference Include='PN2'>
    <HintPath>$(SolutionDir)$(PluginsRoot)\PN2.dll</HintPath>
</Reference>
<Reference Include='PN2Editor'>
    <HintPath>$(SolutionDir)$(PluginsEditorRoot)\PN2Editor.dll</HintPath>
</Reference>
<!--EndDepRefsGroup-->
    </ItemGroup>
</SomeProject> 
";

        private const string EDITOR_FILE_TEXT_WITH_TAGGED_ITEM_GROUP =
@"
<SomeProject>
    <ItemGroup>
<!--DepRefsGroup-->
<Reference Include='PN1'>
    <HintPath>$(SolutionDir)$(PluginsRoot)\PN1.dll</HintPath>
</Reference>
<Reference Include='PN1Editor'>
    <HintPath>$(SolutionDir)$(PluginsEditorRoot)\PN1Editor.dll</HintPath>
</Reference>
<!--EndDepRefsGroup-->
    </ItemGroup>
</SomeProject>
";
        private const string PROJECT_FILE_TEXT_WITH_TAGGED_ITEM_GROUP =
@"
<SomeProject>
    <ItemGroup>
<!--DepRefsGroup-->
<Reference Include='PN1'>
    <HintPath>$(SolutionDir)$(PluginsRoot)\PN1.dll</HintPath>
</Reference>
<!--EndDepRefsGroup-->
    </ItemGroup>
</SomeProject> 
";
        private UnityApi unityApi;
        private FileSystemController fsController;
        private VSModuleProjectManager manager;
        private VSModuleSettingsTO.Builder originalSettingsBuilder;
        private VSModuleSettingsTO.Builder updatedSettingsBuilder;
        private FileEntry projectFile;
        private FileEntry editorFile;
        private VSModuleSettingsTO updated;
        private VSModuleSettingsTO original;
        private List<VSModuleDependencyItem> originalDependencyList;
        private List<VSModuleDependencyItem> updatedDependencyList;
        private VSModuleDependencyItem dep1Updated;
        private VSModuleDependencyItem dep2Updated;
        private VSModuleDependencyItem dep1Origional;
        private VSModuleDependencyTO originalDependency;
        private VSModuleDependencyTO updatedDependency;
        
        [SetUp]
        public void SetUp()
        {
            dep1Origional = new VSModuleDependencyItem(DEP_1_CN, DEP_1_PN);
            dep1Updated = new VSModuleDependencyItem(DEP_1_CN, DEP_1_PN);
            dep2Updated = new VSModuleDependencyItem(DEP_2_CN, DEP_2_PN);
            originalDependencyList = new List<VSModuleDependencyItem>();
            updatedDependencyList = new List<VSModuleDependencyItem>();
            editorFile = Substitute.For<FileEntry>();
            projectFile = Substitute.For<FileEntry>();
            originalSettingsBuilder = new VSModuleSettingsTO.Builder();
            updatedSettingsBuilder = new VSModuleSettingsTO.Builder();
            unityApi = Substitute.For<UnityApi>();
            fsController = Substitute.For<FileSystemController>();
            manager = new VSModuleProjectManagerImpl(unityApi, fsController);
        }

        [Test]
        public void TestUpdateVSProjectForSettingsChange()
        {
            GivenUnityAPIHasAssetRoot();
            GivenOrigionalTOHasUnityLocation();
            GivenOrigionalTOHasProjectName();
            GivenUpdatedTOHasModifiedUnityLocation();
            GivenFileSystemHasProjectFileWithTextContainingUnityRootXmlItem();
            GivenFileSystemHasEditorFileWithTextContainingUnityRootXmlItem();
            WhenProjectSettingsUpdateRequested();
            ThenProjectFileHasUnityRootXmlItemTextUpdated();
            ThenEditorFileHasUnityRootXmlItemTextUpdated();
        }

        [Test]
        public void TestUpdateVSProjectForDependencyChange()
        {
            GivenOrigionalTOHasProjectName();
            GivenUnityAPIHasAssetRoot();
            GivenOrigionalDependenciesHaveDep1();
            GivenUpdatedDependenciesHaveDep2AndDep1();
            GivenFileSystemHasEditorFileContainingTaggedItemGroup();
            GivenFileSystemHasProjectFileContainingTaggedItemGroup();
            WhenProjectDependenciesUpdateRequested();
            ThenProjectFileHasUpdatedReferencesAdded();
            ThenEditorFileHasUpdatedReferencesAdded();
        }

        private void ThenEditorFileHasUpdatedReferencesAdded()
        {
            editorFile.Received().WriteAllText(
                Arg.Is<string>(
                    x => IsEqualNoWhiteSpace(UPDATED_EDITOR_TEXT_WITH_NEW_DEPENDENCIES, x)
                )
            );
        }

        private void ThenProjectFileHasUpdatedReferencesAdded()
        {
            projectFile.Received().WriteAllText(
                Arg.Is<string>(
                    x =>  IsEqualNoWhiteSpace(UPDATED_PROJECT_TEXT_WITH_NEW_DEPENDENCIES, x)
                )
            );
        }

        private bool IsEqualNoWhiteSpace(string s1, string s2)
        {
            s1 = s1.Replace("\n", String.Empty);
            s1 = s1.Replace("\r", String.Empty);
            s1 = s1.Replace(" ", String.Empty);
            s1 = s1.Replace("\t", String.Empty);
            s2 = s2.Replace("\n", String.Empty);
            s2 = s2.Replace("\r", String.Empty);
            s2 = s2.Replace(" ", String.Empty);
            s2 = s2.Replace("\t", String.Empty);
            return s1.Equals(s2);
        }

        private void WhenProjectDependenciesUpdateRequested()
        {
            original = originalSettingsBuilder.Build();
            originalDependency = new VSModuleDependencyTO(originalDependencyList);
            updatedDependency = new VSModuleDependencyTO(updatedDependencyList);
            manager.UpdateVSProjectsForDependencies(originalDependency, updatedDependency, original);
        }

        private void GivenFileSystemHasProjectFileContainingTaggedItemGroup()
        {
            GivenFileSystemHasFileWithText(PROJECT_FILE_LOCATION, projectFile, PROJECT_FILE_TEXT_WITH_TAGGED_ITEM_GROUP);
        }

        private void GivenFileSystemHasEditorFileContainingTaggedItemGroup()
        {
            GivenFileSystemHasFileWithText(EDITOR_FILE_LOCATION, editorFile, EDITOR_FILE_TEXT_WITH_TAGGED_ITEM_GROUP);
        }

        private void GivenUpdatedDependenciesHaveDep2AndDep1()
        {
            updatedDependencyList.Add(dep1Updated);
            updatedDependencyList.Add(dep2Updated);
        }

        private void GivenOrigionalDependenciesHaveDep1()
        {
            originalDependencyList.Add(dep1Origional);
        }

        private void GivenOrigionalTOHasProjectName()
        {
            originalSettingsBuilder.ProjectName = PROJECT_NAME;
        }

        private void ThenEditorFileHasUnityRootXmlItemTextUpdated()
        {
            editorFile.Received().WriteAllText(UPDATED_EDITOR_TEXT);
        }

        private void ThenProjectFileHasUnityRootXmlItemTextUpdated()
        {
            projectFile.Received().WriteAllText(UPDATED_PROJECT_TEXT);
        }

        private void WhenProjectSettingsUpdateRequested()
        {
            original = originalSettingsBuilder.Build();
            updated = updatedSettingsBuilder.Build();
            manager.UpdateVSProjectsForProjectSettings(original, updated);
        }

        private void GivenFileSystemHasEditorFileWithTextContainingUnityRootXmlItem()
        {
            GivenFileSystemHasFileWithText(EDITOR_FILE_LOCATION, editorFile, EDITOR_FILE_TEXT_WITH_UNITY_ROOT);
        }

        private void GivenFileSystemHasProjectFileWithTextContainingUnityRootXmlItem()
        {
            GivenFileSystemHasFileWithText(PROJECT_FILE_LOCATION, projectFile, PROJECT_FILE_TEXT_WITH_UNITY_ROOT);
        }

        private void GivenFileSystemHasFileWithText(string fileLocation, FileEntry file, string text)
        {
            file.ReadAllText().Returns(text);
            fsController.GetExistingFile(fileLocation).Returns(file);
        }

        private void GivenUnityAPIHasAssetRoot()
        {
            unityApi.GetAssetFolder().Returns(ASSET_FOLDER);
        }

        private void GivenUpdatedTOHasModifiedUnityLocation()
        {
            updatedSettingsBuilder.UnityInstallLocation = MODIFIED_UNITY_INSTALL_LOCATION;
        }

        private void GivenOrigionalTOHasUnityLocation()
        {
            originalSettingsBuilder.UnityInstallLocation = ORIGINAL_UNITY_INSTALL_LOCATION;
        }
    }
}
