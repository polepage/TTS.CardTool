; Definitions
#define AppURL "https://github.com/polepage/TTS.CardTool"
#define GlobalAppName "Tabletop Simulator Card Tool"

[Setup]
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
DefaultDirName={autopf}\{#GlobalAppName}
DirExistsWarning=no
DisableProgramGroupPage=yes
InfoBeforeFile=warning.txt
PrivilegesRequired=lowest
OutputDir=bin
OutputBaseFilename={#AppInitialName}_Install
SetupIconFile=..\{#ProjectFolder}\icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\{#ProjectFolder}\bin\Publish\*"; DestDir: "{app}\{#DataFolder}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#GlobalAppName}\{#AppShortName}"; Filename: "{app}\{#DataFolder}\{#Executable}"
Name: "{app}\{#AppInitialName}"; Filename: "{app}\{#DataFolder}\{#Executable}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#DataFolder}\{#Executable}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#DataFolder}\{#Executable}"; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
Filename: "https://dotnet.microsoft.com/download/dotnet-core/current/runtime"; Description: "Open browser to download .NET Core Desktop runtime"; Flags: nowait postinstall shellexec