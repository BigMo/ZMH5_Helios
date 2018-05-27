namespace _ZMH5__Helios.UI.Controls
{
    partial class TriggerSettingsUI
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblName = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.cfgKey = new MetroFramework.Controls.MetroTextBox();
            this.cfgMode = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.cfgEnabled = new MetroFramework.Controls.MetroToggle();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.cfgDelay = new MetroFramework.Controls.MetroTrackBar();
            this.lblDelay = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.cfgBurstEnabled = new MetroFramework.Controls.MetroToggle();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.cfgBurstDelay = new MetroFramework.Controls.MetroTrackBar();
            this.lblBurstDelay = new MetroFramework.Controls.MetroLabel();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.cfgBurstCount = new MetroFramework.Controls.MetroTrackBar();
            this.lblBurstCount = new MetroFramework.Controls.MetroLabel();
            this.cfgKeySelection = new MetroFramework.Controls.MetroComboBox();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cfgKeySelection, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cfgEnabled, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel4, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.cfgKey, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel3, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.cfgMode, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cfgBurstEnabled, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel7, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.cfgDelay, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblDelay, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel5, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.cfgBurstDelay, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblBurstDelay, 4, 2);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel8, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.cfgBurstCount, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblBurstCount, 4, 3);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(470, 148);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // lblName
            // 
            this.lblName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblName.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.lblName, 4);
            this.lblName.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblName.Location = new System.Drawing.Point(195, 3);
            this.lblName.Margin = new System.Windows.Forms.Padding(3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(58, 19);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Trigger";
            // 
            // metroLabel4
            // 
            this.metroLabel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(79, 123);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(29, 19);
            this.metroLabel4.TabIndex = 0;
            this.metroLabel4.Text = "Key";
            // 
            // cfgKey
            // 
            this.cfgKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cfgKey.Location = new System.Drawing.Point(114, 121);
            this.cfgKey.Name = "cfgKey";
            this.cfgKey.Size = new System.Drawing.Size(121, 23);
            this.cfgKey.TabIndex = 3;
            // 
            // cfgMode
            // 
            this.cfgMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cfgMode.FormattingEnabled = true;
            this.cfgMode.ItemHeight = 23;
            this.cfgMode.Location = new System.Drawing.Point(114, 86);
            this.cfgMode.Name = "cfgMode";
            this.cfgMode.Size = new System.Drawing.Size(121, 29);
            this.cfgMode.TabIndex = 2;
            // 
            // metroLabel3
            // 
            this.metroLabel3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(3, 91);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(105, 19);
            this.metroLabel3.TabIndex = 0;
            this.metroLabel3.Text = "Activation Mode";
            // 
            // metroLabel2
            // 
            this.metroLabel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(52, 30);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(56, 19);
            this.metroLabel2.TabIndex = 0;
            this.metroLabel2.Text = "Enabled";
            // 
            // cfgEnabled
            // 
            this.cfgEnabled.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cfgEnabled.AutoSize = true;
            this.cfgEnabled.Location = new System.Drawing.Point(114, 31);
            this.cfgEnabled.Name = "cfgEnabled";
            this.cfgEnabled.Size = new System.Drawing.Size(80, 17);
            this.cfgEnabled.TabIndex = 1;
            this.cfgEnabled.Text = "Aus";
            this.cfgEnabled.UseVisualStyleBackColor = true;
            // 
            // metroLabel7
            // 
            this.metroLabel7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.Location = new System.Drawing.Point(277, 30);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(41, 19);
            this.metroLabel7.TabIndex = 0;
            this.metroLabel7.Text = "Delay";
            // 
            // cfgDelay
            // 
            this.cfgDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cfgDelay.BackColor = System.Drawing.Color.Transparent;
            this.cfgDelay.Location = new System.Drawing.Point(324, 28);
            this.cfgDelay.Maximum = 500;
            this.cfgDelay.Name = "cfgDelay";
            this.cfgDelay.Size = new System.Drawing.Size(121, 23);
            this.cfgDelay.TabIndex = 4;
            this.cfgDelay.Text = "metroTrackBar1";
            this.cfgDelay.Value = 500;
            // 
            // lblDelay
            // 
            this.lblDelay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(451, 30);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(16, 19);
            this.lblDelay.TabIndex = 0;
            this.lblDelay.Text = "0";
            // 
            // metroLabel1
            // 
            this.metroLabel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(19, 59);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(89, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "Burst Enabled";
            // 
            // cfgBurstEnabled
            // 
            this.cfgBurstEnabled.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cfgBurstEnabled.AutoSize = true;
            this.cfgBurstEnabled.Location = new System.Drawing.Point(114, 60);
            this.cfgBurstEnabled.Name = "cfgBurstEnabled";
            this.cfgBurstEnabled.Size = new System.Drawing.Size(80, 17);
            this.cfgBurstEnabled.TabIndex = 1;
            this.cfgBurstEnabled.Text = "Aus";
            this.cfgBurstEnabled.UseVisualStyleBackColor = true;
            // 
            // metroLabel5
            // 
            this.metroLabel5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(244, 59);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(74, 19);
            this.metroLabel5.TabIndex = 0;
            this.metroLabel5.Text = "Burst Delay";
            // 
            // cfgBurstDelay
            // 
            this.cfgBurstDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cfgBurstDelay.BackColor = System.Drawing.Color.Transparent;
            this.cfgBurstDelay.Location = new System.Drawing.Point(324, 57);
            this.cfgBurstDelay.Maximum = 500;
            this.cfgBurstDelay.Name = "cfgBurstDelay";
            this.cfgBurstDelay.Size = new System.Drawing.Size(121, 23);
            this.cfgBurstDelay.TabIndex = 4;
            this.cfgBurstDelay.Text = "metroTrackBar1";
            this.cfgBurstDelay.Value = 500;
            // 
            // lblBurstDelay
            // 
            this.lblBurstDelay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblBurstDelay.AutoSize = true;
            this.lblBurstDelay.Location = new System.Drawing.Point(451, 59);
            this.lblBurstDelay.Name = "lblBurstDelay";
            this.lblBurstDelay.Size = new System.Drawing.Size(16, 19);
            this.lblBurstDelay.TabIndex = 0;
            this.lblBurstDelay.Text = "0";
            // 
            // metroLabel8
            // 
            this.metroLabel8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel8.AutoSize = true;
            this.metroLabel8.Location = new System.Drawing.Point(241, 91);
            this.metroLabel8.Name = "metroLabel8";
            this.metroLabel8.Size = new System.Drawing.Size(77, 19);
            this.metroLabel8.TabIndex = 0;
            this.metroLabel8.Text = "Burst Count";
            // 
            // cfgBurstCount
            // 
            this.cfgBurstCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cfgBurstCount.BackColor = System.Drawing.Color.Transparent;
            this.cfgBurstCount.Location = new System.Drawing.Point(324, 89);
            this.cfgBurstCount.Maximum = 30;
            this.cfgBurstCount.Name = "cfgBurstCount";
            this.cfgBurstCount.Size = new System.Drawing.Size(121, 23);
            this.cfgBurstCount.TabIndex = 4;
            this.cfgBurstCount.Text = "metroTrackBar1";
            this.cfgBurstCount.Value = 30;
            // 
            // lblBurstCount
            // 
            this.lblBurstCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblBurstCount.AutoSize = true;
            this.lblBurstCount.Location = new System.Drawing.Point(451, 91);
            this.lblBurstCount.Name = "lblBurstCount";
            this.lblBurstCount.Size = new System.Drawing.Size(16, 19);
            this.lblBurstCount.TabIndex = 0;
            this.lblBurstCount.Text = "0";
            // 
            // cfgKeySelection
            // 
            this.cfgKeySelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.cfgKeySelection, 3);
            this.cfgKeySelection.FormattingEnabled = true;
            this.cfgKeySelection.ItemHeight = 23;
            this.cfgKeySelection.Location = new System.Drawing.Point(238, 122);
            this.cfgKeySelection.Margin = new System.Windows.Forms.Padding(0);
            this.cfgKeySelection.Name = "cfgKeySelection";
            this.cfgKeySelection.Size = new System.Drawing.Size(232, 29);
            this.cfgKeySelection.TabIndex = 6;
            // 
            // TriggerSettingsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "TriggerSettingsUI";
            this.Size = new System.Drawing.Size(475, 183);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MetroFramework.Controls.MetroLabel lblName;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroToggle cfgEnabled;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroTextBox cfgKey;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroComboBox cfgMode;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroToggle cfgBurstEnabled;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private MetroFramework.Controls.MetroTrackBar cfgDelay;
        private MetroFramework.Controls.MetroLabel lblDelay;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroTrackBar cfgBurstDelay;
        private MetroFramework.Controls.MetroLabel lblBurstDelay;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        private MetroFramework.Controls.MetroTrackBar cfgBurstCount;
        private MetroFramework.Controls.MetroLabel lblBurstCount;
        private MetroFramework.Controls.MetroComboBox cfgKeySelection;
    }
}
