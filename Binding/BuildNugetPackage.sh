#!/bin/bash

rm -f *.nupkg
rm -f *.snupkg

# Go bind LiTr, or exit if it failed.
pushd LiTr
    ./BuildNugetPackage.sh
    if [ $? -ne 0 ]
    then
        exit 1
    fi
popd


# Go bind LiTr.Filters, or exit if it failed.
pushd LiTr.Filters
    ./BuildNugetPackage.sh
    if [ $? -ne 0 ]
    then
        exit 1
    fi
popd
