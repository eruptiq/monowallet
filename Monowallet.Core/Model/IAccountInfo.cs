using System;

namespace Monowallet.Core.Model
{
    public interface IAccountInfo<TKey> where TKey : IComparable<TKey>
    {
        TKey GetUniquePublicKey();
    }
}