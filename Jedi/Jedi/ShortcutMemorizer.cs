using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Jedi
{
    public class ShortcutMemorizer
    {
        private Dictionary<string, string> memos = new Dictionary<string, string>();

        public void AddShortCut(string shortcut, string memo)
        {
            memos[shortcut.ToUpper()] = memo;
        }

        private string GetMemos()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Shortcuts Recorded in Key Jedi");
            builder.AppendLine("------------------------------------");
            foreach (KeyValuePair<string, string> pair in memos)
            {
                builder.AppendFormat("{1} : {0}", pair.Key, pair.Value);
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public void SetCliboardToMemos()
        {
            Clipboard.SetText(GetMemos());
        }
    }
}
