#!/bin/bash

set -e

# Clean Build Folder
rm -rf ./build
echo "Cleaned build folder"

# Build Projects
dotnet build ./src/Silk.csproj -c Debug && \
dotnet build ./updater/Updater.csproj -c Debug
echo "Built projects"

# Update Test Mod
cp ./src/bin/Debug/netstandard2.0/Silk.dll ./testing/lib/
echo "Updated test mod"

# Build Test Mod
dotnet build ./testing/SilkTestMod.csproj -c Debug
echo "Built test mod"

# Create Build Directories
mkdir -p ./build/Silk/Library && \
mkdir -p ./build/Silk/Mods && \
mkdir -p ./build/Silk/Updater && \
mkdir -p ./testing/lib
echo "Created build directories"

# Copy DLLs to Library
cp ./src/bin/Debug/netstandard2.0/*.dll ./build/Silk/Library/
echo "Copied DLLs to library"

# Copy SilkTestMod.dll to Mods
cp ./testing/bin/Debug/netstandard2.0/SilkTestMod.dll ./build/Silk/Mods/
echo "Copied SilkTestMod.dll to mods"

# Copy Doorstop Files
cp -r ./doorstop/development/* ./build/
echo "Copied doorstop files"

# Copy Updater
cp -r ./updater/bin/Debug/net6.0/* ./build/Silk/Updater/
echo "Copied updater"

# Move files are start Spiderheck
cp -r ./build/* /run/media/kyles/Storage/SteamLibrary/steamapps/common/SpiderHeck/ && \
xdg-open steam://launch/1329500
echo "Moved files and started Spiderheck"

# Finished
echo "Development build completed"
