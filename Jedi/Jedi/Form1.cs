using System;
using System.Drawing;
using System.Windows.Forms;
using Jedi.Properties;
using Kennedy.ManagedHooks;

namespace Jedi
{
	public partial class Form1 : Form
	{
		private readonly ShortcutMemorizer memos = new ShortcutMemorizer();
		private readonly KeyStateManager mgr = new KeyStateManager();
		private readonly string mouselessModeKey = Settings.Default.MouselessModeKey;
		private MouseDisabler _disabler;
		private KeyboardHook _keyboardHook;
		private MouseHook _mouseHook;
		private string[] _mouselessModeKeyParts;

		public Form1()
		{
			InitializeComponent();

			_mouseHook = new MouseHook();
			_mouseHook.MouseEvent += MouseHookMouseEvent;
			_keyboardHook = new KeyboardHook();
			_keyboardHook.KeyboardEvent += KeyboardHookKeyboardEvent;

			SetAppTitle();
		}

		private void AddText(string message)
		{
			if (!Focused && (message != null))
			{
				listView1.Items.Insert(0, message);
				if (listView1.Items.Count > 1)
				{
					ListViewItem item = listView1.Items[1];
					item.ForeColor = Color.Gray;
				}
				if (listView1.Items.Count > 5)
				{
					ListViewItem item2 = listView1.Items[5];
					item2.ForeColor = Color.LightGray;
				}
				if (listView1.Items.Count > 10)
				{
					listView1.Items[10].Remove();
				}
				listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		private void CopyAllMemosToClipboardToolStripMenuItemClick(object sender, EventArgs e)
		{
			memos.SetCliboardToMemos();
		}

		private void Form1FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Default["Opacity"] = Opacity;
			Settings.Default.Save();
			Settings.Default.Upgrade();

			if (_mouseHook != null)
			{
				_mouseHook.Dispose();
				_mouseHook = null;
			}
			if (_keyboardHook != null)
			{
				_keyboardHook.Dispose();
				_keyboardHook = null;
			}
		}

		private void Form1Load(object sender, EventArgs e)
		{
			KeyPreview = true;
			listView1.Items.Clear();
			AllowTransparency = true;
			TopMost = true;
			Opacity = Settings.Default.Opacity;
			listView1.Font = Settings.Default.ListFont;
			Refresh();
			mgr.ShortcutActivated += mgr_OnShortcutActivated;
			_keyboardHook.InstallHook();
		}

		private bool IsMsgMouselessModeKey(string msg)
		{
			if (mouselessModeKey == string.Empty)
			{
				return false;
			}
			if (_mouselessModeKeyParts == null)
			{
				_mouselessModeKeyParts = mouselessModeKey.Split(new[] {'+'});
				if (mouselessModeKey.EndsWith("+"))
				{
					_mouselessModeKeyParts[_mouselessModeKeyParts.Length - 1] = "+";
				}
			}
			foreach (string str in _mouselessModeKeyParts)
			{
				if (!msg.Contains(str))
				{
					return false;
				}
			}
			return true;
		}

		private void KeyboardHookKeyboardEvent(KeyboardEvents kEvent, Keys key)
		{
			mgr.Input(kEvent, key);
		}

		private void ListView1DoubleClick(object sender, EventArgs e)
		{
			SetMemoOnSelectedItem();
		}

		private void MouseHookMouseEvent(MouseEvents mEvent, Point point)
		{
			string message = string.Format("Mouse event: {0}: ({1},{2}).", mEvent, point.X, point.Y);
			AddText(message);
		}

		private void MouselessModeOnOffToolStripMenuItemClick(object sender, EventArgs e)
		{
			ToggleMouselessMode();
		}

		private void SetAppTitle()
		{
			label1.Visible = false;
			Text = @"Keyboard Jedi";
			if (Settings.Default.VisualStudioOnly)
			{
				Text = Text + @" (VS Only)";
			}
			if (_disabler != null)
			{
				label1.Visible = true;
				Text = @"Mouseless Mode";
				string str = string.Format("Mouseless Mode on [{0}].\n Press Ctrl+Alt+Shift+F12 to regain mouse control.",
				                           _disabler.AppTitle);
				label1.Text = str;
			}
		}

		private void SetMemoOnSelectedItem()
		{
			if (listView1.SelectedItems.Count != 0)
			{
				ListViewItem item = listView1.SelectedItems[0];
				TopMost = false;

				const string title = "Enter a memo to associate with this shortcut";
				const string promptText = "Memo for shortcut";


				string memo = Interaction.InputBox(title, promptText, "Action triggerd by " + item.Text);
					//, new Point(Location.X, Location.Y));
				TopMost = true;

				if (memo != string.Empty)
				{
					memos.AddShortCut(item.Text, memo);
				}
			}
		}

		private void SetMemoOnThisItemToolStripMenuItemClick(object sender, EventArgs e)
		{
			SetMemoOnSelectedItem();
		}

		private void ToggleMouselessMode()
		{
			if (_disabler != null)
			{
				_disabler.Stop();
				_disabler = null;
				mouselessModeOnOffToolStripMenuItem.Visible = false;
			}
			else
			{
				_disabler = new MouseDisabler(this);
				_disabler.Start();
				mouselessModeOnOffToolStripMenuItem.Visible = true;
			}
			SetAppTitle();
		}

		private void TrackBar1Scroll(object sender, EventArgs e)
		{
			Opacity = trackBar1.Value / 100f;
		}

		private void mgr_OnShortcutActivated(string msg)
		{
			if (IsMsgMouselessModeKey(msg))
			{
				ToggleMouselessMode();
			}
			AddText(msg);
		}
	}
}