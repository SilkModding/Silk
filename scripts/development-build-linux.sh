#!/bin/bash

set -e

# Clean Build Folder
rm -rf ./Build
echo "Cleaned build folder"

# Build Projects
dotnet build ./Main/Silk.csproj -c Debug && \
dotnet build ./Updater/Updater.csproj -c Debug
echo "Built projects"

# Update Test Mod
cp ./Main/bin/Debug/net472/Silk.dll ./Testing/lib/
echo "Updated test mod"

# Build Test Mod
dotnet build ./Testing/SilkTestMod.csproj -c Debug
echo "Built test mod"

# Create Build Directories
mkdir -p ./Build/Silk/Library && \
mkdir -p ./Build/Silk/Mods && \
mkdir -p ./Build/Silk/Updater && \
mkdir -p ./Testing/lib
echo "Created build directories"

# Copy DLLs to Library
cp ./Main/bin/Debug/net472/*.dll ./Build/Silk/Library/
echo "Copied DLLs to library"

# Copy SilkTestMod.dll to Mods
cp ./Testing/bin/Debug/net472/SilkTestMod.dll ./Build/Silk/Mods/
echo "Copied SilkTestMod.dll to mods"

# Copy Silk.dll to Testing/lib
cp ./Build/Silk/Library/Silk.dll ./Testing/lib/
echo "Copied Silk.dll to testing lib"

# Copy Doorstop Files
cp -r ./doorstop/development/* ./Build/
echo "Copied doorstop files"

# Copy Updater
cp -r ./Updater/bin/Debug/net6.0/* ./Build/Silk/Updater/
echo "Copied updater"

# Move files are start Spiderheck
cp -r ./Build/* /run/media/kyles/Storage/SteamLibrary/steamapps/common/SpiderHeck/ && \
xdg-open steam://launch/1329500
echo "Moved files and started Spiderheck"

# Finished
echo "Development build completed"