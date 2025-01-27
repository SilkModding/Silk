#!/bin/bash

set -e

# Clean Build Folder
rm -rf ./Build

# Build Projects
dotnet build ./Main/Silk.csproj -c Debug && \
dotnet build ./Testing/SilkTestMod.csproj -c Debug && \
dotnet build ./SilkUpdateRestarter/SilkUpdateRestarter.csproj -c Debug

# Create Build Directories
mkdir -p ./Build/Silk/Library && \
mkdir -p ./Build/Silk/Mods && \
mkdir -p ./Build/Silk/Updater && \
mkdir -p ./Testing/lib

# Copy DLLs to Library
cp ./Main/bin/Debug/net472/*.dll ./Build/Silk/Library/

# Copy SilkTestMod.dll to Mods
cp ./Testing/bin/Debug/net472/SilkTestMod.dll ./Build/Silk/Mods/

# Copy Silk.dll to Testing/lib
cp ./Build/Silk/Library/Silk.dll ./Testing/lib/

# Copy Doorstop Files
cp -r ./doorstop/development/* ./Build/

# Copy Updater
cp -r ./SilkUpdateRestarter/bin/Debug/net6.0/* ./Build/Silk/Updater/

# Move files are start Spiderheck
cp -r ./Build/* /run/media/kyles/Storage/SteamLibrary/steamapps/common/SpiderHeck/ && \
xdg-open steam://launch/1329500

# Update Test Mod
dotnet build ./Main/Silk.csproj -c Debug && \
cp ./Main/bin/Debug/net472/Silk.dll ./Testing/lib/

