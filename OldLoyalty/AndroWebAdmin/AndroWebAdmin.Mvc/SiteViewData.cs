using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AndroWebAdmin.Mvc
{
    public interface ISiteViewData : IViewDataContainer
    {

    }

    public class SiteViewDataDictionary : ViewDataDictionary { }

    public class SiteViewData : SiteViewDataDictionary, ISiteViewData
    {
        #region IViewDataContainer Members

        public ViewDataDictionary ViewData { get; set; }

        #endregion
    }
}
