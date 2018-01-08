using EGIS.ShapeFileLib;
using System.Collections.Generic;
using System.Drawing;

namespace SaudiMap
{
    public class SaudiRenderSettings : ICustomRenderSettings
    {
        private List<Color> colorList;
        RenderSettings defaultSettings;
        public SaudiRenderSettings(RenderSettings defaultSettings, string typeField, Dictionary<string, Color> roadtypeColors)
        {
            this.defaultSettings = defaultSettings;
            BuildColorList(defaultSettings, typeField, roadtypeColors);
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
}
