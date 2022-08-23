#!/bin/bash

VERSION=1.5.0

# Remove bin and obj directories.
rm -rf bin/
rm -rf obj/

# Empty jars directory.
rm -rf Jars/
mkdir Jars/

# Download LiTr.Filters library.
echo "Downloading litr-filters.aar"
curl --silent --location --output Jars/litr-filters.aar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr-filters/${VERSION}/litr-filters-${VERSION}.aar"
if [ $? -ne 0 ]
then
    echo "LiTr.Filters Error: Failed to fetch litr-filters.aar."
    exit 1
fi

# Build library.
msbuild Xam.Plugin.LinkedIn.LiTr.Filters.csproj -property:Configuration=Release -target:Restore,Clean,Build
if [ $? -ne 0 ]
then
    echo "LiTr.Filters Error: Failed to build binding."
    exit 1
fi

# Build nuget.
nuget pack Xam.Plugin.LinkedIn.LiTr.Filters.nuspec -Symbols -SymbolPackageFormat snupkg
if [ $? -ne 0 ]
then
    echo "LiTr.Filters Error: Failed to pack nugets."
    exit 1
fi

mv *.nupkg ../
mv *.snupkg ../
