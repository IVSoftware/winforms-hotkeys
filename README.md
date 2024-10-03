If I understand your intent, you probably want to disable the buttons while a test is running _regardless_ of whether invoked by a click or by a shortcut. And since it sounds as though you need to do some asynchronous things (i.e. without blocking), the MRE below shows how to use `BeginInvoke` and `async` lambdas in order to do that without resorting to `Application.DoEvents()`.

As far as shortcuts go, one of the easiest ways to make a hot key is to use the ampersand in conjunction with the button text, for example `&Run`. Then, pressing the `[ALT]` key will make the shortcuts visible, and pressing `[Alt] + R` will raise the click event on the button (whichever character follows the ampersand, so `R&un` would activate on `[Alt] + U` instead). This is a built-in feature of `Button``.

Nested buttons are shown at the end.

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
___

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

#### Complete with Nested Commands

> Is having "nested" key handler calls a problem?

No it is not, but it will help if you maintain an awaitable `CommandContext`

[![nested buttons][2]][2]

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
                    CurrentCommandContext = new CommandContext();
                    textBox.Text = "RunningText" + Environment.NewLine;
                    var buttonArray = new[] { radioPhase1, radioPhase2, radioPhase3 };
                    foreach (var button in buttonArray) button.Checked = false;
                    EnableButtons(false);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    for (int i = 0; i < 3; i++)
                    {
                        CurrentCommandContext = new CommandContext();
                        BeginInvoke(() =>
                        {
                            var button = buttonArray[i];
                            button.Enabled = true;
                            button.PerformClick();
                            button.Enabled = false;
                        });
                        await CurrentCommandContext;
                    }
                    CurrentCommandContext = null;
                }
                finally
                {
                    EnableButtons(true);
                }
            });
        };
        radioPhase1.Click += async (sender, e) =>
        {
            textBox.AppendText($"Running {(sender as Control)?.Text}{Environment.NewLine}");
            await Task.Delay(TimeSpan.FromSeconds(1));
            CurrentCommandContext?.Release();
        };
        radioPhase2.Click += async (sender, e) =>
        {
            textBox.AppendText($"Running {(sender as Control)?.Text}{Environment.NewLine}");
            await Task.Delay(TimeSpan.FromSeconds(1));
            CurrentCommandContext?.Release();
        };
        radioPhase3.Click += async(sender, e) =>
        {
            textBox.AppendText($"Running {(sender as Control)?.Text}{Environment.NewLine}");
            await Task.Delay(TimeSpan.FromSeconds(1));
            CurrentCommandContext?.Release();
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
        foreach (var button in Controls.OfType<ButtonBase>())
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
    /// Manage asynchronicity.
    /// </summary>
    CommandContext? CurrentCommandContext { get; set; }
}

/// <summary>
/// Manage asynchronicity.
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
```


  [1]: https://i.sstatic.net/mLONNzhD.png
  [2]: https://i.sstatic.net/v89OJfso.png