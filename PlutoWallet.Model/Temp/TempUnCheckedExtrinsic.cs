using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;

namespace Substrate.NetApi.Model.Extrinsics;

//
// Summary:
//     Unchecked Extrinsic
public class TempUnCheckedExtrinsic : TempExtrinsic
{
    //
    // Summary:
    //     Genesis Hash
    private Hash Genesis { get; }

    //
    // Summary:
    //     Start Era
    private Hash StartEra { get; }

    //
    // Summary:
    //     Initializes a new instance of the Substrate.NetApi.Model.Extrinsics.UnCheckedExtrinsic
    //     class.
    //
    // Parameters:
    //   signed:
    //     if set to true [signed].
    //
    //   account:
    //     The account.
    //
    //   method:
    //     The method.
    //
    //   era:
    //     The era.
    //
    //   nonce:
    //     The nonce.
    //
    //   charge:
    //
    //   genesis:
    //     The genesis.
    //
    //   startEra:
    //     The start era.
    public TempUnCheckedExtrinsic(bool signed, Account account, Method method, Era era, CompactInteger nonce, ChargeType charge, Hash genesis, Hash startEra, uint addressVersion, bool checkMetadata)
        : base(signed, account, nonce, method, era, charge)
    {
        Genesis = genesis;
        StartEra = startEra;
        AddressVersion = addressVersion;
        CheckMetadata = checkMetadata;
    }

    //
    // Summary:
    //     Gets the payload.
    //
    // Parameters:
    //   runtime:
    //     The runtime.
    public TempPayload GetPayload(RuntimeVersion runtime)
    {
        return new TempPayload(base.Method, new TempSignedExtensions(runtime.SpecVersion, runtime.TransactionVersion, Genesis, StartEra, base.Era, base.Nonce, base.Charge, CheckMetadata));
    }

    //
    // Summary:
    //     Adds the payload signature.
    //
    // Parameters:
    //   signature:
    //     The signature.
    public void AddPayloadSignature(byte[] signature)
    {
        base.Signature = signature;
    }

    public uint AddressVersion;

    public bool CheckMetadata;

    /// <summary>
    /// https://polkadot.js.org/docs/api/FAQ/#i-cannot-send-transactions-sending-yields-decoding-failures
    /// </summary>
    public byte[] AccountEncode()
    {
        List<byte> list = new List<byte>();
        switch (AddressVersion)
        {
            case 0u:
                return Account.Bytes;
            case 1u:
                list.Add(byte.MaxValue);
                list.AddRange(Account.Bytes);
                return list.ToArray();
            case 2u:
                list.Add(0);
                list.AddRange(Account.Bytes);
                return list.ToArray();
            default:
                throw new NotImplementedException("Unknown address version please refer to PlutoAccountBase");
        }
    }

    //
    // Summary:
    //     Encode this instance, returns the encoded bytes.
    //
    // Exceptions:
    //   T:System.NotSupportedException:
    public new byte[] Encode()
    {
        if (base.Signed && base.Signature == null)
        {
            throw new NotSupportedException("Missing payload signature for signed transaction.");
        }

        List<byte> list = new List<byte>();
        list.Add((byte)(4u | (base.Signed ? 128u : 0u)));
        list.AddRange(AccountEncode());
        list.Add(base.Account.KeyTypeByte);
        if (base.Signature != null)
        {
            list.AddRange(base.Signature);
        }

        list.AddRange(base.Era.Encode());
        list.AddRange(base.Nonce.Encode());
        list.AddRange(base.Charge.Encode());
        if (CheckMetadata)
        {
            list.AddRange(base.CheckMetadataHash.EncodeExtra());
        }
        list.AddRange(base.Method.Encode());
        return Utils.SizePrefixedByteArray(list);
    }
}

public static class TempUnCheckedExtrinsicHelper
{
    public static TempUnCheckedExtrinsic ToTempUnCheckedExtrinsic(this UnCheckedExtrinsic original, Payload originalPayload, uint addressVersion, bool checkMetadata)
    {
        return new TempUnCheckedExtrinsic
        (
            signed: original.Signed,
            account: original.Account,
            method: original.Method,
            era: original.Era,
            nonce: original.Nonce,
            charge: original.Charge,
            genesis: originalPayload.SignedExtension.Genesis,
            startEra: originalPayload.SignedExtension.StartEra,
            addressVersion: addressVersion,
            checkMetadata: checkMetadata
        );
    }
}