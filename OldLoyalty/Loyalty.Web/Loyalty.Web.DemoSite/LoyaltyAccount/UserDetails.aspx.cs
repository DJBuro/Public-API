using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoyaltyWS;

public partial class UserDetails : LoyaltyPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        isLoggedIn = Request.GetAuthorisationCookie();
        
        if (!isLoggedIn)
            Response.Redirect("~/LoyaltyAccount/Login.aspx");

        loyaltyAccountId = Request.GetAuthoriationCookieLoyaltyAccountId();

        loyaltyAccount = lib.GetLoyaltyAccount(loyaltyAccountId);

        pointsValue =  loyaltyAccount.Points / lib.LoyaltyRatios().RedemptionRatio;
    }


    protected void btnEditUserDetails_Click(object sender, EventArgs e)
    {
        Response.Redirect("EditUserDetails.aspx");
    }
    protected void btnEditAccountDetails_Click(object sender, EventArgs e)
    {
        Response.Redirect("EditAccountDetails.aspx");
    }
}
