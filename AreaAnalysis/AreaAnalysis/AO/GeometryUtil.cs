using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace AreaAnalysis.AO
{
    public class GeometryUtil
    {
        private static string sErroCoordinatesIsNull = "坐标字符串为空！无法构建Polygon！";
        private static string sErroCoordinatesValueIllegal = "坐标字符串非法！无法构建Polygon！";
        private static string sErroGeometryIsNull = "图形为空！无法计算相交面积！";

        /// <summary>
        /// 将字符串格式转化为Geometry
        /// </summary>
        /// <param name="geom">    客户端传递多边形格式 1) x1,y1;x2,y2;x3,y3.......xn,yn;x1,y1;保证起始闭合 为无环
        ///                                            2) x1,y1,flag3;x2,y2,flag3;x3,y3,flag3.......xn,yn,,flagn;
        /// </param>
        /// <returns></returns>
        public static IPolygon BuildPolygon(string geom)
        {
            if (string.IsNullOrEmpty(geom) == true)
            {
                throw new Exception(sErroCoordinatesIsNull);
            }

            string[] points = geom.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (points != null && points.Length > 2)
            {
                int pointCount = points.Length;
                IPolygon polygon = null;
                IPoint point = null;
                object missing = Type.Missing;
                IGeometryCollection pGeoColl = new PolygonClass() as IGeometryCollection;
                IPointCollection pPointCol = new RingClass();
                for (int i = 0; i < pointCount; i++)
                {
                    string[] pts = points[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    point = new PointClass();
                    double x = 0.0;
                    double y = 0.0;
                    int flag = 0;
                    bool bX = double.TryParse(pts[0], out x);
                    bool bY = double.TryParse(pts[1], out y);
                    bool bFlag = int.TryParse(pts[2], out flag);
                    if (bX && bY && bFlag)
                    {
                        pPointCol.AddPoint(point, ref  missing, ref missing);
                        if (flag == -1 || i == (pointCount - 1))
                        {
                            pGeoColl.AddGeometry(pPointCol as IRing, ref missing, ref missing);
                            pPointCol = null;
                            break;
                        }
                        else if (flag == -2)
                        {
                            pGeoColl.AddGeometry(pPointCol as IRing, ref missing, ref missing);
                            pPointCol = new RingClass();
                            continue;
                        }
                    }
                    else
                    {
                        throw new Exception(sErroCoordinatesValueIllegal);
                    }
                }
                if (pPointCol.PointCount > 0)
                {
                    pGeoColl.AddGeometry(pPointCol as IRing, ref missing, ref missing);
                }
                polygon = pGeoColl as IPolygon;
                SimplifyGeometry(polygon);
                return polygon;
            }
            else
            {
                throw new Exception(sErroCoordinatesValueIllegal);
            }
        }


        /// <summary>
        /// 获取Polygon相交面积
        /// </summary>
        /// <param name="pPolygon"></param>
        /// <param name="tPolygon"></param>
        /// <returns></returns>
        public static double GetIntersectArea(IPolygon pPolygon, IPolygon tPolygon)
        {
            double area = 0.0;
            if (pPolygon != null && tPolygon != null)
            {
                /*
                 * 是否需要Project？
                IGeometry pGeo = pPolygon as IGeometry;
                IGeometry tGeo = tPolygon as IGeometry;
                if (pGeo.SpatialReference == null && tGeo.SpatialReference != null)
                {
                    pGeo.Project(tGeo.SpatialReference);
                }
                 */

                SimplifyGeometry(pPolygon);
                SimplifyGeometry(tPolygon);
                ITopologicalOperator pTop = pPolygon as ITopologicalOperator;
                IGeometry pIntersect = pTop.Intersect(tPolygon, esriGeometryDimension.esriGeometry2Dimension);
                area = (pIntersect as IArea).Area;
            }
            else
            {
                throw new Exception(sErroGeometryIsNull);
            }
            return area;
        }


        /// <summary>
        /// 简化Geometry
        /// </summary>
        /// <param name="pGeometry"></param>
        public static void SimplifyGeometry(IGeometry pGeometry)
        {
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            if (pTop.IsSimple == false)
            {
                pTop.Simplify();
            }
        }
    }
}
