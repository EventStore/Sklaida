using System;
using Messages;
using System.Threading.Tasks;
using System.Threading;

namespace BackendServices
{
    public class AcceptsAllOuroSellerEndPoint : IOuroSellerEndpoint
    {
        private readonly string _companyName;
        private readonly Uri _imageUri;
        private readonly int _timeToRespond;

        public AcceptsAllOuroSellerEndPoint(string companyName, Uri imageUri, int timeToRespond)
        {
            _companyName = companyName;
            _imageUri = imageUri;
            _timeToRespond = timeToRespond;
        }

        public Event GetQuoteFor(SearchRequested request)
        {
            //Delay the results all showing up immediately in the results stream.
            return Task.Delay(_timeToRespond)
                       .ContinueWith<OuroResultFound>(t =>
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
            }).Result;
        }
    }
}