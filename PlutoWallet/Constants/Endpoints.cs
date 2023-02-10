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
				URL = "wss://rpc.polkadot.io",
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

