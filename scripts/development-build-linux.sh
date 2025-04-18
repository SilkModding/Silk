#!/bin/bash

set -e

# Clean Build Folder
rm -rf ./build
echo "Cleaned build folder"

# Build Projects
dotnet build ./src/Silk.csproj -c Debug && \
dotnet build ./Updater/Updater.csproj -c Debug
echo "Built projects"

# Update Test Mod
cp ./src/bin/Debug/net472/Silk.dll ./Testing/lib/
echo "Updated test mod"

# Build Test Mod
dotnet build ./Testing/SilkTestMod.csproj -c Debug
echo "Built test mod"

# Create Build Directories
mkdir -p ./build/Silk/Library && \
mkdir -p ./build/Silk/Mods && \
mkdir -p ./build/Silk/Updater && \
mkdir -p ./Testing/lib
echo "Created build directories"

# Copy DLLs to Library
cp ./src/bin/Debug/net472/*.dll ./build/Silk/Library/
echo "Copied DLLs to library"

# Copy SilkTestMod.dll to Mods
cp ./Testing/bin/Debug/net472/SilkTestMod.dll ./build/Silk/Mods/
echo "Copied SilkTestMod.dll to mods"

# Copy Silk.dll to Testing/lib
cp ./build/Silk/Library/Silk.dll ./Testing/lib/
echo "Copied Silk.dll to testing lib"

# Copy Doorstop Files
cp -r ./doorstop/development/* ./build/
echo "Copied doorstop files"

# Copy Updater
cp -r ./Updater/bin/Debug/net6.0/* ./build/Silk/Updater/
echo "Copied updater"

# Move files are start Spiderheck
cp -r ./build/* /run/media/kyles/Storage/SteamLibrary/steamapps/common/SpiderHeck/ && \
xdg-open steam://launch/1329500
echo "Moved files and started Spiderheck"

# Finished
echo "Development build completed"
