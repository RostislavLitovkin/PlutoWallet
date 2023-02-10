using System;
using Ajuna.NetApi.Attributes;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Metadata.V14;

namespace PlutoWallet.Types.AjunaExtTypes
{


    /// <summary>
    /// >> 5 - Composite[pallet_balances.AccountData]
    /// </summary>
    [AjunaNodeType(TypeDefEnum.Composite)]
    public sealed class AccountData : BaseType
    {

        /// <summary>
        /// >> free
        /// </summary>
        private Ajuna.NetApi.Model.Types.Primitive.U128 _free;

        /// <summary>
        /// >> reserved
        /// </summary>
        private Ajuna.NetApi.Model.Types.Primitive.U128 _reserved;

        /// <summary>
        /// >> misc_frozen
        /// </summary>
        private Ajuna.NetApi.Model.Types.Primitive.U128 _miscFrozen;

        /// <summary>
        /// >> fee_frozen
        /// </summary>
        private Ajuna.NetApi.Model.Types.Primitive.U128 _feeFrozen;

        public Ajuna.NetApi.Model.Types.Primitive.U128 Free
        {
            get
            {
                return this._free;
            }
            set
            {
                this._free = value;
            }
        }

        public Ajuna.NetApi.Model.Types.Primitive.U128 Reserved
        {
            get
            {
                return this._reserved;
            }
            set
            {
                this._reserved = value;
            }
        }

        public Ajuna.NetApi.Model.Types.Primitive.U128 MiscFrozen
        {
            get
            {
                return this._miscFrozen;
            }
            set
            {
                this._miscFrozen = value;
            }
        }

        public Ajuna.NetApi.Model.Types.Primitive.U128 FeeFrozen
        {
            get
            {
                return this._feeFrozen;
            }
            set
            {
                this._feeFrozen = value;
            }
        }

        public override string TypeName()
        {
            return "AccountData";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Free.Encode());
            result.AddRange(Reserved.Encode());
            result.AddRange(MiscFrozen.Encode());
            result.AddRange(FeeFrozen.Encode());
            return result.ToArray();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Free = new Ajuna.NetApi.Model.Types.Primitive.U128();
            Free.Decode(byteArray, ref p);
            Reserved = new Ajuna.NetApi.Model.Types.Primitive.U128();
            Reserved.Decode(byteArray, ref p);
            MiscFrozen = new Ajuna.NetApi.Model.Types.Primitive.U128();
            MiscFrozen.Decode(byteArray, ref p);
            FeeFrozen = new Ajuna.NetApi.Model.Types.Primitive.U128();
            FeeFrozen.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}

