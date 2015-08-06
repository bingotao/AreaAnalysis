using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;

namespace AreaAnalysis.AreaAnalysis
{
    public class GHYT
    {
        public string BSM = "BSM";
        public string YSDM = "YSDM";
        public string XZQHDM = "XZQHDM";
        public string GNFQLXDM = "GNFQLXDM";
        public string TDLYGNFQBH = "TDLYGNFQBH";
        public string GNFQMJ = "GNFQMJ";
        public string GNFQLXMC = "";
        public double SJMJ = 0;

        public GHYT(IFeatureClass pFeatureClass, IFeature pFeatrue)
        {
            this.BSM = pFeatrue.get_Value(pFeatureClass.Fields.FindField(this.BSM)).ToString();
            this.YSDM = pFeatrue.get_Value(pFeatureClass.Fields.FindField(this.YSDM)).ToString();
            this.XZQHDM = pFeatrue.get_Value(pFeatureClass.Fields.FindField(this.XZQHDM)).ToString();
            this.GNFQLXDM = pFeatrue.get_Value(pFeatureClass.Fields.FindField(this.GNFQLXDM)).ToString();
            this.TDLYGNFQBH = pFeatrue.get_Value(pFeatureClass.Fields.FindField(this.TDLYGNFQBH)).ToString();
            this.GNFQMJ = pFeatrue.get_Value(pFeatureClass.Fields.FindField(this.GNFQMJ)).ToString();
        }
    }
}
