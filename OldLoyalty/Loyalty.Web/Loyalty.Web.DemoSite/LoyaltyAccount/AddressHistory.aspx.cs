using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoyaltyWS;

public partial class AddressHistory : LoyaltyPage
{
    public List<RamesesAddress> address { get; set; }



    protected void Page_Load(object sender, EventArgs e)
    {
        isLoggedIn = Request.GetAuthorisationCookie();
        if (!isLoggedIn)
            Response.Redirect("~/LoyaltyAccount/Login.aspx");

        if (Session["LoyaltyCard"] == null)
            Response.Redirect("~/LoyaltyAccount/MyAccount.aspx");

        loyaltyAccountId = Request.GetAuthoriationCookieLoyaltyAccountId();

        loyaltyAccount = lib.GetLoyaltyAccount(loyaltyAccountId);

        pointsValue = loyaltyAccount.Points / lib.LoyaltyRatios().RedemptionRatio;

        LoyaltyCardNumber = Session["LoyaltyCard"].ToString(); 

        address = lib.FindAssociatedAddress(LoyaltyCardNumber);

        var x = Request.QueryString.Get("address");

        if(x != null)
        {
            lib.RemoveAddressFromCard(LoyaltyCardNumber, Convert.ToInt32(x));
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }

}
