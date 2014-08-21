using Messages;

namespace BackendServices
{
    public interface IOuroSellerEndpoint
    {
        Event GetQuoteFor(SearchRequested request);
    }
}