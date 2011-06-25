using System.Collections.Generic;
using System.Drawing;

namespace Jedi
{
	public interface IMainFormView
	{
		void DisplayKeys(List<KeyViewModel> keys);
		void SetOpacity(double opacity);
		void SetFont(Font font);
		void Refresh();
		void HideMouselessModeInstructions();
		void DisplayMouselessModeInstructions(string instructions);
		void DisplayTitle(string title);
		Rectangle Bounds { get; }
		Point Location { get; }
		void HideMouselessModeMenuOption();
		void DisplayMouselessModeMenuOptions();
	}
}