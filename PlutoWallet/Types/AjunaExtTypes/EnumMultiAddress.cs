using System;
using Ajuna.NetApi.Model.Types.Base;

namespace PlutoWallet.Types.AjunaExtTypes
{

    public enum MultiAddress
    {

        Id = 0,

        Index = 1,

        Raw = 2,

        Address32 = 3,

        Address20 = 4,
    }

    /// <summary>
    /// >> 106 - Variant[sp_runtime.multiaddress.MultiAddress]
    /// </summary>
    public sealed class EnumMultiAddress : BaseEnumExt<MultiAddress, AccountId32, Ajuna.NetApi.Model.Types.Base.BaseCom<Ajuna.NetApi.Model.Types.Base.BaseTuple>, Ajuna.NetApi.Model.Types.Base.BaseVec<Ajuna.NetApi.Model.Types.Primitive.U8>, AjunaExample.NetApiExt.Generated.Types.Base.Arr32U8, AjunaExample.NetApiExt.Generated.Types.Base.Arr20U8>
    {
    }
}

