[Overview](OVERVIEW.md) | Specification | [Developing](DEVELOPMENT.md)

# UVSModule Manager Specifications#
## Specifications For Module Builder / Maintenance  ##


###Application Common Specification###
Application install location **[APP_LOCATION]** defined by registry key entry

	HKEY_LOCAL_MACHINE\Software\UnityVSmoduleBuilder\install-location

Root of directory found in application install location defines configuration file
	
	AppSettings.xml

Configuration file contains

	Element: ApplicationSettingsXMLModel
	Attributes:
			repositoryLocation

Application repository location **[REPO_LOCATION]** defined by *repositoryLocation* attribute. If file is not present or malformed, default repository location is assumed to be 

	[APP_LOCATION]\UVSRepo

###Builder Specification###
Building the project will take a request with the following parameters

* Template Copy Target Location **[COPY_LOCATION]**:  Valid Location on file system with read / write user privileges or higher
* Project Name **[PROJECT_NAME]** : Case Sensitive alpha / numeric no whitespace name for project.
* Company Name **[COMPANY_NAME]** : Case Sensitive alpha / numeric name for company
* Company Short Name **[COMPANY\_SHORT_NAME]** : Case Sensitive alpha / numeric no whitespace abbreviated name for company
* Unity Location **[UNITY_LOCATION]** : Install location of unity on the file system.

The builder will copy a project template located at the following location.

	[APP_LOCATION]\ProjectTemplate

Project template folder will be copied to the [COPY_LOCATION] defined in the request

TBD

###Unity Editor Specification###

TBD