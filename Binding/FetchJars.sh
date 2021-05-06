#!/bin/sh

VERSION=1.4.14

rm -f  Jars/*.aar 
rm -f  Jars/*.jar

curl -L -o Jars/litr.aar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr/${VERSION}/litr-${VERSION}.aar"
curl -L -o Jars/litr-javadoc.jar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr/${VERSION}/litr-${VERSION}-javadoc.jar"
curl -L -o Jars/litr-sources.jar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr/${VERSION}/litr-${VERSION}-sources.jar"

rm -rf JavaDocs
mkdir JavaDocs
pushd JavaDocs
    cp ../Jars/litr-javadoc.jar .
    jar xf litr-javadoc.jar
    rm -f litr-javadoc.jar
popd