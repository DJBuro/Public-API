using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoyaltyWS;

public partial class MyAccount : LoyaltyPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        isLoggedIn = Request.GetAuthorisationCookie();

        if (!isLoggedIn)
            Response.Redirect("~/LoyaltyAccount/Login.aspx");

        loyaltyAccountId = Request.GetAuthoriationCookieLoyaltyAccountId();

        loyaltyAccount = lib.GetLoyaltyAccount(loyaltyAccountId);

        pointsValue =  loyaltyAccount.Points / lib.LoyaltyRatios().RedemptionRatio;


        foreach (serviceLoyaltyCard slc in this.loyaltyAccount.LoyaltyCards)
        {
            var but = new Button();
            but.Text = slc.CardNumber;
            but.CommandName = "cmdName";
            but.Click += new System.EventHandler(this.but_Click);
            add_button(but);
        }

    }

    protected void add_button(Button button)
    {
            //add to a container on the page
        PnlButtons.Controls.Add(new LiteralControl("<dd>"));

        PnlButtons.Controls.Add(button);
        PnlButtons.Controls.Add(new LiteralControl("</dd>"));
    }

    protected void but_Click(object sender, EventArgs e)
    {
        Session.Remove("LoyaltyCard");
        var p = (Button) sender;
        Session["LoyaltyCard"] = p.Text;
        Response.Redirect("LoyaltyCardTransactions.aspx");
    }
}
