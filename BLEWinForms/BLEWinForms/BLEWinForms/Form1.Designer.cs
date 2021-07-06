
namespace BLEWinForms
{
    partial class Form1
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.connectionLabel = new System.Windows.Forms.Label();
            this.deviceListView = new System.Windows.Forms.ComboBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.scanButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.streamButton = new System.Windows.Forms.Button();
            this.subjectNumberBox = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.udpPortBox = new System.Windows.Forms.TextBox();
            this.outputHistoryBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.61702F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.38298F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.scanButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 563F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1423, 827);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.connectionLabel);
            this.groupBox1.Controls.Add(this.deviceListView);
            this.groupBox1.Controls.Add(this.connectButton);
            this.groupBox1.Location = new System.Drawing.Point(339, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1081, 210);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // connectionLabel
            // 
            this.connectionLabel.AutoSize = true;
            this.connectionLabel.ForeColor = System.Drawing.Color.Red;
            this.connectionLabel.Location = new System.Drawing.Point(873, 136);
            this.connectionLabel.Name = "connectionLabel";
            this.connectionLabel.Size = new System.Drawing.Size(199, 41);
            this.connectionLabel.TabIndex = 2;
            this.connectionLabel.Text = "Disconnected";
            this.connectionLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // deviceListView
            // 
            this.deviceListView.FormattingEnabled = true;
            this.deviceListView.Location = new System.Drawing.Point(32, 46);
            this.deviceListView.Name = "deviceListView";
            this.deviceListView.Size = new System.Drawing.Size(823, 49);
            this.deviceListView.TabIndex = 1;
            this.deviceListView.SelectedIndexChanged += new System.EventHandler(this.deviceListView_SelectedIndexChanged);
            // 
            // connectButton
            // 
            this.connectButton.Enabled = false;
            this.connectButton.Location = new System.Drawing.Point(884, 46);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(188, 57);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // scanButton
            // 
            this.scanButton.Location = new System.Drawing.Point(50, 50);
            this.scanButton.Margin = new System.Windows.Forms.Padding(50);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(188, 55);
            this.scanButton.TabIndex = 1;
            this.scanButton.Text = "Start Scan";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.outputHistoryBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.streamButton);
            this.groupBox2.Controls.Add(this.subjectNumberBox);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(339, 219);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1081, 557);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(308, 41);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter subject number:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // streamButton
            // 
            this.streamButton.Enabled = false;
            this.streamButton.Location = new System.Drawing.Point(32, 138);
            this.streamButton.Name = "streamButton";
            this.streamButton.Size = new System.Drawing.Size(264, 58);
            this.streamButton.TabIndex = 2;
            this.streamButton.Text = "Begin Streaming";
            this.streamButton.UseVisualStyleBackColor = true;
            this.streamButton.Click += new System.EventHandler(this.streamButton_Click);
            // 
            // subjectNumberBox
            // 
            this.subjectNumberBox.Location = new System.Drawing.Point(346, 56);
            this.subjectNumberBox.Name = "subjectNumberBox";
            this.subjectNumberBox.Size = new System.Drawing.Size(155, 47);
            this.subjectNumberBox.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip, 2);
            this.statusStrip.Location = new System.Drawing.Point(3, 779);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(0, 41);
            this.statusStrip.TabIndex = 5;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.udpPortBox);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(3, 219);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(330, 430);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 41);
            this.label2.TabIndex = 0;
            this.label2.Text = "Enter UDP Port:";
            // 
            // udpPortBox
            // 
            this.udpPortBox.Location = new System.Drawing.Point(9, 87);
            this.udpPortBox.Name = "udpPortBox";
            this.udpPortBox.Size = new System.Drawing.Size(250, 47);
            this.udpPortBox.TabIndex = 1;
            // 
            // outputHistoryBox
            // 
            this.outputHistoryBox.Enabled = false;
            this.outputHistoryBox.Location = new System.Drawing.Point(32, 217);
            this.outputHistoryBox.Multiline = true;
            this.outputHistoryBox.Name = "outputHistoryBox";
            this.outputHistoryBox.Size = new System.Drawing.Size(1000, 304);
            this.outputHistoryBox.TabIndex = 4;
            this.outputHistoryBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1423, 827);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.ComboBox deviceListView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button streamButton;
        private System.Windows.Forms.TextBox subjectNumberBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label statusStrip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label connectionLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox udpPortBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox outputHistoryBox;
    }
}