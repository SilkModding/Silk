@echo off
setlocal enabledelayedexpansion

:: Define the version for the release 
set VERSION=1.0.0  

:: Clean Build Folder
echo Cleaning up previous build...
rmdir /s /q .\Build

:: Build Projects in Release Mode
echo Building projects in Release mode...
dotnet build .\Main\Silk.csproj -c Release
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet build .\Testing\SilkTestMod.csproj -c Release
if %errorlevel% neq 0 exit /b %errorlevel%

:: Create Build Directories
echo Creating build directories...
mkdir .\Build\Silk\Library
mkdir .\Build\Silk\Mods
mkdir .\Testing\lib

:: Copy DLLs to Library
echo Copying main DLLs to Library...
xcopy .\Main\bin\Release\net472\*.dll .\Build\Silk\Library\ /Y /S

:: Copy SilkTestMod.dll to Mods
echo Copying test mod DLL to Mods...
xcopy .\Testing\bin\Release\net472\SilkTestMod.dll .\Build\Silk\Mods\ /Y /S

:: Copy Silk.dll to Testing/lib
echo Copying Silk.dll to Testing\lib...
xcopy .\Build\Silk\Library\Silk.dll .\Testing\lib\ /Y /S

:: Copy Doorstop Files
echo Copying Doorstop files...
xcopy .\doorstop\release\* .\Build\ /Y /S

:: Copy Changelog and README
echo Copying README and CHANGELOG files...
copy .\README.md .\Build\
copy .\CHANGELOG.md .\Build\

:: Create the Versioned Zip Archive
echo Creating versioned zip archive...
cd .\Build
powershell -command "Compress-Archive -Path * -DestinationPath Silk-v%VERSION%.zip"
cd ..

:: Finished
echo Release build completed: Silk-v%VERSION%.zip
pause
