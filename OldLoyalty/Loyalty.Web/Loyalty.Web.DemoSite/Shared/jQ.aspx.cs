using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Shared_jQ : LoyaltyPage
{
    private readonly LoyaltyLibrary lib = new LoyaltyLibrary();

    //note: better with MVC, as you can just call the controller/method
    protected void Page_Load(object sender, EventArgs e)
    {

        var method = Request.QueryString.Get("method");
        if (method.Length == 0)
            return;

        if(Session["LoyaltyCard"] ==  null)
            return;

        LoyaltyCardNumber = Session["LoyaltyCard"].ToString(); 

        switch (method)
        {
            case "RemoveAddressFromCard":
                this.RemoveAddressFromCard(this.Request.Params.Get("id"));
                break;


        }


        //var userId = this.Request.Params.Get("LoyaltyCardUserId");

        //if (userId != null)
        //{

        //    var id = Convert.ToInt32(userId);

        //    var request = this.Request.Params.Get("id");

        //    var message = lib.RemoveCardFromAccount(request);
        //}

    }

    private void RemoveAddressFromCard(string addressId)
    {
        lib.RemoveAddressFromCard(LoyaltyCardNumber, Convert.ToInt32(addressId));
    }

}
