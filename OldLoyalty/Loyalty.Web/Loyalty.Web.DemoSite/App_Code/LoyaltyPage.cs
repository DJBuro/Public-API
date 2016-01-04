
using serviceLoyaltyAccount=LoyaltyWS.serviceLoyaltyAccount;
using serviceLoyaltyUser=LoyaltyWS.serviceLoyaltyUser;


/// <summary>
/// Summary description for LoyaltyPage
/// </summary>
public class LoyaltyPage : System.Web.UI.Page
{
    public readonly LoyaltyLibrary lib = new LoyaltyLibrary();
    public serviceLoyaltyAccount loyaltyAccount { get; set; }
    public serviceLoyaltyUser loyaltyUser { get; set; }
    public int pointsValue { get; set; }
    public bool isLoggedIn { get; set; }
    public int loyaltyAccountId { get; set; }

    public string LoyaltyCardNumber{ get; set;}


    //todo: add user details?

    public LoyaltyPage()
    {

    }
}

//public class  LoyaltyAdminPage : System.Web.UI.Page
//{
//    public readonly LoyaltyAdminLibrary lib = new LoyaltyAdminLibrary();
//    public LoyaltyAdmin.serviceLoyaltyAccount[] AllAccounts { get; set; }
//    public LoyaltyAdmin.serviceSite[] AllSites { get; set; }
//    public LoyaltyAdmin.serviceSite LoyaltySite { get; set; }
//    public LoyaltyAdmin.serviceCompany LoyaltyCompany { get; set; }

//    public LoyaltyAdmin.serviceLoyaltyAccount loyaltyAccount { get; set; }
//    public serviceLoyaltyUser loyaltyUser { get; set; }
//    public int pointsValue { get; set; }
//    public bool isLoggedIn { get; set; }
//    public int loyaltyAccountId { get; set; }

//    private string _loyaltyCardNumber;
//    public string LoyaltyCardNumber{ get; set;}


//    //todo: add user details?

//    public LoyaltyAdminPage()
//    {

//    }
//}



//public class LoyaltyAndroPage : System.Web.UI.Page
//{
//    public readonly LoyaltyAndroAdmin lib = new LoyaltyAndroAdmin();
//    public serviceCompany[] AllCompanies { get; set; }
//    public serviceCompany Company { get; set; }
//    public serviceSite LoyaltySite { get; set; }
//    public serviceCountry Country { get; set; }
//    public UserTitle UserTitle { get; set; }
//    public string LoyaltyCardNumber { get; set; }


//    //todo: add user details?

//    public LoyaltyAndroPage()
//    {

//    }
//}

//public class LoyaltyAdminUc : System.Web.UI.UserControl
//{
//    public readonly LoyaltyAdminLibrary lib = new LoyaltyAdminLibrary();
//    public LoyaltyAdmin.serviceLoyaltyAccount[] AllAccounts { get; set; }
//    public LoyaltyAdmin.serviceSite[] AllSites { get; set; }

//}

public class LoyaltyMasterPage : System.Web.UI.MasterPage
{

    public LoyaltyMasterPage()
    {

    }
}
