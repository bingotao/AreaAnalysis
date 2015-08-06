using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AreaAnalysis.AreaAnalysis
{
    public class GHAreaAnalysisResult
    {
        public IList<GHYT> result = null;

        //是否符合规划，默认“true”符合
        public bool isFHGH = true;
        //基本农田保护区总面积
        public double jbntqmj = 0;
        //一般农地区总面积
        public double ybndmj = 0;
        //城镇村建设用地区总面积
        public double jsydqmj = 0;
        //独立工矿区总面积
        public double gkydmj = 0;
        //生态环境安全控制区总面积
        public double sthjmj = 0;
        //自然与文化遗产保护区总面积
        public double ycbhqmj = 0;
        //林业用地总面积
        public double lyydmj = 0;
        //总面积
        public double total = 0;
        //不符合规划面积
        public double bfhghmj = 0;
    }
}
