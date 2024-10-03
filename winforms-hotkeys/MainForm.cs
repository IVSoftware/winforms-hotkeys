using System.Runtime.CompilerServices;

namespace winforms_hotkeys
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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
        }

        private void EnableButtons(bool enabled)
        {
            foreach (Button button in Controls.OfType<Button>())
            {
                button.Enabled = enabled;
            }
        }

        private Dictionary<Keys, Action> Shortcuts { get; }

        CommandContext? CurrentCommandContext { get; set; }
    }
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
