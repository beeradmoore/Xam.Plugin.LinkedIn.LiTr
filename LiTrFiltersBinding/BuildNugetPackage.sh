#!/bin/bash

#./FetchJars.sh

msbuild Xam.Plugin.LinkedIn.LiTr-Filters.csproj -property:Configuration=Release -target:Restore,Clean,Build
nuget pack Xam.Plugin.LinkedIn.LiTr-Filters.nuspec -Symbols -SymbolPackageFormat snupkg

mv *.nupkg ../Binding/
mv *.snupkg ../Binding/
