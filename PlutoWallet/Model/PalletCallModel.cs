using System;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi;
using Newtonsoft.Json;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Meta;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using Substrate.NetApi.Generated.Model.sp_runtime.multiaddress;
using Substrate.NetApi.Model.Types;
using Chaos.NaCl;
using Schnorrkel;

namespace PlutoWallet.Model
{
    public class PalletNotFoundException : Exception
    {
        public PalletNotFoundException()
        {
        }

        public PalletNotFoundException(string message)
            : base(message)
        {
        }

        public PalletNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class CallNotFoundException : Exception
    {
        public CallNotFoundException()
        {
        }

        public CallNotFoundException(string message)
            : base(message)
        {
        }

        public CallNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class PalletCallModel
	{


        /// <summary>
        /// Method that returns the pallet index and call index for the respective pallet and call names
        /// </summary>
        /// <param name="client">SubstrateClient</param>
        /// <param name="palletName"></param>
        /// <param name="callName"></param>
        /// <returns>byte values for the pallet and call</returns>
        /// <exception cref="PalletNotFoundException"></exception>
        /// <exception cref="CallNotFoundException"></exception>
        public static (byte, byte) GetPalletAndCallIndex(SubstrateClientExt client, string palletName, string callName)
		{
            var pallets = client.MetaData.NodeMetadata.Modules.Values.ToList<PalletModule>();

            int palletIndex = -1;
            int metadataPalletIndex = -1;

            for (int i = 0; i < pallets.Count; i++)
            {
                //Console.WriteLine(i + ") " + pallets[i].Name + " :: " + pallets[i].Index);
                if (pallets[i].Name == palletName)
                {
                    palletIndex = (int)pallets[i].Index;
                    metadataPalletIndex = i;
                    break;
                }
            }

            if (metadataPalletIndex == -1)
            {
                throw new PalletNotFoundException("There is no Balances pallet.");
            }

            long callIndex = -1;

            for (int i = 0; i < client.CustomMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Variants.Count(); i++)
            {
                /*Console.WriteLine(
                    i + ") " +
                    client.CustomMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Variants[i].Name
                    + " :: " +
                    client.CustomMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Variants[i].Index);
                */

                //Console.WriteLine(client.CustomMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Path[0]);

                if (client.CustomMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Variants[i].Name == callName)
                {
                    callIndex = client.CustomMetadata.NodeMetadata.Types[pallets[metadataPalletIndex].Calls.TypeId.ToString()].Variants[i].Index;
                }
            }

            if (callIndex == -1)
            {
                throw new CallNotFoundException("There is no transfer call.");
            }

            return ((byte)palletIndex, (byte)callIndex);
        }

        public static (string, string) GetPalletAndCallName(SubstrateClientExt client, byte palletIndex, byte callIndex)
        {
            string palletName = client.CustomMetadata.NodeMetadata.Modules[palletIndex.ToString()].Name;

            string callName = "";

            foreach (var variant in client.CustomMetadata.NodeMetadata.Types[client.CustomMetadata.NodeMetadata.Modules[palletIndex.ToString()].Calls.TypeId.ToString()].Variants)
            {
                if (variant.Index == callIndex)
                {
                    callName = variant.Name;
                    break;
                }
            }

            return (palletName, callName);
        }

        /// <summary>
        /// Finds and formats a Method in a JSON format according to the Metadata found in the supplied SubstrateClient
        /// </summary>
        /// <param name="client">SubstrateClient</param>
        /// <param name="method">Method</param>
        /// <returns>JSON string</returns>
        public static string GetJsonMethod(SubstrateClientExt client, Method method)
        {
            string palletName = client.CustomMetadata.NodeMetadata.Modules[method.ModuleIndex.ToString()].Name;

            string callName = "";

            string parameters = "";

            TypeField[] typeFields = new TypeField[0];

            foreach (var variant in client.CustomMetadata.NodeMetadata.Types[client.CustomMetadata.NodeMetadata.Modules[method.ModuleIndex.ToString()].Calls.TypeId.ToString()].Variants)
            {
                if (variant.Index == method.CallIndex)
                {
                    callName = variant.Name;

                    typeFields = variant.TypeFields;
                    break;
                }
            }


            // Add parameters if there are any
            if (typeFields.Length != 0)
            {

                // byte[] position
                int p = 0;

                foreach (var typeField in typeFields)
                {
                    string typeId = typeField.TypeId.ToString();

                    string data = "";

                    TypeValue t = client.CustomMetadata.NodeMetadata.Types[typeId];

                    Console.WriteLine("This info is for " + typeField.TypeName + ": ");

                    if (t.TypeFields != null)
                    {
                        Console.WriteLine("It has got typefields: " + t.TypeFields.Length);
                    }

                    if (t.Path != null)
                    {
                        Console.WriteLine("It has got path: " + t.Path);
                    }

                    if (t.Variants != null)
                    {
                        Console.WriteLine("It has got variants: " + t.Variants.Length);
                    }

                    if (t.Primitive != null)
                    {
                        Console.WriteLine("It has got primitives: " + t.Primitive);
                    }

                    if (t.TypeParams != null)
                    {
                        Console.WriteLine("It has got params: " + t.TypeParams);
                    }

                    if (t.TypeId != null)
                    {
                        Console.WriteLine("It has got typeId: " + t.TypeId);
                    }

                    if (t.Length != null)
                    {
                        Console.WriteLine("It has got a length: " + t.Length);
                    }

                    Console.WriteLine("Orig id: " + typeField.TypeId);

                    Console.WriteLine("It has got typeDef: " + t.TypeDef);

                    switch (t.TypeDef)
                    {
                        case TypeDef.Sequence:
                            TypeValue subType = client.CustomMetadata.NodeMetadata.Types[t.TypeId.ToString()];

                            if (subType.Primitive != null)
                            {
                                var temp = Model.ToStringModel.SequenceValueToString(subType, method.Parameters, ref p);
                                if (temp == null)
                                {
                                    parameters = "Unable to show";
                                    goto Unsupported;
                                }
                                data = temp;
                                break;
                            }
                            else
                            {
                                parameters = "Unable to show";
                                goto Unsupported;
                            }

                        case TypeDef.Compact:
                            BaseCom<U128> com = new BaseCom<U128>();
                            com.Decode(method.Parameters, ref p);

                            data = com.Value.ToString();
                            break;

                        case TypeDef.Primitive:
                            data = Model.ToStringModel.PrimitiveValueToString(t, method.Parameters, ref p);
                            break;

                        case TypeDef.Variant:
                            switch (typeField.TypeName)
                            {
                                case "AccountIdLookupOf<T>":
                                    var multiAddress = new EnumMultiAddress();
                                    multiAddress.Decode(method.Parameters, ref p);

                                    if (multiAddress.Value != MultiAddress.Index)
                                    {
                                        data = Utils.GetAddressFrom(multiAddress.Value2.Encode());
                                        break;
                                    }
                                    else
                                    {
                                        parameters = "Unable to show";
                                        goto Unsupported;
                                    }
                                default:
                                    parameters = "Unable to show";
                                    goto Unsupported;
                            }
                            break;

                        default:
                            parameters = "Unable to show";
                            goto Unsupported;
                    }

                    parameters += "\n\t" + typeField.Name + ": " + data + ",";
                }
                parameters = parameters.Substring(0, parameters.Length - 1);
            }
            
        Unsupported:

            // Construct the final JSON
            string resultJson = palletName + "." + callName + "(" + parameters + ")";

            return resultJson;
        }
    }
}

