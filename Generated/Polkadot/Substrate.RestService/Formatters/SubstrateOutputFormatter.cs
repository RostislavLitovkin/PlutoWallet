using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Substrate.RestService.Formatters
{
   /// <summary>
   /// >> SubstrateOutputFormatter
   /// The SubstrateOutputFormatter implements a custom formatter to easily encode any substrate type.
   /// Types are hex-encoded and uses the media type text/substrate.
   /// </summary>
   public class SubstrateOutputFormatter : TextOutputFormatter
   {
      /// <summary>
      /// Initializes the custom output formatter.
      /// </summary>
      public SubstrateOutputFormatter()
      {
         SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/substrate"));
         SupportedEncodings.Add(Encoding.UTF8);
         SupportedEncodings.Add(Encoding.Unicode);
      }

      /// <summary>
      /// Validates the given runtime type and checks whether it is assignable from BaseType class that is the base
      /// type of any substrate custom type.
      /// </summary>
      /// <param name="type">The given type to check against.</param>
      /// <returns>Returns true whether the requested type is formattable or not.</returns>
      protected override bool CanWriteType(Type type)
      {
         return typeof(IType).IsAssignableFrom(type);
      }

      /// <summary>
      /// Encodes and writes the given context object to the output stream.
      /// </summary>
      /// <param name="context">The given context.</param>
      /// <param name="selectedEncoding">The given encoding.</param>
      public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
      {
         HttpContext httpContext = context.HttpContext;
         var baseType = (IType)context.Object;

         if (baseType == null)
         {
            await NotFoundAsync(selectedEncoding, httpContext);
         }
         else
         {
            try
            {
               byte[] encoded = baseType.Encode();
               if (encoded == null)
               {
                  await NotFoundAsync(selectedEncoding, httpContext);
               }
               else
               {
                  await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { result = Utils.Bytes2HexString(encoded) }), selectedEncoding);
               }
            }
            catch (ArgumentNullException)
            {
               await NotFoundAsync(selectedEncoding, httpContext);
            }
         }
      }

      private static async Task NotFoundAsync(Encoding selectedEncoding, HttpContext httpContext)
      {
         httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
         await httpContext.Response.WriteAsync(string.Empty, selectedEncoding);
      }
   }
}
