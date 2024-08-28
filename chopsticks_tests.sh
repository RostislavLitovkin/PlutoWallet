echo "Starting Chopsticks tests :)"

SELECTED_NAME=""

# Check if a name was passed as an argument
if [ $# -gt 0 ]; then
    SELECTED_NAME="$1"
fi

# Load chains data from the chains.json file
CHAINS=$(jq -c '.chains[]' "chains.json")

cd PlutoWalletTests

# Loop through all chains
echo "$CHAINS" | while read -r chain; do
# Extract the name and websocket values from the current chain object
  NAME=$(echo "$chain" | jq -r '.name')

  # skip other chains, generate only the selected name chain
  if [ "$SELECTED_NAME" != "" ]; then
    if [ "$SELECTED_NAME" != $NAME ]; then
      continue
    fi
  fi

  echo "Starting chopsticks for: "$NAME

  # Prepare data to be used as parameter
  WEBSOCKET=$(echo "$chain" | jq -r '.websocket')
  
  npx @acala-network/chopsticks@latest --endpoint=$WEBSOCKET &

  PID=$!

  echo "Now running actual tests :)))"

  dotnet test --filter "FullyQualifiedName~PlutoWalletTests.TransferTests"

  echo "dotnet test finished"

  # Kill the process using its PID
  kill $PID

  # Optionally, wait for the process to stop
  wait $PID

  echo "Chopsticks stopped"

done
