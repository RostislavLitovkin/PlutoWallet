#!/bin/bash

echo "Starting Substrate.Net.Toolchain code generation :)"

# File name of the JSON containing the chains array
CHAINS_FILE="chains.json"

# Read the JSON file and extract the chains array
CHAINS=$(jq -c '.chains[]' "$CHAINS_FILE")

cd Generated

# Loop through each element in the chains array
echo "$CHAINS" | while read -r chain; do
  # Extract the name and websocket values from the current chain object
  NAME=$(echo "$chain" | jq -r '.name')

  echo "Generating: "$NAME
  cd $NAME

  PROJECTNAME=$NAME".NetApi"
  WEBSOCKET=$(echo "$chain" | jq -r '.websocket')
  
  # Run the dotnet command with the extracted values
  dotnet new substrate --sdk_version 0.6.5 --metadata_websocket $WEBSOCKET --net_api $PROJECTNAME --force --allow-scripts yes --generate_openapi_documentation false

  cd $PROJECTNAME
  rm Client -r

  cd ..

  rm Substrate.Integration -r
  rm Substrate.RestClient -r
  rm Substrate.RestClient.Mockup -r
  rm Substrate.RestClient.Test -r
  rm Substrate.RestService -r

  cd ..
done