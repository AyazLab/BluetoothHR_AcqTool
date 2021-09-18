
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.udpPortBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.streamButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.subjectNumberBox = new System.Windows.Forms.TextBox();
            this.connectionLabel = new System.Windows.Forms.Label();
            this.deviceListView = new System.Windows.Forms.ComboBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.scanButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.outputText = new System.Windows.Forms.TextBox();
            this.outputHistoryBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.zedGraph1 = new ZedGraph.ZedGraphControl();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 412F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1133, 662);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.udpPortBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.streamButton);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.subjectNumberBox);
            this.groupBox1.Controls.Add(this.connectionLabel);
            this.groupBox1.Controls.Add(this.deviceListView);
            this.groupBox1.Controls.Add(this.connectButton);
            this.groupBox1.Location = new System.Drawing.Point(269, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(763, 211);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // udpPortBox
            // 
            this.udpPortBox.Location = new System.Drawing.Point(209, 152);
            this.udpPortBox.Margin = new System.Windows.Forms.Padding(2);
            this.udpPortBox.Name = "udpPortBox";
            this.udpPortBox.Size = new System.Drawing.Size(169, 35);
            this.udpPortBox.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 155);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 30);
            this.label2.TabIndex = 8;
            this.label2.Text = "UDP Marker Port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 30);
            this.label3.TabIndex = 7;
            this.label3.Text = "Device:";
            // 
            // streamButton
            // 
            this.streamButton.Location = new System.Drawing.Point(397, 95);
            this.streamButton.Margin = new System.Windows.Forms.Padding(2);
            this.streamButton.Name = "streamButton";
            this.streamButton.Size = new System.Drawing.Size(141, 42);
            this.streamButton.TabIndex = 6;
            this.streamButton.Text = "Record";
            this.streamButton.UseVisualStyleBackColor = true;
            this.streamButton.Click += new System.EventHandler(this.streamButton_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 101);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "Subject ID:";
            // 
            // subjectNumberBox
            // 
            this.subjectNumberBox.Location = new System.Drawing.Point(157, 98);
            this.subjectNumberBox.Margin = new System.Windows.Forms.Padding(2);
            this.subjectNumberBox.Name = "subjectNumberBox";
            this.subjectNumberBox.Size = new System.Drawing.Size(221, 35);
            this.subjectNumberBox.TabIndex = 4;
            this.subjectNumberBox.Text = "TestSubject";
            // 
            // connectionLabel
            // 
            this.connectionLabel.AutoSize = true;
            this.connectionLabel.ForeColor = System.Drawing.Color.Red;
            this.connectionLabel.Location = new System.Drawing.Point(607, 37);
            this.connectionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.connectionLabel.Name = "connectionLabel";
            this.connectionLabel.Size = new System.Drawing.Size(139, 30);
            this.connectionLabel.TabIndex = 2;
            this.connectionLabel.Text = "Disconnected";
            this.connectionLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // deviceListView
            // 
            this.deviceListView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceListView.FormattingEnabled = true;
            this.deviceListView.Location = new System.Drawing.Point(108, 37);
            this.deviceListView.Margin = new System.Windows.Forms.Padding(2);
            this.deviceListView.Name = "deviceListView";
            this.deviceListView.Size = new System.Drawing.Size(270, 38);
            this.deviceListView.TabIndex = 1;
            this.deviceListView.SelectedIndexChanged += new System.EventHandler(this.deviceListView_SelectedIndexChanged);
            // 
            // connectButton
            // 
            this.connectButton.Enabled = false;
            this.connectButton.Location = new System.Drawing.Point(397, 33);
            this.connectButton.Margin = new System.Windows.Forms.Padding(2);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(141, 42);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // scanButton
            // 
            this.scanButton.Location = new System.Drawing.Point(35, 37);
            this.scanButton.Margin = new System.Windows.Forms.Padding(35, 37, 35, 37);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(133, 40);
            this.scanButton.TabIndex = 1;
            this.scanButton.Text = "Start Scan";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip, 2);
            this.statusStrip.Location = new System.Drawing.Point(2, 627);
            this.statusStrip.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(0, 30);
            this.statusStrip.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.outputText);
            this.groupBox3.Controls.Add(this.outputHistoryBox);
            this.groupBox3.Location = new System.Drawing.Point(2, 217);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(232, 393);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Streaming Values";
            // 
            // outputText
            // 
            this.outputText.Enabled = false;
            this.outputText.Location = new System.Drawing.Point(18, 59);
            this.outputText.Margin = new System.Windows.Forms.Padding(2);
            this.outputText.Multiline = true;
            this.outputText.Name = "outputText";
            this.outputText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputText.Size = new System.Drawing.Size(199, 300);
            this.outputText.TabIndex = 5;
            // 
            // outputHistoryBox
            // 
            this.outputHistoryBox.Enabled = false;
            this.outputHistoryBox.Location = new System.Drawing.Point(18, 121);
            this.outputHistoryBox.Margin = new System.Windows.Forms.Padding(2);
            this.outputHistoryBox.Multiline = true;
            this.outputHistoryBox.Name = "outputHistoryBox";
            this.outputHistoryBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputHistoryBox.Size = new System.Drawing.Size(199, 246);
            this.outputHistoryBox.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.zedGraph1);
            this.groupBox2.Location = new System.Drawing.Point(269, 217);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(838, 408);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // zedGraph1
            // 
            this.zedGraph1.Location = new System.Drawing.Point(25, 38);
            this.zedGraph1.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.zedGraph1.Name = "zedGraph1";
            this.zedGraph1.ScrollGrace = 0D;
            this.zedGraph1.ScrollMaxX = 0D;
            this.zedGraph1.ScrollMaxY = 0D;
            this.zedGraph1.ScrollMaxY2 = 0D;
            this.zedGraph1.ScrollMinX = 0D;
            this.zedGraph1.ScrollMinY = 0D;
            this.zedGraph1.ScrollMinY2 = 0D;
            this.zedGraph1.Size = new System.Drawing.Size(787, 347);
            this.zedGraph1.TabIndex = 5;
            this.zedGraph1.UseExtendedPrintDialog = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 662);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "BLE Heartrate Acquisition";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.ComboBox deviceListView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox subjectNumberBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label statusStrip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label connectionLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button streamButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox udpPortBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox outputHistoryBox;
        private ZedGraph.ZedGraphControl zedGraph1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox outputText;
    }
}