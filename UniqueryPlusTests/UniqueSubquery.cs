using Newtonsoft.Json;
using UniqueryPlus;

namespace UniqueryPlusTests
{
    internal class UniqueSubquery
    {
        [Test]
        public async Task GetNftsOwnedByAsync()
        {
            var uniqueSubqueryClient = Indexers.GetUniqueSubqueryClient();
            var result = await uniqueSubqueryClient.GetNftsOwnedBy.ExecuteAsync("5DkMtAbCBBBkycT5Gowo2ddkkmf7nVV1TW62fVZeSDLqRtEc", 1, 0);

            if (
                result is null ||
                result.Errors.Count > 0 ||
                result.Data is null
            )
            {
                Console.WriteLine(JsonConvert.SerializeObject(result));

                Console.WriteLine(result?.Errors[0].Message);
                Console.WriteLine(result?.Errors[0].Exception?.Data);

                Console.WriteLine("Failed: " + result?.Errors.Count);
            }
            else
            {
                Console.WriteLine(result.Data.Tokens.Count);
            }
        }
    }
}
