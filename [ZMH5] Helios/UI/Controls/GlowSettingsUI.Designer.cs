namespace _ZMH5__Helios.UI.Controls
{
    partial class GlowSettingsUI
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
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.espAlliesEnabled = new _ZMH5__Helios.UI.Controls.SynchToggle();
            this.metroLabel15 = new MetroFramework.Controls.MetroLabel();
            this.espAlliesColor = new _ZMH5__Helios.UI.Controls.SynchColorPicker();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.lblName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel7, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.espAlliesEnabled, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel15, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.espAlliesColor, 1, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(476, 175);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // lblName
            // 
            this.lblName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblName.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.lblName, 4);
            this.lblName.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblName.Location = new System.Drawing.Point(215, 3);
            this.lblName.Margin = new System.Windows.Forms.Padding(3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(45, 19);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Allies";
            // 
            // metroLabel7
            // 
            this.metroLabel7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.Location = new System.Drawing.Point(3, 29);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(56, 19);
            this.metroLabel7.TabIndex = 0;
            this.metroLabel7.Text = "Enabled";
            // 
            // espAlliesEnabled
            // 
            this.espAlliesEnabled.AutoSize = true;
            this.espAlliesEnabled.Checked = false;
            this.espAlliesEnabled.Location = new System.Drawing.Point(65, 28);
            this.espAlliesEnabled.Name = "espAlliesEnabled";
            this.espAlliesEnabled.Observable = null;
            this.espAlliesEnabled.PropertyName = "Enabled";
            this.espAlliesEnabled.Size = new System.Drawing.Size(84, 21);
            this.espAlliesEnabled.TabIndex = 3;
            // 
            // metroLabel15
            // 
            this.metroLabel15.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel15.AutoSize = true;
            this.metroLabel15.Location = new System.Drawing.Point(17, 104);
            this.metroLabel15.Name = "metroLabel15";
            this.metroLabel15.Size = new System.Drawing.Size(42, 19);
            this.metroLabel15.TabIndex = 0;
            this.metroLabel15.Text = "Color";
            // 
            // espAlliesColor
            // 
            this.espAlliesColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.espAlliesColor.AutoSize = true;
            this.espAlliesColor.BackColor = System.Drawing.Color.Transparent;
            this.espAlliesColor.ColorSystem = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tableLayoutPanel2.SetColumnSpan(this.espAlliesColor, 3);
            this.espAlliesColor.Location = new System.Drawing.Point(65, 55);
            this.espAlliesColor.Name = "espAlliesColor";
            this.espAlliesColor.Observable = null;
            this.espAlliesColor.PropertyName = "Color";
            this.espAlliesColor.Size = new System.Drawing.Size(408, 116);
            this.espAlliesColor.TabIndex = 2;
            // 
            // GlowSettingsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "GlowSettingsUI";
            this.Size = new System.Drawing.Size(478, 176);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MetroFramework.Controls.MetroLabel lblName;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private SynchToggle espAlliesEnabled;
        private MetroFramework.Controls.MetroLabel metroLabel15;
        private SynchColorPicker espAlliesColor;
    }
}
