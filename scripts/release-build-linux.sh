#!/bin/bash

set -e

# Define the version for the release 
VERSION="0.5.0"

# Clean Build Folder
echo "Cleaning up previous build..."
rm -rf ./Build

# Build Projects in Release Mode
echo "Building projects in Release mode..."
dotnet build ./Main/Silk.csproj -c Release && \
dotnet build ./Testing/SilkTestMod.csproj -c Release && \
dotnet build ./Updater/Updater.csproj -c Release

# Create Build Directories
echo "Creating build directories..."
mkdir -p ./Build/Silk/Library && \
mkdir -p ./Build/Silk/Mods && \
mkdir -p ./Build/Silk/Updater && \
mkdir -p ./Testing/lib

# Copy DLLs to Library
echo "Copying main DLLs to Library..."
cp ./Main/bin/Release/net472/*.dll ./Build/Silk/Library/

# Copy SilkTestMod.dll to Mods
echo "Copying test mod DLL to Mods..."
cp ./Testing/bin/Release/net472/SilkTestMod.dll ./Build/Silk/Mods/

# Copy Silk.dll to Testing/lib
echo "Copying Silk.dll to Testing/lib..."
cp ./Build/Silk/Library/Silk.dll ./Testing/lib/

# Copy Doorstop Files
echo "Copying Doorstop files..."
cp -r ./doorstop/release/* ./Build/

# Copy Updater
echo "Copying updater files..."
cp -r ./Updater/bin/Release/net6.0/* ./Build/Silk/Updater/

# Copy Changelog and README
echo "Copying README and CHANGELOG files..."
cp ./README.md ./Build/
cp ./CHANGELOG.md ./Build/

# Zip the Build into a Versioned Archive
echo "Creating versioned zip archive..."
cd ./Build && zip -r "Silk-v${VERSION}.zip" * && cd ..

# Move the zip file to a distribution folder
mkdir -p ./Releases
mv ./Build/Silk-v${VERSION}.zip ./Releases/

# Finished
echo "Release build completed: Silk-v${VERSION}.zip"

