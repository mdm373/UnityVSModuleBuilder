Overview | [Specification](SPECIFICATION.md) | [Developing](DEVELOPMENT.md)
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

####How do I use this thing?###

To create a new module project, launch the UVSModule builder. You can configure the repository location from this app using the settings screen found under "File->Settings". The repository is where module dependencies will be exported to and imported from.

* Launch UVSModule Builder 
	* Enter Module project name, 
	* Enter Module company name 
	* Enter Module company short name
	* Enter Generated module output location
*  If you haven't already, select your unity install folder.
	*  This allows your module's visual studio projects can find is UnityEngine and UnityEditor dependencies. 
* Select Build to generate your project
	* A pop up message should indicate if module was successfully generated

 You can now open the module's Unity3D project in unity.

* Launch Unity and open a new unity project located at the "UnityGame" folder of your generated project
* From unity Select "Window->VSModule Config"
	* This should open the "UnityVSModule Window". From this window you can manage your project
		* **Apply module config**
			* Modify Unity Install location used for visual studio references
			* Update the module repository location on your PC
			* This will not copy the existing repo to the new location. Existing dependencies used from the old repo location will need to be copied or exported to the new location
		* **Export module to repo**
			* Export your module to the local repository for other modules to use as a dependency
		* **Import from repo**
			* This allows you to import the asset bundle associated with another module as a dependency on your module
			* You will need the company short name and project name for the new dependency
			* This dependency must exist in the local repository currently either from having been copied over manually or exported by the dependent project via the "Export module to repo" step.
		* **Update selected** module dependencies
			* Pull in updates for selected modules from the repo
		* **Remove selected** module dependencies
			* Remove the dependency reference to dependency modules. This will not remove the associated assets from your project
	
You can also open the module's Visual Studio solution

* Launch Visual Studio 2013
	* Select the solution file located in the generated project's "VisualStudio" folder
	* Solution contains a **ProjectName** project
		* Builds deploy to the ManagedCode folder of your unity assets folder
		* Has dependencies on UnityEngine 
		* Has dependencies on dependency modules 
			* Automatically maintained by managing dependencies as shown in the above config window section 
		* Used to develop scripts as a managed code DLL to be used by the unity game at game runtime
	* Solution contains a  **ProjectName**Editor project
		* Builds deploy to the ManagedCode folder of your unity game's editor assets folder
		* Has dependencies on UnityEngine and UnityEditor
		* Has dependencies on dependency modules and their editor modules
			* Automatically maintained by managing dependencies as shown in the above config window section
		* Used to develop scripts to be used in the unity editor but not deployed to the game for distribution at runtime

####What does developing with this look like?###

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

Lastly, you add *CoreStuff* as a dependency to both *BriallaintPlatform* and *InnovativePhysics* Now, as you go back and forth between development on your platformer and physcis puzzler you can share common development in your core project. All that's needed is to open up core project, work away, and when your finished, click "Export" on the UVSManager window in Unity. When you open up your platformer or physics games later, you can use the UVSManager window to update your dependencies on the core project and pull those changes in. 

Now you're not duplicating assets and code and you you won't need to manage asset bundle imports and exports manually. So long as you develop in the appropriate folders outlined above, syncing your project dependencies is now simplified. Additionally, since your visual studio project isn't managed by Unity, you don't need to worry about Unity knocking out you visual studio project settings when syncing. 