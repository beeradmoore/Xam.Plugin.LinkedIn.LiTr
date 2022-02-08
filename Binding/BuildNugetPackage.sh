#!/bin/bash

#./FetchJars.sh

msbuild Xam.Plugin.LinkedIn.LiTr.csproj -property:Configuration=Release -target:Clean,Build
nuget pack Xam.Plugin.LinkedIn.LiTr.nuspec -Symbols -SymbolPackageFormat snupkg