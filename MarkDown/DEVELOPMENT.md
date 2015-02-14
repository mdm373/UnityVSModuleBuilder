[Overview](OVERVIEW.md) | [Specification](SPECIFICATION.md) | Developing

# UVSModule Manager Development#
## Development Structure And Practices  ##


###Visual Studio###

This application has been developed in C# using Visual Studio Community Edition.

The solution is located under "UnityVSModuleBuilder" broken down into the following projects

* UnityVSModuleBuilder (DLL)
	* All logic and processing for building module projects from template.
	* See "TemplateProjectBuilderImpl.DoBuild" entry point details
	* Dependes on 
		* UnityVSModuleCommon
* UnityVSModuleBuilderCommandLine (EXE)
	* Command Line UI for "UnityVSModuleBuilder" build requests
	* Depends on
		* UnityVSModuleBuilder
		* UnityVSModuleCommon
* UnityVSModuleBuilderGUI (EXE)
	* Windows Forms interface for generating "UnityVSModuleBuilder" build requests
	* Depends on
		* UnityVSModuleBuilder
		* UnityVSModuleCommon 
* UnityVSModuleCommon (DLL)
	* Common "Application" code that can be shared between both the editor and builder
	* Xml model abstraction "XMLStore" that allows for serialized persistence of common code.
	* Provides wrapping interfaces for common functionality needed to inject dependencies for unit testing
		* FileSystem
		* XmlSerialization
		* Logging
		* Registry Access
		* Stream Access
* UnityVSModuleDrivers (EXE)
	* Driver code that allows for easy integration testing of any exposed dependency functionality.
	* Ensures that development debugging utils are included in separate artifact and not included in runtime distribution of the application
	* Allows for mock debugging of unity through mock unityAPI interface so testing of editor tools can be performed absent the unity engine.
	* Depends on:
		* Anything as needed with the exception of test code
* UnityVSModuleEditor (UnityEditor DLL)
	* UnityEditor window and menu item for module management operations
	* Depends on
		* UnityVSModuleCommon
		* UnityEngine
		* UnityEditor
* Unit Test Projects
	* Development of this project has been done using TDD as much as possible.
		* UI Has been excluded
		* "3rd party" wrapped dependencies have been excluded as needed
			* FileSystem and registry access
			* UnityAPI
			* XML Serialization
	* Ideally, tests should be structured as
		* "Given" some series of conditions are in place
		*  "When" some operation occurs
		*  "Then" some series of results are verified
	* UnityVSModuleEditorTests
		* Tests should cover as much of "UnityVsModuleEditor.MiddleTier" namespace as possible
	* UnityVSModuleCommonTests
		* Tests should cover as much of "UnityVSModuleCommon.Application" namespace as possible
	* UnityVSModuleBuilderTests
		* Tests should cover as much of "UnityVSModuleBuilder" namespace and child namespaces as possible
	* These projects contain NUnit tests that cover as much of their associated projects as allowed.
	* Associated projects us MSBuildTask.Nunit to fail their respective builds if tests fail.
	* Separation of tests into these projects ensures that test artifacts are fully separated from final release and not embedded in runtime level artifacts.




###Inno###
Inno folder includes script used by Inno to generate Installation msi package.

Release builds of the visual studio project are configured to build to the Inno/Content folder

###Illustrator###

The Illustrator folder contains Adobe Illustrator files used to create logo art included in window forms application for project builder

###MarkDown###

Includes these super helpful, sometimes meta, markdown files to explain whats going on here.