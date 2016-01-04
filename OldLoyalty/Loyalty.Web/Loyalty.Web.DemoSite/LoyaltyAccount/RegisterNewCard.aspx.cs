using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoyaltyWS;

public partial class RegisterNewCard : LoyaltyPage
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

    protected void btnRegisterCard_Click(object sender, EventArgs e)
    {
        if(tbLoyaltyCard.Text != "")
            {
                var message = lib.AddCardToAccount(tbLoyaltyCard.Text, loyaltyAccount);

                this.btnRegisterCard.Visible = false;

            if(message.error !=null)
            {
                this.ltFinished.Text = message.error;
            }
            else
            {
                this.ltFinished.Text = "This card has been registered";
            }
                
                this.ltFinished.Visible = true;
                
            }
    }
}
