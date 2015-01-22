# UnityVSModuleBuilder
Unity3D project generator for modular development with separated Visual Studio C# scripting solutions.

## Goals
Provide a standalone app / library to allow developers to generate a project from a template Unity module project. Module project is structured so that script assets are maintained in separate visual studio project which deploys script as a managed DLL asset dependency for the runtime game and separate editor DLL asset for the Unity editor.

Module projects can be exported / imported as dependencies to any unity project as a .assetpackage

Modulue projects will include unity editor functionality to update project to a module repository and update project dependencies from repository via automated import / export

## Development Structure
TBD

## Status
Structuring this doc.
Defining out requirements in this doc
Incorporating test automation into build process using nunit

## Requirements
- Generation enables the following configurations
    - Project Name
      - Used as name of generated project folder
      - Unity Project Settings "Product Name"
      - Unity asset folder under company name configured folder
      - Formatted inAndroid bundle identifier as "XXXX.productname.XXXX"
      - Visual Studio project Name
      - Visual Studio project assembly name
      - Prefix for supporting "Editor" Visual Studio project name
      - Prefix for supporting "Editor" Visual Studio assembly name.
      - Appended to default namespace of both VS and VS Editor projects
    - Company Name
      - Unity Project Settings "ComapnyName" 
      - Formatted in Android bundle identifier as "companyname.XXXX.XXXX"
      - Folder in root of unity project assets directory
    - List of project dependency modules
       - maintained list of dependency unity module projects
      - See the dependency import export section of goals for details
    - File location of dependency repo
      - See the dependency import export section of goals for details
    - Output directory folder
     - TBD Default
   - Unity install location
      - Used for Visual Studio project dependencies
- Generation creates project name directory
- Generation creates  .gitignore file from template provided .gitignore in root
- Generated project includes "UnityGame" project folder in root\
   - Unity project contents copied from template project
   - Unity project configured for above company name setting / project name setting
   - Unity project configured to hide metadata from source control
   - Unity project includes assets to facilitate dependency module import / export from editor
     - TBD Further documentation
- Generated project includes "VisualStudio" project folder in root
    - contents copied from template project
    - contents include solution with child "productName" and "productNameEditor cs projects
    - productName project includes core unity library references
    - productName project includes dependency productName project module references
    - productName project deploys to "ManagedCode" location under companyname folder of unity assets directory
    - TBD Further documentation
    - editor project includes "productName" project reference
    - editor project includes unity editor libarary reference
    - editor project includes dependency module editor project references
    - editor project project deploys to "ManagedCode" location under companyname folder of unity editor assets directory
   
