using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nosleep
{
    public partial class MainWindow : Form, IDisposable
    {
        private static bool NoSleepEnabled;

        [Flags]
        public enum ExecutionState : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint SetThreadExecutionState(ExecutionState esFlags);

        public MainWindow()
        {
            InitializeComponent();
            toggleVisual();
        }

        private void toggleBtn_Click(object sender, EventArgs e)
        {
            if (NoSleepEnabled)
            {
                this.DeactivateNoSleep();
            }
            else
            {
                this.ActivateNoSleep();
            }

            this.toggleVisual();
        }

        private void ActivateNoSleep()
        {
            SetThreadExecutionState(ExecutionState.ES_CONTINUOUS | ExecutionState.ES_DISPLAY_REQUIRED);
            NoSleepEnabled = true;
        }

        private void DeactivateNoSleep()
        {
            SetThreadExecutionState(ExecutionState.ES_CONTINUOUS);
            NoSleepEnabled = false;
        }

        private void toggleVisual()
        {
            if (NoSleepEnabled)
            {
                this.toggleBtn.BackColor = Color.Chartreuse;
                this.toggleBtn.Text = "Activated";
            }
            else
            {
                this.toggleBtn.BackColor = Color.White;
                this.toggleBtn.Text = "Deactivated";
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.DeactivateNoSleep();
        }
    }
}
