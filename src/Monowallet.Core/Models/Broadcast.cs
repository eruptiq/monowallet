namespace Monowallet.Core.Models
{
    public struct Broadcast
    {
        public Broadcast(string address, string data, bool fromSelf)
        {
            Address = address;
            Data = data;
            FromSelf = fromSelf;
        }

        public string Address { get; }
        public string Data { get; }
        public bool FromSelf { get; }
    }
}
