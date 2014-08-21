using System;

namespace Messages
{
    public class OuroResultFound : Event
    {
        public OuroSearchType SearchType { get; set; }
        public DateTime DesiredDeliveryDate { get; set; }
        public string DeliveryPostCode { get; set; }

        public OwnLeaseOption Ownership { get; set; }
        public int NumberOfWings { get; set; }

        public bool WaterproofRequired { get; set; }
        public bool HistoricInterestRequired { get; set; }

        public bool PreviouslyOwnedOuros { get; set; }
        public decimal? ValueOfPreviousOuros { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Vendor { get; set; }
        public string VendorImageUrl { get; set; }

        public override string ToString()
        {
            return string.Format("SearchType: {0}, Price: {1}, Vendor: {2}", SearchType, Price, Vendor);
        }
    }
}