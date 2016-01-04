using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoyaltyWS;

public partial class RemoveCard : LoyaltyPage
{

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

        if(Page.IsPostBack)
        {
            Response.Redirect("CardRemoved.aspx");
        }
    }

    protected void imgRemoveCard_Click(object sender, EventArgs e)
    {

    }
}
