using System;
namespace PlutoWallet.Constants
{
	public class Endpoints
	{
		public static string RococoFrequency => "wss://rpc.rococo.frequency.xyz";

		/*public static Dictionary<string, string> GetAllEndpoints => new Dictionary<string, string>()
		{
			{ "polkadot", "wss://rpc.polkadot.io" },
			{ "rococoFrequency", "wss://rpc.rococo.frequency.xyz" }

		};*/

        public static List<Endpoint> GetAllEndpoints => new List<Endpoint>()
        {
            new Endpoint
			{
				Name = "Polkadot",
				URL = "wss://polkadot.api.onfinality.io/public-ws",
            },
			new Endpoint
			{
				Name = "Kusama",
				URL = "wss://kusama-rpc.polkadot.io"
            },
			new Endpoint
			{
				Name = "Westend Polkadot",
				URL = "wss://westend-rpc.polkadot.io"
            },
            new Endpoint
            {
                Name = "Rococo Polkadot",
                URL = "wss://rococo-rpc.polkadot.io"
            },
            new Endpoint
			{
				Name = "Unique",
				URL = "wss://eu-ws-quartz.unique.network"
            },
			new Endpoint
			{
				Name = "Opal",
				URL = "wss://eu-ws-opal.unique.network",
			},
			new Endpoint
			{
                Name = "Rococo Frequency",
                URL = "wss://rpc.rococo.frequency.xyz",
            },
			new Endpoint
			{
				Name = "Acala",
				URL = "wss://acala-rpc-1.aca-api.network",
            },
			new Endpoint
			{
				Name = "(Local) ws://127.0.0.1:9944",
                URL = "ws://127.0.0.1:9944"
			}
        };
    }

	public class Endpoint
	{
        public string Name { get; set; }
        public string URL { get; set; }
    }
}

