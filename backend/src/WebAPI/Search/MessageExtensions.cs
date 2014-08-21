using System;
using Messages;

namespace WebAPI.Search
{
    internal static class MessageExtensions
    {
        private static Messages.OuroSearchType ToMessageType(this OuroSearchType self)
        {
            switch (self)
            {
                case OuroSearchType.Template:
                    return Messages.OuroSearchType.Template;
                case OuroSearchType.SelfAssembly:
                    return Messages.OuroSearchType.SelfAssembly;
                case OuroSearchType.Complete:
                    return Messages.OuroSearchType.Complete;
                default:
                    throw new ArgumentOutOfRangeException("self");
            }
        }

        private static Messages.OwnLeaseOption ToMessageType(this OwnLeaseOption self)
        {
            switch (self)
            {
                case OwnLeaseOption.Own:
                    return Messages.OwnLeaseOption.Own;
                case OwnLeaseOption.Lease:
                    return Messages.OwnLeaseOption.Lease;
                default:
                    throw new ArgumentOutOfRangeException("self");
            }
        }

        public static SearchRequested ToEvent(this OuroSearchRequest self)
        {
            return new SearchRequested(self.SearchType.ToMessageType(), self.DesiredDeliveryDate, self.DeliveryPostCode,
                self.Ownership.ToMessageType(), self.NumberOfWings, self.WaterproofRequired, self.HistoricInterestRequired,
                self.PreviouslyOwnedOuros, self.ValueOfPreviousOuros);
        }
    }
}