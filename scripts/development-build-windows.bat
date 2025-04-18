@echo off

:: Clean Build Folder
rmdir /s /q ..\build

:: Build Projects
dotnet build ..\src\Silk.csproj -c Debug && ^
dotnet build ..\Testing\SilkTestMod.csproj -c Debug && ^
dotnet build ..\Updater\Updater.csproj -c Debug

:: Create Build Directories
mkdir ..\build\Silk\Library
mkdir ..\build\Silk\Mods
mkdir ..\build\Silk\Updater
mkdir ..\Testing\lib

:: Copy DLLs to Library
copy ..\src\bin\Debug\net472\*.dll ..\build\Silk\Library\

:: Copy SilkTestMod.dll to Mods
copy ..\Testing\bin\Debug\net472\SilkTestMod.dll ..\build\Silk\Mods\

:: Copy Silk.dll to Testing\lib
copy ..\build\Silk\Library\Silk.dll ..\Testing\lib\

:: Copy Doorstop Files
xcopy /e /i ..\doorstop\development\* ..\build\

:: Copy updater
xcopy /e /i ..\Updater\bin\Debug\net6.0\* ..\build\Silk\Updater

:: Move files and start Spiderheck
xcopy /e /i ..\build\* "C:\Program Files (x86)\Steam\steamapps\common\SpiderHeck"
pause
start steam://launch/1329500

:: Update Test Mod
dotnet build ..\src\Silk.csproj -c Debug && ^
copy ..\src\bin\Debug\net472\Silk.dll ..\Testing\lib\
