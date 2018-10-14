using System;
using System.Security;

namespace Monowallet.Core.Model.Multisig
{
    public class KeyBox<TKey> where TKey : IComparable<TKey>
    {
        public OrderedTokenSet<TKey> CombinationKey { get; set; }

        public KeyBox()
        {
        }

        public override string ToString()
        {
            return CombinationKey.ToString();
        }
    }
}
