using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace Jedi
{
    

    public class MouseDisabler
    {
        private string appTitleToDisableOn;
        private readonly Form dockRootForm;
        private System.Timers.Timer timer = new System.Timers.Timer(50.0);

        public MouseDisabler(Form dockRootForm)
        {
            this.dockRootForm = dockRootForm;
            this.timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        public void Start()
        {
            appTitleToDisableOn = NativeHelpers.ActiveApplTitle();
            if (appTitleToDisableOn.Contains("Visual Studio"))
            {
                appTitleToDisableOn = "Visual Studio";
            }
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (NativeHelpers.ActiveApplTitle().Contains(appTitleToDisableOn))
            {
                Point position = Cursor.Position;
                Rectangle rectangle = new Rectangle(position, new Size(1, 1));
                if (!rectangle.IntersectsWith(dockRootForm.Bounds))
                {
                    Cursor.Position = dockRootForm.Location;
                }
            }
        }

        public string AppTitle
        {
            get
            {
                return appTitleToDisableOn;
            }
        }
    }
}
