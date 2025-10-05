@echo off
setlocal enabledelayedexpansion

:: Define the version for the release 
set VERSION=0.6.1 

:: Clean Build Folder
echo Cleaning up previous build...
rmdir /s /q .\build

:: Build Projects in Release Mode
echo Building projects in Release mode...
dotnet build .\src\Silk.csproj -c Release
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet build .\testing\SilkTestMod.csproj -c Release
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet build .\updater\Updater.csproj -c Release
if %errorlevel% neq 0 exit /b %errorlevel%

:: Create Build Directories
echo Creating build directories...
mkdir .\build\Silk\Library
mkdir .\build\Silk\Mods
mkdir .\build\Silk\Updater
mkdir .\testing\lib

:: Copy DLLs to Library
echo Copying main DLLs to Library...
xcopy .\src\bin\Release\netstandard2.0\*.dll .\build\Silk\Library\ /Y /S

:: Copy SilkTestMod.dll to Mods
echo Copying test mod DLL to Mods...
xcopy .\testing\bin\Release\netstandard2.0\SilkTestMod.dll .\build\Silk\Mods\ /Y /S

:: Copy Doorstop Files
echo Copying Doorstop files...
xcopy .\doorstop\release\* .\build\ /Y /S

:: Copy Updater
echo Copying updater files...
xcopy /e /i .\updater\bin\Release\net6.0\* .\build\Silk\Updater

:: Copy Changelog and README
echo Copying README and CHANGELOG files...
copy .\README.md .\build\
copy .\CHANGELOG.md .\build\

:: Create the Versioned Zip Archive
echo Creating versioned zip archive...
cd .\build
powershell -command "Compress-Archive -Path * -DestinationPath Silk-v%VERSION%.zip"
cd ..

:: Move the zip file to a distribution folder
mkdir .\release
move .\build\Silk-v%VERSION%.zip .\release\

:: Finished
echo Release build completed: Silk-v%VERSION%.zip
pause
