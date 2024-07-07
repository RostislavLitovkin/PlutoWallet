using Substrate.NetApi;
using Substrate.NetApi.Model.Rpc;
using Substrate.ServiceLayer.Model;
using Substrate.ServiceLayer.Storage;
using Substrate.ServiceLayer.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Substrate.RestService.Controller
{
   [Controller]
   [Route("[controller]")]
   [SubstrateControllerIgnore]
   public class MockupController : ControllerBase
   {
      private readonly IStorageDataProvider _storageDataProvider;
      private readonly IWebHostEnvironment _webEnv;

      public MockupController(IStorageDataProvider storageDataProvider, IWebHostEnvironment env)
      {
         _storageDataProvider = storageDataProvider;
         _webEnv = env;
      }

      [HttpPost("data")]
      [Produces("application/json")]
      [ProducesResponseType(typeof(bool), 200)]
      public IActionResult HandleMockupData([FromBody] MockupRequest request)
      {
         if (!_webEnv.IsDevelopment())
         {
            // Do not make this available in production environments.
            return NotFound();
         }

         if (string.IsNullOrEmpty(request.Key))
         {
            return Ok(false);
         }

         if (request.Value == null)
         {
            return Ok(false);
         }

         try
         {
            string id = string.Empty;

            // We simulate a regular StorageChangeSet here so that processing
            // of the data work the same way as with a regular Substrate client.

            var changeSet = new StorageChangeSet()
            {
               Block = new Substrate.NetApi.Model.Types.Base.Hash() { },
               Changes = new string[1][]
            };

            string changeData = Utils.Bytes2HexString(request.Value);
            changeSet.Changes[0] = new string[]
            {
               request.Key,
               changeData
            };

            _storageDataProvider.BroadcastLocalStorageChange(id, changeSet);

            return Ok(true);
         }
         catch (Exception)
         {
            // Ignored for now.
            // TODO (svnscha) Will be logged as soon as we expose a configurable logging interface to the RestService template.
         }

         return Ok(false);
      }
   }
}
