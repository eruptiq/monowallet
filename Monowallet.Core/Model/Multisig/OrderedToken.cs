using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Monowallet.Core.Model.Multisig
{
    public class OrderedToken<T> : IEquatable<OrderedToken<T>>, IComparable<OrderedToken<T>> where T : IComparable<T>
    {
        public OrderedToken(IOrderedEnumerable<T> tokens)
        {
            Set = ImmutableSortedSet.CreateRange(tokens.Distinct());
        }

        protected ImmutableSortedSet<T> Set { get; private set; }

        public override string ToString()
        {
            return string.Concat(Set.SelectMany(t => t.ToString()));
        }

        public override bool Equals(object obj)
        {
            if (obj is OrderedToken<T>)
            {
                return Equals((OrderedToken<T>)obj);
            }

            return base.Equals(obj);
        }

        public static bool operator ==(OrderedToken<T> first, OrderedToken<T> second)
        {
            if ((object)first == null)
            {
                return (object)second == null;
            }

            return first.Equals(second);
        }

        public static bool operator !=(OrderedToken<T> first, OrderedToken<T> second)
        {
            return !(first == second);
        }

        public bool Equals(OrderedToken<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Set.Count == other.Set.Count && CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 47;
                if (Set != null)
                {
                    hashCode = (hashCode * 53) ^ EqualityComparer<ImmutableSortedSet<T>>.Default.GetHashCode(Set);
                }

                return hashCode;
            }
        }

        public int CompareTo(OrderedToken<T> other)
        {
            if ((other?.Set?.Count ?? 0) == 0)
            {
                if (Set.Count > 0) return 1; else return 0;
            }

            int i = 0;

            foreach (var item in Set)
            {
                if (i >= other.Set.Count) { return 1; }
                var result = item.CompareTo(other.Set[i]);
                if (result != 0)
                {
                    return result;
                }
                i++;
            }

            return Set.Count < other.Set.Count ? -1 : 0;
        }
    }
}