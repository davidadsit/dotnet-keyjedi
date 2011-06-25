using System.Windows.Forms;
using JediModel;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class KeyExtensionsTests
	{
		[Test]
		public void ControlKeys_are_ComboKeys()
		{
			Assert.IsTrue(Keys.Control.IsComboKey());
			Assert.IsTrue(Keys.ControlKey.IsComboKey());
			Assert.IsTrue(Keys.Alt.IsComboKey());
			Assert.IsTrue(Keys.Shift.IsComboKey());
			Assert.IsTrue(Keys.ShiftKey.IsComboKey());
			Assert.IsTrue(Keys.LWin.IsComboKey());
			Assert.IsTrue(Keys.RWin.IsComboKey());
		}

		[Test]
		public void DisplayName_returns_ToString_value_for_Key_without_specified_DisplayName()
		{
			Assert.AreEqual(Keys.Apps.ToString(), Keys.Apps.DisplayName());
		}

		[Test]
		public void DisplayName_returns_correct_value_for_Key_with_specified_DisplayName()
		{
			Assert.AreEqual("Winkey", Keys.LWin.DisplayName());
		}

		[Test]
		public void FunctionKeys_are_SpecialSingleKeys()
		{
			Assert.IsTrue(Keys.F1.IsSpecialSingleKey());
			Assert.IsTrue(Keys.F2.IsSpecialSingleKey());
			Assert.IsTrue(Keys.F3.IsSpecialSingleKey());
			Assert.IsTrue(Keys.F4.IsSpecialSingleKey());
			Assert.IsTrue(Keys.F5.IsSpecialSingleKey());
			Assert.IsTrue(Keys.F6.IsSpecialSingleKey());
			Assert.IsTrue(Keys.F7.IsSpecialSingleKey());
			Assert.IsTrue(Keys.F8.IsSpecialSingleKey());
			Assert.IsTrue(Keys.F9.IsSpecialSingleKey());
			Assert.IsTrue(Keys.F10.IsSpecialSingleKey());
			Assert.IsTrue(Keys.F11.IsSpecialSingleKey());
			Assert.IsTrue(Keys.F12.IsSpecialSingleKey());
		}


		[Test]
		public void SpecialSingleKeys_can_be_added()
		{
			Assert.IsFalse(Keys.PageUp.IsSpecialSingleKey());
			Keys.PageUp.AddToSpecialSingleKeys();
			Assert.IsTrue(Keys.PageUp.IsSpecialSingleKey());
		}
	}
}