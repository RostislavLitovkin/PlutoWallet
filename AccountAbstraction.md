
1) extrinsic: Inv4.CreateCore(..)
  - 50% = Perbill(500_000_000)

2) Zjistit Core ID
  - za pomocí event listeningu - velmi rychlá odezva
  - za pomocí query: Inv4.NextCoreId - nejméně efektivní, ale jednoduché na programování (trochu bruteforce metoda), ale rozhodně funguje
  - za pomocí query: CoreAssets.accounts(accountId, [coreId]) - vrací list přístupů ke všem core accountům (včetně také toho, kolik tokenů jich má, default je 1_000_000 milion)

3) Ujistit se, že daný core account existuje a že má jednoho člena (ten, kdo ho vytvořil)
  - za pomocí query: Inv4.coreMembers(coreId, [accountId])
    - takto se dají zjistit všichni členové daného core accountu

4) zjistit public klíč core accountu podle coreId
  - query: Inv4.coreStorage(Option(coreId))

5) Nově vytvořený core account má 0 Varch tokenů - musí se mu nějaké poslat:
  - extrinsic: Balances.transfer_keep_alive(accountId, amount)

~~5) CoreAssets jsou defaultně frozen po vytvoření (Ale to asi nevadí). Musí se změnit Core parametry:~~
  - extrinsic: https://polkadot.js.org/apps/?rpc=wss%3A%2F%2Finvarch-rpc.dwellir.com#/extrinsics/decode/0x470313000000000047090000000100

5) přidat další členy
  - extrinsic: https://polkadot.js.org/apps/?rpc=wss%3A%2F%2Finvarch-rpc.dwellir.com#/extrinsics/decode/0x4703130000000000470140420f000000000000000000000000008eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48
  - dále se musí potvrdit tento extrinsic (pokud by se nepřekročil threshhold)
