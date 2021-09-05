#!/bin/bash -e
dotnet build src/CustomHeader/CustomHeader.csproj -c Release
dotnet pack src/CustomHeader/CustomHeader.csproj -c Release
dotnet nuget push src/CustomHeader/bin/Release/Beginor.AspNetCore.Middlewares.CustomHeader.1.0.0.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm src/CustomHeader/bin/Release/*.nupkg

dotnet build src/GzipStatic/GzipStatic.csproj -c Release
dotnet pack src/GzipStatic/GzipStatic.csproj -c Release
dotnet nuget push src/GzipStatic/bin/Release/Beginor.AspNetCore.Middlewares.GzipStatic.1.0.0.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm src/GzipStatic/bin/Release/*.nupkg

dotnet build src/SpaFailback/SpaFailback.csproj -c Release
dotnet pack src/SpaFailback/SpaFailback.csproj -c Release
dotnet nuget push src/SpaFailback/bin/Release/Beginor.AspNetCore.Middlewares.SpaFailback.1.0.0.nupkg -s nuget.org -k $(cat ~/.nuget/key.txt)
rm src/SpaFailback/bin/Release/*.nupkg
