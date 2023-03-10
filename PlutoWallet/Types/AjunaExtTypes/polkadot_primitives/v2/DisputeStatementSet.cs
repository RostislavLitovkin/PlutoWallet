//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ajuna.NetApi.Attributes;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Metadata.V14;
using System.Collections.Generic;


namespace PlutoWallet.NetApiExt.Generated.Model.polkadot_primitives.v2
{
    
    
    /// <summary>
    /// >> 398 - Composite[polkadot_primitives.v2.DisputeStatementSet]
    /// </summary>
    [AjunaNodeType(TypeDefEnum.Composite)]
    public sealed class DisputeStatementSet : BaseType
    {
        
        /// <summary>
        /// >> candidate_hash
        /// </summary>
        private PlutoWallet.NetApiExt.Generated.Model.polkadot_core_primitives.CandidateHash _candidateHash;
        
        /// <summary>
        /// >> session
        /// </summary>
        private Ajuna.NetApi.Model.Types.Primitive.U32 _session;
        
        /// <summary>
        /// >> statements
        /// </summary>
        private Ajuna.NetApi.Model.Types.Base.BaseVec<Ajuna.NetApi.Model.Types.Base.BaseTuple<PlutoWallet.NetApiExt.Generated.Model.polkadot_primitives.v2.EnumDisputeStatement, PlutoWallet.NetApiExt.Generated.Model.polkadot_primitives.v2.ValidatorIndex, PlutoWallet.NetApiExt.Generated.Model.polkadot_primitives.v2.validator_app.Signature>> _statements;
        
        public PlutoWallet.NetApiExt.Generated.Model.polkadot_core_primitives.CandidateHash CandidateHash
        {
            get
            {
                return this._candidateHash;
            }
            set
            {
                this._candidateHash = value;
            }
        }
        
        public Ajuna.NetApi.Model.Types.Primitive.U32 Session
        {
            get
            {
                return this._session;
            }
            set
            {
                this._session = value;
            }
        }
        
        public Ajuna.NetApi.Model.Types.Base.BaseVec<Ajuna.NetApi.Model.Types.Base.BaseTuple<PlutoWallet.NetApiExt.Generated.Model.polkadot_primitives.v2.EnumDisputeStatement, PlutoWallet.NetApiExt.Generated.Model.polkadot_primitives.v2.ValidatorIndex, PlutoWallet.NetApiExt.Generated.Model.polkadot_primitives.v2.validator_app.Signature>> Statements
        {
            get
            {
                return this._statements;
            }
            set
            {
                this._statements = value;
            }
        }
        
        public override string TypeName()
        {
            return "DisputeStatementSet";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(CandidateHash.Encode());
            result.AddRange(Session.Encode());
            result.AddRange(Statements.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            CandidateHash = new PlutoWallet.NetApiExt.Generated.Model.polkadot_core_primitives.CandidateHash();
            CandidateHash.Decode(byteArray, ref p);
            Session = new Ajuna.NetApi.Model.Types.Primitive.U32();
            Session.Decode(byteArray, ref p);
            Statements = new Ajuna.NetApi.Model.Types.Base.BaseVec<Ajuna.NetApi.Model.Types.Base.BaseTuple<PlutoWallet.NetApiExt.Generated.Model.polkadot_primitives.v2.EnumDisputeStatement, PlutoWallet.NetApiExt.Generated.Model.polkadot_primitives.v2.ValidatorIndex, PlutoWallet.NetApiExt.Generated.Model.polkadot_primitives.v2.validator_app.Signature>>();
            Statements.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
