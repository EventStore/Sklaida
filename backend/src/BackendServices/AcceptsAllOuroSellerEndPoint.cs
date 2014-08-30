using System;
using Messages;

namespace BackendServices
{
    public class AcceptsAllOuroSellerEndPoint : IOuroSellerEndpoint
    {
        private readonly string _companyName;
        private readonly Uri _imageUri;

        public AcceptsAllOuroSellerEndPoint(string companyName, Uri imageUri)
        {
            _companyName = companyName;
            _imageUri = imageUri;
        }

        public Event GetQuoteFor(SearchRequested request)
        {
            return new OuroResultFound()
            {
                DeliveryPostCode = request.DeliveryPostCode,
                DesiredDeliveryDate = request.DesiredDeliveryDate,
                HistoricInterestRequired = request.HistoricInterestRequired,
                Vendor = _companyName,
                VendorImageUrl = _imageUri.ToString(),
                ImageUrl = _imageUri.ToString(),
                NumberOfWings = request.NumberOfWings,
                Ownership = request.Ownership,
                PreviouslyOwnedOuros = request.PreviouslyOwnedOuros,
                Price = new Random().Next(0, 1000),
                SearchType = request.SearchType,
                ValueOfPreviousOuros = request.ValueOfPreviousOuros,
                ClickThroughUrl = new Uri("http://google.com?q=where%20is%20ouro?").ToString()
            };
        }
    }
}