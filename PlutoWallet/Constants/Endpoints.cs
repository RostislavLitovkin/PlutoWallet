using System;
namespace PlutoWallet.Constants
{
	public class Endpoints
	{
        public static int[] DefaultNetworks => new int[4] { 0, 2, 3, -1 };

		public static List<int[]> NetworkOptions
		{
			get
			{
				List<int[]> options = new List<int[]>();

				options.Add(DefaultNetworks);
                options.Add(new int[4] { 0, 6, -1, -1 });
                options.Add(new int[4] { 0, 1, -1, -1 });


				for (int i = 0; i < GetAllEndpoints.Count; i++)
				{
                    options.Add(new int[4] { i, -1, -1, -1 });
                }

                return options;
			}
		} 

        public static List<Endpoint> GetAllEndpoints => new List<Endpoint>()
        {
            new Endpoint
			{
				Name = "Polkadot",
				URL = "wss://polkadot.api.onfinality.io/public-ws",
				Icon = "polkadot.png",
				CalamarChainName = "polkadot",
				Unit = "Dot",
				Decimals = 10,
				SS58Prefix = 0,
                ChainType = "Substrate",
            },
			new Endpoint
			{
				Name = "Kusama",
				URL = "wss://kusama-rpc.polkadot.io",
                Icon = "kusama.png",
                CalamarChainName = "kusama",
                Unit = "KSM",
                Decimals = 12,
                SS58Prefix = 2,
                ChainType = "Substrate",
            },
			new Endpoint
			{
				Name = "Moonbeam",
				URL = "wss://wss.api.moonbeam.network",
                Icon = "moonbeam.png",
                CalamarChainName = "moonbeam",
                Unit = "GLMR",
                Decimals = 18,
                SS58Prefix = 1284,
                ChainType = "Ethereum",
            },
			new Endpoint
			{
				Name = "Astar",
				URL = "wss://rpc.astar.network",
                Icon = "astar.png",
                CalamarChainName = "astar",
                Unit = "ASTR",
                Decimals = 18,
                SS58Prefix = 5,
                ChainType = "Substrate",
            },
			new Endpoint
			{
				Name = "Westend",
				URL = "wss://westend-rpc.polkadot.io",
                Icon = "westend.png",
                Unit = "WND",
                Decimals = 12,
                SS58Prefix = 42,
                ChainType = "Substrate",
            },
            new Endpoint
            {
                Name = "Rococo",
                URL = "wss://rococo-rpc.polkadot.io",
                Icon = "rococo.png",
                CalamarChainName = "rococo",
                Unit = "ROC",
                Decimals = 12,
                SS58Prefix = 42,
                ChainType = "Substrate",
            },
            new Endpoint
			{
				Name = "Unique",
				URL = "wss://ws.unique.network",
                Icon = "unique.png",
                CalamarChainName = "unique",
                Unit = "UNQ",
                Decimals = 18,
                SS58Prefix = 7391,
                ChainType = "Substrate",
            },
			new Endpoint
			{
				Name = "Opal",
				URL = "wss://eu-ws-opal.unique.network",
                Icon = "opal.png",
                CalamarChainName = "opal",
                Unit = "OPL",
                Decimals = 18,
                SS58Prefix = 42,
                ChainType = "Substrate",
            },
            new Endpoint
            {
                Name = "Moonbase Alpha",
                URL = "wss://wss.api.moonbase.moonbeam.network",
                Icon = "moonbase.png",
                CalamarChainName = "moonbase",
                Unit = "DEV",
                Decimals = 18,
                SS58Prefix = 1287,
                ChainType = "Ethereum",
            },
			new Endpoint
			{
				Name = "Acala",
				URL = "wss://acala-rpc-1.aca-api.network",
                Icon = "acala.png",
                CalamarChainName = "acala",
                Unit = "ACA",
                Decimals = 12,
                SS58Prefix = 10,
                ChainType = "Substrate",
            },
			new Endpoint
			{
				Name = "(Local) ws://127.0.0.1:9944",
                URL = "ws://127.0.0.1:9944",
                Icon = "substrate.png",
                Unit = "",
				Decimals = 0,
				SS58Prefix = 42,
                ChainType = "Substrate",
            }
        };
    }

	public class Endpoint
	{
        public string Name { get; set; }
        public string URL { get; set; }
		public string Icon { get; set; }
		public string CalamarChainName { get; set; }

        // Symbol and Unit are interchangeable names.
		public string Unit { get; set; }
		public int Decimals { get; set; }
		public short SS58Prefix { get; set; }
        public string ChainType { get; set; }
    }
}

