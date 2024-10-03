If I understand your intent, you probably want to disable the buttons while a test is running _regardless_ of whether invoked by a click or by a shortcut. And since it sounds as though you need to do some asynchronous things (i.e. without blocking), the MRE below shows how to use `BeginInvoke` and `async` lambdas in order to do that without resorting to `Application.DoEvents()`.

As far as shortcuts go, one of the easiest ways to make a hot key is to use the ampersand in conjunction with the button text, for example `&Run`. Then, pressing the `[ALT]` key will make the shortcuts visible, and pressing `[Alt] + R` will raise the click event on the button (whichever character follows the ampersand, so `R&un` would activate on `[Alt] + U` instead).

___

##### Minimal Reproducible Example

[![shortcut keys][1]][1]

```
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
}
```

#### Modifier Key Combinations

For an expanded palette of key combinations, the `IMessageFilter` interface is easy to implement for a `Form` and is ideal for this because it can hook the `KeyDown` message regardless of which child control on the `Form` currently has the focus.

Here, the example is expanded to include `[Control] + R` and  `[Control] + C`.

```
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
}
```

___


  [1]: https://i.sstatic.net/mLONNzhD.png