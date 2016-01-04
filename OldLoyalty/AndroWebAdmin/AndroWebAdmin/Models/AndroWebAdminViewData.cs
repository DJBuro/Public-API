using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroWebAdmin.Mvc;
using Loyalty.Dao.Domain;

namespace AndroWebAdmin.Models
{
    public class AndroWebAdminViewData : SiteViewData
    {
        /// <summary>
        /// Masterpage, all other pages inherit from this.
        /// </summary>
        public class AndroWebAdminBaseViewData : SiteViewData
        {
            public Company Company;
            public IList<Country> Countries;

        }

        public class IndexViewData : AndroWebAdminBaseViewData
        {
            
        }

        public class GlobalViewData : AndroWebAdminBaseViewData
        {
            public IList<Company> Companies;
            public Site Site;
            public IList<Currency> Currencies;
            public Currency Currency;
            public IList<LoyaltyLog> LoyaltyLogs;
            public Country Country;
            public IEnumerable<SelectListItem> CurrencyListItems;

            public LoyaltyLog LoyaltyLog;
        }


        public class CompanyViewData : AndroWebAdminBaseViewData
        {

            public Site Site;
            public IList<Company> Companies;
            public IEnumerable<SelectListItem> CountryListItems;
            public IEnumerable<SelectListItem> UserTitleListItems;
            public IList<Site> Sites;
            public IList<LoyaltyAccount> LoyaltyAccounts;
            public IList<CompanyUserTitle> CompanyUserTitles;
            public CompanyUserTitle CompanyUserTitle;
        }

        public class LoyaltyAccountViewData : AndroWebAdminBaseViewData
        {
            public IList<LoyaltyAccount> LoyaltyAccounts;
            public LoyaltyAccount LoyaltyAccount;
            public IList<Company> Companies;
            public IList<AccountStatus> AccountStatuses;
            public AccountStatus AccountStatus;

            public IEnumerable<SelectListItem> AccountStatusListItems;
        }

        public class LoyaltyCardViewData : AndroWebAdminBaseViewData
        {
            public IList<LoyaltyAccount> LoyaltyAccounts;
            public LoyaltyAccount LoyaltyAccount;
            public IEnumerable<SelectListItem> CompanyListItems;
            public IEnumerable<SelectListItem> AccountStatusListItems;
            public IList<Company> Companies;
            public LoyaltyCard LoyaltyCard;
            public IList<LoyaltyCard> LoyaltyCards;
            public IList<Status> Statuses;
            public Status Status;
        }

        public class LoyaltyUserViewData : AndroWebAdminBaseViewData
        {
            public IList<LoyaltyAccount> LoyaltyAccounts;
            public IList<LoyaltyUser> LoyaltyUsers;
            public IList<Company> Companies;
             
        }        

        public class SearchViewData : AndroWebAdminBaseViewData
        {
            public IList<LoyaltyCard> LoyaltyCards;
            public IList<LoyaltyAccount> LoyaltyAccounts;
            public LoyaltyUser LoyaltyUser;
            public IList<LoyaltyUser> LoyaltyUsers;
            public IEnumerable<SelectListItem> CompanyListItems;
        }

    }
}
