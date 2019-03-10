namespace Monowallet.Core.Models
{
    public class Node
    {
        public string Address { get; set; }
        public byte[] AddressBytes { get; set; }
        public bool IsSelf { get; set; }
    }
}
