#!/bin/bash

VERSION=1.4.19

# Remove bin and obj directories.
rm -rf bin/
rm -rf obj/

# Empty jars directory.
rm -rf Jars/
mkdir Jars/

# Download litr library.
echo "Downloading litr.aar"
curl --silent --location --output Jars/litr.aar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr/${VERSION}/litr-${VERSION}.aar"
if [ $? -ne 0 ]
then
    echo "LiTr Error: Failed to fetch litr.aar."
    exit 1
fi

# Build library.
msbuild Xam.Plugin.LinkedIn.LiTr.csproj -property:Configuration=Release -target:Restore,Clean,Build
if [ $? -ne 0 ]
then
    echo "LiTr Error: Failed to build binding."
    exit 1
fi

# Build nuget.
nuget pack Xam.Plugin.LinkedIn.LiTr.nuspec -Symbols -SymbolPackageFormat snupkg
if [ $? -ne 0 ]
then
    echo "LiTr Error: Failed to pack nugets."
    exit 1
fi

mv *.nupkg ../
mv *.snupkg ../

