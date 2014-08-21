using System;

namespace WebAPI.Search
{
    public class OuroSearchRequest
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

        public override string ToString()
        {
            return string.Format("[SearchType: {0}, DesiredDeliveryDate: {1}, DeliveryPostCode: {2}, Ownership: {3}, NumberOfWings: {4}, WaterproofRequired: {5}, HistoricInterestRequired: {6}, PreviouslyOwnedOuros: {7}, ValueOfPreviousOuros: {8}]", SearchType, DesiredDeliveryDate, DeliveryPostCode, Ownership, NumberOfWings, WaterproofRequired, HistoricInterestRequired, PreviouslyOwnedOuros, ValueOfPreviousOuros);
        }
    }
}