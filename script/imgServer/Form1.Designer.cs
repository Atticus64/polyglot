namespace imgServer
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
            label1 = new Label();
            statusLb = new Label();
            label2 = new Label();
            imgBox = new PictureBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)imgBox).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(353, 47);
            button1.Name = "button1";
            button1.Size = new Size(148, 47);
            button1.TabIndex = 0;
            button1.Text = "Encender Servidor";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(529, 47);
            button2.Name = "button2";
            button2.Size = new Size(148, 47);
            button2.TabIndex = 1;
            button2.Text = "Apagar Servidor";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(353, 9);
            label1.Name = "label1";
            label1.Size = new Size(56, 20);
            label1.TabIndex = 2;
            label1.Text = "Status: ";
            // 
            // statusLb
            // 
            statusLb.AutoSize = true;
            statusLb.Location = new Point(415, 9);
            statusLb.Name = "statusLb";
            statusLb.Size = new Size(92, 20);
            statusLb.TabIndex = 3;
            statusLb.Text = "Apagado 📴";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(70, 47);
            label2.Name = "label2";
            label2.Size = new Size(225, 20);
            label2.TabIndex = 4;
            label2.Text = "Servidor y Cliente socket imagen";
            // 
            // imgBox
            // 
            imgBox.Location = new Point(43, 146);
            imgBox.Name = "imgBox";
            imgBox.Size = new Size(914, 554);
            imgBox.TabIndex = 5;
            imgBox.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(43, 114);
            label3.Name = "label3";
            label3.Size = new Size(124, 20);
            label3.TabIndex = 6;
            label3.Text = "Imagen Recibida:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1146, 732);
            Controls.Add(label3);
            Controls.Add(imgBox);
            Controls.Add(label2);
            Controls.Add(statusLb);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)imgBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label1;
        private Label statusLb;
        private Label label2;
        private PictureBox imgBox;
        private Label label3;
    }
}
