$MyInvocation.MyCommand.Path | Split-Path | Push-Location
$Name = (ls *.csproj).BaseName
dotnet build -c Release
mkdir BepInEx\plugins\$Name
cp bin\Release\netstandard2.0\$Name.dll BepInEx\plugins\$Name\
Compress-Archive .\BepInEx\ $Name-v
rmdir .\BepInEx\ -Recurse