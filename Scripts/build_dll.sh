#!/bin/bash

mkdir -p Build

dotnet build Forms.sln

cp Temp/bin/Debug/CandyCoded.Forms.dll "Build/CandyCoded.Forms.dll"
