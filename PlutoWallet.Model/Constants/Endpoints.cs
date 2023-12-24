using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Net.Mime.MediaTypeNames;

namespace PlutoWallet.Constants
{
    public class Endpoints
    {
        public static List<Endpoint> GetAllEndpoints => GetEndpointDictionary.Values.ToList();

        public static readonly ReadOnlyDictionary<string, string> HashToKey = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()
        {
            { "0x91B171BB158E2D3848FA23A9F1C25182FB8E20313B2C1EB49219DA7A70CE90C3", "polkadot" },
            { "0xB0A8D493285C2DF73290DFB7E61F870F17B41801197A149CA93654499EA3DAFE", "kusama" },
            { "0xFE58EA77779B7ABDA7DA4EC526D14DB9B1E9CD40A217C34892AF80A9B332B76D", "moonbeam" },
            { "0x9EB76C5184C4AB8679D2D5D819FDF90B9C001403E9E17DA2E14B6D8AEC4029C6", "astar" },
            { "0xE358EB1D11B31255A286C12E44FE6780B7EDB171D657905A97E39F71D9C6C3EE", "ajuna" },
            { "0x35A06BFEC2EDF0FF4BE89A6428CCD9FF5BD0167D618C5A0D4341F9600A458D14", "bajun" },
            { "0xF3C7AD88F6A80F366C4BE216691411EF0622E8B809B1046EA297EF106058D4EB", "manta" },
            { "0xE143F23803AC50E8F6F8E62695D1CE9E4E1D68AA36C1CD2CFD15340213F3423E", "westend" },
            { "0x48239EF607D7928874027A43A67689209727DFB3D3DC5E5B03A39BDC2EDA771A", "statemine" },
            { "0x68D56F15F85D3136970EC16946040BC1752654E906147F7E43E9D539D7C3DE2F", "statemint" },
            { "0x84322D9CDDBF35088F1E54E9A85C967A41A56A4F43445768125E61AF166C7D31", "unique" },
            { "0xCD4D732201EBE5D6B014EDDA071C4203E16867305332301DC8D092044B28E554", "quartz" },
            { "0xC87870EF90A438D574B8E320F17DB372C50F62BEB52E479C8FF6EE5B460670B9", "opal" },
            { "0xDDB89973361A170839F80F152D2E9E38A376A5A7ECCEFCADE763F46A8E567019", "shibuya" },
            { "0x91BC6E169807AAA54802737E1C504B2577D4FAFEDD5A02C10293B1CD60E39527", "moonbasealpha" },
            { "0x05D5279C52C484CC80396535A316ADD7D47B1C5B9E0398DD1F584149341460C5", "azerotestnet" },
            { "0xFC41B9BD8EF8FE53D58C7EA67C794C7EC9A73DAF05E6D54B14FF6342C99BA64C", "acala" },
            { "0xA85CFB9B9FD4D622A5B28289A02347AF987D8F73FA3108450E2B4A11C1CE5755", "basilisk" },
            { "0xAFDC188F45C71DACBAA0B62E16A91F726C7B8699A9748CDF715459DE6B7F366D", "hydradx" },
            { "0x401A1F9DCA3DA46F5C4091016C8A2F26DCEA05865116B286F60F668207D1474B", "moonriver" },
            { "0x262E1B2AD728475FD6FE88E62D34C200ABE6FD693931DDAD144059B1EB884E5B", "bifrost" }
        });

        public static readonly ReadOnlyDictionary<string, Endpoint> GetEndpointDictionary = new ReadOnlyDictionary<string, Endpoint>(new Dictionary<string, Endpoint>()
        {
            { "polkadot", new Endpoint
            {
                Name = "Polkadot",
                Key = "polkadot",
                URLs = new string[4] { "wss://polkadot-rpc.dwellir.com", "wss://1rpc.io/dot", "wss://polkadot.api.onfinality.io/public-ws", "wss://polkadot.public.curie.radiumblock.co/ws" },
                Icon = "polkadot.png",
                DarkIcon = "polkadot.png",
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
                URLs = new string[3] { "wss://kusama-rpc.dwellir.com", "wss://rpc.ibp.network/kusama", "wss://kusama.api.onfinality.io/public-ws" },
                Icon = "kusama.png",
                DarkIcon = "kusamawhite.png",
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
                URLs =  new string[2] { "wss://wss.api.moonbeam.network", "wss://moonbeam.api.onfinality.io/public-ws" },
                Icon = "moonbeam.png",
                DarkIcon = "moonbeam.png",
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
                URLs =  new string[2] { "wss://astar-rpc.dwellir.com", "wss://astar.api.onfinality.io/public-ws" },
                Icon = "astar.png",
                DarkIcon = "astar.png",
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
                URLs =  new string[2] { "wss://ajuna.api.onfinality.io/public-ws", "wss://rpc-parachain.ajuna.network" },
                Icon = "ajuna.png",
                DarkIcon = "ajuna.png",
                Unit = "AJUN",
                Decimals = 12,
                SS58Prefix = 1328,
                ChainType = ChainType.Substrate,
            } },
            { "bajun", new Endpoint
            {
                Name = "Bajun",
                Key = "bajun",
                URLs =  new string[2] { "wss://rpc-parachain.bajun.network", "wss://bajun.api.onfinality.io/public-ws" },
                Icon = "bajun.png",
                DarkIcon = "bajun.png",
                Unit = "BAJU",
                CalamarChainName = "bajun",
                Decimals = 12,
                SS58Prefix = 1337,
                ChainType = ChainType.Substrate,
            } },
            { "manta", new Endpoint
            {
                Name = "Manta parachain",
                Key = "manta",
                URLs =  new string[1] { "wss://ws.manta.systems" },
                Icon = "manta.png",
                DarkIcon = "manta.png",
                Unit = "MANTA",
                Decimals = 18,
                SS58Prefix = 77,
                ChainType = ChainType.Substrate,
            } },
            { "westend", new Endpoint
            {
                Name = "Westend",
                Key = "westend",
                URLs =  new string[2] { "wss://westend-rpc.dwellir.com", "wss://westend.api.onfinality.io/public-ws" },
                Icon = "westend.png",
                DarkIcon = "westend.png",
                Unit = "WND",
                Decimals = 12,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } },
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
            { "statemine", new Endpoint
            {
                Name = "Kusama Asset Hub",
                Key = "statemine",
                URLs =  new string[4] { "wss://statemine-rpc.dwellir.com", "wss://kusama.api.onfinality.io/public-ws", "wss://1rpc.io/ksm", "wss://kusama.public.curie.radiumblock.co/ws" },
                Icon = "statemint.png",
                DarkIcon = "statemint.png",
                Unit = "KSM",
                Decimals = 12,
                SS58Prefix = 2,
                ChainType = ChainType.Substrate,
                SupportsNfts = true,
                CalamarChainName = "statemine"
            } },
            { "statemint", new Endpoint {
                Name = "Polkadot Asset Hub",
                Key = "statemint",
                URLs =  new string[4] { "wss://statemint-rpc.dwellir.com", "wss://statemint-rpc-tn.dwellir.com", "wss://statemint.api.onfinality.io/public-ws", "wss://dot-rpc.stakeworld.io/assethub" },
                Icon = "statemint.png",
                DarkIcon = "statemint.png",
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
                URLs =  new string[4] { "wss://ws.unique.network", "wss://eu-ws.unique.network", "wss://us-ws.unique.network", "wss://unique-rpc.dwellir.com" },
                Icon = "unique.png",
                DarkIcon = "unique.png",
                CalamarChainName = "unique",
                Unit = "UNQ",
                Decimals = 18,
                SS58Prefix = 7391,
                ChainType = ChainType.Substrate,
            } },
            { "quartz", new Endpoint
            {
                Name = "QUARTZ by UNIQUE ",
                Key = "quartz",
                URLs =  new string[3] { "wss://eu-ws-quartz.unique.network", "wss://ws-quartz.unique.network", "wss://quartz-rpc.dwellir.com" },
                Icon = "quartz.png",
                DarkIcon = "quartz.png",
                CalamarChainName = "quartz",
                Unit = "QTZ",
                Decimals = 18,
                SS58Prefix = 255,
                ChainType = ChainType.Substrate,
            } },
            { "opal", new Endpoint
            {
                Name = "OPAL by UNIQUE",
                Key = "opal",
                URLs =  new string[2] { "wss://eu-ws-opal.unique.network", "wss://ws-opal.unique.network" },
                Icon = "opal.png",
                DarkIcon = "opal.png",
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
                URLs =  new string[2] { "wss://shibuya-rpc.dwellir.com", "wss://rpc.shibuya.astar.network" },
                Icon = "shibuya.png",
                DarkIcon = "shibuya.png",
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
                URLs =  new string[3] { "wss://wss.api.moonbase.moonbeam.network", "wss://moonbeam-alpha.api.onfinality.io/public-ws", "wss://moonbase-rpc.dwellir.com" },
                Icon = "moonbase.png",
                DarkIcon = "moonbase.png",
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
                URLs =  new string[2] { "wss://ws.test.azero.dev", "wss://aleph-zero-testnet-rpc.dwellir.com" },
                Icon = "alephzerotestnet.png",
                DarkIcon = "alephzerotestnet.png",
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
                URLs =  new string[3] { "wss://acala-rpc-3.aca-api.network/ws", "wss://acala-rpc.dwellir.com", "wss://acala-polkadot.api.onfinality.io/public-ws" },
                Icon = "acala.png",
                DarkIcon = "acala.png",
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
                URLs =  new string[2] { "wss://rpc.basilisk.cloud", "wss://basilisk-rpc.dwellir.com" },
                Icon = "basilisk.png",
                DarkIcon = "basilisk.png",
                Unit = "BSX",
                SS58Prefix = 10041,
                Decimals = 12,
                ChainType = ChainType.Substrate,
            } },
            { "hydradx", new Endpoint
            {
                Name = "HydraDX",
                Key = "hydradx",
                URLs =  new string[2] { "wss://rpc.hydradx.cloud", "wss://hydradx-rpc.dwellir.com" },
                Icon = "hydradxomnipool.png",
                DarkIcon = "hydradxomnipool.png",
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
                "moonriver", new Endpoint
                {
                    Name = "Moonriver",
                    Key = "moonriver",
                    URLs =  new string[2] { "wss://wss.api.moonriver.moonbeam.network", "wss://moonriver-rpc.dwellir.com" },
                    Icon = "moonriver.png",
                    DarkIcon = "moonriver.png",
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
                    URLs =  new string[2] { "wss://bifrost-polkadot.api.onfinality.io/public-ws", "wss://bifrost-rpc.dwellir.com" },
                    Icon = "bifrost.png",
                    DarkIcon = "bifrost.png",
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
                URLs =  new string[1] { "ws://127.0.0.1:9944" },
                Icon = "substrate.png",
                DarkIcon = "substrate.png",
                Unit = "",
                Decimals = 0,
                SS58Prefix = 42,
                ChainType = ChainType.Substrate,
            } }
        });
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
        public string[] URLs { get; set; }
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

