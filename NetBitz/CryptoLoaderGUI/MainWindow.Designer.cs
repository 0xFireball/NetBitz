namespace NetBitz
{
    partial class MainWindow
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
        	this.button1 = new System.Windows.Forms.Button();
        	this.listBox1 = new System.Windows.Forms.ListBox();
        	this.textBox1 = new System.Windows.Forms.TextBox();
        	this.label1 = new System.Windows.Forms.Label();
        	this.label2 = new System.Windows.Forms.Label();
        	this.label3 = new System.Windows.Forms.Label();
        	this.checkBox1 = new System.Windows.Forms.CheckBox();
        	this.label4 = new System.Windows.Forms.Label();
        	this.label5 = new System.Windows.Forms.Label();
        	this.button2 = new System.Windows.Forms.Button();
        	this.SuspendLayout();
        	// 
        	// button1
        	// 
        	this.button1.BackColor = System.Drawing.Color.Black;
        	this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.button1.ForeColor = System.Drawing.Color.White;
        	this.button1.Location = new System.Drawing.Point(188, 292);
        	this.button1.Name = "button1";
        	this.button1.Size = new System.Drawing.Size(81, 33);
        	this.button1.TabIndex = 0;
        	this.button1.Text = "Pack File";
        	this.button1.UseVisualStyleBackColor = false;
        	this.button1.Click += new System.EventHandler(this.button1_Click);
        	// 
        	// listBox1
        	// 
        	this.listBox1.AllowDrop = true;
        	this.listBox1.BackColor = System.Drawing.Color.Black;
        	this.listBox1.ForeColor = System.Drawing.Color.White;
        	this.listBox1.FormattingEnabled = true;
        	this.listBox1.ItemHeight = 17;
        	this.listBox1.Location = new System.Drawing.Point(22, 61);
        	this.listBox1.Name = "listBox1";
        	this.listBox1.Size = new System.Drawing.Size(403, 89);
        	this.listBox1.TabIndex = 1;
        	this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
        	this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
        	// 
        	// textBox1
        	// 
        	this.textBox1.BackColor = System.Drawing.Color.Black;
        	this.textBox1.ForeColor = System.Drawing.Color.White;
        	this.textBox1.Location = new System.Drawing.Point(58, 224);
        	this.textBox1.Name = "textBox1";
        	this.textBox1.Size = new System.Drawing.Size(367, 25);
        	this.textBox1.TabIndex = 2;
        	this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.BackColor = System.Drawing.Color.Black;
        	this.label1.ForeColor = System.Drawing.Color.White;
        	this.label1.Location = new System.Drawing.Point(21, 227);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(31, 19);
        	this.label1.TabIndex = 3;
        	this.label1.Text = "Key";
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.BackColor = System.Drawing.Color.Black;
        	this.label2.ForeColor = System.Drawing.Color.White;
        	this.label2.Location = new System.Drawing.Point(21, 39);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(134, 19);
        	this.label2.TabIndex = 4;
        	this.label2.Text = "Drag and drop a file.";
        	// 
        	// label3
        	// 
        	this.label3.AutoSize = true;
        	this.label3.BackColor = System.Drawing.Color.Black;
        	this.label3.ForeColor = System.Drawing.Color.White;
        	this.label3.Location = new System.Drawing.Point(22, 300);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(49, 19);
        	this.label3.TabIndex = 5;
        	this.label3.Text = "Ready.";
        	// 
        	// checkBox1
        	// 
        	this.checkBox1.AutoSize = true;
        	this.checkBox1.BackColor = System.Drawing.Color.DimGray;
        	this.checkBox1.Enabled = false;
        	this.checkBox1.ForeColor = System.Drawing.Color.White;
        	this.checkBox1.Location = new System.Drawing.Point(21, 195);
        	this.checkBox1.Name = "checkBox1";
        	this.checkBox1.Size = new System.Drawing.Size(101, 23);
        	this.checkBox1.TabIndex = 6;
        	this.checkBox1.Text = "Encrypt File";
        	this.checkBox1.UseVisualStyleBackColor = false;
        	this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
        	// 
        	// label4
        	// 
        	this.label4.AutoSize = true;
        	this.label4.BackColor = System.Drawing.Color.Black;
        	this.label4.ForeColor = System.Drawing.Color.White;
        	this.label4.Location = new System.Drawing.Point(390, 335);
        	this.label4.Name = "label4";
        	this.label4.Size = new System.Drawing.Size(35, 19);
        	this.label4.TabIndex = 7;
        	this.label4.Text = "v1.8";
        	// 
        	// label5
        	// 
        	this.label5.AutoSize = true;
        	this.label5.BackColor = System.Drawing.Color.Black;
        	this.label5.ForeColor = System.Drawing.Color.White;
        	this.label5.Location = new System.Drawing.Point(21, 335);
        	this.label5.Name = "label5";
        	this.label5.Size = new System.Drawing.Size(123, 19);
        	this.label5.TabIndex = 8;
        	this.label5.Text = "(c) 2016, 0xFireball";
        	// 
        	// button2
        	// 
        	this.button2.BackColor = System.Drawing.Color.Black;
        	this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.button2.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.button2.ForeColor = System.Drawing.Color.White;
        	this.button2.Location = new System.Drawing.Point(344, 255);
        	this.button2.Name = "button2";
        	this.button2.Size = new System.Drawing.Size(81, 25);
        	this.button2.TabIndex = 9;
        	this.button2.Text = "Random Key";
        	this.button2.UseVisualStyleBackColor = false;
        	this.button2.Click += new System.EventHandler(this.Button2Click);
        	// 
        	// MainWindow
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(446, 375);
        	this.Controls.Add(this.button2);
        	this.Controls.Add(this.label5);
        	this.Controls.Add(this.label4);
        	this.Controls.Add(this.checkBox1);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.textBox1);
        	this.Controls.Add(this.listBox1);
        	this.Controls.Add(this.button1);
        	this.DisplayHeader = false;
        	this.DisplayTitle = true;
        	this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.Name = "MainWindow";
        	this.Padding = new System.Windows.Forms.Padding(18, 32, 18, 21);
        	this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
        	this.Text = "NetBitz GUI";
        	this.Theme = MetroFramework.MetroThemeStyle.Dark;
        	this.TitleColor = System.Drawing.Color.White;
        	this.ResumeLayout(false);
        	this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
    }
}

