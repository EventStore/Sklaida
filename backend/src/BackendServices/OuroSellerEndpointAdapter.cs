using System;
using System.Text;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Messages;

namespace BackendServices
{
    public class OuroSellerEndpointAdapter
    {
        private readonly IEventStoreConnection _connection;
        private readonly string _adapterName;
        private readonly IOuroSellerEndpoint _endpoint;
        private readonly string _incomingStream;
        private readonly UserCredentials _credentials;
        private EventStoreSubscription _subscription;

        public OuroSellerEndpointAdapter(IEventStoreConnection connection, 
                                         string adapterName,
                                         IOuroSellerEndpoint endpoint, 
                                         string incomingStream, 
                                         UserCredentials credentials=null)
        {
            _connection = connection;
            _adapterName = adapterName;
            _endpoint = endpoint;
            _incomingStream = incomingStream;
            _credentials = credentials;
        }

        public void StartListening()
        {
            _subscription = _connection.SubscribeToStreamAsync(_incomingStream, 
                                                               false, 
                                                               EventAppeared,
                                                               SubscriptionDropped, 
                                                               _credentials).Result;
        }

        public void Stop()
        {
            _subscription.Close();
            _subscription.Dispose();
        }

        private void SubscriptionDropped(EventStoreSubscription sub, SubscriptionDropReason reason, Exception exception)
        {
            Log.Debug(_adapterName + " lost subscription due to " + reason);
        }

        private void EventAppeared(EventStoreSubscription sub, ResolvedEvent e)
        {
            Log.Debug(_adapterName + " received " + Encoding.UTF8.GetString(e.Event.Data));
            var request = Json.From<SearchRequested>(Encoding.UTF8.GetString(e.Event.Data));
            try
            {
                var response = _endpoint.GetQuoteFor(request);
                Publish(request.ClientResponseStream, response);
            }
            catch (Exception ex)
            {
                Log.Exception(_adapterName + " endpoint caused exception", ex);
            }
        }

        private void Publish(string clientResponseStream, Event response)
        {
            var json = Json.To(response);
            var ev = new EventData(Guid.NewGuid(), 
                                   response.GetType().Name, 
                                   true, Encoding.UTF8.GetBytes(json),
                                   new byte[0]);
            _connection.AppendToStreamAsync(clientResponseStream, ExpectedVersion.Any, new[] {ev}, _credentials);
        }
    }
}