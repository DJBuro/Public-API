using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dashboard.Dao.Domain;

namespace Dashboard.Web.Mvc
{
    public interface ISiteViewData : IViewDataContainer
    {
       // DashboardData DashboardData { get; set; }
        //User CameraShopUser { get; set; }
        //Basket Basket { get; set; }
        ////IDictionary<String, Object> Params { get; set; }
        ////void AddParams(IDictionary<String, Object> additions);
        //PagingData PagingData { get; set; }
    }

    public class SiteViewDataDictionary : System.Web.Mvc.ViewDataDictionary { }

    public class SiteViewData : SiteViewDataDictionary, ISiteViewData
    {
        #region IViewDataContainer Members

        public ViewDataDictionary ViewData {get; set;}


        //public DashboardData DashboardData{get; set;}

        #endregion
    }
}
