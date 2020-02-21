using System;
using System.Collections.Generic;

namespace PseudoEnumerableClassTask
{
    /// <summary>Comparer adapter.</summary>
    /// <typeparam name="T">Type of elements.</typeparam>
    /// <seealso cref="IComparer{T}" />
    public class ComparerAdapter<T> : IComparer<T>
    {
        private Comparison<T> comparer;

        /// <summary>Initializes a new instance of the <see cref="ComparerAdapter{T}"/> class.</summary>
        /// <param name="comparer">The comparer.</param>
        /// <exception cref="ArgumentNullException">Thrown when comparer is null.</exception>
        public ComparerAdapter(Comparison<T> comparer)
        {
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.
        /// Value
        /// Meaning
        /// Less than zero
        /// <paramref name="x" /> is less than <paramref name="y" />.
        /// Zero
        /// <paramref name="x" /> equals <paramref name="y" />.
        /// Greater than zero
        /// <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(T x, T y) =>
            this.comparer(x, y);
    }
}
