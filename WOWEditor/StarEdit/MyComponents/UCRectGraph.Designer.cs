namespace StarEdit.MyComponents
{
    partial class UCRectGraph
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
            this.doubleBufferedPanel1 = new StarEdit.MyComponents.DoubleBufferedPanel();
            this.SuspendLayout();
            // 
            // doubleBufferedPanel1
            // 
            this.doubleBufferedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doubleBufferedPanel1.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferedPanel1.Name = "doubleBufferedPanel1";
            this.doubleBufferedPanel1.Size = new System.Drawing.Size(480, 240);
            this.doubleBufferedPanel1.TabIndex = 0;
            this.doubleBufferedPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.doubleBufferedPanel1_Paint);
            // 
            // UCRectGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.doubleBufferedPanel1);
            this.Name = "UCRectGraph";
            this.Size = new System.Drawing.Size(480, 240);
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel doubleBufferedPanel1;
    }
}
