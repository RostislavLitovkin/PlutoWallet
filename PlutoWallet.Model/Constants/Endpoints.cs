using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Net.Mime.MediaTypeNames;

namespace PlutoWallet.Constants
{
    public enum EndpointEnum
    {
        // Needs to be here, because of the Binding bug
        None,

        Polkadot,
        Kusama,
        Moonbeam,
        Astar,
        Ajuna,
        Bajun,
        Manta,
        Westend,
        KusamaAssetHub,
        PolkadotAssetHub,
        PolkadotPeople,
        Unique,
        Quartz,
        Opal,
        Shibuya,
        MoonbaseAlpha,
        AzeroTestnet,
        Acala,
        Basilisk,
        Hydration,
        Moonriver,
        Bifrost,
        Local,
        Local8000,
        Local8001,
        Local8002,
    }
    public class Endpoints
    {
        public static List<Endpoint> GetAllEndpoints => GetEndpointDictionary.Values.ToList();

        public static readonly ReadOnlyDictionary<string, EndpointEnum> HashToKey = new ReadOnlyDictionary<string, EndpointEnum>(new Dictionary<string, EndpointEnum>()
        {
            { "0x91b171bb158e2d3848fa23a9f1c25182fb8e20313b2c1eb49219da7a70ce90c3", EndpointEnum.Polkadot },
            { "0xb0a8d493285c2df73290dfb7e61f870f17b41801197a149ca93654499ea3dafe", EndpointEnum.Kusama },
            { "0xfe58ea77779b7abda7da4ec526d14db9b1e9cd40a217c34892af80a9b332b76d", EndpointEnum.Moonbeam },
            { "0x9eb76c5184c4ab8679d2d5d819fdf90b9c001403e9e17da2e14b6d8aec4029c6", EndpointEnum.Astar },
            { "0xe358eb1d11b31255a286c12e44fe6780b7edb171d657905a97e39f71d9c6c3ee", EndpointEnum.Ajuna },
            { "0x35a06bfec2edf0ff4be89a6428ccd9ff5bd0167d618c5a0d4341f9600a458d14", EndpointEnum.Bajun },
            { "0xf3c7ad88f6a80f366c4be216691411ef0622e8b809b1046ea297ef106058d4eb", EndpointEnum.Manta },
            { "0xe143f23803ac50e8f6f8e62695d1ce9e4e1d68aa36c1cd2cfd15340213f3423e", EndpointEnum.Westend },
            { "0x48239ef607d7928874027a43a67689209727dfb3d3dc5e5b03a39bdc2eda771a", EndpointEnum.KusamaAssetHub },
            { "0x68d56f15f85d3136970ec16946040bc1752654e906147f7e43e9d539d7c3de2f", EndpointEnum.PolkadotAssetHub },
            { "0x67fa177a097bfa18f77ea95ab56e9bcdfeb0e5b8a40e46298bb93e16b6fc5008", EndpointEnum.PolkadotPeople },
            { "0x84322d9cddbf35088f1e54e9a85c967a41a56a4f43445768125e61af166c7d31", EndpointEnum.Unique },
            { "0xcd4d732201ebe5d6b014edda071c4203e16867305332301dc8d092044b28e554", EndpointEnum.Quartz },
            { "0xc87870ef90a438d574b8e320f17db372c50f62beb52e479c8ff6ee5b460670b9", EndpointEnum.Opal },
            { "0xddb89973361a170839f80f152d2e9e38a376a5a7eccefcade763f46a8e567019", EndpointEnum.Shibuya },
            { "0x91bc6e169807aaa54802737e1c504b2577d4fafedd5a02c10293b1cd60e39527", EndpointEnum.MoonbaseAlpha },
            { "0x05d5279c52c484cc80396535a316add7d47b1c5b9e0398dd1f584149341460c5", EndpointEnum.AzeroTestnet },
            { "0xfc41b9bd8ef8fe53d58c7ea67c794c7ec9a73daf05e6d54b14ff6342c99ba64c", EndpointEnum.Acala },
            { "0xa85cfb9b9fd4d622a5b28289a02347af987d8f73fa3108450e2b4a11c1ce5755", EndpointEnum.Basilisk },
            { "0xafdc188f45c71dacbaa0b62e16a91f726c7b8699a9748cdf715459de6b7f366d", EndpointEnum.Hydration },
            { "0x401a1f9dca3da46f5c4091016c8a2f26dcea05865116b286f60f668207d1474b", EndpointEnum.Moonriver },
            { "0x262e1b2ad728475fd6fe88e62d34c200abe6fd693931ddad144059b1eb884e5b", EndpointEnum.Bifrost },
        });

        public static readonly ReadOnlyDictionary<EndpointEnum, Endpoint> GetEndpointDictionary = new ReadOnlyDictionary<EndpointEnum, Endpoint>(new Dictionary<EndpointEnum, Endpoint>()
        {
            { EndpointEnum.Polkadot, new Endpoint
            {
                Name = "Polkadot",
                Key = EndpointEnum.Polkadot,
                URLs = new string[5] { "wss://polkadot-rpc.dwellir.com", "wss://polkadot-public-rpc.blockops.network/ws", "wss://rpc.ibp.network/polkadot", "wss://polkadot.api.onfinality.io/public-ws", "wss://polkadot.public.curie.radiumblock.co/ws" },
                Icon = "polkadot.png",
                DarkIcon = "polkadot.png",
                CalamarChainName = "polkadot",
                SubSquareChainName = "polkadot",
                SubscanChainName = "polkadot",
                Unit = "DOT",
                Decimals = 10,
                SS58Prefix = 0,
                ChainType = ChainType.Substrate,
                ParachainId = new ParachainId
                {
                    Chain = Chain.Relay,
                    Id = null,
                }
            } },
            { EndpointEnum.Kusama, new Endpoint
            {
                Name = "Kusama",
                Key = EndpointEnum.Kusama,
                URLs = new string[4] { "wss://kusama-rpc.dwellir.com", "wss://rpc.ibp.network/kusama", "wss://kusama.public.curie.radiumblock.co/ws", "wss://kusama.api.onfinality.io/public-ws" },
                Icon = "kusama.png",
                DarkIcon = "kusamawhite.png",
                CalamarChainName = "kusama",
                SubSquareChainName = "kusama",
                SubscanChainName = "kusama",
                Unit = "KSM",
                Decimals = 12,
                SS58Prefix = 2,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.Moonbeam, new Endpoint
            {
                Name = "Moonbeam",
                Key = EndpointEnum.Moonbeam,
                URLs =  new string[3] { "wss://wss.api.moonbeam.network", "wss://moonbeam.unitedbloc.com", "wss://moonbeam.api.onfinality.io/public-ws" },
                Icon = "moonbeam.png",
                DarkIcon = "moonbeam.png",
                CalamarChainName = "moonbeam",
                SubscanChainName = "moonbeam",
                Unit = "GLMR",
                Decimals = 18,
                SS58Prefix = 1284,
                ChainType = ChainType.Ethereum,
            } },
            { EndpointEnum.Astar, new Endpoint
            {
                Name = "Astar",
                Key = EndpointEnum.Astar,
                URLs =  new string[2] { "wss://astar-rpc.dwellir.com", "wss://astar.public.blastapi.io" },
                Icon = "astar.png",
                DarkIcon = "astar.png",
                CalamarChainName = "astar",
                SubscanChainName = "astar",
                Unit = "ASTR",
                Decimals = 18,
                SS58Prefix = 5,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.Ajuna, new Endpoint
            {
                Name = "Ajuna",
                Key = EndpointEnum.Ajuna,
                URLs =  new string[1] { "wss://ajuna.api.onfinality.io/public-ws" /*"wss://rpc-parachain.ajuna.network"*/ },
                Icon = "ajuna.png",
                DarkIcon = "ajuna.png",
                SubscanChainName = "ajuna",
                Unit = "AJUN",
                Decimals = 12,
                SS58Prefix = 1328,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.Bajun, new Endpoint
            {
                Name = "Bajun",
                Key = EndpointEnum.Bajun,
                URLs =  new string[2] { "wss://rpc-parachain.bajun.network", "wss://bajun.api.onfinality.io/public-ws" /*"wss://bajun.public.curie.radiumblock.co/ws"*/  },
                Icon = "bajun.png",
                DarkIcon = "bajun.png",
                Unit = "BAJU",
                CalamarChainName = "bajun",
                SubscanChainName = "bajun",
                Decimals = 12,
                SS58Prefix = 1337,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.Manta, new Endpoint
            {
                Name = "Manta parachain",
                Key = EndpointEnum.Manta,
                URLs =  new string[1] { "wss://ws.manta.systems" },
                Icon = "manta.png",
                DarkIcon = "manta.png",
                SubscanChainName = "manta",
                Unit = "MANTA",
                Decimals = 18,
                SS58Prefix = 77,
                ChainType = ChainType.Substrate,
            } },
            /*{ EndpointEnum.Westend, new Endpoint
            {
                Name = "Westend",
                Key = EndpointEnum.Westend,
                URLs =  new string[2] { "wss://westend-rpc.dwellir.com", "wss://westend.api.onfinality.io/public-ws" },
                Icon = "westend.png",
                DarkIcon = "westend.png",
                Unit = "WND",
                Decimals = 12,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } },*/
            /*{ "rococo", new Endpoint
            {
                Name = "Rococo",
                Key = "rococo",
                URLs = "wss://rococo-rpc.polkadot.io",
                Icon = "rococo.png",
                DarkIcon = "rococo.png",
                CalamarChainName = "rococo",
                Unit = "ROC",
                Decimals = 12,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } },*/
            /*{ "rockmine", new Endpoint
            {
                Name = "Rockmine",
                Key = "rockmine",
                URLs = "wss://rococo-rockmine-rpc.polkadot.io",
                Icon = "statemint.png",
                DarkIcon = "statemint.png",
                Unit = "ROC",
                Decimals = 12,
                SS58Prefix = 2,
                ChainType = ChainType.Substrate,
                SupportsNfts = true,
            } },*/
            /*{ "bajunrococo", new Endpoint
            {
                Name = "Bajun rococo",
                Key = "bajunrococo",
                URLs = "wss://rpc-rococo.bajun.network",
                Icon = "bajun.png",
                DarkIcon = "bajun.png",
                Unit = "BAJU",
                Decimals = 12,
                SS58Prefix = 1337,
                ChainType = ChainType.Substrate,
            } },*/
            { EndpointEnum.KusamaAssetHub, new Endpoint
            {
                Name = "Kusama Asset Hub",
                Key = EndpointEnum.KusamaAssetHub,
                URLs =  new string[4] { "wss://statemine-rpc.dwellir.com", "wss://rpc-asset-hub-kusama.luckyfriday.io", "wss://ksm-rpc.stakeworld.io/assethub", "wss://statemine-rpc-tn.dwellir.com" },
                Icon = "kusamaassethub.png",
                DarkIcon = "kusamaassethub.png",
                Unit = "KSM",
                Decimals = 12,
                SS58Prefix = 2,
                ChainType = ChainType.Substrate,
                SupportsNfts = true,
                CalamarChainName = "statemine",
                SubscanChainName = "assethub-kusama",
            } },
            { EndpointEnum.PolkadotAssetHub, new Endpoint {
                Name = "Polkadot Asset Hub",
                Key = EndpointEnum.PolkadotAssetHub,
                URLs =  new string[4] { "wss://statemint-rpc.dwellir.com", "wss://statemint-rpc-tn.dwellir.com", "wss://statemint.api.onfinality.io/public-ws", "wss://dot-rpc.stakeworld.io/assethub" },
                Icon = "polkadotassethub.png",
                DarkIcon = "polkadotassethub.png",
                Unit = "DOT",
                Decimals = 10,
                SS58Prefix = 0,
                ChainType = ChainType.Substrate,
                SupportsNfts = true,
                CalamarChainName = "statemint",
                SubscanChainName = "assethub-polkadot",
                ParachainId = new ParachainId
                {
                    Chain = Chain.Parachain,
                    Id = 1000,
                }
            } },
            { EndpointEnum.PolkadotPeople, new Endpoint {
                Name = "Polkadot People",
                Key = EndpointEnum.PolkadotPeople,
                URLs = ["wss://polkadot-people-rpc.polkadot.io"],
                Icon = "polkadot.png",
                DarkIcon = "polkadot.png",
                Unit = "DOT",
                Decimals = 10,
                SS58Prefix = 0,
                ChainType = ChainType.Substrate,

            } },
            { EndpointEnum.Unique, new Endpoint
            {
                Name = "Unique",
                Key = EndpointEnum.Unique,
                URLs =  new string[4] { "wss://ws.unique.network", "wss://eu-ws.unique.network", "wss://us-ws.unique.network", "wss://unique-rpc.dwellir.com" },
                Icon = "unique.png",
                DarkIcon = "unique.png",
                CalamarChainName = "unique",
                SubscanChainName = "unique",
                Unit = "UNQ",
                Decimals = 18,
                SS58Prefix = 7391,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.Quartz, new Endpoint
            {
                Name = "QUARTZ by UNIQUE ",
                Key = EndpointEnum.Quartz,
                URLs =  new string[3] { "wss://eu-ws-quartz.unique.network", "wss://ws-quartz.unique.network", "wss://quartz-rpc.dwellir.com" },
                Icon = "quartz.png",
                DarkIcon = "quartz.png",
                CalamarChainName = "quartz",
                SubscanChainName = "quartz",
                Unit = "QTZ",
                Decimals = 18,
                SS58Prefix = 255,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.Opal, new Endpoint
            {
                Name = "OPAL by UNIQUE",
                Key = EndpointEnum.Opal,
                URLs =  new string[2] { "wss://eu-ws-opal.unique.network", "wss://ws-opal.unique.network" },
                Icon = "opal.png",
                DarkIcon = "opal.png",
                CalamarChainName = "opal",
                SubscanChainName = "opal",
                Unit = "OPL",
                Decimals = 18,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.Shibuya, new Endpoint
            {
                Name = "Shibuya",
                Key = EndpointEnum.Shibuya,
                URLs =  new string[2] { "wss://shibuya-rpc.dwellir.com", "wss://rpc.shibuya.astar.network" },
                Icon = "shibuya.png",
                DarkIcon = "shibuya.png",
                CalamarChainName = "shibuya",
                SubscanChainName = "shibuya",
                Unit = "SBY",
                Decimals = 18,
                SS58Prefix = 5,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.MoonbaseAlpha, new Endpoint
            {
                Name = "Moonbase Alpha",
                Key = EndpointEnum.MoonbaseAlpha,
                URLs =  new string[3] { "wss://wss.api.moonbase.moonbeam.network", "wss://moonbeam-alpha.api.onfinality.io/public-ws", "wss://moonbase-rpc.dwellir.com" },
                Icon = "moonbase.png",
                DarkIcon = "moonbase.png",
                CalamarChainName = "moonbase",
                SubscanChainName = "moonbase",
                Unit = "DEV",
                Decimals = 18,
                SS58Prefix = 1287,
                ChainType = ChainType.Ethereum,
            } },
            { EndpointEnum.AzeroTestnet, new Endpoint
            {
                Name = "Aleph Zero Testnet",
                Key = EndpointEnum.AzeroTestnet,
                URLs =  new string[2] { "wss://ws.test.azero.dev", "wss://aleph-zero-testnet-rpc.dwellir.com" },
                Icon = "alephzerotestnet.png",
                DarkIcon = "alephzerotestnet.png",
                Unit = "TZERO",
                CalamarChainName = "aleph-zero-testnet",
                SubscanChainName = "alephzero-testnet",
                Decimals = 12,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.Acala, new Endpoint
            {
                Name = "Acala",
                Key = EndpointEnum.Acala,
                URLs =  new string[3] { "wss://acala-rpc-3.aca-api.network/ws", "wss://acala-rpc.dwellir.com", "wss://acala-polkadot.api.onfinality.io/public-ws" },
                Icon = "acala.png",
                DarkIcon = "acala.png",
                CalamarChainName = "acala",
                SubscanChainName = "acala",
                Unit = "ACA",
                Decimals = 12,
                SS58Prefix = 10,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.Basilisk, new Endpoint
            {
                Name = "Basilisk",
                Key = EndpointEnum.Basilisk,
                URLs =  new string[2] { "wss://rpc.basilisk.cloud", "wss://basilisk-rpc.dwellir.com" },
                Icon = "basilisk.png",
                DarkIcon = "basilisk.png",
                SubscanChainName = "basilisk",
                Unit = "BSX",
                SS58Prefix = 10041,
                Decimals = 12,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.Hydration, new Endpoint
            {
                Name = "Hydration",
                Key = EndpointEnum.Hydration,
                URLs =  new string[3] { "wss://rpc.helikon.io/hydradx", "wss://rpc.hydradx.cloud", "wss://hydradx-rpc.dwellir.com" },
                Icon = "hydration.png",
                DarkIcon = "hydration.png",
                SubscanChainName = "hydradx",
                Unit = "HDX",
                SS58Prefix = 63,
                Decimals = 12,
                ChainType = ChainType.Substrate,
                CalamarChainName = "hydradx",
                SupportsNfts = true
            } },
            /*{
                "xcavate", new Endpoint
                {
                    Name = "XCavate",
                    Key = "xcavate",
                    URLs =  new string[2] { "wss://fraa-dancebox-3031-rpc.a.dancebox.tanssi.network" },
                    Icon = "xcavate.png",
                    DarkIcon = "xcavate.png",
                    Unit = "XCAV",
                    SS58Prefix = 42,
                    Decimals = 12,
                    ChainType = ChainType.Substrate,
                    SupportsNfts = true,
                }
            },*/
            {
                EndpointEnum.Moonriver, new Endpoint
                {
                    Name = "Moonriver",
                    Key = EndpointEnum.Moonriver,
                    URLs =  new string[2] { "wss://wss.api.moonriver.moonbeam.network", "wss://moonriver-rpc.dwellir.com" },
                    Icon = "moonriver.png",
                    DarkIcon = "moonriver.png",
                    SubscanChainName = "moonriver",
                    Unit = "MOVR",
                    SS58Prefix = 1285,
                    Decimals = 18,
                    ChainType = ChainType.Substrate,
                }
            },
            {
                EndpointEnum.Bifrost, new Endpoint
                {
                    Name = "Bifrost",
                    Key = EndpointEnum.Bifrost,
                    URLs =  new string[3] { "wss://bifrost-polkadot.api.onfinality.io/public-ws", "wss://eu.bifrost-polkadot-rpc.liebi.com/ws", "wss://hk.p.bifrost-rpc.liebi.com/ws" /*"wss://bifrost-rpc.dwellir.com"*/ },
                    Icon = "bifrost.png",
                    DarkIcon = "bifrost.png",
                    SubscanChainName = "bifrost",
                    Unit = "BNC",
                    SS58Prefix = 6,
                    Decimals = 12,
                    ChainType = ChainType.Substrate,
                }
            },
            { EndpointEnum.Local, new Endpoint
            {
                Name = "(Local) ws://127.0.0.1:9944",
                Key = EndpointEnum.Local,
                URLs =  new string[1] { "ws://127.0.0.1:9944" },
                Icon = "substrate.png",
                DarkIcon = "substrate.png",
                Unit = "",
                Decimals = 0,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } },
            { EndpointEnum.Local8000, new Endpoint
            {
                Name = "(Local) ws://127.0.0.1:8000",
                Key = EndpointEnum.Local8000,
                URLs =  new string[1] { "ws://172.26.118.8:8000" },
                Icon = "substrate.png",
                DarkIcon = "substrate.png",
                Unit = "",
                Decimals = 0,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
                ParachainId = new ParachainId
                {
                    Chain = Chain.Parachain,
                    Id = 2034,
                }
            } },
            { EndpointEnum.Local8001, new Endpoint
            {
                Name = "(Local) ws://127.0.0.1:8001",
                Key = EndpointEnum.Local8001,
                URLs =  new string[1] { "ws://127.0.0.1:8001" },
                Icon = "substrate.png",
                DarkIcon = "substrate.png",
                Unit = "",
                Decimals = 0,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
                ParachainId = new ParachainId
                {
                    Chain = Chain.Parachain,
                    Id = 1000,
                }
            } },
            { EndpointEnum.Local8002, new Endpoint
            {
                Name = "(Local) ws://127.0.0.1:8002",
                Key = EndpointEnum.Local8002,
                URLs =  new string[1] { "ws://127.0.0.1:8002" },
                Icon = "substrate.png",
                DarkIcon = "substrate.png",
                Unit = "",
                Decimals = 0,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
                ParachainId = new ParachainId
                {
                    Chain = Chain.Relay,
                    Id = null,
                }
            } }
        });
    }

    public enum ChainType
    {
        Substrate,
        Ethereum,
        Other,
    }

    public class ParachainId
    {
        public Chain Chain { get; set; }
        public uint? Id { get; set; }
    }

    public enum Chain
    {
        Relay,
        Parachain,
        Solo,
    }

    public class Endpoint
    {
        public string Name { get; set; }
        public string[] URLs { get; set; }
        public string Icon { get; set; }
        public string DarkIcon { get; set; }
        public string? CalamarChainName { get; set; }
        public string? SubSquareChainName { get; set; }
        public string? SubscanChainName { get; set; }
        public EndpointEnum Key { get; set; }

        // Symbol and Unit are interchangeable names.
        public string Unit { get; set; }
        public int Decimals { get; set; }
        public short SS58Prefix { get; set; }
        public ChainType ChainType { get; set; }
        public bool SupportsNfts { get; set; } = false;

        public ParachainId? ParachainId { get; set; }

        /*public Endpoint Clone()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;
                return (Endpoint)formatter.Deserialize(ms);
            }
        }*/

        public Endpoint Clone()
        {
            return new Endpoint
            {
                Name = this.Name,
                URLs = this.URLs,
                Icon = this.Icon,
                DarkIcon = this.DarkIcon,
                CalamarChainName = this.CalamarChainName,
                SubSquareChainName = this.SubSquareChainName,
                SubscanChainName = this.SubscanChainName,
                Key = this.Key,
                Unit = this.Unit,
                Decimals = this.Decimals,
                SS58Prefix = this.SS58Prefix,
                ChainType = this.ChainType,
                SupportsNfts = this.SupportsNfts,
            };
        }
    }
}

