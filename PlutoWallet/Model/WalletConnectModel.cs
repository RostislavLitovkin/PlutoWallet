using System;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Events;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine.Events;
using WalletConnectSharp.Web3Wallet;

namespace PlutoWallet.Model
{
	public class WalletConnectModel
	{
        private static bool loading = false;
		public static async Task Connect(string uri)
		{
            if (loading)
            {
                return;
            }

            loading = true;
            DotNetEnv.Env.Load("/Users/rostislavlitovkin/Programming/PlutoWallet/.env");

            var options = new CoreOptions()
            {
                ProjectId = DotNetEnv.Env.GetString("WALLET_CONNECT_ID"),
                Name = "PlutoWallet",
            };

            var core = new WalletConnectCore(options);

            var metadata = new Metadata()
            {
                Description = "Mobile wallet for all substrate based chains.",
                Icons = new[] { "https://plutonication-53tvi.ondigitalocean.app/plutowalleticon" },
                Name = "PlutoWallet",
                Url = "https://github.com/rostislavlitovkin/plutowallet",
            };

            var sdk = await Web3WalletClient.Init(core, metadata, metadata.Name);

            await sdk.Pair(uri);

            Console.WriteLine("Paired to wc");

            sdk.On<SessionProposalEvent>(EngineEvents.SessionProposal, async (sender, @event) =>
            {
                ProposalStruct proposal = @event.EventData.Proposal;


                Console.WriteLine("proposal: ");
                Console.WriteLine(proposal);
                RequiredNamespaces requiredNamespaces = proposal.RequiredNamespaces;
                Console.WriteLine("Req namespaces: ");

                string chain_id = "";
                foreach (string key in requiredNamespaces.Keys)
                {
                    Console.WriteLine(key);
                    ProposedNamespace proposedNamespace = requiredNamespaces[key];
                    Console.WriteLine("Chains: ");
                    foreach (string chain in proposedNamespace.Chains)
                    {
                        Console.WriteLine(chain);
                        chain_id = chain;
                    }
                    Console.WriteLine("Methods: ");
                    foreach (string method in proposedNamespace.Methods)
                    {
                        Console.WriteLine(method);
                    }

                    Console.WriteLine("Events: ");
                    foreach (string e in proposedNamespace.Events)
                    {
                        Console.WriteLine(e);
                    }
                }

                string wc_address = chain_id + ":" + Substrate.NetApi.Utils.GetAddressFrom(KeysModel.GetPublicKeyBytes(), AjunaClientModel.SelectedEndpoint.SS58Prefix);

                Console.WriteLine("Here is the address for " + wc_address);

                var sessionData = await sdk.ApproveSession(proposal, wc_address);

                Console.WriteLine("Approved :D nice!");
                //var sessionTopic = sessionData.Topic;
            });
        }
    }
}

