using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Common.Diagnostics;
using Common.Linq.HelperClasses;

namespace Common.Linq.Extensions
{
	/// <summary>
	/// Extensions to the Linq DataContext
	/// </summary>
	public static class DataContextExtensions
	{
		/// <summary>
		/// Get an item in the database by the primary key.
		/// </summary>
		/// <typeparam name="T">The type of entity to get.</typeparam>
		/// <param name="context">DataContext to use.</param>
		/// <param name="parameters">List of primary key parameters.</param>
		/// <returns>Item from database of T type.</returns>
		public static T GetItemByKey<T>(this DataContext context, params ObjectParam[] parameters) where T : class
		{
			T entity = null;
			var entityType = typeof(T);

			var table = context.GetTable<T>();
			var metadata = context.Mapping.GetMetaType(entityType);

			var primarykeycolumn = metadata.DataMembers.Where(o => o.IsPrimaryKey);

			// Build an expression tree looking for the keys.
			BinaryExpression where = null;
			ParameterExpression paramExpression = Expression.Parameter(entityType, "o");
			foreach(var param in parameters)
			{
				var column = primarykeycolumn.Where(o => o.Name == param.PropertyName).SingleOrDefault();

				if(column == null)
				{
					throw new ApplicationException(string.Format("GetItemByKey cannot find column {0} in table {1}", param.PropertyName, metadata.Table.TableName));
				}

				var columnValue = Convert.ChangeType(param.PropertyValue, column.Type);

				BinaryExpression condition = Expression.Equal(Expression.Property(paramExpression, param.PropertyName), Expression.Constant(columnValue));

				if(where == null)
				{
					where = condition;
				}
				else
				{
					where = Expression.And(where, condition);
				}
			}

			Expression<Func<T, bool>> predicate = Expression.Lambda<Func<T, bool>>(where, new ParameterExpression[] { paramExpression });

			entity = table.SingleOrDefault(predicate);
			return entity;
		}

		/// <summary>
		/// Get Items from a Linq DataContext given parameters that represent the object's primary key values.
		/// This overload is for those items with a single key.
		/// </summary>
		/// <typeparam name="T">Type within the DataContext.</typeparam>
		/// <param name="context">The DataContext.</param>
		/// <param name="parameters">List of parameters.</param>
		/// <returns></returns>
		public static IEnumerable<T> GetItemsByKeyList<T>(this DataContext context, params ObjectParam[] parameters) where T : class
		{
			// Create a list of parameter lists with one item and call the overload.
			List<ObjectParamList> paramList = new List<ObjectParamList>();
			foreach(var parameter in parameters)
			{
				var param = new ObjectParamList() { KeyParams = { parameter } };
				paramList.Add(param);
			}

			return GetItemsByKeyList<T>(context, paramList);
		}

		/// <summary>
		/// Get Items from a Linq DataContext given parameters that represent the object's primary key values.
		/// This overload is for those items with a composite key.
		/// </summary>
		/// <typeparam name="T">Type within the DataContext.</typeparam>
		/// <param name="context">The DataContext.</param>
		/// <param name="paramList">A collection of parameter lists representing the key values for the items.</param>
		/// <returns></returns>
		public static IEnumerable<T> GetItemsByKeyList<T>(this DataContext context, IEnumerable<ObjectParamList> paramList) where T : class
		{
			// This is going to build an expression tree for the query to get the items in question.
			IEnumerable<T> entity = null;
			var entityType = typeof(T);

			var table = context.GetTable<T>();
			var metadata = context.Mapping.GetMetaType(entityType);

			var primarykeycolumn = metadata.DataMembers.Where(o => o.IsPrimaryKey);

			BinaryExpression where = null;
			ParameterExpression paramExpression = Expression.Parameter(entityType, "o");
			foreach(var paramSet in paramList)
			{
				BinaryExpression wherePart = null;
				foreach(var param in paramSet.KeyParams)
				{
					var column = primarykeycolumn.Where(o => o.Name == param.PropertyName).SingleOrDefault();

					if(column == null)
					{
						throw new ApplicationException(string.Format("GetItemByKey cannot find column {0} in table {1}", param.PropertyName, metadata.Table.TableName));
					}

					var columnValue = Convert.ChangeType(param.PropertyValue, column.Type);

					BinaryExpression condition = Expression.Equal(Expression.Property(paramExpression, param.PropertyName), Expression.Constant(columnValue));

					wherePart = wherePart == null ? condition : Expression.And(wherePart, condition);
				}

				where = where == null ? wherePart : Expression.Or(where, wherePart);
			}

			Expression<Func<T, bool>> predicate = null;

			if(where != null)
			{
				predicate = Expression.Lambda<Func<T, bool>>(where, new ParameterExpression[] { paramExpression });
			}

			entity = predicate == null ? table : table.Where(predicate);
			return entity;
		}

	}
}

namespace System.Data.Linq
{
	/// <summary>
	/// Extensions to the Linq DataContext
	/// </summary>
	public static class ExtDataContext
	{

		/// <summary>
		/// Sets the context's log property to a new instance of a DebugTextWriter so that content logging
		/// can be picked up by trace/debug listeners.
		/// Please note that the log property will be overwritten.
		/// </summary>
		/// <param name="context">The context on which the writer will be set.</param>
		/// <param name="category">Message category.</param>
		public static T SetLogDebugWriter<T>(this T context, string category = null) where T : DataContext
		{
			context.Log = new DebugTextWriter(category);
			return context;
		}
		
	}

	/// <summary>
	///	
	/// </summary>
	public static class LinqExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <typeparam name="TEntity"></typeparam>
		/// <returns></returns>
		public static ReadOnlyCollection<MetaDataMember> ColumnNames<TEntity>(this DataContext source)
		{
			return source.Mapping.MappingSource.GetModel(typeof(DataContext)).GetMetaType(typeof(TEntity)).PersistentDataMembers;
		}
	}
}