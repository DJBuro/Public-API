using System;
using System.Web.Mvc;


namespace OrderTrackingAdmin.Mvc
{
    public interface ISiteViewData : IViewDataContainer
    {
        String LanguageCode{ get; set;}
    }

    public class SiteViewDataDictionary : ViewDataDictionary
    {
        
    }

    public class SiteViewData :  SiteViewDataDictionary, ISiteViewData
    {
        public ViewDataDictionary ViewData { get; set; }

        #region ISiteViewData Members

        public string LanguageCode
        {
            get; set;
        }

        #endregion
    }

}
