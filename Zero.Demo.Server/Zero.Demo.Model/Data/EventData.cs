using System.Runtime.InteropServices;

namespace Zero.Demo.Model.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public struct EventData
    {
        [FieldOffset(0)]
        private readonly byte _eventType;

        public EventData(EventType eventType)
        {
            _eventType = (byte)eventType;
        }

        public EventType EventType => (EventType)_eventType;
    }
}
