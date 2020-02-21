using System;
using PseudoEnumerableClassTask.Interfaces;

namespace PseudoEnumerableClassTask
{
    /// <summary>Predicate adapter.</summary>
    /// <typeparam name="T">Type of value.</typeparam>
    /// <seealso cref="IPredicate{T}" />
    public class PredicateAdapter<T> : IPredicate<T>
    {
        private Predicate<T> predicate;

        /// <summary>Initializes a new instance of the <see cref="PredicateAdapter{T}"/> class.</summary>
        /// <param name="predicate">The predicate.</param>
        /// <exception cref="ArgumentNullException">Thrown when predicate is null.</exception>
        public PredicateAdapter(Predicate<T> predicate)
        {
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        /// <summary>Determines whether specified value matches a specific condition.</summary>
        /// <param name="value">Value.</param>
        /// <returns>true if a value matches a specific condition; otherwise, false.</returns>
        public bool IsMatch(T value) =>
            this.predicate(value);
    }
}
