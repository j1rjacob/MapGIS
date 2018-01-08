using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SaudiMap
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();

            OpenRoadShapefile(Application.StartupPath+ "\\Highway\\saudi_arabia_highway.shp");
            
            //sfMap1.SetZoomAndCentre(25000, new EGIS.ShapeFileLib.PointD(120.63, 24.175));
        }
        
        private void OpenRoadShapefile(string path)
        {

            // open the shapefile passing in the path, display name of the shapefile and
            // the field name to be used when rendering the shapes (we use an empty string
            // as the field name (3rd parameter) can not be null)
            EGIS.ShapeFileLib.ShapeFile sf = this.sfMap1.AddShapeFile(path, "ShapeFile", "");

            // Setup a dictionary collection of road types and colors
            // We will use this when creating a RoadTypeCustomRenderSettings class to setup which
            //colors should be used to render each type of road
            sf.RenderSettings.FieldName = "name";
            sf.RenderSettings.Font = new Font(this.Font.FontFamily, 12);
            Dictionary<string, Color> colors = new Dictionary<string, Color>();
            colors.Add("service", Color.Green);
            colors.Add("secondary", Color.Green);
            colors.Add("residential", Color.Blue);
            colors.Add("primary", Color.Yellow);
            SaudiRenderSettings rs = new SaudiRenderSettings(sf.RenderSettings, "TYPE", colors);
            sf.RenderSettings.CustomRenderSettings = rs;
            sf.RenderSettings.UseToolTip = true;
            sf.RenderSettings.ToolTipFieldName = "name";
            sf.RenderSettings.MaxPixelPenWidth = 20;
        }

        private const int ScaleLineWidth = 150;
        private const int ScaleOffY = 10;
        private void sfMap1_Paint(object sender, PaintEventArgs e)
        {
            DisplayInstructions(e.Graphics);
            //draw a simple scale at the bottom of the map
            double w = 0, h = 0;
            //get the map width/height in meters
            GetMapDimensionsInMeters(ref w, ref h);

            //draw the scale line
            Point p1 = new Point(10, sfMap1.ClientSize.Height - ScaleOffY);

            e.Graphics.DrawLine(Pens.Black, p1.X, p1.Y, p1.X + ScaleLineWidth, p1.Y);
            e.Graphics.DrawLine(Pens.Black, p1.X, p1.Y, p1.X, p1.Y - 8);
            e.Graphics.DrawLine(Pens.Black, p1.X + ScaleLineWidth, p1.Y, p1.X + ScaleLineWidth, p1.Y - 8);
            StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(w.ToString("0000000.0m"), this.Font, Brushes.Black, new RectangleF(p1.X, p1.Y - 20, ScaleLineWidth, 20), sf);
        }

        private void GetMapDimensionsInMeters(ref double w, ref double h)
        {
            RectangleF r = this.sfMap1.VisibleExtent;
            if (IsMapUsingLatLong())
            {
                //assume using latitude longitude
                w = EGIS.ShapeFileLib.ConversionFunctions.DistanceBetweenLatLongPoints(EGIS.ShapeFileLib.ConversionFunctions.RefEllipse,
                    r.Bottom, r.Left, r.Bottom, r.Right);
                h = EGIS.ShapeFileLib.ConversionFunctions.DistanceBetweenLatLongPoints(EGIS.ShapeFileLib.ConversionFunctions.RefEllipse,
                    r.Bottom, r.Left, r.Top, r.Left);
            }
            else
            {
                //assume coord in meters
                w = r.Width;
                h = r.Height;
            }
        }

        private bool IsMapUsingLatLong()
        {
            RectangleF ext = sfMap1.Extent;
            return (ext.Top <= 90 && ext.Bottom >= -90);
        }

        private bool instructionsDisplayed = false;

        private void DisplayInstructions(Graphics g)
        {
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            if (instructionsDisplayed) return;
            instructionsDisplayed = true;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Font f = new Font(this.Font.FontFamily, 32, FontStyle.Bold);
            g.DrawString("Double click mouse or use mouse wheel to Zoom in/out\nClick and drag mouse to pan", f, Brushes.Black, new RectangleF(0, 0, sfMap1.ClientSize.Width, sfMap1.ClientSize.Height), sf);
        }

        private void sfMap1_MouseUp(object sender, MouseEventArgs e)
        {
            //display a message box of a shape's attributes if it is clicked (within 5 pixels)
            Point pt = new Point(e.X, e.Y);
            //loop backwards on the shapefiles as layers are drawn in the order
            for (int n = sfMap1.ShapeFileCount - 1; n >= 0; --n)
            {
                int recordNumber = sfMap1.GetShapeIndexAtPixelCoord(n, pt, 5);
                if (recordNumber >= 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string[] attributeValues = sfMap1[n].GetAttributeFieldValues(recordNumber);
                    string[] fieldNames = sfMap1[n].GetAttributeFieldNames();
                    for (int i = 0; i < fieldNames.Length; ++i)
                    {
                        if (i > 0) sb.Append("\n");
                        sb.Append(string.Format("{0}:{1}", fieldNames[i], attributeValues[i].Trim()));
                    }
                    MessageBox.Show(this, sb.ToString());
                    return;
                }

            }
        }
    }
}
