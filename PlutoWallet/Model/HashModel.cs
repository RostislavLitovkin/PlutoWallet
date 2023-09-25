using System;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Primitive;
using System.Numerics;

namespace PlutoWallet.Model
{
	public class HashModel
	{
		public static U32 GetU32FromBlake2_128Concat(string hash)
		{
			if (hash.Substring(0, 2) == "0x")
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

        public static U64 GetU64FromBlake2_128Concat(string hash)
        {
            if (hash.Substring(0, 2) == "0x")
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
            if (hash.Substring(0, 2) == "0x")
            {
                hash = hash.Substring(2);
            }

            if (hash.Length != 64)
            {
                throw new Exception("bad hash input");
            }

            U128 num = new U128();
            num.Create(Utils.HexToByteArray(hash.Substring(32, 32)));

            return num;
        }

        public static U32 GetU32FromTwox_64Concat(string hash)
        {
            if (hash.Substring(0, 2) == "0x")
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

        public static BigInteger GetBigIntegerFromBlake2_128Concat(string hash)
        {
            if (hash.Substring(0, 2) == "0x")
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
            if (hash.Substring(0, 2) == "0x")
            {
                hash = hash.Substring(2);
            }

            if (hash.Length == 24)
            {
                return GetU32FromTwox_64Concat(hash).Value;
            }

            throw new Exception("Hash is in bad format");
        }
    }
}

