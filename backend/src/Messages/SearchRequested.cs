using System;

namespace Messages
{
    public class SearchRequested : Event
    {
        public string ClientResponseStream { get; private set; }

        public OuroSearchType SearchType { get; private set; }
        public DateTime DesiredDeliveryDate { get; private set; }
        public string DeliveryPostCode { get; private set; }

        public OwnLeaseOption Ownership { get; private set; }
        public int NumberOfWings { get; private set; }

        public bool WaterproofRequired { get; private set; }
        public bool HistoricInterestRequired { get; private set; }

        public bool PreviouslyOwnedOuros { get; private set; }
        public decimal? ValueOfPreviousOuros { get; private set; }
        
        public SearchRequested(string clientResponseStream, OuroSearchType searchType, DateTime desiredDeliveryDate, string deliveryPostCode, OwnLeaseOption ownership, int numberOfWings, bool waterproofRequired, bool historicInterestRequired, bool previouslyOwnedOuros, decimal? valueOfPreviousOuros)
        {
	    ClientResponseStream = clientResponseStream;
            SearchType = searchType;
            DesiredDeliveryDate = desiredDeliveryDate;
            DeliveryPostCode = deliveryPostCode;
            Ownership = ownership;
            NumberOfWings = numberOfWings;
            WaterproofRequired = waterproofRequired;
            HistoricInterestRequired = historicInterestRequired;
            PreviouslyOwnedOuros = previouslyOwnedOuros;
            ValueOfPreviousOuros = valueOfPreviousOuros;
        }

        public override string ToString()
        {
            return string.Format("ClientResponseStream: {9}, SearchType: {0}, DesiredDeliveryDate: {1}, DeliveryPostCode: {2}, Ownership: {3}, NumberOfWings: {4}, WaterproofRequired: {5}, HistoricInterestRequired: {6}, PreviouslyOwnedOuros: {7}, ValueOfPreviousOuros: {8}",
                    SearchType, DesiredDeliveryDate, DeliveryPostCode, Ownership, NumberOfWings, WaterproofRequired,
                    HistoricInterestRequired, PreviouslyOwnedOuros, ValueOfPreviousOuros, ClientResponseStream);
        }
    }
}
