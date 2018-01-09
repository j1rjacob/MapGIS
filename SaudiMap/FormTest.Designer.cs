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
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtLat = new System.Windows.Forms.TextBox();
            this.txtLon = new System.Windows.Forms.TextBox();
            this.LabelMarkerY = new System.Windows.Forms.Label();
            this.LabelMarkerX = new System.Windows.Forms.Label();
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
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(364, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Draw Marker at:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(666, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "GO";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtLat
            // 
            this.txtLat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLat.Location = new System.Drawing.Point(560, 8);
            this.txtLat.Name = "txtLat";
            this.txtLat.Size = new System.Drawing.Size(100, 20);
            this.txtLat.TabIndex = 7;
            this.txtLat.Text = "-37.8038";
            // 
            // txtLon
            // 
            this.txtLon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLon.Location = new System.Drawing.Point(453, 8);
            this.txtLon.Name = "txtLon";
            this.txtLon.Size = new System.Drawing.Size(100, 20);
            this.txtLon.TabIndex = 6;
            this.txtLon.Text = "145.0285";
            // 
            // LabelMarkerY
            // 
            this.LabelMarkerY.AutoSize = true;
            this.LabelMarkerY.Location = new System.Drawing.Point(8, 8);
            this.LabelMarkerY.Name = "LabelMarkerY";
            this.LabelMarkerY.Size = new System.Drawing.Size(50, 13);
            this.LabelMarkerY.TabIndex = 10;
            this.LabelMarkerY.Text = "Marker Y";
            // 
            // LabelMarkerX
            // 
            this.LabelMarkerX.AutoSize = true;
            this.LabelMarkerX.Location = new System.Drawing.Point(11, 24);
            this.LabelMarkerX.Name = "LabelMarkerX";
            this.LabelMarkerX.Size = new System.Drawing.Size(50, 13);
            this.LabelMarkerX.TabIndex = 11;
            this.LabelMarkerX.Text = "Marker X";
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 504);
            this.Controls.Add(this.LabelMarkerX);
            this.Controls.Add(this.LabelMarkerY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtLat);
            this.Controls.Add(this.txtLon);
            this.Controls.Add(this.sfMap1);
            this.Name = "FormTest";
            this.Text = "Saudi Arabia Administrative";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EGIS.Controls.SFMap sfMap1;
        private System.Windows.Forms.OpenFileDialog OpenFileDialogAdministrative;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.TextBox txtLon;
        private System.Windows.Forms.Label LabelMarkerY;
        private System.Windows.Forms.Label LabelMarkerX;
    }
}

