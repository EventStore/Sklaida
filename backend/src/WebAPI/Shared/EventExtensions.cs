using System;
using System.Text;
using EventStore.ClientAPI;
using Messages;

namespace WebAPI.Shared
{
    internal static class EventExtensions
    {
        public static EventData ToEventData(this Event self, string eventType)
        {
            return new EventData(Guid.NewGuid(), eventType, true, Encoding.UTF8.GetBytes(Json.To(self)), new byte[0]);
        }
    }
}