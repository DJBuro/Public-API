using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoyaltyWS;

public partial class EditUserDetails : LoyaltyPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        isLoggedIn = Request.GetAuthorisationCookie();

        if (!isLoggedIn)
            Response.Redirect("~/LoyaltyAccount/Login.aspx");

        loyaltyAccountId = Request.GetAuthoriationCookieLoyaltyAccountId();
        loyaltyAccount = lib.GetLoyaltyAccount(loyaltyAccountId);
        pointsValue = loyaltyAccount.Points / lib.LoyaltyRatios().RedemptionRatio;

        if (!Page.IsPostBack)
        {
            var userTitles = lib.GetUserTitles();

            foreach (ListItem userTitle in userTitles)
            {
                ddlTitle.Items.Add(userTitle);
            }

            ddlTitle.SelectedValue = loyaltyAccount.LoyaltyUser.UserTitle.Id.ToString();

            tbFirstName.Text = loyaltyAccount.LoyaltyUser.FirstName;
            tbMiddleInitial.Text = loyaltyAccount.LoyaltyUser.MiddleInitial;
            tbSurname.Text = loyaltyAccount.LoyaltyUser.SurName;

            tbEmailAddress.Text = loyaltyAccount.LoyaltyUser.EmailAddress;
            //todo: dual password boxes etc.
            tbPassword.Text = loyaltyAccount.LoyaltyUser.Password;
        }
    }


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
            //note, we need the original id/details, so this is the easiest
            loyaltyUser = loyaltyAccount.LoyaltyUser;

            loyaltyUser.UserTitle.Id = Int32.Parse(ddlTitle.SelectedItem.Value);
            loyaltyUser.FirstName = this.tbFirstName.Text;
            loyaltyUser.MiddleInitial = tbMiddleInitial.Text;
            loyaltyUser.SurName = tbSurname.Text;

            loyaltyUser.EmailAddress = tbEmailAddress.Text;
            loyaltyUser.Password = tbPassword.Text;

            lib.Save(loyaltyUser);

            btnUpdate.Visible = false;

            Response.Redirect("UserDetails.aspx");
    }
}
