using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroCloudWCFHelper
{
    public class SpacialHelper
    {
        public static double CalcDistanceBetweenTwoPoints(double longitude1, double latitude1, double longitude2, double latitude2)
        {
            double e = (3.1415926538 * longitude1 / 180);
            double f = (3.1415926538 * latitude1 / 180);
            double g = (3.1415926538 * longitude2 / 180);
            double h = (3.1415926538 * latitude2 / 180);
            double i = (Math.Cos(e) * Math.Cos(g) * Math.Cos(f) * Math.Cos(h) + Math.Cos(e) * Math.Sin(f) * Math.Cos(g) * Math.Sin(h) + Math.Sin(e) * Math.Sin(g));
            double j = (Math.Acos(i));
            double k = (6371 * j);

            return k;
        }
    }
}
