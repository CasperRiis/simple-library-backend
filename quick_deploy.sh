##!/bin/bash

# Use this script to quickly deploy the app to Azure App Service without Terraform

set -e

dotnet clean
dotnet restore
dotnet publish -c Release

powershell 'Compress-Archive -Path ./bin/Release/net8.0/publish/* -DestinationPath ./bin/Release/publish.zip -Force'

az webapp deploy --resource-group simple-library --name simple-library-backend --src-path "./bin/Release/publish.zip"

rm ./bin/Release/publish.zip # Clean up the zip file