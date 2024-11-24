using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi;
using System.Numerics;
using System.Collections;

namespace UniqueryPlus
{
    public static class Helpers
    {
        public static Mythos.NetApi.Generated.Model.primitive_types.U256 GetMythosU256FromBigInteger(BigInteger value)
        {
            // Ensure the BigInteger fits within 256 bits
            if (value < BigInteger.Zero || value >= BigInteger.Pow(2, 256))
                throw new ArgumentOutOfRangeException(nameof(value), "BigInteger must fit within 256 bits.");

            // Create an array of 4 longs
            var parts = new U64[4];

            // Get the 64-bit segments
            for (int i = 0; i < parts.Length; i++)
            {
                // Shift right by 64 * i bits, then convert to long
                parts[i] = new U64((ulong)(value >> (64 * i) & BigInteger.Pow(2, 64) - 1));
            }

            return new Mythos.NetApi.Generated.Model.primitive_types.U256
            {
                Value = new Mythos.NetApi.Generated.Types.Base.Arr4U64
                {
                    Value = parts
                }
            };
        }

        public static BigInteger GetBigIntegerFromMythosU256(Mythos.NetApi.Generated.Model.primitive_types.U256 value)
        {
            BigInteger bigInt = BigInteger.Zero;

            for (int i = 0; i < 4; i++)
            {
                // Shift the part back to its original position and add to BigInteger
                bigInt |= (BigInteger)value.Value.Value[i].Value << (64 * i);
            }

            return bigInt;
        }
        public static U32 GetU32FromBlake2_128Concat(string hash)
        {
            if (hash.Substring(0, 2).Equals("0x") || hash.Substring(0, 2).Equals("0X"))
            {
                hash = hash.Substring(2);
            }

            if (hash.Length != 40)
            {
                throw new Exception("bad hash input");
            }

            U32 num = new U32();
            num.Create(Utils.HexToByteArray(hash.Substring(32, 8)));

            return num;
        }

        public static byte[] AnyAddressToEthereumAccountId20Encoded(string address)
        {
            if (address.Substring(0, 2).Equals("0x"))
            {
                if (address.Length < 42 || address.Length % 2 == 1)
                {
                    throw new Exception("Address is in bad format, It is either too short or has an odd number of characters");
                }
                return Utils.HexToByteArray(address.Substring(0, 42));
            }

            return Utils.GetPublicKeyFrom(address);
        }

        public static U64 GetU64FromBlake2_128Concat(string hash)
        {
            if (hash.Substring(0, 2).Equals("0x") || hash.Substring(0, 2).Equals("0X"))
            {
                hash = hash.Substring(2);
            }

            if (hash.Length != 48)
            {
                throw new Exception("bad hash input");
            }

            U64 num = new U64();
            num.Create(Utils.HexToByteArray(hash.Substring(32, 16)));

            return num;
        }

        public static U128 GetU128FromBlake2_128Concat(string hash)
        {
            if (hash.Substring(0, 2).Equals("0x") || hash.Substring(0, 2).Equals("0X"))
            {
                hash = hash.Substring(2);
            }

            if (hash.Length != 64)
            {
                throw new Exception("bad hash input");
            }

            U128 num = new U128();

            var encodedBytes = Utils.HexToByteArray(hash.Substring(32, 32));
            num.Create(encodedBytes);

            num.Value = new BigInteger(encodedBytes, isUnsigned: true);

            return num;
        }

        public static U32 GetU32FromTwox_64Concat(string hash)
        {
            if (hash.Substring(0, 2).Equals("0x") || hash.Substring(0, 2).Equals("0X"))
            {
                hash = hash.Substring(2);
            }

            if (hash.Length != 24)
            {
                throw new Exception("bad hash input");
            }

            U32 num = new U32();
            num.Create(Utils.HexToByteArray(hash.Substring(16, 8)));

            return num;
        }

        public static U16 GetU16FromTwox_64Concat(string hash)
        {
            if (hash.Substring(0, 2).Equals("0x") || hash.Substring(0, 2).Equals("0X"))
            {
                hash = hash.Substring(2);
            }

            if (hash.Length != 20)
            {
                throw new Exception("bad hash input");
            }

            U16 num = new U16();
            num.Create(Utils.HexToByteArray(hash.Substring(16, 4)));

            return num;
        }

        public static BigInteger GetBigIntegerFromBlake2_128Concat_MythosU256(string hash)
        {
            if (hash.Substring(0, 2).Equals("0x") || hash.Substring(0, 2).Equals("0X"))
            {
                hash = hash.Substring(2);
            }

            if (hash.Length == 96)
            {
                var encodedBytes = Utils.HexToByteArray(hash.Substring(32));

                var u256 = new Mythos.NetApi.Generated.Model.primitive_types.U256();
                int _p = 0;
                u256.Decode(encodedBytes, ref _p);

                return GetBigIntegerFromMythosU256(u256);
            }

            throw new Exception("Hash is in bad format");
        }

        public static BigInteger GetBigIntegerFromBlake2_128Concat(string hash)
        {
            if (hash.Substring(0, 2).Equals("0x") || hash.Substring(0, 2).Equals("0X"))
            {
                hash = hash.Substring(2);
            }

            if (hash.Length == 40)
            {
                return GetU32FromBlake2_128Concat(hash).Value;
            }
            else if (hash.Length == 48)
            {
                return GetU64FromBlake2_128Concat(hash).Value;
            }
            else if (hash.Length == 64)
            {
                return GetU128FromBlake2_128Concat(hash).Value;
            }

            throw new Exception("Hash is in bad format");
        }

        public static BigInteger GetBigIntegerFromTwox_64Concat(string hash)
        {
            if (hash.Substring(0, 2).Equals("0x") || hash.Substring(0, 2).Equals("0X"))
            {
                hash = hash.Substring(2);
            }

            if (hash.Length == 24)
            {
                return GetU32FromTwox_64Concat(hash).Value;
            }

            if (hash.Length == 20)
            {
                return GetU16FromTwox_64Concat(hash).Value;
            }

            throw new Exception("Hash is in bad format");
        }

        public static BigInteger? GetValueOrNull(this Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128> value)
        {
            if (value.OptionFlag)
                return value.Value;
            return null;
        }

        public static uint? GetValueOrNull(this Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U32> value)
        {
            if (value.OptionFlag)
                return value.Value;
            return null;
        }

        public static byte[] RemoveCompactIntegerPrefix(byte[] bytes)
        {
            int p = 0;
            CompactInteger.Decode(bytes, ref p);

            return bytes[p..];
        }
    }
}
