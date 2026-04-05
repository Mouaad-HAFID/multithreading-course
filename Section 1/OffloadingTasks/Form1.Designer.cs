namespace OffloadingTasks
{
    partial class Form1
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
            button1 = new Button();
            button2 = new Button();
            msgTxtBox = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(244, 371);
            button1.Name = "button1";
            button1.Size = new Size(108, 23);
            button1.TabIndex = 0;
            button1.Text = "First Message";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(429, 371);
            button2.Name = "button2";
            button2.Size = new Size(106, 23);
            button2.TabIndex = 1;
            button2.Text = "Second Message";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // msgTxtBox
            // 
            msgTxtBox.Location = new Point(244, 158);
            msgTxtBox.Multiline = true;
            msgTxtBox.Name = "msgTxtBox";
            msgTxtBox.Size = new Size(291, 88);
            msgTxtBox.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(msgTxtBox);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private TextBox msgTxtBox;
    }
}
