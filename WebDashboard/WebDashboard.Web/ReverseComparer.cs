using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDashboard.Web
{
    public class ReverseComparer : IComparer<string>
    {
        public int Compare(string x, string y) { return y.CompareTo(x); }
    }
}
