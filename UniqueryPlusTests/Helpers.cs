using Mythos.NetApi.Generated.Model.runtime_common;
using NUnit.Framework;
using Substrate.NetApi;
using System.Numerics;
using UniqueryPlus;

namespace UniqueryPlusTests
{
    public class HelpersTests
    {
        [Test]
        public void GetMythosU256FromBigInteger()
        {
            var x = Helpers.GetMythosU256FromBigInteger(1);


            var testedValue = new IncrementableU256
            {
                Value = x
            };

            Assert.That(Utils.Bytes2HexString(testedValue.Encode()), Is.EqualTo("0x0100000000000000000000000000000000000000000000000000000000000000"));
        }

        [Test]
        public void GetBigIntegerFromMythosU256()
        {
            var testedValue = new IncrementableU256();

            int p = 0;
            testedValue.Decode(Utils.HexToByteArray("0x0100000000000000000000000000000000000000000000000000000000000000"), ref p);

            var expectedValue = Helpers.GetBigIntegerFromMythosU256(testedValue.Value);


            Assert.That(expectedValue, Is.EqualTo(new BigInteger(1)));

        }
    }
}
