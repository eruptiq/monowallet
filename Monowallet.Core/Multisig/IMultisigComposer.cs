using Monowallet.Core.Model;
using Monowallet.Core.Model.Multisig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monowallet.Core.Multisig
{
    public interface IMultisigComposer<TKey> where TKey : IComparable<TKey>
    {
        Task<IEnumerable<KeyBox<TKey>>> GetKeyBoxesAsync(Action<TKey[]> singleResultAction = null);
    }
}
