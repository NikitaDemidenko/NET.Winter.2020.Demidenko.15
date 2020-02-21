using System;
using System.Collections.Generic;
using System.Text;

namespace PseudoEnumerableClassTask.Interfaces
{
    /// <summary>IComparer interface.</summary>
    /// <typeparam name="T">Type of elements</typeparam>
    public interface IComparer<in T>
    {
        /// <summary>Compares specified elements.</summary>
        /// <param name="lhs">Left element.</param>
        /// <param name="rhs">Right element.</param>
        /// <returns>
        ///   <para>
        /// Integer:
        /// </para>
        ///   <list type="bullet">
        ///     <item> Less than zero, if lhs is less than rhs;
        /// </item>
        ///     <item> Zero, if lhs equals rhs;
        /// </item>
        ///     <item> Greater than zero, if lhs is greater than rhs;
        /// </item>
        ///   </list>
        /// </returns>
        int Compare(T lhs, T rhs);
    }
}
