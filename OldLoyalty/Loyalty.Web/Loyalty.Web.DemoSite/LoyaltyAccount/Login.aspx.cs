using System;
using System.Web.UI;



public partial class Login : LoyaltyPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        isLoggedIn = Request.GetAuthorisationCookie();
        
        //if(isLoggedIn)
        //    Response.Redirect("~/LoyaltyAccount/MyAccount.aspx");

        if (this.tbEmailAddress.Text != "" & this.tbPassword.Text != "")
        {
            var serviceLoyaltyAccount = lib.Login(tbEmailAddress.Text, tbPassword.Text);

            var userData = "ApplicationSpecific data for this user.";

            if (serviceLoyaltyAccount != null)
            {
                //MembershipCreateStatus createStatus;

                Response.SetAuthoriationCookie(serviceLoyaltyAccount);

                Response.Redirect("~/LoyaltyAccount/MyAccount.aspx");
            }
            else
            {
                //todo: this is null: archived account,wrong password etc
            }
        }

    }

    protected void btnLogin_Click(object sender, ImageClickEventArgs e)
    {


    }
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        Response.Redirect("Register.aspx");
    }
}
