
$orgLocation = $pwd
.\update-version.ps1 -version "increment"
$version  = get-content -Path version.txt
set-location ./Nachonet.Common.Systemd
dotnet pack /p:AssemblyVersion=$version /p:Version=$version

# move ".\bin\Release\nachonet-common-systemd.%VER%.nupkg" "%DEV_PACKAGE_DIR%"
set-location $orgLocation
