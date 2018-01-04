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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUpdateNumber = new System.Windows.Forms.Button();
            this.grdDevices = new System.Windows.Forms.DataGridView();
            this.BtnUpdateAll = new System.Windows.Forms.Button();
            this.BtnUpdate = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblDevicesCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdDevices)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 433);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(124, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(144, 433);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(124, 23);
            this.btnDisconnect.TabIndex = 5;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.BtnDisconnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Available Device/s:";
            // 
            // btnUpdateNumber
            // 
            this.btnUpdateNumber.Location = new System.Drawing.Point(12, 462);
            this.btnUpdateNumber.Name = "btnUpdateNumber";
            this.btnUpdateNumber.Size = new System.Drawing.Size(256, 23);
            this.btnUpdateNumber.TabIndex = 11;
            this.btnUpdateNumber.Text = "Update Device Number";
            this.btnUpdateNumber.UseVisualStyleBackColor = true;
            this.btnUpdateNumber.Click += new System.EventHandler(this.BtnUpdateNumber_Click);
            // 
            // grdDevices
            // 
            this.grdDevices.AllowUserToAddRows = false;
            this.grdDevices.AllowUserToDeleteRows = false;
            this.grdDevices.AllowUserToResizeColumns = false;
            this.grdDevices.AllowUserToResizeRows = false;
            this.grdDevices.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grdDevices.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdDevices.CausesValidation = false;
            this.grdDevices.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDevices.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdDevices.Location = new System.Drawing.Point(15, 64);
            this.grdDevices.Name = "grdDevices";
            this.grdDevices.ReadOnly = true;
            this.grdDevices.RowHeadersVisible = false;
            this.grdDevices.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdDevices.ShowCellErrors = false;
            this.grdDevices.ShowCellToolTips = false;
            this.grdDevices.ShowEditingIcon = false;
            this.grdDevices.ShowRowErrors = false;
            this.grdDevices.Size = new System.Drawing.Size(252, 363);
            this.grdDevices.TabIndex = 12;
            this.grdDevices.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdDevices_CellDoubleClick);
            this.grdDevices.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdDevices_DataError);
            this.grdDevices.SelectionChanged += new System.EventHandler(this.grdDevices_SelectionChanged);
            // 
            // BtnUpdateAll
            // 
            this.BtnUpdateAll.Location = new System.Drawing.Point(190, 35);
            this.BtnUpdateAll.Name = "BtnUpdateAll";
            this.BtnUpdateAll.Size = new System.Drawing.Size(78, 23);
            this.BtnUpdateAll.TabIndex = 14;
            this.BtnUpdateAll.Text = "Update All";
            this.BtnUpdateAll.UseVisualStyleBackColor = true;
            this.BtnUpdateAll.Click += new System.EventHandler(this.BtnUpdateAll_Click);
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Location = new System.Drawing.Point(102, 35);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(78, 23);
            this.BtnUpdate.TabIndex = 13;
            this.BtnUpdate.Text = "Update";
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(15, 35);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(78, 23);
            this.btnRefresh.TabIndex = 15;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // lblDevicesCount
            // 
            this.lblDevicesCount.AutoSize = true;
            this.lblDevicesCount.Location = new System.Drawing.Point(115, 9);
            this.lblDevicesCount.Name = "lblDevicesCount";
            this.lblDevicesCount.Size = new System.Drawing.Size(0, 13);
            this.lblDevicesCount.TabIndex = 16;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 506);
            this.Controls.Add(this.lblDevicesCount);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.BtnUpdateAll);
            this.Controls.Add(this.BtnUpdate);
            this.Controls.Add(this.grdDevices);
            this.Controls.Add(this.btnUpdateNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMS Reciever";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.grdDevices)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpdateNumber;
        private System.Windows.Forms.DataGridView grdDevices;
        private System.Windows.Forms.Button BtnUpdateAll;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblDevicesCount;
    }
}