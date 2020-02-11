using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KubeMQ.SDK.csharp.Events;
using KubeMQ.SDK.csharp.Subscription;
using Newtonsoft.Json;
using SpeedTestLogger.Models;

namespace SpeedTestLogger
{
  public class KubeMQClient
  {
    private readonly string _clientId;
    private readonly string _channel;
    private readonly string _address;

    public KubeMQClient(string address, int loggerId, string channel)
    {
      _clientId = $"speedtest-logger-{loggerId}";
      _channel = channel;
      _address = address;
    }

    public void SubscribeToEvents(Subscriber.HandleEventDelegate handler)
    {
      var subscriber = new Subscriber(_address);
      SubscribeRequest subscribeRequest = CreateSubscribeRequest();
      Subscriber.HandleEventErrorDelegate errorHandler = (errorHandler) => Console.WriteLine(errorHandler.Message);
      subscriber.SubscribeToEvents(subscribeRequest, handler, errorHandler);
      Console.WriteLine($"Subscribing to channel: {_channel} at KubeMQ({_address})");
    }

    protected SubscribeRequest CreateSubscribeRequest()
    {
      SubscribeRequest subscribeRequest = new SubscribeRequest()
      {
        Channel = _channel,
        ClientID = _clientId,
        EventsStoreType = EventsStoreType.Undefined,
        EventsStoreTypeValue = 0,
        Group = "",
        SubscribeType = SubscribeType.Events
      };
      return subscribeRequest;
    }
  }
}
