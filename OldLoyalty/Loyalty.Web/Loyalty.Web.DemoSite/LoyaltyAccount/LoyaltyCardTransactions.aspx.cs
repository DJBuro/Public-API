using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoyaltyWS;

public partial class LoyaltyCardTransactions : LoyaltyPage
{

    public string loyaltyCardNumber { get; set; }
    public List<serviceTransactionHistory> transactionHistory { get; set; }
    

    protected void Page_Load(object sender, EventArgs e)
    {
        isLoggedIn = Request.GetAuthorisationCookie();
        if (!isLoggedIn)
            Response.Redirect("~/LoyaltyAccount/Login.aspx");

        if (Session["LoyaltyCard"] == null)
            Response.Redirect("~/LoyaltyAccount/MyAccount.aspx");

        loyaltyAccountId = Request.GetAuthoriationCookieLoyaltyAccountId();

        loyaltyAccount = lib.GetLoyaltyAccount(loyaltyAccountId);

        loyaltyCardNumber = Session["LoyaltyCard"].ToString();

        transactionHistory = lib.GetTransactionHistory(loyaltyCardNumber);



        pointsValue = loyaltyAccount.Points / lib.LoyaltyRatios().RedemptionRatio;
    }
}
