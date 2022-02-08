#!/bin/bash

VERSION=1.4.18

rm -f  Jars/*.aar 
rm -f  Jars/*.jar

curl -L -o Jars/litr-filters.aar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr-filters/${VERSION}/litr-filters-${VERSION}.aar"
curl -L -o Jars/litr-filters-javadoc.jar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr-filters/${VERSION}/litr-filters-${VERSION}-javadoc.jar"
curl -L -o Jars/litr-filters-sources.jar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr-filters/${VERSION}/litr-filters-${VERSION}-sources.jar"
