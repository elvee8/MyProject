namespace FormSMSMultipleInstance
{
    partial class Main
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
            this.lstAvailableDevice = new System.Windows.Forms.ListBox();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.GridSms = new System.Windows.Forms.DataGridView();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnCheckSMS = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GridSms)).BeginInit();
            this.SuspendLayout();
            // 
            // lstAvailableDevice
            // 
            this.lstAvailableDevice.FormattingEnabled = true;
            this.lstAvailableDevice.Location = new System.Drawing.Point(29, 40);
            this.lstAvailableDevice.Name = "lstAvailableDevice";
            this.lstAvailableDevice.Size = new System.Drawing.Size(152, 420);
            this.lstAvailableDevice.TabIndex = 1;
            this.lstAvailableDevice.SelectedIndexChanged += new System.EventHandler(this.LstAvailableDevice_SelectedIndexChanged);
            // 
            // txtMessage
            // 
            this.txtMessage.Enabled = false;
            this.txtMessage.Location = new System.Drawing.Point(199, 328);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(415, 131);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.Text = "";
            // 
            // GridSms
            // 
            this.GridSms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridSms.Location = new System.Drawing.Point(199, 108);
            this.GridSms.Name = "GridSms";
            this.GridSms.Size = new System.Drawing.Size(415, 214);
            this.GridSms.TabIndex = 2;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(199, 40);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(110, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // btnCheckSMS
            // 
            this.btnCheckSMS.Location = new System.Drawing.Point(202, 471);
            this.btnCheckSMS.Name = "btnCheckSMS";
            this.btnCheckSMS.Size = new System.Drawing.Size(110, 23);
            this.btnCheckSMS.TabIndex = 4;
            this.btnCheckSMS.Text = "Check Message";
            this.btnCheckSMS.UseVisualStyleBackColor = true;
            this.btnCheckSMS.Click += new System.EventHandler(this.BtnCheckSMS_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(199, 69);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(110, 23);
            this.btnDisconnect.TabIndex = 5;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.BtnDisconnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Available Device";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(359, 471);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Delete Message";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(507, 471);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Delete all Message";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 472);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "* Connected Device";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 506);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnCheckSMS);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.GridSms);
            this.Controls.Add(this.lstAvailableDevice);
            this.Controls.Add(this.txtMessage);
            this.Name = "Main";
            this.Text = "SMS Reciever";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.GridSms)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstAvailableDevice;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.DataGridView GridSms;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnCheckSMS;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
    }
}