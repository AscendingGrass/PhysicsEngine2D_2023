namespace PhysicsEngine2D_2023
{
    partial class EnvironmentVisualizer
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
            this.SuspendLayout();
            // 
            // EnvironmentVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Name = "EnvironmentVisualizer";
            this.Size = new System.Drawing.Size(666, 402);
            this.Load += new System.EventHandler(this.EnvironmentVisualizer_Load);
            this.SizeChanged += new System.EventHandler(this.EnvironmentVisualizer_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.EnvironmentVisualizer_Paint);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
