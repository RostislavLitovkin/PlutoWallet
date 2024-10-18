
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Numerics;
using Unique.NetApi.Generated.Model.primitive_types;
using Unique.NetApi.Generated.Storage;
using Unique.NetApi.Generated.Types.Base;
using U256 = Substrate.NetApi.Model.Types.Primitive.U256;

namespace UniqueryPlus.EVM
{
    public static class Helpers
    {
        private static BigInteger SubstrateAddressToUint256(string address) => new BigInteger(Utils.GetPublicKeyFrom(address), true, true);
        
        public static CrossAddress ToCrossAddress(string address)
        {
            return address[0..2] switch
            {
                "0x" => new CrossAddress
                {
                    Eth = address,
                    Sub = 0,
                },
                _ => new CrossAddress
                {
                    Eth = "0x0000000000000000000000000000000000000000",
                    Sub = EVM.Helpers.SubstrateAddressToUint256(address),
                }
            };
        }

        public static Method GetUniqueEVMCallMethod(string substrateAddress, string contractAddress, byte[] calldata, BigInteger? amountToSend)
        {
            int p = 0;
            var source = new H160();
            source.Decode(Utils.GetPublicKeyFrom(substrateAddress), ref p);

            p = 0;
            var target = new H160();
            target.Decode(Utils.HexToByteArray(UniqueContracts.UNIQUE_SELL_CONTRACT_ADDRESS), ref p);

            p = 0;
            BaseVec<U8> input = new BaseVec<U8>();
            input.Decode(calldata, ref p);

            p = 0;
            var value = new Unique.NetApi.Generated.Model.primitive_types.U256();
            var valueArray = new Arr4U64();
            valueArray.Decode(new U256(amountToSend ?? 0).Encode(), ref p);
            value.Value = valueArray;

            // Value taken from https://unique.subscan.io/extrinsic/5908919-2
            var gasLimit = new U64(200_000);

            // Value taken from https://unique.subscan.io/extrinsic/5908919-2
            p = 0;
            var maxFeePerGas = new Unique.NetApi.Generated.Model.primitive_types.U256();
            maxFeePerGas.Decode(Utils.HexToByteArray("0x2bbc2938b4010000000000000000000000000000000000000000000000000000"), ref p);

            var marPriorityFeePerGas = new BaseOpt<Unique.NetApi.Generated.Model.primitive_types.U256>();

            var nonce = new BaseOpt<Unique.NetApi.Generated.Model.primitive_types.U256>();

            var accessList = new Substrate.NetApi.Model.Types.Base.BaseVec<BaseTuple<H160, BaseVec<H256>>>();

            return EVMCalls.Call(source, target, input, value, gasLimit, maxFeePerGas, maxFeePerGas, nonce, accessList);
        }
    }
}
