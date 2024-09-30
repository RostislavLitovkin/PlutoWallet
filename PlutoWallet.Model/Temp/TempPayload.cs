using Substrate.NetApi.Model.Types;
using System.Linq;

namespace Substrate.NetApi.Model.Extrinsics
{
    /// <summary>
    /// Payload
    /// </summary>
    public class TempPayload : IEncodable
    {
        /// <summary>
        /// The call
        /// </summary>
        public Method Call { get; }

        /// <summary>
        /// Signed extension
        /// </summary>
        public TempSignedExtensions SignedExtension { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Payload"/> class.
        /// </summary>
        /// <param name="call">The call.</param>
        /// <param name="signedExtensions">The signed extensions.</param>
        public TempPayload(Method call, TempSignedExtensions signedExtensions)
        {
            Call = call;
            SignedExtension = signedExtensions;
        }

        /// <summary>
        /// Encodes this instance, returns the encoded bytes. Additionally, if
        /// the encoded bytes are longer than 256 bytes, they are hashed using `blake2_256`.
        /// </summary>
        /// <returns></returns>
        public byte[] Encode()
        {
            byte[] bytes = Call.Encode().Concat(SignedExtension.Encode()).ToArray();

            // Payloads longer than 256 bytes are going to be `blake2_256`-hashed.
            if (bytes.Length > 256)
            {
                bytes = HashExtension.Blake2(bytes, 256);
            }

            return bytes;
        }
    }
}