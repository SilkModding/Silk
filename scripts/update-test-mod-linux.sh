#!/bin/bash

# Update Test Mod
echo "Updating Test Mod..."
dotnet build ./Main/Silk.csproj -c Debug && \
cp ./Main/bin/Debug/net472/Silk.dll ./Testing/lib/
