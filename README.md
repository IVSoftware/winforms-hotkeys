If I understand your intent, you probably want to disable the buttons while a test is running _regardless_ of whether invoked by a click or by a shortcut. And since it sounds as though you need to do some asynchronous things without blocking, the MRE below shows how to do that without resorting to `Application.DoEvents()`.

As far as shortcuts go, one of the easiest ways to make a hot key is to us the ampersand in combination with the button text, for example `&Run`. Then, pressing the [ALT] key will make the shortcuts visible, and pressing[Alt] + R will run the test.

___

##### Minimal Reproducible Example

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

    private Dictionary<Keys, Action> Shortcuts { get; }

    CommandContext? CurrentCommandContext { get; set; }
}
```