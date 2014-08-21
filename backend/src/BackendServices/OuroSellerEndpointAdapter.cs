using System;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace BackendServices
{
    public class OuroSellerEndpointAdapter
    {
        private readonly IEventStoreConnection _connection;
        private readonly IOuroSellerEndpoint _endpoint;
        private readonly string _incomingStream;
        private readonly UserCredentials _credentials;
        private EventStoreSubscription _subscription;

        public OuroSellerEndpointAdapter(IEventStoreConnection connection, 
                                         IOuroSellerEndpoint endpoint, 
                                         string incomingStream, 
                                         UserCredentials credentials=null)
        {
            _connection = connection;
            _endpoint = endpoint;
            _incomingStream = incomingStream;
            _credentials = credentials;
        }

        public void StartListening()
        {
            _subscription = _connection.SubscribeToStreamAsync(_incomingStream, 
                                                               false, 
                                                               eventAppeared,
                                                               subscriptionDropped, 
                                                               _credentials).Result;
        }

        private void subscriptionDropped(EventStoreSubscription arg1, SubscriptionDropReason arg2, Exception arg3)
        {
            throw new NotImplementedException();
        }

        private void eventAppeared(EventStoreSubscription arg1, ResolvedEvent arg2)
        {
            throw new System.NotImplementedException();
        }
    }
}