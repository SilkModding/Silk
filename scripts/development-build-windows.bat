@echo off

:: Clean Build Folder
rmdir /s /q ..\build

:: Build Projects
dotnet build ..\src\Silk.csproj -c Debug && ^
dotnet build ..\testing\SilkTestMod.csproj -c Debug && ^
dotnet build ..\updater\Updater.csproj -c Debug

:: Create Build Directories
mkdir ..\build\Silk\Library
mkdir ..\build\Silk\Mods
mkdir ..\build\Silk\Updater
mkdir ..\testing\lib

:: Copy DLLs to Library
copy ..\src\bin\Debug\netstandard2.0\*.dll ..\build\Silk\Library\

:: Copy SilkTestMod.dll to Mods
copy ..\testing\bin\Debug\netstandard2.0\SilkTestMod.dll ..\build\Silk\Mods\

:: Copy Doorstop Files
xcopy /e /i ..\doorstop\development\* ..\build\

:: Copy updater
xcopy /e /i ..\updater\bin\Debug\net6.0\* ..\build\Silk\Updater

:: Move files and start Spiderheck
xcopy /e /i ..\build\* "C:\Program Files (x86)\Steam\steamapps\common\SpiderHeck"
pause
start steam://launch/1329500

:: Update Test Mod
dotnet build ..\src\Silk.csproj -c Debug && ^
copy ..\src\bin\Debug\netstandard2.0\Silk.dll ..\testing\lib\
