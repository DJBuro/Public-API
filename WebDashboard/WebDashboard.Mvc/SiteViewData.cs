
using System.Web.Mvc;

namespace WebDashboard.Mvc
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
