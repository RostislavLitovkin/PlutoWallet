Everything in this master branch was coded until the 17.2.2023 (Polkadot Global Series deadline date).

To see the most recent working version, go to the [devel branch](https://github.com/RostislavLitovkin/PlutoWallet/tree/devel).

# Download

Android apk: https://rostislavlitovkin.pythonanywhere.com/downloadplutowallet

Other platforms will be available for download probably in August.

# PlutoWallet
A sample wallet that implements [Plutonication](https://github.com/cisar2218/Plutonication).

Supported platforms:
- Android & WearOS
- iOS & ipadOS
- MacCatalyst
- Windows

The wallet supports these functionalities:
- generating Mnemonics & creating a privateKey
- showing and sharing your public key and ss58 key
- connecting to any substrate based blockchain/parachain
- getting your actual balance
- transfer of native assets

3rd party integrations:
- [Calamar explorer](https://github.com/topmonks/calamar)
- [Coingecko](https://www.coingecko.com/)
- [Staking dashboard](https://staking.polkadot.network/)

# Differentiating factors

### 1) Multinetwork optimisation
The UI/UX is optimised to support multichain out of the box.
Instead of having one chain selected at a time, you can have chain groups.
This makes the UX more approachable and UI simpler.

### 2) Custom layouts (PlutoLayouts)
Extremely important tool for onboarding new users.
Not only can you optimise UI layouts to your needs, you can also export them to other users.
This can be especially handy for dApp projects:
- In a typical use-case, dApp developers would have to teach new users how to use their wallets. It is usually a big hassle, the user has to find the chain they are looking for and they would still see many confusing crypto functionalities that they would not care about.
- With PlutoLayouts, dApp developers can export the ideal layout (UX optimised for their specific app) and share them to the new users so that they have got the ideal UX out of the box. They will see only features they would actually care about.
- It is highly customisable. When the users decide they want to do more in the ecosystem, they can easily add more functionalities to the wallet. They can learn to use Polkadot on their own pace.

### 3) Plutonication ability
Plutonication allows users to connect PlutoWallet to other dApps seamlessly (on any platforms).
DApp just generates a QR code and once it is scanned in the wallet, they will pair and the wallet will be able to receive transaction requests from the dApp. To learn more, visit https://github.com/cisar2218/Plutonication.

### 4) Multiplatform development
This project is developed using .net MAUI framework, which allows simple development of native mobile apps on many different platforms from a single codebase.
This is crucial for quick (and easier) development while preserving the quality.

# [Plutonication](https://github.com/cisar2218/Plutonication)
Allows the wallet to communicate with any dApps and sign their respective transaction requests without the risk of compromising the private key.


# Terminology
- dApp = any application that uses crypto functionalities. In order to use dApps, it needs to communicate with the crypto wallet somehow, or it needs to know your private key (very insecure).
- Substrate key = ss58 encoded key with "42" prefix.
- Chain-specific key = ss58 encoded key with a custom prefix.
- Chain = either standalone blockchain, relay chain, or parachain. (and/or combination of them)
- Extrinsic = A transaction (more info: https://wiki.polkadot.network/docs/learn-extrinsics)
- PlutoLayout = a simple way to save and export custom layouts.


# Work / milestones

### part 1 - Plutonication (Pluto connector)
> **Note for development:** to update git submodule, run: `git submodule update --remote`
0) [x] Create a simple dApp to test the connection between the Wallet and dApp.
1) [x] Find out how to connect the 2 apps via WebSocket (look into resources) - solved via TCP protocol.
2) [x] Create the 2 basic call operations/methods:
  - SendMessage(PlutoMessage{Code, Data}) 
  - ReceiveMessage(PlutoMessage{Code, Data})
    - where code is ID which determinate type of message. Data are content of message.
3) [x] Make this a modular package -> release to Nuget.
4) [X] Class with URI link (containing IP address, port, and authentication token).
  - Also allow parameters: Name (dApp name), Icon (URL to dApp icon). These two params may help identify the nature of the incoming transaction when scanning QR code.
  - QR code generation will probably be part of PlutoWallet.
6) [X] Convert to async.
7) [X] Create safe listen+connection.
  - Wallet (client): `Connect(ipAddress, port, auth)`, dApp (server): `StartServer(port, auth)`
  - Listen will compare received `auth` with held `auth`. If match: OK, else: don't match -> kick.
8) [X] create client which handles infinite receive loop completely. Event driven architecture:
  - Start (Connect/Listen), which will pair automatically using given credentials and set up a loop that will receive messages (until CloseConnection is called).
  - Include Recv. message event.
  - ConnectionClosed event.
  - ConnectionEstablished event.
9) [ ] Experimental object class sending including object serialization (Status: deserialization is unreliable. May work on this feature in the future. May use a totally different approach. See the issue [here](https://github.com/cisar2218/Plutonication/issues/6), branch here)

15) [X] nuget package
16) [X] ip address on android devices
50) [X] (VERY IMPORTANT) create a very detailed (and begginer friendly) documentation with how to use it and add examples

101) [ ] polkadot js integration (ask Rosťa for more)

102) [ ] make a javascript version

##### Usecases
1. pair devices
2. send publickey to wallet (string)
3. send transaction <"header",byte, byte, byte[]> from dApp to wallet:
  - wallet y/n -> response (failed due sth/rejected/accepted <enum>)
  - dApp: display status of transaction (response)
4. both wallet and dApp are able to send data in form of:
  - `byte[]` byte array
  - `string`
  - alone `MessageCode` enum (can be used for example to send confirmation with `MesageCode.Success`, `MessageCode.Refused`, `MessageCode.FilledOut`)
4. close connection (from both sides):
 - on connection lost event
 - throws exceptions in proper cases/places

##### QR code docs (format):
1) Starts with ``` plutonication: ```
2) Query parameters:
  - url = dApp url to connect to (with port),
    example: ``` url=192.168.0.1:1234 ```
   - key  = password key to connect,
    example: ``` key=password123 ```
  - name (optional) = dApp name,
    example: ``` name=Galaxy logic game ```
  - icon (optional) = dApp icon url
    example: ``` icon=http://rostislavlitovkin.pythonanywhere.com/logo ```

A complete example: ``` plutonication:?url=192.168.0.1:8000&key=password123&name=Galaxy logic game&icon=http://rostislavlitovkin.pythonanywhere.com/logo ```

### part 2 - PlutoWallet
1) [x] generate mnemonics and show it to the user
1) [x] enter mnemonics
1) [ ] enter private key
1) [x] show the user raw privateKey
2) [x] save the privateKey securely
3) [ ] (extra) secure with password/biometrics
4) [x] generate a publicKey from the privateKey
5) [x] (extra - EASY) make ss58 encoded publicKeys
6) [x] get the current balance
7) [ ] (extra) show the balance in USD (use coingecko free api)
8) [x] add a transfer functionality
9) [ ] add the ability to sign any transaction (Can be done through plutonication)
10) [x] QR scanner
11) [x] improve UI
12) [ ] further improve UI
13) [x] add multiple chain support
14) [x] (extra) the ability to add other unknown chains manually
15) [ ] (extra) NFT implementation
16) [ ] show other funganble tokens
17) [ ] plutonication deep link
18) [ ] add loading animations
19) [x] icon and splash screen
20) [ ] implement nicks pallet
21) [x] show basic error messages
22) [ ] show more error messages
23) [ ] XCM
24) [ ] credits
25) [ ] more animations
26) [x] update the color theme
27) [x] change button names to icons

### Other milestones

#### #1 NFTs integration
- show all owned NFTs
- allow minting your own NFTs
- NFT dex implementation

#### #2 Staking
- show stake pools
- show gains
- more...

#### #3 dApp gallery
- dApp promotion page

#### #4 Voting

#### #5 ink!

#### #6 buy crypto
  
### (extra) part 3 - browser extension
Acts like any other browser wallet, 
with the functionality of connecting to 
Web dApps, but instead of storing a private key, 
will generate a QR code for Plutonication. 
It will then pass all the requested data to the PlutoWallet.

### part 4 - video and presentation

# Resources to use

### Hackathon:
- https://www.polkadotglobalseries.com/

### Socket solutions:
- [ ] https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services?source=recommendations
- [ ] https://github.com/sta/websocket-sharp / http://sta.github.io/websocket-sharp/#secure-connection

### Polkadot solutions:
- https://github.com/ajuna-network
- Ajuna transaction example: https://github.com/ajuna-network/SubstrateNetApi/issues/21#issuecomment-940421149
- reading storage: https://www.shawntabrizi.com/substrate/querying-substrate-storage-via-rpc/

### Blockchain communication tools
- https://polkadot.js.org/apps/

### parity signer
- [ ] https://www.parity.io/technologies/signer/
- [ ] https://paritytech.github.io/parity-signer/about/Security-And-Privacy.html

### NFTs
- https://wiki.polkadot.network/docs/learn-nft

### Inspiration:
- https://walletconnect.com/

### Other useful pallet calls / queries:
- query.balances.existentialDeposit()
- query.system.ss58prefix()

Assets
- query.assets.asset()
