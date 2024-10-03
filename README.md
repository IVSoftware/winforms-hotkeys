As I understand it, you'd like to establish some hot keys without cluttering up the code with a massive amount of `KeyDown` hooks by way of events.  The `IMessageFilter` interface, which is easy to implement for a `Form`, is ideal for this because it doesn't matter which child control on the `Form` currently has the focus.

___

##### Minimal Reproducible Example

```
public partial class MainForm : Form, IMessageFilter
{
    public MainForm()
    {
        InitializeComponent();
        Application.AddMessageFilter(this);
        Disposed += (sender, e) => Application.RemoveMessageFilter(this);

        // Add the shortcut methods you need
        Shortcuts = new Dictionary<Keys, Action>
        {
            { Keys.F9, OnKeyF9 },
            { Keys.Control | Keys.C, OnExit },
        };
    }

    private void OnExit()
    {
        if(DialogResult.OK == MessageBox.Show("Application is exiting!", "Control-C Detected", MessageBoxButtons.OKCancel))
        {
            Close();
        }
    }

    private Dictionary<Keys, Action> Shortcuts { get; }

    private void OnKeyF9()
    {
        textBox1.Text = $"{nameof(OnKeyF9)} Detected!";
    }
    public bool PreFilterMessage(ref Message m)
    {
        if (m.Msg == 0x100)
        {
            Keys keyData = (Keys)(int)m.WParam | Control.ModifierKeys;
            if (Shortcuts.TryGetValue(keyData, out var action))
            {
                action();
                return true;
            }
        }
        return false;
    }
}
```