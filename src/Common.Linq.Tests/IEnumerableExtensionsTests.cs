using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Linq.Tests
{
	[TestClass]
	public class IEnumerableExtensionsTests
	{
		private IEnumerable<System.String> _colors;

		public IEnumerableExtensionsTests()
		{
			_colors = new string[] { "red", "orange", "yellow", "green", "blue", "violet" }.ToList();
		}

		[TestMethod]
		public void TestSingleOrValue()
		{
			Assert.AreEqual("unknown", _colors.SingleOrValue(c => c == "mauve", "unknown"));
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestSingleOrException()
		{
			_colors.SingleOrException(c => c == "mauve", () => new InvalidOperationException("color missing"));
		}

	}
}
