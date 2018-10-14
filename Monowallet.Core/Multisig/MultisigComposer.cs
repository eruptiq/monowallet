using Monowallet.Core.Model;
using Monowallet.Core.Model.Multisig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monowallet.Core.Multisig
{
    public class MultisigComposer<TKey> : IMultisigComposer<TKey> where TKey : IComparable<TKey>
    {
        private readonly IReadOnlyList<IAccountInfo<TKey>> accountInfos;

        public MultisigComposer(IReadOnlyList<IAccountInfo<TKey>> accountInfos, int m, int n)
        {
            if (m < 2 || m > n - 1)
                throw new ArgumentException($"{nameof(M)} must be greater than 1 and less then {nameof(N)}", nameof(M));

            if (n < 3)
                throw new ArgumentException($"{nameof(N)} must be greater than 3", nameof(N));

            if (accountInfos.Count < n)
            {
                throw new ArgumentException($"accouts count: {accountInfos.Count} is less than N: {n} ", nameof(accountInfos));
            }

            this.accountInfos = accountInfos;

            M = m;
            N = n;
        }

        public int M { get; }
        public int N { get; }

        public Task<IEnumerable<KeyBox<TKey>>> GetKeyBoxesAsync(Action<TKey[]> singleResultAction = null)
        {
            return Task.Run(() =>
            {
                var combinationResult = new Dictionary<TKey, IList<OrderedTokenSet<TKey>>>();
                combinationResult = GetCombinations(accountInfos.Select(a => a.GetUniquePublicKey()).ToArray(), singleResultAction);

                var orderedKeyTokens = combinationResult.Keys.OrderBy(k => k.ToString());

                var result = orderedKeyTokens.SelectMany(
                    k => combinationResult[k]).Distinct()
                        .OrderBy(l => l.First())
                        .Select(l => new KeyBox<TKey> { CombinationKey = new OrderedTokenSet<TKey>(l) });

                return result;
            });
        }

        private Dictionary<T, IList<OrderedTokenSet<T>>> GetCombinations<T>(
            T[] allKeys, Action<T[]> singleResultAction = null)
            where T : IComparable<T>
        {
            var result = new Dictionary<T, IList<OrderedTokenSet<T>>>();
            Action<T[]> action = (r) =>
            {
                var ordered = r.OrderBy(t => t);
                var first = ordered.First();
                if (!result.TryGetValue(first, out IList<OrderedTokenSet<T>> combinations))
                {
                    combinations = new List<OrderedTokenSet<T>>();
                    result.Add(first, combinations);
                }
                combinations.Add(new OrderedTokenSet<T>(ordered));
                singleResultAction?.Invoke(r);
            };

            Util.Combination(allKeys, N, M, action);
            return result;
        }
    }
}
