using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AreaAnalysis.AO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace AreaAnalysis.AreaAnalysis
{
    public class GHAreaAnalysis
    {
        public static GHAreaAnalysisResult Analysis(IFeatureClass pGHYTFeatureClass, IPolygon pPolygon)
        {
            IList<GHYT> oList = GetIntersetGHYTFeatures(pPolygon, pGHYTFeatureClass);
            GHAreaAnalysisResult result = staticResult(oList);
            return result;
        }

        private static IList<GHYT> GetIntersetGHYTFeatures(IGeometry pGeometry, IFeatureClass pGHYTFeatureClass)
        {
            ISpatialFilter filter = null;
            IFeatureCursor pFeatureCursor = null;
            ITopologicalOperator pTopo = null;
            double number = 0;
            IFeature feature = null;
            filter = new SpatialFilterClass
            {
                Geometry = pGeometry,
                SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects
            };
            pFeatureCursor = pGHYTFeatureClass.Search(filter, true);
            IList<GHYT> ghytList = new List<GHYT>();

            #region 分析规划用途
            if (pFeatureCursor != null)
            {
                while ((feature = pFeatureCursor.NextFeature()) != null)
                {
                    GHYT ghytItem = new GHYT(pGHYTFeatureClass, feature);
                    pTopo = feature.Shape as ITopologicalOperator;
                    if (pTopo.IsSimple == false)
                    {
                        pTopo.Simplify();
                    }
                    //进行判断前最好Simple一下
                    (pGeometry as ITopologicalOperator).Simplify();
                    number = GeometryUtil.GetIntersectArea(pGeometry as IPolygon, feature.Shape as IPolygon);
                    if (number > 1.0)
                    {
                        ghytItem.SJMJ = number;
                        switch (ghytItem.GNFQLXDM.ToLower())
                        {
                            case "010":
                                ghytItem.GNFQLXMC = "基本农田保护区";
                                break;
                            case "020":
                                ghytItem.GNFQLXMC = "一般农地区";
                                break;
                            case "030":
                                ghytItem.GNFQLXMC = "城镇村建设用地区";
                                break;
                            case "050":
                                ghytItem.GNFQLXMC = "独立工矿区";
                                break;
                            case "070":
                                ghytItem.GNFQLXMC = "生态环境安全控制区";
                                break;
                            case "080":
                                ghytItem.GNFQLXMC = "自然与文化遗产保护区";
                                break;
                            case "090":
                                ghytItem.GNFQLXMC = "林业用地区";
                                break;
                            default:
                                break;
                        }
                        ghytList.Add(ghytItem);
                    }
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(feature);
                }
            }
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pFeatureCursor);
            #endregion
            return ghytList;
        }

        /// <summary>
        /// 最终结果统计
        /// </summary>
        /// <param name="pList"></param>
        /// <returns></returns>
        private static GHAreaAnalysisResult staticResult(IList<GHYT> pList)
        {
            IList<GHYT> oGHYT = new List<GHYT>();
            int m = 0;

            GHAreaAnalysisResult result = new GHAreaAnalysisResult();

            foreach (GHYT item in pList)
            {
                if (m < 20)
                    oGHYT.Add(item);
                m++;
                switch (item.GNFQLXDM.ToLower())
                {
                    case "010":
                        result.jbntqmj += item.SJMJ;
                        break;
                    case "020":
                        result.ybndmj += item.SJMJ;
                        break;
                    case "030":
                        result.jsydqmj += item.SJMJ;
                        break;
                    case "050":
                        result.gkydmj += item.SJMJ;
                        break;
                    case "070":
                        result.sthjmj += item.SJMJ;
                        break;
                    case "080":
                        result.ycbhqmj += item.SJMJ;
                        break;
                    case "090":
                        result.lyydmj += item.SJMJ;
                        break;
                    default:
                        break;
                }
            }
            result.total = Math.Round((result.jbntqmj + result.ybndmj + result.jsydqmj + result.gkydmj + result.sthjmj + result.ycbhqmj + result.lyydmj), 4);
            result.jbntqmj = Math.Round(result.jbntqmj, 4);
            result.ybndmj = Math.Round(result.ybndmj, 4);
            result.jsydqmj = Math.Round(result.jsydqmj, 4);
            result.gkydmj = Math.Round(result.gkydmj, 4);
            result.sthjmj = Math.Round(result.sthjmj, 4);
            result.ycbhqmj = Math.Round(result.ycbhqmj, 4);
            result.lyydmj = Math.Round(result.lyydmj, 4);
            result.bfhghmj = Math.Round((result.jbntqmj + result.ybndmj + result.gkydmj + result.sthjmj + result.ycbhqmj + result.lyydmj), 4);
            if (result.bfhghmj > 5)
            {
                result.isFHGH = true;
            }
            result.result = oGHYT;
            return result;
        }
    }
}
