
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
            components = new System.ComponentModel.Container();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openLogFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            statusStrip = new System.Windows.Forms.Label();
            groupBox3 = new System.Windows.Forms.GroupBox();
            outputText = new System.Windows.Forms.TextBox();
            outputHistoryBox = new System.Windows.Forms.TextBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            zedGraph1 = new ZedGraph.ZedGraphControl();
            groupBox4 = new System.Windows.Forms.GroupBox();
            label4 = new System.Windows.Forms.Label();
            statusUptimeLabel = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            statusPacketsLabel = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            statusLastDataLabel = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            statusDataRateLabel = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            panel1 = new System.Windows.Forms.Panel();
            characteristicListBox = new System.Windows.Forms.ListBox();
            label8 = new System.Windows.Forms.Label();
            scanButton = new System.Windows.Forms.Button();
            udpPortBox = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            streamButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            subjectNumberBox = new System.Windows.Forms.TextBox();
            connectionLabel = new System.Windows.Forms.Label();
            deviceListView = new System.Windows.Forms.ComboBox();
            connectButton = new System.Windows.Forms.Button();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            menuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(1256, 33);
            menuStrip1.TabIndex = 6;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { openLogFolderToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // openLogFolderToolStripMenuItem
            // 
            openLogFolderToolStripMenuItem.Name = "openLogFolderToolStripMenuItem";
            openLogFolderToolStripMenuItem.Size = new System.Drawing.Size(248, 34);
            openLogFolderToolStripMenuItem.Text = "Open Log Folder";
            openLogFolderToolStripMenuItem.Click += openLogFolderToolStripMenuItem_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.61702F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.38298F));
            tableLayoutPanel1.Controls.Add(statusStrip, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox2, 1, 1);
            tableLayoutPanel1.Controls.Add(groupBox4, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox1, 1, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 33);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 343F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1256, 643);
            tableLayoutPanel1.TabIndex = 0;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint_1;
            // 
            // statusStrip
            // 
            statusStrip.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(statusStrip, 2);
            statusStrip.Location = new System.Drawing.Point(2, 614);
            statusStrip.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(0, 25);
            statusStrip.TabIndex = 5;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(outputText);
            groupBox3.Controls.Add(outputHistoryBox);
            groupBox3.Location = new System.Drawing.Point(2, 273);
            groupBox3.Margin = new System.Windows.Forms.Padding(2);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(2);
            groupBox3.Size = new System.Drawing.Size(193, 328);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Streaming Values";
            // 
            // outputText
            // 
            outputText.Enabled = false;
            outputText.Location = new System.Drawing.Point(15, 49);
            outputText.Margin = new System.Windows.Forms.Padding(2);
            outputText.Multiline = true;
            outputText.Name = "outputText";
            outputText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            outputText.Size = new System.Drawing.Size(166, 251);
            outputText.TabIndex = 5;
            // 
            // outputHistoryBox
            // 
            outputHistoryBox.Enabled = false;
            outputHistoryBox.Location = new System.Drawing.Point(15, 101);
            outputHistoryBox.Margin = new System.Windows.Forms.Padding(2);
            outputHistoryBox.Multiline = true;
            outputHistoryBox.Name = "outputHistoryBox";
            outputHistoryBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            outputHistoryBox.Size = new System.Drawing.Size(166, 206);
            outputHistoryBox.TabIndex = 5;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(zedGraph1);
            groupBox2.Location = new System.Drawing.Point(298, 273);
            groupBox2.Margin = new System.Windows.Forms.Padding(2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(2);
            groupBox2.Size = new System.Drawing.Size(698, 339);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Enter += groupBox2_Enter;
            // 
            // zedGraph1
            // 
            zedGraph1.Location = new System.Drawing.Point(21, 32);
            zedGraph1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            zedGraph1.Name = "zedGraph1";
            zedGraph1.ScrollGrace = 0D;
            zedGraph1.ScrollMaxX = 0D;
            zedGraph1.ScrollMaxY = 0D;
            zedGraph1.ScrollMaxY2 = 0D;
            zedGraph1.ScrollMinX = 0D;
            zedGraph1.ScrollMinY = 0D;
            zedGraph1.ScrollMinY2 = 0D;
            zedGraph1.Size = new System.Drawing.Size(656, 289);
            zedGraph1.TabIndex = 5;
            zedGraph1.UseExtendedPrintDialog = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(statusUptimeLabel);
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(statusPacketsLabel);
            groupBox4.Controls.Add(label6);
            groupBox4.Controls.Add(statusLastDataLabel);
            groupBox4.Controls.Add(label7);
            groupBox4.Controls.Add(statusDataRateLabel);
            groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox4.Location = new System.Drawing.Point(2, 2);
            groupBox4.Margin = new System.Windows.Forms.Padding(2);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(2);
            groupBox4.Size = new System.Drawing.Size(292, 267);
            groupBox4.TabIndex = 7;
            groupBox4.TabStop = false;
            groupBox4.Text = "Connection Status";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(8, 38);
            label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(74, 25);
            label4.TabIndex = 0;
            label4.Text = "Uptime:";
            // 
            // statusUptimeLabel
            // 
            statusUptimeLabel.AutoSize = true;
            statusUptimeLabel.Location = new System.Drawing.Point(92, 38);
            statusUptimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            statusUptimeLabel.Name = "statusUptimeLabel";
            statusUptimeLabel.Size = new System.Drawing.Size(46, 25);
            statusUptimeLabel.TabIndex = 1;
            statusUptimeLabel.Text = "0:00";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(8, 67);
            label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(74, 25);
            label5.TabIndex = 2;
            label5.Text = "Packets:";
            // 
            // statusPacketsLabel
            // 
            statusPacketsLabel.AutoSize = true;
            statusPacketsLabel.Location = new System.Drawing.Point(92, 67);
            statusPacketsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            statusPacketsLabel.Name = "statusPacketsLabel";
            statusPacketsLabel.Size = new System.Drawing.Size(22, 25);
            statusPacketsLabel.TabIndex = 3;
            statusPacketsLabel.Text = "0";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(8, 96);
            label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(89, 25);
            label6.TabIndex = 4;
            label6.Text = "Last Data:";
            // 
            // statusLastDataLabel
            // 
            statusLastDataLabel.AutoSize = true;
            statusLastDataLabel.Location = new System.Drawing.Point(104, 96);
            statusLastDataLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            statusLastDataLabel.Name = "statusLastDataLabel";
            statusLastDataLabel.Size = new System.Drawing.Size(58, 25);
            statusLastDataLabel.TabIndex = 5;
            statusLastDataLabel.Text = "Never";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(8, 125);
            label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(93, 25);
            label7.TabIndex = 6;
            label7.Text = "Data Rate:";
            // 
            // statusDataRateLabel
            // 
            statusDataRateLabel.AutoSize = true;
            statusDataRateLabel.Location = new System.Drawing.Point(104, 125);
            statusDataRateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            statusDataRateLabel.Name = "statusDataRateLabel";
            statusDataRateLabel.Size = new System.Drawing.Size(62, 25);
            statusDataRateLabel.TabIndex = 7;
            statusDataRateLabel.Text = "0.0 Hz";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(panel1);
            groupBox1.Controls.Add(scanButton);
            groupBox1.Controls.Add(udpPortBox);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(streamButton);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(subjectNumberBox);
            groupBox1.Controls.Add(connectionLabel);
            groupBox1.Controls.Add(deviceListView);
            groupBox1.Controls.Add(connectButton);
            groupBox1.Location = new System.Drawing.Point(298, 2);
            groupBox1.Margin = new System.Windows.Forms.Padding(2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(2);
            groupBox1.Size = new System.Drawing.Size(956, 267);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Enter += groupBox1_Enter_1;
            // 
            // panel1
            // 
            panel1.Controls.Add(characteristicListBox);
            panel1.Controls.Add(label8);
            panel1.Location = new System.Drawing.Point(703, 12);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(255, 251);
            panel1.TabIndex = 13;
            // 
            // characteristicListBox
            // 
            characteristicListBox.FormattingEnabled = true;
            characteristicListBox.ItemHeight = 25;
            characteristicListBox.Location = new System.Drawing.Point(7, 51);
            characteristicListBox.Margin = new System.Windows.Forms.Padding(2);
            characteristicListBox.Name = "characteristicListBox";
            characteristicListBox.Size = new System.Drawing.Size(242, 204);
            characteristicListBox.TabIndex = 11;
            characteristicListBox.SelectedIndexChanged += characteristicListBox_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(24, 14);
            label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(128, 25);
            label8.TabIndex = 10;
            label8.Text = "Characteristics:";
            // 
            // scanButton
            // 
            scanButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            scanButton.Location = new System.Drawing.Point(332, 33);
            scanButton.Margin = new System.Windows.Forms.Padding(2);
            scanButton.Name = "scanButton";
            scanButton.Size = new System.Drawing.Size(117, 35);
            scanButton.TabIndex = 12;
            scanButton.Text = "Start Scan";
            scanButton.UseVisualStyleBackColor = true;
            scanButton.Click += scanButton_Click;
            // 
            // udpPortBox
            // 
            udpPortBox.Location = new System.Drawing.Point(246, 125);
            udpPortBox.Margin = new System.Windows.Forms.Padding(2);
            udpPortBox.Name = "udpPortBox";
            udpPortBox.Size = new System.Drawing.Size(142, 31);
            udpPortBox.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(19, 129);
            label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(223, 25);
            label2.TabIndex = 8;
            label2.Text = "UDP Marker Listening Port:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(19, 31);
            label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(68, 25);
            label3.TabIndex = 7;
            label3.Text = "Device:";
            // 
            // streamButton
            // 
            streamButton.Location = new System.Drawing.Point(331, 79);
            streamButton.Margin = new System.Windows.Forms.Padding(2);
            streamButton.Name = "streamButton";
            streamButton.Size = new System.Drawing.Size(118, 35);
            streamButton.TabIndex = 6;
            streamButton.Text = "Record";
            streamButton.UseVisualStyleBackColor = true;
            streamButton.Click += streamButton_Click_1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(19, 84);
            label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(97, 25);
            label1.TabIndex = 5;
            label1.Text = "Subject ID:";
            // 
            // subjectNumberBox
            // 
            subjectNumberBox.Location = new System.Drawing.Point(131, 82);
            subjectNumberBox.Margin = new System.Windows.Forms.Padding(2);
            subjectNumberBox.Name = "subjectNumberBox";
            subjectNumberBox.Size = new System.Drawing.Size(185, 31);
            subjectNumberBox.TabIndex = 4;
            subjectNumberBox.Text = "TestSubject";
            // 
            // connectionLabel
            // 
            connectionLabel.AutoSize = true;
            connectionLabel.ForeColor = System.Drawing.Color.Red;
            connectionLabel.Location = new System.Drawing.Point(453, 79);
            connectionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            connectionLabel.Name = "connectionLabel";
            connectionLabel.Size = new System.Drawing.Size(119, 25);
            connectionLabel.TabIndex = 2;
            connectionLabel.Text = "Disconnected";
            connectionLabel.Click += label2_Click;
            // 
            // deviceListView
            // 
            deviceListView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            deviceListView.FormattingEnabled = true;
            deviceListView.Location = new System.Drawing.Point(90, 31);
            deviceListView.Margin = new System.Windows.Forms.Padding(2);
            deviceListView.Name = "deviceListView";
            deviceListView.Size = new System.Drawing.Size(226, 33);
            deviceListView.TabIndex = 1;
            deviceListView.SelectedIndexChanged += deviceListView_SelectedIndexChanged;
            // 
            // connectButton
            // 
            connectButton.Enabled = false;
            connectButton.Location = new System.Drawing.Point(453, 33);
            connectButton.Margin = new System.Windows.Forms.Padding(2);
            connectButton.Name = "connectButton";
            connectButton.Size = new System.Drawing.Size(118, 35);
            connectButton.TabIndex = 0;
            connectButton.Text = "Connect";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1256, 676);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(2);
            Name = "Form1";
            Text = "BLE Heartrate Acquisition";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLogFolderToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button connectButton;
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
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label statusUptimeLabel;
        private System.Windows.Forms.Label statusPacketsLabel;
        private System.Windows.Forms.Label statusLastDataLabel;
        private System.Windows.Forms.Label statusDataRateLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox characteristicListBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button scanButton;
    }
}