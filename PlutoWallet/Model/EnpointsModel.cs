using System;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Constants;

namespace PlutoWallet.Model
{
    public class EndpointsModel
    {
        public static string DefaultEndpoints = "[polkadot, kusama]";

        public static string[] GetSelectedEndpointKeys()
        {
            string[] endpointKeys = Preferences.Get("SelectedNetworks", DefaultEndpoints).Trim(new char[] { '[', ']' }).Split(',');

            for (int i = 0; i < endpointKeys.Length; i++)
            {
                endpointKeys[i] = endpointKeys[i].Trim();
            }

            return endpointKeys;
        }

        public static void SaveEndpoints(List<string> keys)
        {
            string result = "[";
            if (keys.Count() == 0)
            {
                result = "[polkadot]";
            }
            else
            {
                foreach (string key in keys)
                {
                    result += key + ", ";
                }

                result = result.Substring(0, result.Length - 2) + "]";
            }

            Preferences.Set("SelectedNetworks", result);

            // Save it in the plutolayout:
            string plutoLayoutString = Preferences.Get("PlutoLayout", CustomLayoutModel.DEFAULT_PLUTO_LAYOUT);

            string[] itemsAndNetworksStrings = plutoLayoutString.Split(";");

            string plutoLayoutResult = itemsAndNetworksStrings[0] + ";" + result;

            for (int i = 2; i < itemsAndNetworksStrings.Length; i++)
            {
                plutoLayoutResult += ";" + itemsAndNetworksStrings[i];
            }

            Preferences.Set("PlutoLayout", plutoLayoutResult);
            
            Console.WriteLine("Other Save Endpoint -> Calling MultiNetworkSelectViewModel.SetupDefault()");

            var viewModel = DependencyService.Get<MultiNetworkSelectViewModel>();

            viewModel.SetupDefault();
        }

        public static Endpoint GetEndpoint(string key, bool reverse = false)
        {
            Endpoint endpoint = Endpoints.GetEndpointDictionary[key];

            return endpoint;
        }

        public static List<Endpoint> GetNftEndpoints
        {
            get
            {
                List<Endpoint> endpoints = Endpoints.GetAllEndpoints;

                foreach (Endpoint endpoint in endpoints)
                {
                    if (!endpoint.SupportsNfts)
                    {
                        endpoints.Remove(endpoint);
                    }
                }

                return endpoints;
            }
        }
    }
}

