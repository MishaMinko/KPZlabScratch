namespace LabScratch
{
    partial class CreateNodeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            listBox1 = new ListBox();
            comboBox1 = new ComboBox();
            label1 = new Label();
            comboBox2 = new ComboBox();
            comboBox3 = new ComboBox();
            label2 = new Label();
            comboBox4 = new ComboBox();
            numericUpDown1 = new NumericUpDown();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.ShowImageMargin = false;
            contextMenuStrip1.Size = new Size(36, 4);
            contextMenuStrip1.Click += contextMenuStrip1_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 28;
            listBox1.Location = new Point(13, 13);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(321, 32);
            listBox1.TabIndex = 1;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(13, 80);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(321, 36);
            comboBox1.TabIndex = 2;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 49);
            label1.Name = "label1";
            label1.Size = new Size(171, 28);
            label1.TabIndex = 3;
            label1.Text = "Choose node type";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(12, 122);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(321, 36);
            comboBox2.TabIndex = 4;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(12, 164);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(321, 36);
            comboBox3.TabIndex = 5;
            comboBox3.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(149, 203);
            label2.Name = "label2";
            label2.Size = new Size(22, 28);
            label2.TabIndex = 6;
            label2.Text = "||";
            label2.Visible = false;
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(12, 234);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(321, 36);
            comboBox4.TabIndex = 7;
            comboBox4.Visible = false;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(15, 235);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(320, 34);
            numericUpDown1.TabIndex = 8;
            numericUpDown1.Visible = false;
            // 
            // button1
            // 
            button1.Location = new Point(11, 289);
            button1.Name = "button1";
            button1.Size = new Size(323, 46);
            button1.TabIndex = 9;
            button1.Text = "Create node";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // CreateNodeForm
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(347, 347);
            Controls.Add(button1);
            Controls.Add(numericUpDown1);
            Controls.Add(comboBox4);
            Controls.Add(label2);
            Controls.Add(comboBox3);
            Controls.Add(comboBox2);
            Controls.Add(label1);
            Controls.Add(comboBox1);
            Controls.Add(listBox1);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Margin = new Padding(4);
            Name = "CreateNodeForm";
            Text = "CreateNodeForm";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private ListBox listBox1;
        private ComboBox comboBox1;
        private Label label1;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private Label label2;
        private ComboBox comboBox4;
        private NumericUpDown numericUpDown1;
        private Button button1;
    }
}