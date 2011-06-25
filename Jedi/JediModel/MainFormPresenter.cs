using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Jedi
{
	public class MainFormPresenter
	{
		private const string MOUSELESS_MODE_INSTRUCTIONS =
			"Mouseless Mode on [{0}].\n Press Ctrl+Alt+Shift+F12 to regain mouse control.";

		private readonly List<KeyViewModel> keys = new List<KeyViewModel>();
		private readonly string mouselessModeKey;

		private readonly Dictionary<int, string> positionColors =
			new Dictionary<int, string>
				{
					{0, "#000000"},
					{-1, "#151515"},
					{-2, "#2A2A2A"},
					{-3, "#3F3F3F"},
					{1, "#555555"},
					{2, "#6A6A6A"},
					{3, "#7F7F7F"},
					{4, "#949494"},
					{5, "#AAAAAA"},
					{6, "#BFBFBF"},
					{7, "#D4D4D4"},
					{8, "#E9E9E9"},
					{9, "#E9E9E9"},
					{10, "#E9E9E9"}
				};

		private readonly IJediSettings settings;
		private readonly IMainFormView view;
		private MouseDisabler disabler;
		private string[] mouselessModeKeyParts;

		public MainFormPresenter(IMainFormView view, IJediSettings settings)
		{
			this.view = view;
			this.settings = settings;
			mouselessModeKey = settings.MouselessModeKey;
		}

		public void HandleLoad()
		{
			view.DisplayKeys(keys);
			view.SetOpacity(settings.Opacity);
			view.SetFont(settings.Font);
			view.Refresh();
			SetAppTitle();
		}

		public void HandleMouselessModeClick()
		{
			ToggleMouselessMode();
		}

		public void HandleShortcut(string msg)
		{
			if (IsMsgMouselessModeKey(msg))
			{
				ToggleMouselessMode();
			}
			AddMessage(msg);
			DisplayKeys();
		}

		private void AddMessage(string message)
		{
			keys.Insert(0, new KeyViewModel {Text = message});
			while (keys.Count > 10)
			{
				keys.RemoveAt(10);
			}
		}

		private void DisplayKeys()
		{
			for (int i = 0; i < keys.Count; i++)
			{
				string positionColor = positionColors[i];
				keys[i].Color = ColorTranslator.FromHtml(positionColor);
			}
			view.DisplayKeys(keys);
		}

		private bool IsMsgMouselessModeKey(string msg)
		{
			if (mouselessModeKey == string.Empty)
			{
				return false;
			}
			if (mouselessModeKeyParts == null)
			{
				mouselessModeKeyParts = mouselessModeKey.Split(new[] {'+'});
				if (mouselessModeKey.EndsWith("+"))
				{
					mouselessModeKeyParts[mouselessModeKeyParts.Length - 1] = "+";
				}
			}
			return mouselessModeKeyParts.All(msg.Contains);
		}

		private void SetAppTitle()
		{
			view.HideMouselessModeInstructions();
			string title = @"Keyboard Jedi";
			if (settings.VisualStudioOnly)
			{
				title = title + @" (VS Only)";
			}
			if (disabler != null)
			{
				title = @"Mouseless Mode";
				string instructions = string.Format(MOUSELESS_MODE_INSTRUCTIONS, disabler.AppTitle);
				view.DisplayMouselessModeInstructions(instructions);
			}
			view.DisplayTitle(title);
		}

		private void ToggleMouselessMode()
		{
			if (disabler != null)
			{
				disabler.Stop();
				disabler = null;
				view.HideMouselessModeMenuOption();
			}
			else
			{
				disabler = new MouseDisabler(view);
				disabler.Start();
				view.DisplayMouselessModeMenuOptions();
			}
			SetAppTitle();
		}
	}
}