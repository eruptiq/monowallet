namespace Monowallet.Core.Models
{
    public class Node
    {
        public string Address { get; set; }
        public bool IsSelf { get; set; }
        public bool Broken { get; set; }
    }
}
