using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jedi.Properties;
using JediModel;
using Kennedy.ManagedHooks;

namespace Jedi
{
	public class KeyStateManager
	{
		private readonly Keys[] comboKeys = new[]
		                                    	{
		                                    		Keys.Control, Keys.ControlKey, Keys.Alt, Keys.Shift, Keys.ShiftKey, Keys.LWin,
		                                    		Keys.RWin
		                                    	};

		private readonly Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>();

		private readonly bool visualStudioOnly = Settings.Default.VisualStudioOnly;
		private bool isSystemKeyDown;

		private Keys[] specialSingleKeys = new[]
		                                   	{
		                                   		Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9,
		                                   		Keys.F10, Keys.F11, Keys.F12
		                                   	};

		public delegate void ShorcutDelegate(string msg);

		public KeyStateManager()
		{
			InitSpecialSingleKeys();
		}

		public void Input(KeyboardEvents kEvents, Keys currentKey)
		{
			if (!visualStudioOnly || NativeHelpers.ActiveApplTitle().Contains("Microsoft Visual Studio"))
			{
				if (((kEvents == KeyboardEvents.SystemKeyDown) || (kEvents == KeyboardEvents.KeyDown)) &&
				    isSpecialKey(comboKeys, currentKey))
				{
					keyStates[currentKey] = true;
					isSystemKeyDown = true;
				}
				if (((kEvents == KeyboardEvents.SystemKeyUp) || (kEvents == KeyboardEvents.KeyUp)) &&
				    isSpecialKey(comboKeys, currentKey))
				{
					keyStates[currentKey] = false;
					isSystemKeyDown = AreAllSystemKeysDown(keyStates);
				}
				if (((kEvents == KeyboardEvents.SystemKeyDown) || (kEvents == KeyboardEvents.KeyDown)) &&
				    (!isSpecialKey(comboKeys, currentKey) && isSystemKeyDown))
				{
					ShortcutActivated(BuildKeyMessage(keyStates, currentKey));
				}
				else if ((kEvents == KeyboardEvents.KeyDown) && isSpecialSingleKey(currentKey))
				{
					ShortcutActivated(BuildKeyMessage(keyStates, currentKey));
				}
			}
		}

		public event ShorcutDelegate ShortcutActivated;

		private void AppendToArray(Keys[] appendArr, ref Keys[] targetArr)
		{
			Keys[] array = new Keys[specialSingleKeys.Length + appendArr.Length];
			specialSingleKeys.CopyTo(array, 0);
			appendArr.CopyTo(array, 0);
			targetArr = array;
		}

		private bool AreAllSystemKeysDown(Dictionary<Keys, bool> states)
		{
			foreach (KeyValuePair<Keys, bool> pair in states)
			{
				if (pair.Value)
				{
					return true;
				}
			}
			return false;
		}

		private string BuildKeyMessage(Dictionary<Keys, bool> states, Keys key)
		{
			StringBuilder builder = new StringBuilder();
			foreach (KeyValuePair<Keys, bool> pair in states)
			{
				if (pair.Value)
				{
					builder.Append(pair.Key.DisplayName());
					builder.Append("+");
				}
			}
			builder.Append(key.DisplayName());
			return builder.ToString();
		}

		private void InitSpecialSingleKeys()
		{
			if (Settings.Default.ShowTabs)
			{
				Keys[] appendArr = new[] {Keys.Tab};
				AppendToArray(appendArr, ref specialSingleKeys);
			}
			if (Settings.Default.ShowEnter)
			{
				Keys[] keysArray2 = new[] {Keys.Return, Keys.Return};
				AppendToArray(keysArray2, ref specialSingleKeys);
			}
			if (Settings.Default.ShowPageUpDown)
			{
				Keys[] keysArray3 = new[] {Keys.Next, Keys.Prior};
				AppendToArray(keysArray3, ref specialSingleKeys);
			}
		}

		private bool isSpecialKey(Keys[] specialKeys, Keys currentKey)
		{
			foreach (Keys keys in specialKeys)
			{
				if (keys == currentKey)
				{
					return true;
				}
			}
			return false;
		}

		private bool isSpecialSingleKey(Keys key)
		{
			return specialSingleKeys.Any(keys => keys == key);
		}
	}
}