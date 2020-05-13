namespace Common.Linq.HelperClasses
{
	/// <summary>
	/// Defines an object parameter.
	/// </summary>
	public class ObjectParam
	{
		/// <summary>
		/// Creates a new instance with default values.
		/// </summary>
		public ObjectParam(){}

		/// <summary>
		/// Creates a new instance with the specified property name and value.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="propertyValue"></param>
		public ObjectParam(string propertyName, object propertyValue)
		{
			this.PropertyName = propertyName;
			this.PropertyValue = propertyValue;
		}

		/// <summary>
		/// Get/set the property name
		/// </summary>
		public string PropertyName { get; set; }

		/// <summary>
		/// Get/set the property value
		/// </summary>
		public object PropertyValue { get; set; }
	}
}
