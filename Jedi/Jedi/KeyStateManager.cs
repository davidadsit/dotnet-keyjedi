using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Jedi.Properties;
using Kennedy.ManagedHooks;

namespace Jedi
{
    public class KeyStateManager
    {
        private Keys[] comboKeys = new Keys[] { Keys.Control, Keys.ControlKey, Keys.Alt, Keys.Shift, Keys.ShiftKey, Keys.LWin, Keys.RWin };
        private bool isSystemKeyDown;
        private Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>();
        private Keys[] specialSingleKeys = new Keys[] { Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12 };
        private bool visualStudioOnly = Settings.Default.VisualStudioOnly;

        public event ShorcutDelegate ShortcutActivated;

        public KeyStateManager()
        {
            InitSpecialSingleKeys();
        }

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
                    builder.Append(getKeyAsString(pair.Key));
                    builder.Append("+");
                }
            }
            builder.Append(getKeyAsString(key));
            return builder.ToString();
        }

        private static string getKeyAsString(Keys key)
        {
            switch (key)
            {
                case Keys.D0:
                    return "0";

                case Keys.D1:
                    return "1";

                case Keys.D2:
                    return "2";

                case Keys.D3:
                    return "3";

                case Keys.D4:
                    return "4";

                case Keys.D5:
                    return "5";

                case Keys.D6:
                    return "6";

                case Keys.D7:
                    return "7";

                case Keys.D8:
                    return "8";

                case Keys.D9:
                    return "9";

                case Keys.LWin:
                    return "Winkey";

                case Keys.RWin:
                    return "Winkey";

                case Keys.NumPad0:
                    return "Num0";

                case Keys.NumPad1:
                    return "Num1";

                case Keys.NumPad2:
                    return "Num2";

                case Keys.NumPad3:
                    return "Num3";

                case Keys.NumPad4:
                    return "Num4";

                case Keys.NumPad5:
                    return "Num5";

                case Keys.NumPad6:
                    return "Num6";

                case Keys.NumPad7:
                    return "Num7";

                case Keys.NumPad8:
                    return "Num8";

                case Keys.NumPad9:
                    return "Num9";

                case Keys.OemSemicolon:
                    return ";";

                case Keys.Oemplus:
                    return "+";

                case Keys.Oemcomma:
                    return ",";

                case Keys.OemMinus:
                    return "-";

                case Keys.OemPeriod:
                    return ".";

                case Keys.OemQuestion:
                    return "?";

                case Keys.Oemtilde:
                    return "'";

                case Keys.OemOpenBrackets:
                    return "[";

                case Keys.OemPipe:
                    return "/";

                case Keys.OemCloseBrackets:
                    return "]";

                case Keys.OemQuotes:
                    return "'";

                case Keys.OemBackslash:
                    return @"\";

                case Keys.Control:
                    return "Ctrl";
            }
            return key.ToString();
        }

        private void InitSpecialSingleKeys()
        {
            if (Settings.Default.ShowTabs)
            {
                Keys[] appendArr = new [] { Keys.Tab };
                AppendToArray(appendArr, ref specialSingleKeys);
            }
            if (Settings.Default.ShowEnter)
            {
                Keys[] keysArray2 = new [] { Keys.Return, Keys.Return };
                AppendToArray(keysArray2, ref specialSingleKeys);
            }
            if (Settings.Default.ShowPageUpDown)
            {
                Keys[] keysArray3 = new [] { Keys.Next, Keys.Prior };
                AppendToArray(keysArray3, ref specialSingleKeys);
            }
        }

        public void Input(KeyboardEvents kEvents, Keys currentKey)
        {
            if (!visualStudioOnly || NativeHelpers.ActiveApplTitle().Contains("Microsoft Visual Studio"))
            {
                if (((kEvents == KeyboardEvents.SystemKeyDown) || (kEvents == KeyboardEvents.KeyDown)) && isSpecialKey(comboKeys, currentKey))
                {
                    keyStates[currentKey] = true;
                    isSystemKeyDown = true;
                }
                if (((kEvents == KeyboardEvents.SystemKeyUp) || (kEvents == KeyboardEvents.KeyUp)) && isSpecialKey(comboKeys, currentKey))
                {
                    keyStates[currentKey] = false;
                    isSystemKeyDown = AreAllSystemKeysDown(keyStates);
                }
                if (((kEvents == KeyboardEvents.SystemKeyDown) || (kEvents == KeyboardEvents.KeyDown)) && (!isSpecialKey(comboKeys, currentKey) && isSystemKeyDown))
                {
                    ShortcutActivated(BuildKeyMessage(keyStates, currentKey));
                }
                else if ((kEvents == KeyboardEvents.KeyDown) && isSpecialSingleKey(currentKey))
                {
                    ShortcutActivated(BuildKeyMessage(keyStates, currentKey));
                }
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
            foreach (Keys keys in specialSingleKeys)
            {
                if (keys == key)
                {
                    return true;
                }
            }
            return false;
        }

        public delegate void ShorcutDelegate(string msg);
    }
}
