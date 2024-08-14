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
    public TempUnCheckedExtrinsic(bool signed, Account account, Method method, Era era, CompactInteger nonce, ChargeType charge, Hash genesis, Hash startEra)
        : base(signed, account, nonce, method, era, charge)
    {
        Genesis = genesis;
        StartEra = startEra;
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
        return new TempPayload(base.Method, new TempSignedExtensions(runtime.SpecVersion, runtime.TransactionVersion, Genesis, StartEra, base.Era, base.Nonce, base.Charge));
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
        list.AddRange(base.Account.Encode());
        list.Add(base.Account.KeyTypeByte);
        if (base.Signature != null)
        {
            list.AddRange(base.Signature);
        }

        list.AddRange(base.Era.Encode());
        list.AddRange(base.Nonce.Encode());
        list.AddRange(base.Charge.Encode());
        list.AddRange(base.CheckMetadataHash.EncodeExtra());
        list.AddRange(base.Method.Encode());
        return Utils.SizePrefixedByteArray(list);
    }
}