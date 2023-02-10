
1)
dApp se chce napojit - vygeneruje link/QR (ws://link, ip; EXTRA: jméno dAppky, iconka dAppky...) --otveře se link->  otevře Walletku a walletka se napojí --proběhne napojení-> dAppka získá publicKey (string)  

- zároveň si to budou pamatovat, že jsou napojený. si zapamatujou sessionu

2)
dAppka chce udělat transakci --pošle data transakce do walletky-> walletka na to reaguje (Potvrdit/zamítnout - čeká na input), --po inputu pošle podepsaný data-> dAppka dál dělá věci s podepsanými daty

- data/podepsaný data = byte[]


MVP = minimum viable product