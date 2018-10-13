using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monowallet.Core.Test
{
    public interface IMultisigComposer<T> where T : IComparable<T>
    {
        Task<IEnumerable<KeyBox>> GetKeyBoxesAsync(Action<OrderedToken<T>[]> singleResultAction = null);
    }
}
