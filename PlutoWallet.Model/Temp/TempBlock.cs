using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;

namespace PlutoWallet.Model.Temp
{
    public class TempBlock
    {
        //
        // Summary:
        //     Extrinsics
        public TempExtrinsic[] Extrinsics { get; set; }

        //
        // Summary:
        //     Header
        public Header Header { get; set; }
    }
}
