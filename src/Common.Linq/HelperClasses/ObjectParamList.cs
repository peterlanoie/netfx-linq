using System.Collections.Generic;

namespace Common.Linq.HelperClasses
{
	/// <summary>
	/// Defines and object parameter list.
	/// </summary>
	public class ObjectParamList
	{
		/// <summary>
		/// Create a new instance with default values.
		/// </summary>
		public ObjectParamList()
		{
			this.KeyParams = new List<ObjectParam>();
		}

		/// <summary>
		/// Gets/set the key parameters.
		/// </summary>
		public List<ObjectParam> KeyParams { get; set; }
	}
}
