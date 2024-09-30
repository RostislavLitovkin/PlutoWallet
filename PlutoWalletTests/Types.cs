using System;
using Newtonsoft.Json;

namespace PlutoWalletTests
{
    public class Properties
    {
        public int? SS58Prefix { get; set; }

        public int Ss58Format { get; set; }

        public int TokenDecimals { get; set; }

        public string TokenSymbol { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject((object)this);
        }
    }
}

