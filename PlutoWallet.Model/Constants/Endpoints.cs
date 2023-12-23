using System;
using static System.Net.Mime.MediaTypeNames;

namespace PlutoWallet.Constants
{
    public class Endpoints
    {
        public static List<Endpoint> GetAllEndpoints => GetEndpointDictionary.Values.ToList();

        public static readonly Dictionary<string, Endpoint> GetEndpointDictionary = new Dictionary<string, Endpoint>()
        {
            { "polkadot", new Endpoint
            {
                Name = "Polkadot",
                Key = "polkadot",
                URL = "wss://rpc.polkadot.io",
                Icon = "polkadot.png",
                CalamarChainName = "polkadot",
                SubSquareChainName = "polkadot",
                Unit = "DOT",
                Decimals = 10,
                SS58Prefix = 0,
                ChainType = ChainType.Substrate,
            } },
            { "kusama", new Endpoint
            {
                Name = "Kusama",
                Key = "kusama",
                URL = "wss://kusama-rpc.polkadot.io",
                Icon = "kusama.png",
                DarkIcon = "kusama.png",
                CalamarChainName = "kusama",
                SubSquareChainName = "kusama",
                Unit = "KSM",
                Decimals = 12,
                SS58Prefix = 2,
                ChainType = ChainType.Substrate,
            } },
            { "moonbeam", new Endpoint
            {
                Name = "Moonbeam",
                Key = "moonbeam",
                URL = "wss://wss.api.moonbeam.network",
                Icon = "moonbeam.png",
                CalamarChainName = "moonbeam",
                Unit = "GLMR",
                Decimals = 18,
                SS58Prefix = 1284,
                ChainType = ChainType.Ethereum,
            } },
            { "astar", new Endpoint
            {
                Name = "Astar",
                Key = "astar",
                URL = "wss://rpc.astar.network",
                Icon = "astar.png",
                CalamarChainName = "astar",
                Unit = "ASTR",
                Decimals = 18,
                SS58Prefix = 5,
                ChainType = ChainType.Substrate,
            } },
            { "ajuna", new Endpoint
            {
                Name = "Ajuna",
                Key = "ajuna",
                URL = "wss://rpc-parachain.ajuna.network",
                Icon = "ajuna.png",
                Unit = "AJUN",
                Decimals = 12,
                SS58Prefix = 1328,
                ChainType = ChainType.Substrate,
            } },
            { "bajun", new Endpoint
            {
                Name = "Bajun",
                Key = "bajun",
                URL = "wss://rpc-parachain.bajun.network",
                Icon = "bajun.png",
                Unit = "BAJU",
                CalamarChainName = "bajun",
                Decimals = 12,
                SS58Prefix = 1337,
                ChainType = ChainType.Substrate,
            } },
            { "manta", new Endpoint
            {
                Name = "Manta",
                Key = "manta",
                URL = "wss://ws.manta.systems",
                Icon = "manta.png",
                Unit = "MANTA",
                Decimals = 18,
                SS58Prefix = 77,
                ChainType = ChainType.Substrate,
            } },
            { "westend", new Endpoint
            {
                Name = "Westend",
                Key = "westend",
                URL = "wss://westend-rpc.polkadot.io",
                Icon = "westend.png",
                Unit = "WND",
                Decimals = 12,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } },
            { "rococo", new Endpoint
            {
                Name = "Rococo",
                Key = "rococo",
                URL = "wss://rococo-rpc.polkadot.io",
                Icon = "rococo.png",
                CalamarChainName = "rococo",
                Unit = "ROC",
                Decimals = 12,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } },
            { "rockmine", new Endpoint
            {
                Name = "Rockmine",
                Key = "rockmine",
                URL = "wss://rococo-rockmine-rpc.polkadot.io",
                Icon = "statemint.png",
                Unit = "ROC",
                Decimals = 12,
                SS58Prefix = 2,
                ChainType = ChainType.Substrate,
                SupportsNfts = true,
            } },
            { "bajunrococo", new Endpoint
            {
                Name = "Bajun rococo",
                Key = "bajunrococo",
                URL = "wss://rpc-rococo.bajun.network",
                Icon = "bajun.png",
                Unit = "BAJU",
                Decimals = 12,
                SS58Prefix = 1337,
                ChainType = ChainType.Substrate,
            } },
            { "statemine", new Endpoint
            {
                Name = "Statemine",
                Key = "statemine",
                URL = "wss://kusama-asset-hub-rpc.polkadot.io",
                Icon = "statemint.png",
                Unit = "KSM",
                Decimals = 12,
                SS58Prefix = 2,
                ChainType = ChainType.Substrate,
                SupportsNfts = true,
                CalamarChainName = "statemine"
            } },
            { "statemint", new Endpoint {
                Name = "Statemint",
                Key = "statemint",
                URL = "wss://polkadot-asset-hub-rpc.polkadot.io",
                Icon = "statemint.png",
                Unit = "DOT",
                Decimals = 10,
                SS58Prefix = 0,
                ChainType = ChainType.Substrate,
                SupportsNfts = true,
                CalamarChainName = "statemint"
            } },
            { "unique", new Endpoint
            {
                Name = "Unique",
                Key = "unique",
                URL = "wss://ws.unique.network",
                Icon = "unique.png",
                CalamarChainName = "unique",
                Unit = "UNQ",
                Decimals = 18,
                SS58Prefix = 7391,
                ChainType = ChainType.Substrate,
            } },
            { "quartz", new Endpoint
            {
                Name = "Quartz",
                Key = "quartz",
                URL = "wss://eu-ws-quartz.unique.network",
                Icon = "quartz.png",
                CalamarChainName = "quartz",
                Unit = "QTZ",
                Decimals = 18,
                SS58Prefix = 255,
                ChainType = ChainType.Substrate,
            } },
            { "opal", new Endpoint
            {
                Name = "Opal",
                Key = "opal",
                URL = "wss://eu-ws-opal.unique.network",
                Icon = "opal.png",
                CalamarChainName = "opal",
                Unit = "OPL",
                Decimals = 18,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } },
            { "shibuya", new Endpoint
            {
                Name = "Shibuya",
                Key = "shibuya",
                URL = "wss://shibuya-rpc.dwellir.com",
                Icon = "shibuya.png",
                CalamarChainName = "shibuya",
                Unit = "SBY",
                Decimals = 18,
                SS58Prefix = 5,
                ChainType = ChainType.Substrate,
            } },
            { "moonbasealpha", new Endpoint
            {
                Name = "Moonbase Alpha",
                Key = "moonbasealpha",
                URL = "wss://wss.api.moonbase.moonbeam.network",
                Icon = "moonbase.png",
                CalamarChainName = "moonbase",
                Unit = "DEV",
                Decimals = 18,
                SS58Prefix = 1287,
                ChainType = ChainType.Ethereum,
            } },
            { "azerotestnet", new Endpoint
            {
                Name = "Aleph Zero Testnet",
                Key = "azerotestnet",
                URL = "wss://ws.test.azero.dev",
                Icon = "alephzerotestnet.png",
                Unit = "TZERO",
                CalamarChainName = "aleph-zero-testnet",
                Decimals = 12,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } },
            { "acala", new Endpoint
            {
                Name = "Acala",
                Key = "acala",
                URL = "wss://acala-rpc-3.aca-api.network/ws",
                Icon = "acala.png",
                CalamarChainName = "acala",
                Unit = "ACA",
                Decimals = 12,
                SS58Prefix = 10,
                ChainType = ChainType.Substrate,
            } },
            { "basilisk", new Endpoint
            {
                Name = "Basilisk",
                Key = "basilisk",
                URL = "wss://rpc.basilisk.cloud",
                Icon = "basilisk.png",
                Unit = "BSX",
                SS58Prefix = 10041,
                Decimals = 12,
                ChainType = ChainType.Substrate,
            } },
            { "hydradx", new Endpoint
            {
                Name = "HydraDX",
                Key = "hydradx",
                URL = "wss://rpc.hydradx.cloud",
                Icon = "hydradxomnipool.png",
                Unit = "HDX",
                SS58Prefix = 63,
                Decimals = 12,
                ChainType = ChainType.Substrate,
                CalamarChainName = "hydradx",
                SupportsNfts = true
            } },
            {
                "xcavate", new Endpoint
                {
                    Name = "XCavate",
                    Key = "xcavate",
                    URL = "wss://fraa-dancebox-3031-rpc.a.dancebox.tanssi.network",
                    Icon = "xcavate.png",
                    Unit = "XCAV",
                    SS58Prefix = 42,
                    Decimals = 12,
                    ChainType = ChainType.Substrate,
                    SupportsNfts = true,
                }
            },
            {
                "moonriver", new Endpoint
                {
                    Name = "Moonriver",
                    Key = "moonriver",
                    URL = "wss://wss.api.moonriver.moonbeam.network",
                    Icon = "moonriver.png",
                    Unit = "MOVR",
                    SS58Prefix = 1285,
                    Decimals = 18,
                    ChainType = ChainType.Substrate,
                }
            },
            {
                "bifrost", new Endpoint
                {
                    Name = "Bifrost",
                    Key = "bifrost",
                    URL = "wss://bifrost-polkadot.api.onfinality.io/public-ws",
                    Icon = "bifrost.png",
                    Unit = "BNC",
                    SS58Prefix = 6,
                    Decimals = 12,
                    ChainType = ChainType.Substrate,
                }
            },
            { "local", new Endpoint
            {
                Name = "(Local) ws://127.0.0.1:9944",
                Key = "local",
                URL = "ws://127.0.0.1:9944",
                Icon = "substrate.png",
                Unit = "",
                Decimals = 0,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } }
        };
    }

    public enum ChainType
    {
        Substrate,
        Ethereum,
        Other,
    }

	public class Endpoint
	{
        public string Name { get; set; }
        public string URL { get; set; }
		public string Icon { get; set; }
        public string DarkIcon { get; set; }
		public string CalamarChainName { get; set; }
        public string SubSquareChainName { get; set; }
        public string Key { get; set; }

        // Symbol and Unit are interchangeable names.
		public string Unit { get; set; }
		public int Decimals { get; set; }
		public short SS58Prefix { get; set; }
        public ChainType ChainType { get; set; }
        public bool SupportsNfts { get; set; } = false;
    }
}

