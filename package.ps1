
$orgLocation = $pwd
.\update-version.ps1 -version "increment"
$version  = get-content -Path version.txt
set-location ./Nachonet.Common.Systemd
dotnet pack /p:AssemblyVersion=$version /p:Version=$version

# move ".\bin\Release\nachonet-common-systemd.%VER%.nupkg" "%DEV_PACKAGE_DIR%"
set-location ./bin/Release
dotnet nuget push "Nachonet.Common.Systemd.$($version).nupkg" --api-key "$($env:NUGET_APIKEY)" --source https://api.nuget.org/v3/index.json
set-location $orgLocation
Start-Sleep -Seconds 5
