# Download

Android apk: https://rostislavlitovkin.pythonanywhere.com/downloadplutowallet

Other platforms will be available for download probably in November.

# Build and Debug locally

```
git clone https://github.com/RostislavLitovkin/PlutoWallet

git checkout devel

git submodule init
git submodule update
```

I recommend using Visual Studio 2022: https://visualstudio.microsoft.com/vs/community/

Install **.net MAUI**: https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation?tabs=vswin

Detailed description on how to run the code: https://docs.microsoft.com/en-us/dotnet/maui/get-started/first-app?tabs=vswin&pivots=devices-android

# PlutoWallet
<img width="1512" alt="Screenshot 2023-08-19 at 23 28 08" src="https://github.com/RostislavLitovkin/PlutoWallet/assets/77352013/d09f81ff-a85d-44ed-a4be-075861de4caf">

Multi-platform mobile wallet for substrate-based chains.
Focuses on the best UX.
First C# mobile wallet in Polkadot ecosystem.

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
- fee calculation
- shows transaction status
- Nfts (powered by [Uniquery.Net](https://github.com/RostislavLitovkin/Uniquery.Net))
- Contracts (Currently just Counter Sample)
- connect to any dApp thanks to [Plutonication](https://github.com/cisar2218/Plutonication)

3rd party integrations:
- [Calamar explorer](https://github.com/topmonks/calamar)
- [Kodadot unlockables](https://hello.kodadot.xyz/fandom-toolbox/audience-growth/drop-page)
- [Coingecko](https://www.coingecko.com/)
- [Staking dashboard](https://staking.polkadot.network/)
- [Awesome Ajuna Avatars](https://aaa.ajuna.io/)
- [Azero.id](https://azero.id/)

# Differentiating factors

### 1) Multinetwork optimisation
The UI/UX is optimised to support multichain out of the box.
Instead of having one chain selected at a time, you can have chain groups.
This makes the UI more approachable and simple.

### 2) Custom layouts (PlutoLayouts)

- Well prepared video presentation: https://youtu.be/ojU8-w6u5Oo

Extremely important tool for onboarding new users.
Not only can you optimise UI layouts to your needs, you can also export them to other users.
This can be especially handy for dApp projects:
- In a typical use-case, dApp developers would have to teach new users how to use their wallets. It is usually a big hassle, the user has to find the chain they are looking for and they would still see many confusing crypto functionalities that they would not care about.
- With PlutoLayouts, dApp developers can export the ideal layout (UX optimised for their specific app) and share them to the new users so that they have got the ideal UX out of the box. They will see only features they would actually care about.
- It is highly customisable. When the users decide they want to do more in the ecosystem, they can easily add more functionalities to the wallet. They can learn to use Polkadot on their own pace.

### 3) Plutonication
Plutonication allows users to connect PlutoWallet to other dApps seamlessly on any platforms, accross multiple codebases.
DApp just generates a QR code and once it is scanned in the wallet, they will pair and the wallet will be able to receive transaction requests from the dApp. To learn more, visit https://github.com/cisar2218/Plutonication.

- It works the same way as WalletConnect protocol.

### 4) Multiplatform development
This project is developed using .net MAUI framework, which allows simple development of native mobile apps on many different platforms from a single codebase.
This is crucial for quick (and easier) development while preserving the quality.

### 5) Market leading NFT support
The largest NFT support in the whole Polkadot ecosystem, thanks to [Uniquery.Net](https://github.com/RostislavLitovkin/Uniquery.Net).

Supported Networks/NftStandards:
- [x] Rmrk
- [x] RmrkV2
- [x] Basilisk
- [x] Glmr
- [x] Movr
- [x] Unique
- [x] Quartz
- [x] Opal
- [x] Acala (OnFinality BETA)
- [x] Astar (OnFinality BETA)
- [x] Shiden (OnFinality BETA)
- [x] Statemine/Statemint/Rockmine Nfts pallet
- [x] Statemine/Statemint/Rockmine Uniques pallet
- [x] tzero.id (currently just Aleph Zero Testnet)
- [ ] azero.id (soon)
- [ ] more to come...

# [Plutonication](https://github.com/cisar2218/Plutonication)
Allows the wallet to communicate with any dApps and sign their respective transaction requests without the risk of compromising the private key.
<img width="1512" alt="Screenshot 2023-08-19 at 22 50 21" src="https://github.com/RostislavLitovkin/PlutoWallet/assets/77352013/6c10fbe1-ad31-4aa3-8e3f-dcfbfeaf4aaf">

# Achievements
- 2nd place at Polkadot Global Series hackathon 2023, Europe edition in the Web3 & Tooling category

# Current team

#### [Rostislav Litovkin](http://rostislavlitovkin.pythonanywhere.com/aboutme)
- Alumnus at Polkadot Blockchain Academy 2023 in Berkeley
- Experienced .net MAUI developer, e.g.:
   - [Galaxy Logic Game](https://github.com/RostislavLitovkin/galaxylogicgamemaui) (successful game for watches and mobiles, 50k+ downloads)
- Frontend developer at [Calamar explorer](https://calamar.app/)
- Successful student at Polkadot DevCamp #2
- Successful student at [Solana Summer School](https://ackeeblockchain.com/school-of-solana)
- Polkadot Global Series 2023 (Europe) - second place
- Audience choice prize at EthPrague 2023

#### Dušan Jánsky
- Alumnus at Polkadot Blockchain Academy 2023 in Berkeley
- Student at Faculty of Electrical Engineering Czech Technical University in Prague - Opens Informatics (specialization in computer games and computer graphics)
- Fullstack developer at [Universal Scientific Technologies](https://www.ust.cz/)
- Polkadot Global Series 2023 (Europe) - second place

# Tech Stack
- C# programming language
- [.net MAUI](https://dotnet.microsoft.com/en-us/apps/maui)
- [Substrate.NetApi](https://github.com/SubstrateGaming/Substrate.NET.API)
- [Uniquery.net](https://github.com/RostislavLitovkin/Uniquery.Net)
- [Plutonication](https://github.com/cisar2218/plutonication)
- [Plugin.Fingerprint](https://www.nuget.org/packages/Plugin.Fingerprint/)
- [CommunityToolkit.Maui](https://github.com/CommunityToolkit/Maui)
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet)
- [MarkDig](https://github.com/xoofx/markdig)

# Terminology
- dApp = any application that uses crypto functionalities. In order to use dApps, it needs to communicate with the crypto wallet somehow, or it needs to know your private key (very insecure).
- Substrate key = ss58 encoded key with "42" prefix.
- Chain-specific key = ss58 encoded key with a custom prefix.
- Chain = either standalone blockchain, relay chain, or parachain. (and/or combination of them)
- Extrinsic = A transaction (more info: https://wiki.polkadot.network/docs/learn-extrinsics)
- PlutoLayout = a simple way to save and export custom layouts.

development terminology:
- Entry page - the first screen that the users see (now the Mnemonics page)

# Project folder structure

- `/Platforms` platform spacific codes, mainly code that ensures the boot on multiple platforms


- `/Resources` all resources, including icons, images, fonts, splashscreen...


- `/Components` organised by subfolders of different components. `/<subfolder-name>` contains 1 or more Views (Always a ContentView) and ViewModels. It's respective Model (if existant) lives in `/Model`


- `/Types` stores custom types. Mainly Ajuna generated ones.


- `/Constants`


- `/View` stores pages and views shown on pages


- `/ViewModel` view's respective ViewModel


- `/Properties` nothing important
