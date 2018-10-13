using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monowallet.Core.Test
{
    public class MultisigComposer : IMultisigComposer<char>
    {
        private readonly IReadOnlyList<IAccountInfo> accountInfos;

        public MultisigComposer(IReadOnlyList<IAccountInfo> accountInfos, int m, int n)
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

        public Task<IEnumerable<KeyBox>> GetKeyBoxesAsync(Action<OrderedToken<char>[]> singleResultAction = null)
        {
            return Task.Run(() =>
            {
                var combinationResult = new Dictionary<OrderedToken<char>, IList<IList<OrderedToken<char>>>>();
                var p = accountInfos.Select(k => new OrderedToken<char>(k.GetUniquePublickKey().OrderBy(c => c))).ToArray();
                combinationResult = GetCombinations(p, singleResultAction);

                var orderedKeyTokens = combinationResult.Keys.OrderBy(k => k.ToString());

                var result = orderedKeyTokens.SelectMany(
                    k => combinationResult[k]).Distinct()
                        .OrderBy(l => l.First())
                        .Select(l =>
                            new KeyBox
                            {
                                Key = new OrderedToken<char>(string.Concat(l.Select(t => t.ToString())).OrderBy(c => c))
                            });

                return result;
            });
        }

        private Dictionary<OrderedToken<T>, IList<IList<OrderedToken<T>>>> GetCombinations<T>(
            OrderedToken<T>[] allKeys, Action<OrderedToken<T>[]> singleResultAction = null)
            where T : IComparable<T>
        {
            var result = new Dictionary<OrderedToken<T>, IList<IList<OrderedToken<T>>>>();
            Action<OrderedToken<T>[]> action = (r) =>
            {
                if (!result.TryGetValue(r[0], out IList<IList<OrderedToken<T>>> combinations))
                {
                    combinations = new List<IList<OrderedToken<T>>>();
                    result.Add(r[0], combinations);
                }
                combinations.Add(new List<OrderedToken<T>>(r));
                singleResultAction?.Invoke(r);
            };

            Util.Combination(allKeys, N, M, action);
            return result;
        }
    }
}
