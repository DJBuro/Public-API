using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoyaltyWS;

public partial class Register : LoyaltyPage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        var UserTitles = lib.GetUserTitles();
        var countries = lib.GetCountries();

        foreach (ListItem userTitle in UserTitles)
        {
            ddlTitle.Items.Add(userTitle);
        }

        foreach (ListItem country in countries)
        {
            ddlCountry.Items.Add(country);
        }
    }


    protected void btnStep1_Click(object sender, EventArgs e)
    {

        var register = new serviceRegister();

        //only UserTitle.Id is needed - webservice will hydrate rest of object server side
        register.UserTitle = new UserTitle {Id = Int32.Parse(ddlTitle.SelectedItem.Value)};
        
        register.Country = new Country();

        register.FirstName = tbFirstName.Text;
        register.MiddleInitial = tbMiddleInitial.Text;
        register.SurName = tbSurname.Text;

        register.EmailAddress = tbEmailAddress.Text;
        register.Password = tbPassword.Text;

        register.AddressLineOne = txtAddressLn1.Text;
        register.AddressLineTwo = txtAddressLn2.Text;
        register.AddressLineThree = txtAddressLn3.Text;
        register.AddressLineFour = txtAddressLn4.Text;
        register.TownCity = txtTownCity.Text;
        register.PostCode = txtPostCode.Text;
        register.CountyProvince = txtCountyProvince.Text;


        //only Country.Id is needed - webservice will hydrate rest of object server side
        register.Country = new Country {Id = Int32.Parse(ddlCountry.SelectedItem.Value)};

        

        register.LoyaltyCardNumber = txtLCNumber.Text;

        var accountId =lib.Register(register);

        ltError.Text = null;

        if (accountId.success != null)
        {
            var serviceLoyaltyAccount = lib.GetLoyaltyAccount(Convert.ToInt32(accountId.success));

            Response.SetAuthoriationCookie(serviceLoyaltyAccount);
            Response.Redirect("~/LoyaltyAccount/MyAccount.aspx");
        }
        else
        {
            ltError.Text = accountId.error;
        }

    }
}
