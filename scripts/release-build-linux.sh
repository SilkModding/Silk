#!/bin/bash

set -e

# Define the version for the release 
VERSION="0.5.0"

# Clean Build Folder
echo "Cleaning up previous build..."
rm -rf ./build

# Build Projects in Release Mode
echo "Building projects in Release mode..."
dotnet build ./src/Silk.csproj -c Release && \
dotnet build ./Testing/SilkTestMod.csproj -c Release && \
dotnet build ./Updater/Updater.csproj -c Release

# Create Build Directories
echo "Creating build directories..."
mkdir -p ./build/Silk/Library && \
mkdir -p ./build/Silk/Mods && \
mkdir -p ./build/Silk/Updater && \
mkdir -p ./Testing/lib

# Copy DLLs to Library
echo "Copying main DLLs to Library..."
cp ./src/bin/Release/net472/*.dll ./build/Silk/Library/

# Copy SilkTestMod.dll to Mods
echo "Copying test mod DLL to Mods..."
cp ./Testing/bin/Release/net472/SilkTestMod.dll ./build/Silk/Mods/

# Copy Silk.dll to Testing/lib
echo "Copying Silk.dll to Testing/lib..."
cp ./build/Silk/Library/Silk.dll ./Testing/lib/

# Copy Doorstop Files
echo "Copying Doorstop files..."
cp -r ./doorstop/release/* ./build/

# Copy Updater
echo "Copying updater files..."
cp -r ./Updater/bin/Release/net6.0/* ./build/Silk/Updater/

# Copy Changelog and README
echo "Copying README and CHANGELOG files..."
cp ./README.md ./build/
cp ./CHANGELOG.md ./build/

# Zip the Build into a Versioned Archive
echo "Creating versioned zip archive..."
cd ./build && zip -r "Silk-v${VERSION}.zip" * && cd ..

# Move the zip file to a distribution folder
mkdir -p ./release
mv ./build/Silk-v${VERSION}.zip ./release/

# Finished
echo "Release build completed: Silk-v${VERSION}.zip"

