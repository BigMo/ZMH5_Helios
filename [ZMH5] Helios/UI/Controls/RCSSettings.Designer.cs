namespace _ZMH5__Helios.UI.Controls
{
    partial class RCSSettings
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
            this.cfgEnabled = new MetroFramework.Controls.MetroToggle();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.cfgForce = new MetroFramework.Controls.MetroTrackBar();
            this.lblForce = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.cfgCrosshairEnable = new MetroFramework.Controls.MetroToggle();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.cfgNoRecoilSemiAuto = new MetroFramework.Controls.MetroToggle();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.metroLabel3, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cfgEnabled, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel7, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cfgForce, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblForce, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.metroLabel2, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.cfgCrosshairEnable, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.cfgNoRecoilSemiAuto, 1, 4);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(470, 130);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // lblName
            // 
            this.lblName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblName.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.lblName, 3);
            this.lblName.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblName.Location = new System.Drawing.Point(217, 3);
            this.lblName.Margin = new System.Windows.Forms.Padding(3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 19);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "RCS";
            // 
            // cfgEnabled
            // 
            this.cfgEnabled.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cfgEnabled.AutoSize = true;
            this.cfgEnabled.Location = new System.Drawing.Point(148, 28);
            this.cfgEnabled.Name = "cfgEnabled";
            this.cfgEnabled.Size = new System.Drawing.Size(80, 17);
            this.cfgEnabled.TabIndex = 1;
            this.cfgEnabled.Text = "Off";
            this.cfgEnabled.UseVisualStyleBackColor = true;
            // 
            // metroLabel7
            // 
            this.metroLabel7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.Location = new System.Drawing.Point(100, 53);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(42, 19);
            this.metroLabel7.TabIndex = 0;
            this.metroLabel7.Text = "Force";
            // 
            // cfgForce
            // 
            this.cfgForce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cfgForce.BackColor = System.Drawing.Color.Transparent;
            this.cfgForce.Location = new System.Drawing.Point(148, 51);
            this.cfgForce.Name = "cfgForce";
            this.cfgForce.Size = new System.Drawing.Size(280, 23);
            this.cfgForce.TabIndex = 4;
            this.cfgForce.Text = "metroTrackBar1";
            this.cfgForce.Value = 100;
            // 
            // lblForce
            // 
            this.lblForce.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblForce.AutoSize = true;
            this.lblForce.Location = new System.Drawing.Point(434, 53);
            this.lblForce.Name = "lblForce";
            this.lblForce.Size = new System.Drawing.Size(33, 19);
            this.lblForce.TabIndex = 0;
            this.lblForce.Text = "0.00";
            // 
            // metroLabel1
            // 
            this.metroLabel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(86, 27);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(56, 19);
            this.metroLabel1.TabIndex = 6;
            this.metroLabel1.Text = "Enabled";
            // 
            // metroLabel2
            // 
            this.metroLabel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(40, 80);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(102, 19);
            this.metroLabel2.TabIndex = 0;
            this.metroLabel2.Text = "Recoil Crosshair";
            // 
            // cfgCrosshairEnable
            // 
            this.cfgCrosshairEnable.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cfgCrosshairEnable.AutoSize = true;
            this.cfgCrosshairEnable.Location = new System.Drawing.Point(148, 81);
            this.cfgCrosshairEnable.Name = "cfgCrosshairEnable";
            this.cfgCrosshairEnable.Size = new System.Drawing.Size(80, 17);
            this.cfgCrosshairEnable.TabIndex = 7;
            this.cfgCrosshairEnable.Text = "Off";
            this.cfgCrosshairEnable.UseVisualStyleBackColor = true;
            // 
            // metroLabel3
            // 
            this.metroLabel3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(3, 107);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(139, 19);
            this.metroLabel3.TabIndex = 8;
            this.metroLabel3.Text = "Disable for Semi-Auto";
            // 
            // cfgNoRecoilSemiAuto
            // 
            this.cfgNoRecoilSemiAuto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cfgNoRecoilSemiAuto.AutoSize = true;
            this.cfgNoRecoilSemiAuto.Location = new System.Drawing.Point(148, 108);
            this.cfgNoRecoilSemiAuto.Name = "cfgNoRecoilSemiAuto";
            this.cfgNoRecoilSemiAuto.Size = new System.Drawing.Size(80, 17);
            this.cfgNoRecoilSemiAuto.TabIndex = 9;
            this.cfgNoRecoilSemiAuto.Text = "Off";
            this.cfgNoRecoilSemiAuto.UseVisualStyleBackColor = true;
            // 
            // RCSSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "RCSSettings";
            this.Size = new System.Drawing.Size(477, 133);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MetroFramework.Controls.MetroLabel lblName;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroToggle cfgEnabled;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private MetroFramework.Controls.MetroTrackBar cfgForce;
        private MetroFramework.Controls.MetroLabel lblForce;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroToggle cfgCrosshairEnable;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroToggle cfgNoRecoilSemiAuto;
    }
}
