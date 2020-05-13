// ReSharper disable CheckNamespace
namespace System.Linq.Expressions
// ReSharper restore CheckNamespace
{
	// Taken from http://www.albahari.com/nutshell/predicatebuilder.aspx
	/// <summary>
	/// Contains extension methods for building stacked predicates.
	/// </summary>
	public static class PredicateBuilder
	{
		/// <summary>
		/// Returns a default predicate for all items in the set.
		/// Use this to begin a stack of subtractive statements (i.e. x AND y).
		/// </summary>
		/// <typeparam name="T">The set's item type.</typeparam>
		/// <returns></returns>
		public static Expression<Func<T, bool>> True<T>() { return f => true; }

		/// <summary>
		/// Returns a default predicate for no items in the set.
		/// Use this to begin a stack of additive statements (i.e. x OR y).
		/// </summary>
		/// <typeparam name="T">The set's item type.</typeparam>
		/// <returns></returns>
		public static Expression<Func<T, bool>> False<T>() { return f => false; }

		/// <summary>
		/// Stacks an OR operation onto an existing expression.
		/// </summary>
		/// <typeparam name="T">The set's item type.</typeparam>
		/// <param name="expr1"></param>
		/// <param name="expr2"></param>
		/// <returns></returns>
		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
															Expression<Func<T, bool>> expr2)
		{
			var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
			return Expression.Lambda<Func<T, bool>>
				  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
		}

		/// <summary>
		/// Stacks an AND operation onto an existing expression.
		/// </summary>
		/// <typeparam name="T">The set's item type.</typeparam>
		/// <param name="expr1"></param>
		/// <param name="expr2"></param>
		/// <returns></returns>
		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
															 Expression<Func<T, bool>> expr2)
		{
			var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
			return Expression.Lambda<Func<T, bool>>
				  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
		}
	}
}
