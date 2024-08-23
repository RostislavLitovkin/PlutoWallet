using PlutoWallet.Constants;
using PlutoWallet.Model;
using Substrate.NetApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoWalletTests
{
    internal class XcmTests
    {
        [Test]
        public void IsMethodXcm()
        {
            var transfer = new Substrate.NetApi.Model.Extrinsics.Method(Utils.HexToByteArray("0x89")[0], 1, Utils.HexToByteArray("0x0300010300a10f043205011f00fe4e580003010200a10f01006a4e76d530fa715a95388b889ad33c1665062c3dec9bf0aca3a9e4ff45781e4800"));

            var endpoint = Endpoints.GetEndpointDictionary[EndpointEnum.Hydration];

            Assert.That(!(XcmModel.IsMethodXcm(endpoint, transfer) is null));
        }
    }
}
