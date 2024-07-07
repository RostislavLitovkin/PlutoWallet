
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Opal.NetApi.Generated.Model.primitive_types;
using Opal.NetApi.Generated.Model.sp_core.crypto;
using Opal.NetApi.Generated.Model.sp_runtime.multiaddress;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;

namespace Opal.NetApi.Helper
{
    public static class Generic
    {
       // Define methods
       public static System.Numerics.BigInteger UnitToDecimals(double amount, int decimals)
       {
           return new System.Numerics.BigInteger(Convert.ToUInt64(amount * Math.Pow(10, decimals)));
       }

       public static string ToHexString(this H256 h256)
       {
           return Utils.Bytes2HexString(h256.Value.Value.Select(p => p.Value).ToArray());
       }

       public static string ToPublicKeyHex(this AccountId32 account32)
       {
           return Utils.Bytes2HexString(account32.ToPublicKey());
       }

       public static byte[] ToPublicKey(this string address)
       {
           return Utils.GetPublicKeyFrom(address);
       }

       public static byte[] ToPublicKey(this AccountId32 account32)
       {
           return account32.Value.Value.Select(p => p.Value).ToArray();
       }

       public static string ToAddress(this AccountId32 account32, short ss58 = 42)
       {
           byte[] pubKey = account32.Value.Value.Select(p => p.Value).ToArray();
           return pubKey.ToAddress(ss58);
       }

       public static string ToAddress(this byte[] publicKey, short ss58 = 42)
       {
           return Utils.GetAddressFrom(publicKey, ss58);
       }

       public static AccountId32 ToAccountId32(this byte[] publicKey)
       {
           var account32 = new AccountId32();
           account32.Create(publicKey);
           return account32;
       }

       public static AccountId32 ToAccountId32(this Account account)
       {
           var account32 = new AccountId32();
           account32.Create(account.Bytes);
           return account32;
       }

       public static AccountId32 ToAccountId32(this string address)
       {
           var account32 = new AccountId32();
           account32.Create(address.ToPublicKey());
           return account32;
       }

       public static EnumMultiAddress ToEnumMultiAddress(this AccountId32 accountId32)
       {
           var multiAddress = new EnumMultiAddress();
           multiAddress.Create(MultiAddress.Id, accountId32);
           return multiAddress;
       }

       public static H256 ToH256(this string hash)
       {
           var h256 = new H256();
           h256.Create(hash);
           return h256;
       }

       public static H256 ToHash(this string name)
       {
           byte[] nameHash = HashExtension.Twox256(Encoding.UTF8.GetBytes(name));
           var h256 = new H256();
           h256.Create(nameHash);
           return h256;
       }

       public static U8 ToU8(this byte number)
       {
           var u8 = new U8();
           u8.Create(number);
           return u8;
       }

       public static U8 ToU8(this char character)
       {
           var u8 = new U8();
           u8.Create(BitConverter.GetBytes(character)[0]);
           return u8;
       }

       public static U16 ToU16(this ushort number)
       {
           var u16 = new U16();
           u16.Create(number);
           return u16;
       }

       public static U32 ToU32(this uint number)
       {
           var u32 = new U32();
           u32.Create(number);
           return u32;
       }

       public static U128 ToU128(this System.Numerics.BigInteger number)
       {
           var u128 = new U128();
           u128.Create(number);
           return u128;
       }

       public static U8[] ToU8Array(this byte[] bytes)
       {
           return bytes.Select(p => p.ToU8()).ToArray();
       }

       public static U8[] ToU8Array(this string str)
       {
           return str.Select(p => p.ToU8()).ToArray();
       }

       public static U16[] ToU16Array(this ushort[] bytes)
       {
           return bytes.Select(p => p.ToU16()).ToArray();
       }

       public static U32[] ToU32Array(this uint[] bytes)
       {
           return bytes.Select(p => p.ToU32()).ToArray();
       }

       public static BaseOpt<U8> ToBaseOpt(this U8 u8)
       {
           var baseOpt = new BaseOpt<U8>();
           baseOpt.Create(u8);
           return baseOpt;
       }

       public static byte[] ToBytes(this H256 h256)
       {
           return h256.Value.Value.ToBytes();
       }

       public static byte[] ToBytes(this BaseVec<U8> baseVecU8)
       {
           return baseVecU8.Value.ToBytes();
       }

       public static byte[] ToBytes(this U8[] arrayU8)
       {
           return arrayU8.Select(p => p.Value).ToArray();
       }

       public static string ToText(this BaseVec<U8> baseVecU8)
       {
           return Encoding.UTF8.GetString(baseVecU8.Value.ToBytes());
       }

       public static IEnumerable<IEnumerable<T>> BuildChunksOf<T>(IEnumerable<T> fullList, int batchSize)
       {
           int total = 0;
           while (total < fullList.Count())
           {
               yield return fullList.Skip(total).Take(batchSize);
               total += batchSize;
           }
       }
   }
}

