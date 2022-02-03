namespace CardTestC
{
    partial class formMain
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
            this.labelCommPort = new System.Windows.Forms.Label();
            this.comboPorts = new System.Windows.Forms.ComboBox();
            this.labelCommStatus = new System.Windows.Forms.Label();
            this.labelTrack1 = new System.Windows.Forms.Label();
            this.labelTrack2 = new System.Windows.Forms.Label();
            this.labelTrack3 = new System.Windows.Forms.Label();
            this.textTrack1 = new System.Windows.Forms.TextBox();
            this.textTrack2 = new System.Windows.Forms.TextBox();
            this.textTrack3 = new System.Windows.Forms.TextBox();
            this.cmdClear = new System.Windows.Forms.Button();
            this.labelQuickSelect = new System.Windows.Forms.Label();
            this.comboQuickSelect = new System.Windows.Forms.ComboBox();
            this.cmdRead = new System.Windows.Forms.Button();
            this.cmdWrite = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdInstructions = new System.Windows.Forms.Button();
            this.cmdTest = new System.Windows.Forms.Button();
            this.labelCommunicationMonitor = new System.Windows.Forms.Label();
            this.textMonitor = new System.Windows.Forms.TextBox();
            this.optionHex = new System.Windows.Forms.RadioButton();
            this.optionASCII = new System.Windows.Forms.RadioButton();
            this.cmdClearMonitor = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelCommPort
            // 
            this.labelCommPort.AutoSize = true;
            this.labelCommPort.Location = new System.Drawing.Point(12, 44);
            this.labelCommPort.Name = "labelCommPort";
            this.labelCommPort.Size = new System.Drawing.Size(77, 13);
            this.labelCommPort.TabIndex = 0;
            this.labelCommPort.Text = "labelCommPort";
            // 
            // comboPorts
            // 
            this.comboPorts.FormattingEnabled = true;
            this.comboPorts.Location = new System.Drawing.Point(96, 41);
            this.comboPorts.Name = "comboPorts";
            this.comboPorts.Size = new System.Drawing.Size(156, 21);
            this.comboPorts.TabIndex = 1;
            this.comboPorts.SelectedIndexChanged += new System.EventHandler(this.comboPorts_SelectedIndexChanged);
            // 
            // labelCommStatus
            // 
            this.labelCommStatus.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelCommStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCommStatus.Location = new System.Drawing.Point(16, 71);
            this.labelCommStatus.Name = "labelCommStatus";
            this.labelCommStatus.Size = new System.Drawing.Size(236, 35);
            this.labelCommStatus.TabIndex = 2;
            this.labelCommStatus.Text = "Comm port status: closed";
            // 
            // labelTrack1
            // 
            this.labelTrack1.AutoSize = true;
            this.labelTrack1.Location = new System.Drawing.Point(26, 126);
            this.labelTrack1.Name = "labelTrack1";
            this.labelTrack1.Size = new System.Drawing.Size(63, 13);
            this.labelTrack1.TabIndex = 3;
            this.labelTrack1.Text = "labelTrack1";
            // 
            // labelTrack2
            // 
            this.labelTrack2.AutoSize = true;
            this.labelTrack2.Location = new System.Drawing.Point(26, 153);
            this.labelTrack2.Name = "labelTrack2";
            this.labelTrack2.Size = new System.Drawing.Size(63, 13);
            this.labelTrack2.TabIndex = 4;
            this.labelTrack2.Text = "labelTrack2";
            // 
            // labelTrack3
            // 
            this.labelTrack3.AutoSize = true;
            this.labelTrack3.Location = new System.Drawing.Point(26, 180);
            this.labelTrack3.Name = "labelTrack3";
            this.labelTrack3.Size = new System.Drawing.Size(63, 13);
            this.labelTrack3.TabIndex = 5;
            this.labelTrack3.Text = "labelTrack3";
            // 
            // textTrack1
            // 
            this.textTrack1.Location = new System.Drawing.Point(96, 123);
            this.textTrack1.Name = "textTrack1";
            this.textTrack1.Size = new System.Drawing.Size(156, 20);
            this.textTrack1.TabIndex = 6;
            // 
            // textTrack2
            // 
            this.textTrack2.Location = new System.Drawing.Point(96, 150);
            this.textTrack2.Name = "textTrack2";
            this.textTrack2.Size = new System.Drawing.Size(156, 20);
            this.textTrack2.TabIndex = 7;
            // 
            // textTrack3
            // 
            this.textTrack3.Location = new System.Drawing.Point(96, 177);
            this.textTrack3.Name = "textTrack3";
            this.textTrack3.Size = new System.Drawing.Size(156, 20);
            this.textTrack3.TabIndex = 8;
            // 
            // cmdClear
            // 
            this.cmdClear.Location = new System.Drawing.Point(177, 203);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(75, 23);
            this.cmdClear.TabIndex = 9;
            this.cmdClear.Text = "Clear";
            this.cmdClear.UseVisualStyleBackColor = true;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // labelQuickSelect
            // 
            this.labelQuickSelect.AutoSize = true;
            this.labelQuickSelect.Location = new System.Drawing.Point(2, 259);
            this.labelQuickSelect.Name = "labelQuickSelect";
            this.labelQuickSelect.Size = new System.Drawing.Size(87, 13);
            this.labelQuickSelect.TabIndex = 10;
            this.labelQuickSelect.Text = "labelQuickSelect";
            this.labelQuickSelect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboQuickSelect
            // 
            this.comboQuickSelect.FormattingEnabled = true;
            this.comboQuickSelect.Location = new System.Drawing.Point(96, 256);
            this.comboQuickSelect.Name = "comboQuickSelect";
            this.comboQuickSelect.Size = new System.Drawing.Size(156, 21);
            this.comboQuickSelect.TabIndex = 11;
            this.comboQuickSelect.SelectedIndexChanged += new System.EventHandler(this.comboQuickSelect_SelectedIndexChanged);
            // 
            // cmdRead
            // 
            this.cmdRead.Location = new System.Drawing.Point(16, 295);
            this.cmdRead.Name = "cmdRead";
            this.cmdRead.Size = new System.Drawing.Size(75, 23);
            this.cmdRead.TabIndex = 12;
            this.cmdRead.Text = "Read";
            this.cmdRead.UseVisualStyleBackColor = true;
            this.cmdRead.Click += new System.EventHandler(this.cmdRead_Click);
            // 
            // cmdWrite
            // 
            this.cmdWrite.Location = new System.Drawing.Point(96, 295);
            this.cmdWrite.Name = "cmdWrite";
            this.cmdWrite.Size = new System.Drawing.Size(75, 23);
            this.cmdWrite.TabIndex = 13;
            this.cmdWrite.Text = "Write";
            this.cmdWrite.UseVisualStyleBackColor = true;
            this.cmdWrite.Click += new System.EventHandler(this.cmdWrite_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.Location = new System.Drawing.Point(177, 295);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(75, 23);
            this.cmdExit.TabIndex = 14;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(16, 324);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(156, 23);
            this.cmdCancel.TabIndex = 15;
            this.cmdCancel.Text = "Cancel Read/Write";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdInstructions
            // 
            this.cmdInstructions.Location = new System.Drawing.Point(16, 364);
            this.cmdInstructions.Name = "cmdInstructions";
            this.cmdInstructions.Size = new System.Drawing.Size(75, 23);
            this.cmdInstructions.TabIndex = 16;
            this.cmdInstructions.Text = "Instructions";
            this.cmdInstructions.UseVisualStyleBackColor = true;
            this.cmdInstructions.Click += new System.EventHandler(this.cmdInstructions_Click);
            // 
            // cmdTest
            // 
            this.cmdTest.Location = new System.Drawing.Point(177, 361);
            this.cmdTest.Name = "cmdTest";
            this.cmdTest.Size = new System.Drawing.Size(75, 23);
            this.cmdTest.TabIndex = 17;
            this.cmdTest.Text = "Test";
            this.cmdTest.UseVisualStyleBackColor = true;
            this.cmdTest.Click += new System.EventHandler(this.cmdTest_Click);
            // 
            // labelCommunicationMonitor
            // 
            this.labelCommunicationMonitor.AutoSize = true;
            this.labelCommunicationMonitor.Location = new System.Drawing.Point(273, 26);
            this.labelCommunicationMonitor.Name = "labelCommunicationMonitor";
            this.labelCommunicationMonitor.Size = new System.Drawing.Size(117, 13);
            this.labelCommunicationMonitor.TabIndex = 18;
            this.labelCommunicationMonitor.Text = "Communication Monitor";
            // 
            // textMonitor
            // 
            this.textMonitor.Location = new System.Drawing.Point(276, 41);
            this.textMonitor.Multiline = true;
            this.textMonitor.Name = "textMonitor";
            this.textMonitor.Size = new System.Drawing.Size(320, 317);
            this.textMonitor.TabIndex = 19;
            // 
            // optionHex
            // 
            this.optionHex.AutoSize = true;
            this.optionHex.Location = new System.Drawing.Point(285, 367);
            this.optionHex.Name = "optionHex";
            this.optionHex.Size = new System.Drawing.Size(47, 17);
            this.optionHex.TabIndex = 20;
            this.optionHex.TabStop = true;
            this.optionHex.Text = "&HEX";
            this.optionHex.UseVisualStyleBackColor = true;
            this.optionHex.Click += new System.EventHandler(this.optionHex_Click);
            // 
            // optionASCII
            // 
            this.optionASCII.AutoSize = true;
            this.optionASCII.Location = new System.Drawing.Point(338, 367);
            this.optionASCII.Name = "optionASCII";
            this.optionASCII.Size = new System.Drawing.Size(52, 17);
            this.optionASCII.TabIndex = 21;
            this.optionASCII.TabStop = true;
            this.optionASCII.Text = "&ASCII";
            this.optionASCII.UseVisualStyleBackColor = true;
            this.optionASCII.Click += new System.EventHandler(this.optionASCII_Click);
            // 
            // cmdClearMonitor
            // 
            this.cmdClearMonitor.Location = new System.Drawing.Point(489, 364);
            this.cmdClearMonitor.Name = "cmdClearMonitor";
            this.cmdClearMonitor.Size = new System.Drawing.Size(107, 23);
            this.cmdClearMonitor.TabIndex = 22;
            this.cmdClearMonitor.Text = "Clear &Monitor";
            this.cmdClearMonitor.UseVisualStyleBackColor = true;
            this.cmdClearMonitor.Click += new System.EventHandler(this.cmdClearMonitor_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelMessage.Location = new System.Drawing.Point(16, 400);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(580, 35);
            this.labelMessage.TabIndex = 23;
            this.labelMessage.Text = "Message : Please open communication port on pull down menu.";
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 444);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.cmdClearMonitor);
            this.Controls.Add(this.optionASCII);
            this.Controls.Add(this.optionHex);
            this.Controls.Add(this.textMonitor);
            this.Controls.Add(this.labelCommunicationMonitor);
            this.Controls.Add(this.cmdTest);
            this.Controls.Add(this.cmdInstructions);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdWrite);
            this.Controls.Add(this.cmdRead);
            this.Controls.Add(this.comboQuickSelect);
            this.Controls.Add(this.labelQuickSelect);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.textTrack3);
            this.Controls.Add(this.textTrack2);
            this.Controls.Add(this.textTrack1);
            this.Controls.Add(this.labelTrack3);
            this.Controls.Add(this.labelTrack2);
            this.Controls.Add(this.labelTrack1);
            this.Controls.Add(this.labelCommStatus);
            this.Controls.Add(this.comboPorts);
            this.Controls.Add(this.labelCommPort);
            this.Name = "formMain";
            this.Text = "Next2020";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCommPort;
        private System.Windows.Forms.ComboBox comboPorts;
        private System.Windows.Forms.Label labelCommStatus;
        private System.Windows.Forms.Label labelTrack1;
        private System.Windows.Forms.Label labelTrack2;
        private System.Windows.Forms.Label labelTrack3;
        private System.Windows.Forms.TextBox textTrack1;
        private System.Windows.Forms.TextBox textTrack2;
        private System.Windows.Forms.TextBox textTrack3;
        private System.Windows.Forms.Button cmdClear;
        private System.Windows.Forms.Label labelQuickSelect;
        private System.Windows.Forms.ComboBox comboQuickSelect;
        private System.Windows.Forms.Button cmdRead;
        private System.Windows.Forms.Button cmdWrite;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdInstructions;
        private System.Windows.Forms.Button cmdTest;
        private System.Windows.Forms.Label labelCommunicationMonitor;
        private System.Windows.Forms.TextBox textMonitor;
        private System.Windows.Forms.RadioButton optionHex;
        private System.Windows.Forms.RadioButton optionASCII;
        private System.Windows.Forms.Button cmdClearMonitor;
        private System.Windows.Forms.Label labelMessage;
    }
}

