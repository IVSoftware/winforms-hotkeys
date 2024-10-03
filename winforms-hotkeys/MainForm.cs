using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace winforms_hotkeys
{
    public partial class MainForm : Form, IMessageFilter
    {
        public MainForm()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
            Disposed += (sender, e) => Application.RemoveMessageFilter(this);
            buttonRun.Click += (sender, e) =>
            {
                BeginInvoke(async() =>
                {
                    try
                    {
                        EnableButtons(false);
                        CurrentCommandContext = new CommandContext();
                        textBox.Text = "RunningText" + Environment.NewLine;
                        for (int i = 1; i <= 5; i++)
                        {
                            await Task.Delay(500);
                            textBox.AppendText($"Phase {i}{Environment.NewLine}");
                        }
                    }
                    finally
                    {
                        EnableButtons(true);
                    }
                });
            };
            buttonClose.Click += (sender, e) =>
            {
                BeginInvoke(() =>
                {
                    if (DialogResult.OK == MessageBox.Show("Application is exiting!", "Alt-C Detected", MessageBoxButtons.OKCancel))
                    {
                        BeginInvoke(()=>Close());
                    }
                });
            };

            Shortcuts = new Dictionary<Keys, Action>
            {
                { Keys.Control | Keys.R, ()=> buttonRun.PerformClick() },
                { Keys.Control | Keys.C, ()=> buttonClose.PerformClick() },
            };
        }

        private void EnableButtons(bool enabled)
        {
            foreach (Button button in Controls.OfType<Button>())
            {
                button.Enabled = enabled;
            }
        }

        private Dictionary<Keys, Action> Shortcuts { get; }

        const int WM_KEYDOWN = 0x0100;
        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_KEYDOWN:
                    Keys keyData = (Keys)(int)m.WParam | Control.ModifierKeys;
                    if (Shortcuts.TryGetValue(keyData, out var action))
                    {
                        action();
                        return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Provides another way to manage asynchronicity. Not used in the current example, however.
        /// </summary>
        CommandContext? CurrentCommandContext { get; set; }
    }

    /// <summary>
    /// Provides another way to manage asynchronicity. Not used in the current example, however.
    /// </summary>
    public class CommandContext
        : EventArgs  // Bonus - Be able to fire any context as an EventArgs e (and potentially await on the server side)
    {
        private SemaphoreSlim _busy { get; } = new SemaphoreSlim(0, 1);
        public TaskAwaiter GetAwaiter()
        {
            return _busy
            .WaitAsync()        // Do not use the Token here
            .GetAwaiter();
        }
        internal void Release() => _busy.Release();
    }
}
