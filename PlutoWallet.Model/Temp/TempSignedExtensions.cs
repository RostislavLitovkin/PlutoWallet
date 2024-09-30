using System.Collections.Generic;
using Substrate.NetApi.Model.Types.Base;

namespace Substrate.NetApi.Model.Extrinsics
{
    /// <summary>
    /// Signed Extensions
    /// </summary>
    public class TempSignedExtensions
    {
        /// <summary>
        /// Specification Version
        /// </summary>
        public uint SpecVersion { get; }

        /// <summary>
        /// Transaction Version
        /// </summary>
        public uint TxVersion { get; }

        /// <summary>
        /// Genesis Hash
        /// </summary>
        public Hash Genesis { get; }

        /// <summary>
        /// Start Era
        /// </summary>
        public Hash StartEra { get; }

        /// <summary>
        /// Mortality
        /// </summary>
        public Era Mortality { get; }

        /// <summary>
        /// Nonce
        /// </summary>
        public CompactInteger Nonce { get; }

        /// <summary>
        /// Charge
        /// </summary>
        public ChargeType Charge { get; }

        /// <summary>
        /// CheckMetadataHash; only disabled supported for now.
        /// </summary>
        public TempCheckMetadataHash MetadataHashSignedExt { get; }

        public bool CheckMetadata { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedExtensions"/> class.
        /// </summary>
        /// <param name="specVersion">The spec version.</param>
        /// <param name="txVersion">The tx version.</param>
        /// <param name="genesis">The genesis.</param>
        /// <param name="startEra">The start era.</param>
        /// <param name="mortality">The mortality.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="charge">The charge transaction payment.</param>
        public TempSignedExtensions(uint specVersion, uint txVersion, Hash genesis, Hash startEra, Era mortality, CompactInteger nonce, ChargeType charge, bool checkMetadata)
        {
            SpecVersion = specVersion;
            TxVersion = txVersion;
            Genesis = genesis;
            StartEra = startEra;
            Mortality = mortality;
            Nonce = nonce;
            Charge = charge;
            MetadataHashSignedExt = new TempCheckMetadataHash();
            CheckMetadata = checkMetadata;
        }

        /// <summary>
        /// Gets the extra.
        /// </summary>
        /// <returns></returns>
        public byte[] GetExtra()
        {
            var bytes = new List<byte>();

            // CheckMortality
            bytes.AddRange(Mortality.Encode());

            // CheckNonce
            bytes.AddRange(Nonce.Encode());

            // ChargeType
            bytes.AddRange(Charge.Encode());

            if (CheckMetadata)
            {
                // CheckMetadataHash
                bytes.AddRange(MetadataHashSignedExt.EncodeExtra());
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// Gets the additional signed.
        /// </summary>
        /// <returns></returns>
        public byte[] GetAdditionalSigned()
        {
            var bytes = new List<byte>();

            // CheckSpecVersion
            bytes.AddRange(Utils.Value2Bytes(SpecVersion));

            // CheckTxVersion
            bytes.AddRange(Utils.Value2Bytes(TxVersion));

            // CheckGenesis
            bytes.AddRange(Genesis.Bytes);

            // CheckMortality, Additional Blockhash check. Immortal = genesis_hash, Mortal = logic
            bytes.AddRange(StartEra.Bytes);

            if (CheckMetadata)
            {
                // CheckMetadataHash
                bytes.AddRange(MetadataHashSignedExt.EncodeAdditional());
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        /// <returns></returns>
        public byte[] Encode()
        {
            var bytes = new List<byte>();

            // Extra: Era, Nonce & Tip
            bytes.AddRange(GetExtra());

            // Additional Signed: SpecVersion, TxVersion, Genesis, Blockhash
            bytes.AddRange(GetAdditionalSigned());

            return bytes.ToArray();
        }
    }
}