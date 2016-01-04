using System.Globalization;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using xyz = Spring.Web.UI;

namespace OrderTrackingAdmin.Models
{
    //todo: move this to MVC
    //todo: test spring on this (for translations)
    //C# no mutiple inheritance...a.rrrrrh...
    public class MasterTest
    {
        public class TestMasterPage : xyz.MasterPage, IViewDataContainer
        {
            public ViewDataDictionary ViewData { get; set; }
        }

       
        public class blah: xyz.MasterPage
        {
            
        }

        public class MasterView : System.Web.Mvc.ViewMasterPage, IViewDataContainer
        {
            public ViewDataDictionary ViewData { get; set; }
        }

        public class MasterPage<TModel> : MasterView where TModel : class
        {
        }
    }
}
