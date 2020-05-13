using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Specify the standard Linq query namespace to eliminate
// additional namespace inclusions to expose methods.
namespace System.Linq //Common.Linq.Extensions
{
	/// <summary>
	/// Defines extensions methods for IEnumerable.
	/// </summary>
	public static class IEnumerable
	{

		/// <summary>
		/// Returns the only element of a sequence that satisfies the condition specified in <paramref name="predicate"/>
		/// or throws the exception created in <paramref name="makeException"/> if no such element exists.  
		/// Use the zero argument function initializer: "() => new Exception()".
		/// </summary>
		/// <typeparam name="TSource">The <see cref="IEnumerable"/> source type.</typeparam>
		/// <param name="source">The <see cref="IEnumerable"/> source.</param>
		/// <param name="predicate">Predicate that defines the selection criteria.</param>
		/// <param name="makeException">Function that generates the item missing condition exception.</param>
		/// <returns></returns>
		public static TSource SingleOrException<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<Exception> makeException)
		{
			TSource item = source.SingleOrDefault(predicate);
			if(item == null)
			{
				throw makeException();
			}
			return item;
		}

		/// <summary>
		/// Returns the only element of a sequence that satisfies the condition specified in <paramref name="predicate"/>
		/// or the supplied <paramref name="value"/> if not matching sequence element exists.
		/// </summary>
		/// <typeparam name="TSource">The <see cref="IEnumerable"/> source type.</typeparam>
		/// <param name="source">The <see cref="IEnumerable"/> source.</param>
		/// <param name="predicate">Predicate that defines the selection criteria.</param>
		/// <param name="value">The alternate value to return if no sequence match is found.</param>
		/// <returns></returns>
		public static TSource SingleOrValue<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource value)
		{
			TSource item = source.SingleOrDefault(predicate);
			if(item == null)
			{
				return value;
			}
			return item;
		}

	}
}
