using System;
using System.Collections.Generic;
using System.Text;

namespace PseudoEnumerableClassTask.Interfaces
{
    /// <summary>ITransformer interface.</summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface ITransformer<in TSource, out TResult>
    {
        /// <summary>Transforms the specified value.</summary>
        /// <param name="value">Value.</param>
        /// <returns>Returns transformed valued.</returns>
        TResult Transform(TSource value);
    }
}
