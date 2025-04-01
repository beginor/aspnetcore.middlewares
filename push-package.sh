#!/bin/bash -e
PACKAGE_VERSION="9.0.0"

dotnet pack src/CustomHeader/CustomHeader.csproj -c Release
dotnet nuget push src/CustomHeader/bin/Release/Beginor.AspNetCore.Middlewares.CustomHeader.$PACKAGE_VERSION.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm src/CustomHeader/bin/Release/*.nupkg

dotnet pack src/GzipStatic/GzipStatic.csproj -c Release
dotnet nuget push src/GzipStatic/bin/Release/Beginor.AspNetCore.Middlewares.GzipStatic.$PACKAGE_VERSION.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm src/GzipStatic/bin/Release/*.nupkg

dotnet pack src/SpaFailback/SpaFailback.csproj -c Release
dotnet nuget push src/SpaFailback/bin/Release/Beginor.AspNetCore.Middlewares.SpaFailback.$PACKAGE_VERSION.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm src/SpaFailback/bin/Release/*.nupkg
