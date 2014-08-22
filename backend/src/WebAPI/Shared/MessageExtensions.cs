using System;
using Messages;
using WebAPI.Search;

namespace WebAPI.Shared
{
    internal static class MessageExtensions
    {
        private static Messages.OuroSearchType ToMessageType(this Search.OuroSearchType self)
        {
            switch (self)
            {
                case Search.OuroSearchType.Template:
                    return Messages.OuroSearchType.Template;
                case Search.OuroSearchType.SelfAssembly:
                    return Messages.OuroSearchType.SelfAssembly;
                case Search.OuroSearchType.Complete:
                    return Messages.OuroSearchType.Complete;
                default:
                    throw new ArgumentOutOfRangeException("self");
            }
        }

        private static Messages.OwnLeaseOption ToMessageType(this Search.OwnLeaseOption self)
        {
            switch (self)
            {
                case Search.OwnLeaseOption.Own:
                    return Messages.OwnLeaseOption.Own;
                case Search.OwnLeaseOption.Lease:
                    return Messages.OwnLeaseOption.Lease;
                default:
                    throw new ArgumentOutOfRangeException("self");
            }
        }

        public static SearchRequested ToEvent(this OuroSearchRequest self, string clientResponseStream)
        {
            return new SearchRequested(clientResponseStream, self.SearchType.ToMessageType(), self.DesiredDeliveryDate, self.DeliveryPostCode,
                self.Ownership.ToMessageType(), self.NumberOfWings, self.WaterproofRequired, self.HistoricInterestRequired,
                self.PreviouslyOwnedOuros, self.ValueOfPreviousOuros);
        }
    }
}