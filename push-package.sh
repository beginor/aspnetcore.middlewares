#!/bin/bash -e
export PACKAGE_VERSION="9.0.0"
export PACKAGE_RELEASE_NOTES="Update to .NET 9.0"
export PACKAGE_TAGS="AspNetCore,Middlewares"
export PACKAGE_PROJECT_URL="https://github.com/beginor/aspnetcore.middlewares"

PROJECTS=( \
  "src/CustomHeader/CustomHeader.csproj" \
  "src/GzipStatic/GzipStatic.csproj" \
  "src/SpaFailback/SpaFailback.csproj" \
)

for PROJ in "${PROJECTS[@]}"
do
  echo "packing $PROJ"
  dotnet pack $PROJ -c Release --output ./nupkgs/
done

dotnet nuget push ./nupkgs/*.nupkg -s nuget.org -k $(cat ~/.nuget/github.txt) \
  --skip-duplicate

rm -rf ./nupkgs
