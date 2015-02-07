#define AppVer "0.0.1.1"
#define AppLongName "Unity VS Module Builder"
#define AppShortName "UnityVSModuleBuilder"
[Setup]
OutputBaseFilename=Intall {#AppLongName}-{#AppVer}
PrivilegesRequired=admin 
AppName={#AppLongName}
AppVersion={#AppVer}
AppPublisher=Mark Mayer
DefaultDirName={pf}\{#AppShortName}
DefaultGroupName={#AppShortName}
UninstallDisplayIcon={app}\{#AppShortName}UI.exe
UninstallDisplayName=Uninstall {#AppLongName}
Compression=lzma2
SolidCompression=yes
OutputDir=Release
ArchitecturesInstallIn64BitMode=x64 ia64

[Dirs]
Name: "{app}"; Permissions: everyone-full

[Files]
Source: "Content\UnityVSModuleBuilder.dll"; DestDir: "{app}"; Flags: solidbreak
Source: "Content\UnityVSModuleBuilderUI.exe"; DestDir: "{app}";
Source: "Content\UnityVSModuleCommon.dll"; DestDir: "{app}";
Source: "Content\UnityVSModuleEditor.dll"; DestDir: "{app}";
Source: "Content\ProjectTemplate\*"; DestDir: "{app}\ProjectTemplate"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\{#AppLongName}"; Filename: "{app}\UnityVSModuleBuilderUI.exe"

[Registry]
Root: HKLM; Subkey: "Software\{#AppShortName}"; ValueName: "install-location"; ValueData: {app}; ValueType: string; Flags: uninsdeletekey; Permissions: everyone-read