#!/bin/bash

echo "Starting Substrate.Net.Toolchain code generation :)"

# Load chains data from the chains.json file
CHAINS=$(jq -c '.chains[]' "chains.json")

cd Generated

# Loop through all chains
echo "$CHAINS" | while read -r chain; do
  # Extract the name and websocket values from the current chain object
  NAME=$(echo "$chain" | jq -r '.name')

  echo "Generating: "$NAME

  # Prepare each chain project/solution
  mkdir $NAME
  cd $NAME
  dotnet new sln

  # Prepare data to be used as parameter
  PROJECTNAME=$NAME".NetApi"
  WEBSOCKET=$(echo "$chain" | jq -r '.websocket')
  
  # Use Substrate.Net.Toolchain to generate the correct c# represenation of the chain
  dotnet new substrate --sdk_version 0.6.5 --metadata_websocket $WEBSOCKET --net_api $PROJECTNAME --force --allow-scripts yes --generate_openapi_documentation false

  # Remove unused folders (Few of them are causing unwanted compile-time errors)
  cd $PROJECTNAME
  rm Client -r
  rm Helper -r

  # Remove unused projects
  cd ..
  rm Substrate.Integration -r
  rm Substrate.RestClient -r
  rm Substrate.RestClient.Mockup -r
  rm Substrate.RestClient.Test -r
  rm Substrate.RestService -r

  # Go back to Generated folder
  cd ..
done
