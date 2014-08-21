using System;
using Messages;

namespace BackendServices
{
    public class DecliningOuroSellerEndpoint : IOuroSellerEndpoint
    {
        private readonly string _companyName;
        private readonly Uri _imageUri;

        public DecliningOuroSellerEndpoint(string companyName, Uri imageUri)
        {
            _companyName = companyName;
            _imageUri = imageUri;
        }

        public Event GetQuoteFor(SearchRequested request)
        {
            return new OuroQuoteDeclined()
            {
                DeliveryPostCode = request.DeliveryPostCode,
                DesiredDeliveryDate = request.DesiredDeliveryDate,
                HistoricInterestRequired = request.HistoricInterestRequired,
                Vendor = _companyName,
                VendorImageUrl = _imageUri.ToString(),
                Ownership = request.Ownership,
                NumberOfWings = request.NumberOfWings,
                PreviouslyOwnedOuros = request.PreviouslyOwnedOuros,
                SearchType = request.SearchType
            };
        }
    }
}