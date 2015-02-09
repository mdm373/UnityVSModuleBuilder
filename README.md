Overview | [Specification](MarkDown/SPECIFICATION.md) | [Developing](MarkDown/DEVELOPMENT.md)
# UVSModule Manager Overview#
## A dependency management tool for Unity 3D project development##

####Whats all this about?####
*Unity Visual Studio Module Manger* allows you to generate module projects that:

* Export your Unity3D project as a "Module" asset package to a module repository.
* Import and update project dependencies on other module projects exported to the module repository.
* Maintain a Visual Studio Solution that deploys game and editor scripting DLL's to your module project.

UVS Consists of two application parts

* A Standalone windows desktop "project builder" application that generates new module projects
* A Unity3D editor window that maintains module dependencies and exports the project to the module repository

####So, What exactly makes up a "Module" project?

To get an exact picture of what's deployed inside a project module, take a look at the Template Project located in the UVSModule install directory *(C:\Program Files\UnityVSModuleBuilder\ProjectTemplate)*.

Modules are defined by the three identifiers "Project Name", "Company Name" and "Company Short Name" where "Project Name" and "Company Short Name" form a unique identifier for the module. This document will refer to them as PN, CN and CSN for short. In general, each module consists of the following folder structure.

* UVSModul
 	* **ModuleConfig.xml:** Defines the properties of the module (Including project name and company short name)
 	* **DependencyInfo.xml:** Defines modules this module depends on. 
* UnityGame
	* **Assets\Editor:** Contains the DLL's for the editor integrated Module configuration screen. 
	* Each of the following folders and their children will be included in the exported module asset package  
	* **Assets\CSN\PN\:** Parent folder for all module assets to be used at runtime for the unity game.
	* **Assets\Editor\CSN\PN:** Parent folder for all assets to be used in the unity editor for the module.
	* **Assets\ManagedCode:** Attached Visual Studio project will automatically deploy builds for unity runtime game project scripts as a managed DLL to this folder
	* **Assets\Editor\ManagedCode:** Attached Visual Studio project will automatically deploy builds for unity editor project scripts as a managed DLL to this folder
	* **Assets\Plugins:** Pro Only, parent folder for all unity plugins
* VisualStudio
	* **Visual Studio Solution:** This is not the default Unity C# visual  studio solution. Instead, this solution is configured to:
		* Deploy to the "ManagedCode" directories detailed in the UnityGame folder section above.
		* The Editor project has references to all dependent deployed module and module editor DLL's, Unity Engine and Unity Editor DLLs as well as NUnit and NSubstitute to encourge TDD development.
		* The module project has references to all dependenct module DLL's and Unity Engine DLL's.
		* User changes to this solution will not be destroyed by Unity Asset syncing.

####What does developing with this look like?

Lets say you spin up a hobby platformer game project. You might start out by using the module builder to generate the following module:

	Project1:
		ProjectName: BrilliantPlatform
		CompanyName: Such Wow So Games
		CompanyShortName: SWSG

But, as you get into development you start yet another project idea and start writing this awesome physics based puzzle game for mobile. Now you go back to the builder again and make:

	Projct2:
		ProjectName: InnovativePhysics
		CompanyName: Such Wow So Games
		CompanyShortName: SWSG

However, as your writing this new physics puzzler, you discover that a chunk of the scripting and assets you've already created for the platformer can be shared between the two projects. So, you use the module builder to make a common project and refactor the common work from *BrilliantPlatform* into *CoreStuff*

	Project3
		 ProjectName: CoreStuff
		CompanyName: Such Wow So Games
		CompanyShortName: SWSG

Now, as you go back and forth between development on your platformer and physcis puzzler you can share common development in your core project. All that's needed is to open up core project, work away, and when your finished, click "Export" on the UVSManager window in Unity. Then, when you open up your platformer or physics games, you can use the UVSManager window to update your dependencies on the core project and pull those changes in. Now you're not duplicating assets and code and you you won't need to manage g asset bundle imports and exports manually. So long as you develop in the appropriate folders outlined above, syncing your project dependencies is now simplified. Additionally, since your visual studio project isn't managed by Unity, you don't need to worry about Unity knocking out you visual studio project settings when syncing. 