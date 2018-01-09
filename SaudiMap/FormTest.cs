using EGIS.ShapeFileLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace SaudiMap
{
    public partial class FormTest : Form
    {
        SerialPort GPScommPort;
        SerialPort GPSSp;
        private EGIS.ShapeFileLib.PointD marker2 = new EGIS.ShapeFileLib.PointD(0, 0);
        private const int MarkerWidth = 10;
        GpsPacket gpsPoint;
        SaudiRenderSettings rs;
        public FormTest()
        {
            InitializeComponent();

            LoadMaps();

            gpsPoint = new GpsPacket();

            gpsPoint.Latitude = 0;// 21.2303608;
            gpsPoint.Longitude = 0;//40.4112419166667;
            marker2.X = gpsPoint.Longitude;
            marker2.Y = gpsPoint.Latitude;

            //OpenRoadShapefile(Application.StartupPath+ "\\Highway\\saudi_arabia_highway.shp");

            GPScommPort = new SerialPort();
            GPScommPort.BaudRate = 19200;
            GPScommPort.Parity = Parity.None;
            GPScommPort.StopBits = StopBits.One;
            GPScommPort.DataBits = 8;
            GPScommPort.Handshake = Handshake.RequestToSendXOnXOff;
            GPScommPort.DataReceived += new SerialDataReceivedEventHandler(GPSDataReceivedHandler);
        }

        void LoadMaps()
        {
            string currentDIR = Application.StartupPath;
            string roadsshapefile = currentDIR + "\\Taif OSM\\Taif Roads.shp";
            string metersshapefile = currentDIR + "\\Taif OSM\\Taif Meters EPSG4326.shp";

            if (File.Exists(roadsshapefile))
                OpenRoadsfile(roadsshapefile);
            else
            {
                MessageBox.Show("Taif Map not found! Exiting program", "Error");

                Application.Exit();
            }

            if (File.Exists(metersshapefile))
                OpenMetersfile(metersshapefile);
            else
            {
                MessageBox.Show("Meter Map unfound! Exiting program", " Error ");

                Application.Exit();
            }
        }
        void OpenRoadsfile(string path)
        {
            this.sfMap1.ClearShapeFiles();

            this.sfMap1.AddShapeFile(path, "Roads", "");
            
            EGIS.ShapeFileLib.ShapeFile sf = this.sfMap1[0];
            
            sf.RenderSettings.UseToolTip = false;
            sf.RenderSettings.ToolTipFieldName = sf.RenderSettings.FieldName;
            sf.RenderSettings.IsSelectable = false;
        }
        void OpenMetersfile(string path)
        {
            this.sfMap1.AddShapeFile(path, "meters", "");
            
            EGIS.ShapeFileLib.ShapeFile sf = this.sfMap1[1];

            rs = new SaudiRenderSettings(sf.RenderSettings, "Serial Num", sf);
            sf.RenderSettings.CustomRenderSettings = rs;
            
            sf.RenderSettings.UseToolTip = false;
            sf.RenderSettings.ToolTipFieldName = sf.RenderSettings.FieldName;
            sf.RenderSettings.IsSelectable = false;
            
            sf.SelectRecord(0, true);
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
        //---------------------------------------------------------------
        private void button1_Click(object sender, System.EventArgs e)
        {
            marker2.Y = double.Parse(txtLat.Text);
            marker2.X = double.Parse(txtLon.Text);

            sfMap1.Refresh();
        }
        private void GPSDataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            GPSSp = (SerialPort)sender;

            string gpsstring = GPSSp.ReadLine();
            //System.Diagnostics.Debug.WriteLine(gpsstring);

            if (gpsstring.Contains("$GPGGA"))
            {
                gpsPoint.UpdateGPS(gpsstring);
                marker2.X = gpsPoint.Longitude;
                marker2.Y = gpsPoint.Latitude;

                System.Diagnostics.Debug.WriteLine(gpsPoint.Longitude.ToString() + " " + gpsPoint.Latitude.ToString());

                LabelMarkerY.Invoke((Action)delegate { LabelMarkerY.Text = marker2.Y.ToString(); });
                LabelMarkerX.Invoke((Action)delegate { LabelMarkerX.Text = marker2.X.ToString(); });
                //refresh the map after receiving GPS
                sfMap1.Invoke((Action)delegate { sfMap1.Refresh(); });

            }
        }
    }

    #region GPSUtil
    public class GpsPacket
    {
        static char[] delimiterChars = { ',' };
        private bool valid;
        private DateTime time;
        private bool fix;
        private double lat, lon;
        public string gpsdata;
        private bool requestingdata = false;

        GpsPacket gpsPoint;

        public GpsPacket()
        {
            this.time = DateTime.Now;
            valid = false;
            lat = lon = 0;
        }

        public GpsPacket(string packet)
        {
            this.time = DateTime.Now;
            gpsdata = packet;

            string[] PacketTokens = packet.Split(new char[] { ',' });
            try
            {
                lat = GetLatitude(PacketTokens[2], PacketTokens[3]);
                lon = GetLongitude(PacketTokens[4], PacketTokens[5]);

                //Check the fix - if fix is greater than zero then the fix quality is good, else there is no fix
                fix = (int.Parse(PacketTokens[6]) > 0);
            }
            catch
            {
                this.valid = false;
            }
        }

        #region public properties

        public bool UpdateGPS(string packet)
        {
            //Example packet: $GPGGA,012711,3749.2776,S,14502.4100,E,1,07,02.04,000039.3,M,-001.8,M,,*7D
            this.time = DateTime.Now;
            gpsdata = packet;

            string[] PacketTokens = packet.Split(delimiterChars);
            try
            {
                lat = GetLatitude(PacketTokens[2], PacketTokens[3]);
                lon = GetLongitude(PacketTokens[4], PacketTokens[5]);

                //Check the fix - if fix is greater than zero then the fix quality is good, else there is no fix
                fix = ((PacketTokens[6]) == "A");
                valid = fix;
                return valid;
            }
            catch
            {
                MessageBox.Show("Chill");
                this.valid = false;
                return valid;
            }
        }

        public double Latitude
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
            }
        }

        public double Longitude
        {
            get
            {
                return lon;
            }
            set
            {
                lon = value;
            }
        }

        public DateTime PacketTime
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }

        public bool Fix
        {
            get
            {
                return fix;
            }
            set
            {
                fix = value;
            }
        }

        public bool IsValid
        {
            get
            {
                return valid;
            }
            set
            {
                valid = value;
            }
        }

        #endregion

        #region static utility methods
        static double GetLatitude(string latitudeToken, string direction)
        {
            //Latitude token: DDMM.mmm,N
            //Decimal Degrees = DD + (MM.mmm / 60)
            string Degrees = latitudeToken.Substring(0, 2);
            string Minutes = latitudeToken.Substring(2);
            double Latitude = double.Parse(Degrees) + (double.Parse(Minutes) / 60.0);
            //direction: N or S
            //If direction is South: * -1
            if (!direction.Equals("N", StringComparison.OrdinalIgnoreCase)) Latitude = -Latitude;
            return Latitude;
        }

        static double GetLongitude(string longitudeToken, string direction)
        {
            //Longtitude token: DDDMM.mmm,E
            //Decimal Degrees = DDD + (MM.mmm / 60)
            string Degrees = longitudeToken.Substring(0, 3);
            string Minutes = longitudeToken.Substring(3);
            double Longtitude = double.Parse(Degrees) + (double.Parse(Minutes) / 60.0);
            //direction: E or W
            //If direction is West: * -1
            if (!direction.Equals("E", StringComparison.OrdinalIgnoreCase)) Longtitude = -Longtitude;
            return Longtitude;
        }

        //static string GetChecksum(string packet)
        //{
        //    //Ignore the '$' at the start of the packet, and the checksum token eg.'*54'
        //    string truncatedPacket = packet.Substring(1, packet.Length - 4);

        //    char[] packetCharacters = truncatedPacket.ToCharArray();
        //    int lastChar;

        //    lastChar = Convert.ToInt32(packetCharacters[0]);

        //    //Use a loop to Xor through the string
        //    for (int i = 1; i < packetCharacters.Length; i++)
        //    {
        //        lastChar = lastChar ^ Convert.ToInt32(packetCharacters[i]);
        //    }

        //    return String.Format("{0:x2}", lastChar).ToUpper();
        //}
        #endregion
    }
    #endregion

    #region SaudiRenderSettings
    public class SaudiRenderSettings : ICustomRenderSettings
    {
        private List<Color> colorList;
        RenderSettings defaultSettings;
        Dictionary<string, int> metersListGIS;
        public SaudiRenderSettings(RenderSettings defaultSettings, string typeField, Dictionary<string, Color> roadtypeColors)
        {
            this.defaultSettings = defaultSettings;
            BuildColorList(defaultSettings, typeField, roadtypeColors);
        }
        public SaudiRenderSettings(RenderSettings defaultSettings, string serialNbr, EGIS.ShapeFileLib.ShapeFile shpfile1)
        {
            this.defaultSettings = defaultSettings;
            //this.defaultSettings.FillColor = Color.Blue;
            this.defaultSettings.FillInterior = true;
            this.defaultSettings.OutlineColor = Color.Transparent;
            this.defaultSettings.MinZoomLevel = -1;
            this.defaultSettings.MaxZoomLevel = -1;
            //this.defaultSettings.RenderQuality = EGIS.ShapeFileLib.RenderQuality.High;

            this.defaultSettings.UseToolTip = false;
            this.defaultSettings.IsSelectable = false;

            BuildColorList(defaultSettings, serialNbr, shpfile1);
        }
        void BuildColorList(RenderSettings defaultSettings, string serialNbr, EGIS.ShapeFileLib.ShapeFile shpfile2)
        {
            int fieldIndex = defaultSettings.DbfReader.IndexOfFieldName(serialNbr);
            if (fieldIndex >= 0)
            {
                colorList = new List<System.Drawing.Color>();
                metersListGIS = new Dictionary<string, int>(40000);

                int numRecords = defaultSettings.DbfReader.DbfRecordHeader.RecordCount;
                for (int n = 0; n < numRecords; ++n)
                {
                    colorList.Add(Color.Red);
                    metersListGIS.Add(shpfile2.GetAttributeFieldValues(n)[fieldIndex].Trim(), n);
                }
            }
            else
            {
                MessageBox.Show("Couldn't find Field Name");
            }
        }
        private void BuildColorList(RenderSettings defaultSettings, string typeField, Dictionary<string, Color> roadtypeColors)
        {
            int fieldIndex = defaultSettings.DbfReader.IndexOfFieldName(typeField);
            if (fieldIndex >= 0)
            {
                colorList = new List<Color>();
                int numRecords = defaultSettings.DbfReader.DbfRecordHeader.RecordCount;
                for (int n = 0; n < numRecords; ++n)
                {
                    string nextField = defaultSettings.DbfReader.GetField(n, fieldIndex).Trim();
                    if (roadtypeColors.ContainsKey(nextField))
                    {
                        colorList.Add(roadtypeColors[nextField]);
                    }
                    else
                    {
                        colorList.Add(defaultSettings.FillColor);
                    }
                }
            }
        }
        #region ICustomRenderSettings Members
        public Color GetRecordFillColor(int recordNumber)
        {
            if (colorList != null)
            {
                return colorList[recordNumber];
            }
            return defaultSettings.FillColor;
        }
        public Color GetRecordOutlineColor(int recordNumber)
        {
            return defaultSettings.OutlineColor;
        }
        public Color GetRecordFontColor(int recordNumber)
        {
            return defaultSettings.FontColor;
        }
        public bool RenderShape(int recordNumber)
        {
            return true;
        }
        public string GetRecordToolTip(int recordNumber)
        {
            return "";
        }
        public Image GetRecordImageSymbol(int recordNumber)
        {
            return defaultSettings.GetImageSymbol();
        }
        public bool UseCustomTooltips { get { return false; } }
        public bool UseCustomImageSymbols { get { return false; } }
        #endregion
    }
    #endregion
}
