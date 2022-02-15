#!/bin/bash

#./FetchJars.sh

msbuild Xam.Plugin.LinkedIn.LiTr.csproj -property:Configuration=Release -target:Restore,Clean,Build
nuget pack Xam.Plugin.LinkedIn.LiTr.nuspec -Symbols -SymbolPackageFormat snupkg