namespace winforms_hotkeys
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox = new TextBox();
            buttonRun = new Button();
            buttonClose = new Button();
            SuspendLayout();
            // 
            // textBox
            // 
            textBox.Font = new Font("Segoe UI", 10F);
            textBox.Location = new Point(195, 18);
            textBox.Multiline = true;
            textBox.Name = "textBox";
            textBox.Size = new Size(254, 150);
            textBox.TabIndex = 0;
            // 
            // buttonRun
            // 
            buttonRun.AutoSize = true;
            buttonRun.Font = new Font("Segoe UI", 9F);
            buttonRun.ForeColor = Color.Maroon;
            buttonRun.Location = new Point(12, 12);
            buttonRun.Name = "buttonRun";
            buttonRun.Padding = new Padding(2);
            buttonRun.Size = new Size(160, 44);
            buttonRun.TabIndex = 1;
            buttonRun.Text = "&Run";
            // 
            // buttonClose
            // 
            buttonClose.AutoSize = true;
            buttonClose.Font = new Font("Segoe UI", 9F);
            buttonClose.ForeColor = Color.Maroon;
            buttonClose.Location = new Point(289, 174);
            buttonClose.Name = "buttonClose";
            buttonClose.Padding = new Padding(2);
            buttonClose.Size = new Size(160, 44);
            buttonClose.TabIndex = 1;
            buttonClose.Text = "&Close";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(478, 244);
            Controls.Add(buttonClose);
            Controls.Add(buttonRun);
            Controls.Add(textBox);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main Form";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox;
        private Button buttonRun;
        private Button buttonClose;
    }
}
