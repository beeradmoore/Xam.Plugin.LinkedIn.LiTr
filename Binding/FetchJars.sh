#!/bin/bash


VERSION=1.4.18

rm -rf Jars/
mkdir Jars/

curl -L -o Jars/litr.aar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr/${VERSION}/litr-${VERSION}.aar"
#curl -L -o Jars/litr-javadoc.jar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr/${VERSION}/litr-${VERSION}-javadoc.jar"
#curl -L -o Jars/litr-sources.jar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr/${VERSION}/litr-${VERSION}-sources.jar"

#curl -L -o Jars/litr-filters.aar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr-filters/${VERSION}/litr-filters-${VERSION}.aar"
#curl -L -o Jars/litr-filters-javadoc.jar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr-filters/${VERSION}/litr-filters-${VERSION}-javadoc.jar"
#curl -L -o Jars/litr-filters-sources.jar "https://search.maven.org/remotecontent?filepath=com/linkedin/android/litr/litr-filters/${VERSION}/litr-filters-${VERSION}-sources.jar"


#pushd Jars/
#    mkdir temp/
#    pushd temp/
#        jar -xf ../litr-filters-sources.jar
#        jar -xf ../litr-sources.jar
#        jar -cf ../litr-plus-filters-sources.jar .
#    popd
#    rm -rf temp/

#cp litr-sources.jar litr-plus-filters-sources.jar
#unzip litr-filters-sources.jar -d litr-filters-sources/     
    #pushd litr-filters-sources/
    #    jar uf ../litr-plus-filters-sources.jar .
    #popd
#popd

# JavaDocs temp disabled.
#rm -rf JavaDocs
#mkdir JavaDocs
#pushd JavaDocs
#    cp ../Jars/litr-javadoc.jar .
#    jar xf litr-javadoc.jar
#    rm -f litr-javadoc.jar
#popd