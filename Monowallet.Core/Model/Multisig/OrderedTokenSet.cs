using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Monowallet.Core.Model.Multisig
{
    public class OrderedTokenSet<T> : IOrderedEnumerable<T>, IEquatable<OrderedTokenSet<T>>, IComparable<OrderedTokenSet<T>> where T : IComparable<T>
    {
        public OrderedTokenSet(IOrderedEnumerable<T> tokens)
        {
            Container = ImmutableSortedSet.CreateRange(tokens.Distinct());
        }

        protected ImmutableSortedSet<T> Container { get; private set; }

        public override string ToString()
        {
            return string.Concat(Container.SelectMany(t => t.ToString()));
        }

        public override bool Equals(object obj)
        {
            if (obj is OrderedTokenSet<T>)
            {
                return Equals((OrderedTokenSet<T>)obj);
            }

            return base.Equals(obj);
        }

        public static bool operator ==(OrderedTokenSet<T> first, OrderedTokenSet<T> second)
        {
            if ((object)first == null)
            {
                return (object)second == null;
            }

            return first.Equals(second);
        }

        public static bool operator !=(OrderedTokenSet<T> first, OrderedTokenSet<T> second)
        {
            return !(first == second);
        }

        public bool Equals(OrderedTokenSet<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Container.Count == other.Container.Count && CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 47;
                if (Container != null)
                {
                    hashCode = (hashCode * 53) ^ EqualityComparer<ImmutableSortedSet<T>>.Default.GetHashCode(Container);
                }

                return hashCode;
            }
        }

        public int CompareTo(OrderedTokenSet<T> other)
        {
            if ((other?.Container?.Count ?? 0) == 0)
            {
                if (Container.Count > 0) return 1; else return 0;
            }

            int i = 0;

            foreach (var item in Container)
            {
                if (i >= other.Container.Count) { return 1; }
                var result = item.CompareTo(other.Container[i]);
                if (result != 0)
                {
                    return result;
                }
                i++;
            }

            return Container.Count < other.Container.Count ? -1 : 0;
        }

        public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(
            Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return descending ? Container.OrderByDescending(keySelector, comparer) : Container.OrderBy(keySelector, comparer);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Container.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (Container as IEnumerable).GetEnumerator();
        }
    }
}