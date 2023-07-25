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
- fee calculation
- shows transaction status
- Nfts (powered by [Uniquery.Net](https://github.com/RostislavLitovkin/Uniquery.Net))
- Contracts (Currently just Counter Sample)
- connect to any dApp thanks to [Plutonication](https://github.com/cisar2218/Plutonication)

3rd party integrations:
- [Calamar explorer](https://github.com/topmonks/calamar)
- [Coingecko](https://www.coingecko.com/)
- [Staking dashboard](https://staking.polkadot.network/)
- [Awesome Ajuna Avatars](https://aaa.ajuna.io/)

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

development terminology:
- Entry page - the first screen that the users see (now the Mnemonics page)

# Project folder structure

- `/platforms` platform spacific codes, mainly code that ensures the boot on multiple platforms


- `/Resources` all resources, including icons, images, fonts, splashscreen...


- `/Components` organised by subfolders of different components. `/<subfolder-name>` contains 1 or more Views (Always a ContentView) and ViewModels. It's respective Model (if existant) lives in `/Model`


- `/Types` stores custom types. Mainly Ajuna generated ones.


- `/Constants`


- `/View` stores pages and views shown on pages


- `/ViewModel` view's respective ViewModel


- `/Properties` nothing important

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

### Inspiration:
- https://walletconnect.com/
