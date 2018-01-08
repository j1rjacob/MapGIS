namespace SaudiMap
{
    partial class FormTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTest));
            this.sfMap1 = new EGIS.Controls.SFMap();
            this.OpenFileDialogAdministrative = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // sfMap1
            // 
            this.sfMap1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sfMap1.CentrePoint2D = ((EGIS.ShapeFileLib.PointD)(resources.GetObject("sfMap1.CentrePoint2D")));
            this.sfMap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sfMap1.Location = new System.Drawing.Point(0, 0);
            this.sfMap1.MapBackColor = System.Drawing.SystemColors.Control;
            this.sfMap1.Name = "sfMap1";
            this.sfMap1.PanSelectMode = EGIS.Controls.PanSelectMode.Pan;
            this.sfMap1.RenderQuality = EGIS.ShapeFileLib.RenderQuality.Auto;
            this.sfMap1.Size = new System.Drawing.Size(739, 504);
            this.sfMap1.TabIndex = 0;
            this.sfMap1.UseMercatorProjection = false;
            this.sfMap1.ZoomLevel = 1D;
            this.sfMap1.ZoomToSelectedExtentWhenCtrlKeydown = false;
            this.sfMap1.Paint += new System.Windows.Forms.PaintEventHandler(this.sfMap1_Paint);
            this.sfMap1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sfMap1_MouseUp);
            // 
            // OpenFileDialogAdministrative
            // 
            this.OpenFileDialogAdministrative.FileName = "openFileDialog1";
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 504);
            this.Controls.Add(this.sfMap1);
            this.Name = "FormTest";
            this.Text = "Saudi Arabia Administrative";
            this.ResumeLayout(false);

        }

        #endregion

        private EGIS.Controls.SFMap sfMap1;
        private System.Windows.Forms.OpenFileDialog OpenFileDialogAdministrative;
    }
}

