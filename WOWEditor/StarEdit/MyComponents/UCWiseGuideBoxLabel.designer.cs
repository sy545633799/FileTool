namespace StarEdit.MyComponents
{
    partial class UCWiseGuideBoxLabel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxData = new System.Windows.Forms.TextBox();
            this.labelData = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxData
            // 
            this.textBoxData.Location = new System.Drawing.Point(0, 0);
            this.textBoxData.Name = "textBoxData";
            this.textBoxData.Size = new System.Drawing.Size(60, 21);
            this.textBoxData.TabIndex = 0;
            this.textBoxData.TextChanged += new System.EventHandler(this.textBoxData_TextChanged);
            // 
            // labelData
            // 
            this.labelData.AutoSize = true;
            this.labelData.Location = new System.Drawing.Point(66, 5);
            this.labelData.Name = "labelData";
            this.labelData.Size = new System.Drawing.Size(0, 12);
            this.labelData.TabIndex = 1;
            // 
            // UCWiseGuideBoxLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelData);
            this.Controls.Add(this.textBoxData);
            this.ForeColor = System.Drawing.Color.Orange;
            this.Name = "UCWiseGuideBoxLabel";
            this.Size = new System.Drawing.Size(150, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxData;
        private System.Windows.Forms.Label labelData;
    }
}
