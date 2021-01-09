#!/bin/bash

mkdir -p Build

BUILD_TAG=$(cat Assets/Plugins/CandyCoded.Forms/Version.txt)

LATEST_UNITY_VERSION=$(find-unity)

echo "Building with ${LATEST_UNITY_VERSION}"

"${LATEST_UNITY_VERSION}/Contents/MacOS/Unity" \
    -batchmode \
    -nographics \
    -silent-crashes \
    -logFile "$(pwd)/unity.log" \
    -projectPath "$(pwd)/" \
    -exportPackage "Assets/Plugins" \
    "$(pwd)/Build/CandyCoded.Forms-${BUILD_TAG}.unitypackage" \
    -quit
