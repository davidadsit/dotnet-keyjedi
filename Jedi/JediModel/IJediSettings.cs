using System.Drawing;

namespace Jedi
{
	public interface IJediSettings
	{
		double Opacity { get; }
		Font Font { get; }
		bool VisualStudioOnly { get; }
		string MouselessModeKey { get; }
	}
}