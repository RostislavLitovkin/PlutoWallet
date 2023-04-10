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
            },
			new Endpoint
			{
				Name = "Kusama",
				URL = "wss://kusama-rpc.polkadot.io",
                Icon = "kusama.png",
                CalamarChainName = "kusama",
            },
			new Endpoint
			{
				Name = "Moonbeam",
				URL = "wss://wss.api.moonbeam.network",
                Icon = "moonbeam.png",
                CalamarChainName = "moonbeam",
            },
			new Endpoint
			{
				Name = "Astar",
				URL = "wss://rpc.astar.network",
                Icon = "astar.png",
                CalamarChainName = "astar",
            },
			new Endpoint
			{
				Name = "Westend Polkadot",
				URL = "wss://westend-rpc.polkadot.io",
                Icon = "westend.png",
            },
            new Endpoint
            {
                Name = "Rococo Polkadot",
                URL = "wss://rococo-rpc.polkadot.io",
                Icon = "rococo.png",
                CalamarChainName = "rococo",
            },
            new Endpoint
			{
				Name = "Unique",
				URL = "wss://eu-ws-quartz.unique.network",
                Icon = "unique.png",
                CalamarChainName = "unique",
            },
			new Endpoint
			{
				Name = "Opal",
				URL = "wss://eu-ws-opal.unique.network",
                Icon = "opal.png",
                CalamarChainName = "opal",
            },
			new Endpoint
			{
				Name = "Acala",
				URL = "wss://acala-rpc-1.aca-api.network",
                Icon = "acala.png",
                CalamarChainName = "acala",
            },
			new Endpoint
			{
				Name = "(Local) ws://127.0.0.1:9944",
                URL = "ws://127.0.0.1:9944",
                Icon = "substrate.png",
            }
        };
    }

	public class Endpoint
	{
        public string Name { get; set; }
        public string URL { get; set; }
		public string Icon { get; set; }
		public string CalamarChainName { get; set; }
    }
}

