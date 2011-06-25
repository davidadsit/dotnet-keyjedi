using Jedi;
using JediModel;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class KeyExtensionsTests
	{
		[Test]
		public void DisplayName_returns_correct_value_for_Key_with_specified_DisplayName()
		{
			Assert.AreEqual("Winkey", System.Windows.Forms.Keys.LWin.DisplayName());
		}

		[Test]
		public void DisplayName_returns_ToString_value_for_Key_without_specified_DisplayName()
		{
			Assert.AreEqual(System.Windows.Forms.Keys.Apps.ToString(), System.Windows.Forms.Keys.Apps.DisplayName());
		}
	}
}