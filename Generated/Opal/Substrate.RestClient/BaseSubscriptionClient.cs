using Substrate.ServiceLayer.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Substrate.RestClient
{
   public delegate void OnStorageChange(StorageChangeMessage message);

   public class BaseSubscriptionClient
   {
      private readonly ClientWebSocket _ws;

      public OnStorageChange OnStorageChange { get; set; }

      public BaseSubscriptionClient(ClientWebSocket websocket)
      {
         _ws = websocket;
      }

      public BaseSubscriptionClient()
      {
         _ws = new ClientWebSocket();
      }

      public void Abort() => _ws.Abort();
      public Task ConnectAsync(Uri uri, CancellationToken cancellationToken) => _ws.ConnectAsync(uri, cancellationToken);
      public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken) => _ws.CloseAsync(closeStatus, statusDescription, cancellationToken);
      public async Task<bool> ReceiveNextAsync(CancellationToken cancellationToken)
      {
         // Get the next message
         string nextMessage = await ReceiveMessageAsync(cancellationToken);
         if (string.IsNullOrEmpty(nextMessage))
         {
            // Something went wrong.
            Abort();
            return false;
         }

         // Parse the next message
         StorageSubscriptionMessage message = JsonConvert.DeserializeObject<StorageSubscriptionMessage>(nextMessage);
         if (message == null)
         {
            // Something went wrong.
            Abort();
            return false;
         }

         if (message.Type == StorageSubscriptionMessageType.StorageChangeMessage)
         {
            // We have just received a storage subscription change.
            // This happens if we register for multiple subscription changes and while we are still registering
            // something just arrived us.
            StorageChangeMessage change = JsonConvert.DeserializeObject<StorageChangeMessage>(message.Payload);
            if (change == null)
            {
               // Something went wrong.
               Abort();
               return false;
            }

            ProcessSubscriptionChange(change);
            return true;
         }

         // Something went wrong.
         // Not supported.
         Abort();
         return false;
      }

      public async Task<bool> SubscribeAsync(string id)
      {
         return await SubscribeAsync(new StorageSubscribeMessage()
         {
            Identifier = id,
            Key = string.Empty
         });
      }

      public async Task<bool> SubscribeAsync(string id, string key)
      {
         return await SubscribeAsync(new StorageSubscribeMessage()
         {
            Identifier = id,
            Key = key
         });
      }

      public async Task<bool> SubscribeAsync(StorageSubscribeMessage request) => await SubscribeAsync(request, CancellationToken.None);
      public async Task<bool> SubscribeAsync(StorageSubscribeMessage request, CancellationToken cancellationToken)
      {
         byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
         await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);

         // Receive messages until we get the confirmation.
         // We might receive other messages, too.
         do
         {
            // Get the next message
            string nextMessage = await ReceiveMessageAsync(cancellationToken);
            if (string.IsNullOrEmpty(nextMessage))
            {
               // Something went wrong.
               Abort();
               return false;
            }

            // Parse the next message
            StorageSubscriptionMessage message = JsonConvert.DeserializeObject<StorageSubscriptionMessage>(nextMessage);
            if (message == null)
            {
               // Something went wrong.
               Abort();
               return false;
            }

            if (message.Type == StorageSubscriptionMessageType.StorageSubscribeMessageResult)
            {
               // This is the subscription result.
               // Parse the confirmation...
               var subscriptionResponseResult = JsonConvert.DeserializeObject<StorageSubscribeMessageResult>(message.Payload);
               if (subscriptionResponseResult == null)
               {
                  return false;
               }

               // Ensure response status is expected
               return subscriptionResponseResult.Status == (int)HttpStatusCode.OK;

            }
            else if (message.Type == StorageSubscriptionMessageType.StorageChangeMessage)
            {
               // We have just received a storage subscription change.
               // This happens if we register for multiple subscription changes and while we are still registering
               // something just arrived us.
               StorageChangeMessage change = JsonConvert.DeserializeObject<StorageChangeMessage>(message.Payload);
               if (change == null)
               {
                  // Something went wrong.
                  Abort();
                  return false;
               }

               ProcessSubscriptionChange(change);
            }
            else
            {
               // Something went wrong.
               // Not supported.
               Abort();
               return false;
            }

         } while (true);

      }

      private async Task<string> ReceiveMessageAsync(CancellationToken cancellationToken)
      {
         byte[] buffer = new byte[ushort.MaxValue];
         WebSocketReceiveResult result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

         if (result.MessageType == WebSocketMessageType.Text && result.EndOfMessage)
         {
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
         }

         await _ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
         return null;
      }

      protected virtual void ProcessSubscriptionChange(StorageChangeMessage message)
      {
         OnStorageChange?.Invoke(message);
      }
   }
}
