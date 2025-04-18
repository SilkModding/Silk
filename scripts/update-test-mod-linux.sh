#!/bin/bash

# Update Test Mod
echo "Updating Test Mod..."
dotnet build ./src/Silk.csproj -c Debug && \
cp ./src/bin/Debug/net472/Silk.dll ./testing/lib/
