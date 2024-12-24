@echo off

:: Clean Build Folder
rmdir /s /q .\Build

:: Build Projects
dotnet build .\Main\Silk.csproj -c Debug && ^
dotnet build .\Testing\SilkTestMod.csproj -c Debug

:: Create Build Directories
mkdir .\Build\Silk\Library
mkdir .\Build\Silk\Mods
mkdir .\Testing\lib

:: Copy DLLs to Library
copy .\Main\bin\Debug\net472\*.dll .\Build\Silk\Library\

:: Copy SilkTestMod.dll to Mods
copy .\Testing\bin\Debug\net472\SilkTestMod.dll .\Build\Silk\Mods\

:: Copy Silk.dll to Testing\lib
copy .\Build\Silk\Library\Silk.dll .\Testing\lib\

:: Copy Doorstop Files
xcopy /e /i .\doorstop\* .\Build\

:: Move files and start Spiderheck
xcopy /e /i .\Build\* "C:\Storage\SteamLibrary\steamapps\common\SpiderHeck\" && ^
start steam://launch/1329500

:: Update Test Mod
dotnet build .\Main\Silk.csproj -c Debug && ^
copy .\Main\bin\Debug\net472\Silk.dll .\Testing\lib\
