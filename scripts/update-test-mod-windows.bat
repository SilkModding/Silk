# Update Test Mod
echo "Updating Test Mod..."
dotnet build .\src\Silk.csproj -c Debug && \
copy .\src\bin\Debug\net472\Silk.dll .\Testing\lib\
