namespace OffloadingTasks;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    // Event handlers are called on the main thread. Assuming that the action is long running
    // the UI will be frozen until it finishe. This is not a responsive design or behaviour
    // We can offload these long running tasks to separate threads to avoid blocking the main thread
    // and thus keeping the GUI funcitonal while waiting for the action to finish

    private void button1_Click(object sender, EventArgs e)
    {
        var thread = new Thread(() => DisplayMessage("First message", 5000));
        thread.Start();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        var thread = new Thread(() => DisplayMessage("Second message", 5000));
        thread.Start();
    }

    private void DisplayMessage(string msg, int delay)
    {
        Thread.Sleep(delay);
        msgTxtBox.Text = msg;
    }
}