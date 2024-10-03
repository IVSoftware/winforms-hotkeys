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
            radioPhase1 = new RadioButton();
            radioPhase2 = new RadioButton();
            radioPhase3 = new RadioButton();
            SuspendLayout();
            // 
            // textBox
            // 
            textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox.Font = new Font("Segoe UI", 10F);
            textBox.Location = new Point(195, 12);
            textBox.Multiline = true;
            textBox.Name = "textBox";
            textBox.Size = new Size(254, 215);
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
            buttonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonClose.AutoSize = true;
            buttonClose.Font = new Font("Segoe UI", 9F);
            buttonClose.ForeColor = Color.Maroon;
            buttonClose.Location = new Point(289, 245);
            buttonClose.Name = "buttonClose";
            buttonClose.Padding = new Padding(2);
            buttonClose.Size = new Size(160, 44);
            buttonClose.TabIndex = 1;
            buttonClose.Text = "&Close";
            // 
            // radioPhase1
            // 
            radioPhase1.BackColor = Color.Azure;
            radioPhase1.Checked = true;
            radioPhase1.Location = new Point(12, 83);
            radioPhase1.Name = "radioPhase1";
            radioPhase1.Size = new Size(160, 44);
            radioPhase1.TabIndex = 2;
            radioPhase1.TabStop = true;
            radioPhase1.Text = "Phase 1";
            radioPhase1.UseVisualStyleBackColor = false;
            // 
            // radioPhase2
            // 
            radioPhase2.BackColor = Color.Azure;
            radioPhase2.Location = new Point(12, 133);
            radioPhase2.Name = "radioPhase2";
            radioPhase2.Size = new Size(160, 44);
            radioPhase2.TabIndex = 2;
            radioPhase2.Text = "Phase 2";
            radioPhase2.UseVisualStyleBackColor = false;
            // 
            // radioPhase3
            // 
            radioPhase3.BackColor = Color.Azure;
            radioPhase3.Location = new Point(12, 183);
            radioPhase3.Name = "radioPhase3";
            radioPhase3.Size = new Size(160, 44);
            radioPhase3.TabIndex = 2;
            radioPhase3.Text = "Phase 3";
            radioPhase3.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(478, 301);
            Controls.Add(radioPhase3);
            Controls.Add(radioPhase2);
            Controls.Add(radioPhase1);
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
        private RadioButton radioPhase1;
        private RadioButton radioPhase2;
        private RadioButton radioPhase3;
    }
}
