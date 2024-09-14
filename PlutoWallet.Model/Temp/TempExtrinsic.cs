using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Substrate.NetApi.Model.Types;

namespace Substrate.NetApi.Model.Extrinsics;

//
// Summary:
//     Extrinsic
public class TempExtrinsic
{
    //
    // Summary:
    //     Signed
    public bool Signed { get; set; }

    //
    // Summary:
    //     Transaction Version
    public byte TransactionVersion { get; set; }

    //
    // Summary:
    //     Account
    public Account Account { get; set; }

    //
    // Summary:
    //     Era
    public Era Era { get; set; }

    //
    // Summary:
    //     Nonce
    public CompactInteger Nonce { get; set; }

    //
    // Summary:
    //     Charge
    public ChargeType Charge { get; set; }

    //
    // Summary:
    //     Method
    public Method Method { get; set; }

    //
    // Summary:
    //     CheckMetadataHash
    public TempCheckMetadataHash CheckMetadataHash { get; set; }

    //
    // Summary:
    //     Signature
    public byte[] Signature { get; set; }

    //
    // Summary:
    //     Initializes a new instance of the Substrate.NetApi.Model.Extrinsics.Extrinsic
    //     class.
    //
    // Parameters:
    //   str:
    //     The string.
    //
    //   chargeType:
    public TempExtrinsic(string str, ChargeType chargeType)
        : this(Utils.HexToByteArray(str).AsMemory(), chargeType)
    {
    }

    //
    // Summary:
    //     Initializes a new instance of the Substrate.NetApi.Model.Extrinsics.Extrinsic
    //     class.
    //
    // Parameters:
    //   memory:
    //
    //   chargeType:
    internal TempExtrinsic(Memory<byte> memory, ChargeType chargeType)
    {
        Console.WriteLine("Trying to decode extrinsic");
        int p = 0;
        CompactInteger.Decode(memory.ToArray(), ref p);
        int num = 1;
        byte b = memory.Slice(p, num).ToArray()[0];
        Signed = b >= 128;
        TransactionVersion = (byte)(b - (Signed ? 128 : 0));
        p += num;
        if (Signed)
        {
            num = 1;
            _ = memory.Slice(p, num).ToArray()[0];
            p += num;
            num = 32;
            byte[] publicKey = memory.Slice(p, num).ToArray();
            p += num;
            num = 1;
            byte keyType = memory.Slice(p, num).ToArray()[0];
            p += num;
            Account account = new Account();
            account.Create((KeyType)keyType, publicKey);
            Account = account;
            num = 64;
            Signature = memory.Slice(p, num).ToArray();
            p += num;
            num = 1;
            byte[] array = memory.Slice(p, num).ToArray();
            if (array[0] != 0)
            {
                num = 2;
                array = memory.Slice(p, num).ToArray();
            }

            Era = Era.Decode(array);
            p += num;
            Nonce = CompactInteger.Decode(memory.ToArray(), ref p);
            Charge = chargeType;
            Charge.Decode(memory.ToArray(), ref p);
            CheckMetadataHash = new TempCheckMetadataHash();
            CheckMetadataHash.Decode(memory.ToArray(), ref p);
        }

        num = 2;
        byte[] array2 = memory.Slice(p, num).ToArray();
        p += num;
        byte[] parameters = memory.Slice(p).ToArray();
        Method = new Method(array2[0], array2[1], parameters);
    }

    //
    // Summary:
    //     Initializes a new instance of the Substrate.NetApi.Model.Extrinsics.Extrinsic
    //     class.
    //
    // Parameters:
    //   signed:
    //     if set to true [signed].
    //
    //   account:
    //     The account.
    //
    //   nonce:
    //     The nonce.
    //
    //   method:
    //     The method.
    //
    //   era:
    //     The era.
    //
    //   charge:
    public TempExtrinsic(bool signed, Account account, CompactInteger nonce, Method method, Era era, ChargeType charge)
    {
        Signed = signed;
        TransactionVersion = 4;
        Account = account;
        Era = era;
        Nonce = nonce;
        Charge = charge;
        CheckMetadataHash = new TempCheckMetadataHash();
        Method = method;
    }

    //
    // Summary:
    //     Encodes this instance.
    public byte[] Encode()
    {
        List<byte> list = new List<byte>();
        byte item = (byte)((Signed ? 128 : 0) + TransactionVersion);
        list.Add(item);
        if (Signed)
        {
            list.AddRange(Account.Encode());
            list.Add(Account.KeyTypeByte);
            list.AddRange(Signature);
            list.AddRange(Era.Encode());
            list.AddRange(Nonce.Encode());
            list.AddRange(Charge.Encode());
            list.AddRange(CheckMetadataHash.EncodeExtra());
        }

        list.AddRange(Method.Encode());
        list.InsertRange(0, new CompactInteger(list.Count).Encode());
        return list.ToArray();
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}