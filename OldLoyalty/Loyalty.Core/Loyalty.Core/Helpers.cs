using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using log4net;
using Loyalty.Dao.Domain;
using Loyalty.Dao.Domain.Service;
using Loyalty.Web.Service.Domain;

namespace Loyalty.Core
{
    public static class Helpers
    {
        #region Logging

        private static readonly ILog LOG = LogManager.GetLogger(typeof(Helpers));

        private static int SiteId;

        public enum Reason
        {
            FatalException,
            Deviation,
            Unknown,
            Invalid_site_details,
            Card_already_registered,
            Card_cannot_be_found,
            User_has_already_registered_account,  //note: not the most descriptive 'Another User has registered the account?'
            Email_address_already_exists,
            Card_has_not_been_registered,
            Card_no_longer_active,
            Card_does_not_have_a_pin_to_change,
            Card_associated_to_address_cannot_be_found,
            Card_already_has_pin,
            Multiple_cards_with_same_number,
            Invalid_Username_Password,
            Invalid_email_address,
            User_does_not_have_account,
            User_cannot_be_found,
            Account_has_been_deactivated,
            Company_does_not_exist,
            Invalid_CompanyId_for_Site,
            Invalid_Company_details
        }

        public static void Log(this System.Reflection.MethodBase methodName, Site site, Reason reason )
        {
            Log(methodName, site, reason, "unknown");  
        }

        /// <summary>
        /// Logs captured errors
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="site"></param>
        /// <param name="reason"></param>
        /// <param name="variables"></param>
        public static void Log(this System.Reflection.MethodBase methodName, Site site, Reason reason, string variables)
        {
            if (site != null)
            {
                SiteId = site.Id.Value;
            }
            else
            {
                SiteId = 0; //set to 0 as it may keep previous ids
            }

            Log(methodName, SiteId, reason, variables);

        }

        /// <summary>
        /// Logs captured Company Admin errors.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="company"></param>
        /// <param name="reason"></param>
        /// <param name="variables"></param>
        public static void Log(this System.Reflection.MethodBase methodName, Company company, Reason reason, string variables)
        {
            if (company != null)
            {
                SiteId = company.Id.Value;
            }
            else
            {
                SiteId = 0; //set to 0 as it may keep previous ids
            }

            Log(methodName, SiteId, reason, variables);

        }

        private static void Log(this System.Reflection.MethodBase methodName, int siteId, Reason reason, string variables)
        {
            log4net.GlobalContext.Properties["siteId"] = siteId;
            log4net.GlobalContext.Properties["method"] = methodName.DeclaringType + "." + methodName.Name;
            log4net.GlobalContext.Properties["variables"] = variables;

            LOG.Error(reason);
        }

        #endregion

        #region Registration
        
        public static LoyaltyUser TranslateToLoyaltyUser(serviceRegister registerDetails)
        {
            var loyaltyUser = new LoyaltyUser
                                  {
                                      FirstName = registerDetails.FirstName,
                                      MiddleInitial = registerDetails.MiddleInitial,
                                      SurName = registerDetails.SurName,
                                      Password = registerDetails.Password,
                                      EmailAddress = registerDetails.EmailAddress,
                                      UserTitle = registerDetails.UserTitle
                                  };

            return loyaltyUser;
        }

        public static AccountAddress TranslateToAccountAddress(serviceRegister registerDetails)
        {
            var accountAddress = new AccountAddress
                                     {
                                         AddressLineOne = registerDetails.AddressLineOne,
                                         AddressLineTwo = registerDetails.AddressLineTwo,
                                         AddressLineThree = registerDetails.AddressLineThree,
                                         AddressLineFour = registerDetails.AddressLineFour,
                                         TownCity = registerDetails.TownCity,
                                         PostCode = registerDetails.PostCode,
                                         CountyProvince = registerDetails.CountyProvince,
                                         Country = registerDetails.Country
                                     };
            return accountAddress;
        }


        public static RamesesAddress TranslateToRamesesAddress(serviceOrderAddress orderAddress)
        {
            var accountAddress = new RamesesAddress
                                     {
                                         Id = orderAddress.Id,
                                         Prem1 = orderAddress.AddressLine1,
                                         Prem2 = orderAddress.AddressLine2,
                                         Prem3 = orderAddress.AddressLine3,
                                         Town = orderAddress.Town,
                                         PostCode = orderAddress.PostCode,
                                         County = orderAddress.Province,
                                         Locality = orderAddress.Country.Name
                                     };

            return accountAddress;
        }

        #endregion


        public static Site TranslateToSite(serviceSite serviceSite)
        {
            var site = new Site
                           {
                               Id = serviceSite.Id,
                               Name = serviceSite.Name,
                               SiteKey = serviceSite.SiteKey,
                               SitePassword = serviceSite.SitePassword
                           };

            return site;
        }

        public static bool CompareSites(serviceSite serviceSite, Site site)
        {
            int isSame = 0;

            isSame += (serviceSite.Id.Value == site.Id.Value ? 0 : 1);
            isSame += (serviceSite.Name == site.Name ? 0 : 1);
            isSame += (serviceSite.RamesesSiteId == site.RamesesSiteId ? 0 : 1);
            isSame += (serviceSite.SiteKey == site.SiteKey ? 0 : 1);
            isSame += (serviceSite.SitePassword== site.SitePassword ? 0 : 1);
            isSame += (serviceSite.Country.Id == site.Country.Id ? 0 : 1);

            if (isSame > 0)
                return false;

            return true;

        }

        public static serviceSite TranslateToServiceSite(Site site)
        {
            var serviceSite = new serviceSite();
            serviceSite.Id = site.Id;
            serviceSite.Name = site.Name;
            serviceSite.SiteKey = site.SiteKey;
            serviceSite.SitePassword = site.SitePassword;
            serviceSite.RamesesSiteId = site.RamesesSiteId;

            serviceSite.Country = new serviceCountry();
            
            if(site.Country !=null)
                serviceSite.Country = Helpers.TranslateToServiceCountry(site.Country);

            return serviceSite;

        }

        public static serviceCountry TranslateToServiceCountry(Country country)
        {
            var serviceCountry = new serviceCountry();

            serviceCountry.Id = country.Id;
            serviceCountry.Name = country.Name;
            serviceCountry.ISOCode = country.ISOCode;

            foreach (CountryCurrency countryCurrency in country.CountryCurrencies)
            {
                serviceCountry.Currency = countryCurrency.Currency;
            }

            return serviceCountry;
        }

        public static Country TranslateToCountry(serviceCountry serviceCountry)
        {
            var country = new Country();

            country.Id = serviceCountry.Id;
            country.Name = serviceCountry.Name;
            country.ISOCode = serviceCountry.ISOCode;

            country.CountryCurrencies.Add(serviceCountry.Currency);

            return country;
        }

        public static Company TranslateToCompany(serviceCompany serviceCompany)
        {
            var company = new Company();

            company.Id = serviceCompany.Id;
            company.Name = serviceCompany.Name;
            company.CompanyKey = serviceCompany.CompanyKey;
            company.CompanyPassword = serviceCompany.CompanyPassword;

            company.RedemptionRatio = serviceCompany.RedemptionRatio;

            company.Country = new Country();
            company.Country = Helpers.TranslateToCountry(serviceCompany.Country);

            //note: doesn't translate dependants eg... sites, lc etc - don't need to
            return company;
        }

        public static serviceCompany TranslateToServiceCompany(Company company)
        {
            var serviceCompany = new serviceCompany();

            serviceCompany.CompanyLoyaltyAccounts = new List<serviceLoyaltyAccount>();
            serviceCompany.Sites = new List<serviceSite>();
            serviceCompany.Country = new serviceCountry();

            serviceCompany.Id = company.Id;
            serviceCompany.Name = company.Name;
            serviceCompany.RedemptionRatio = company.RedemptionRatio;
            serviceCompany.CompanyKey = company.CompanyKey;
            serviceCompany.CompanyPassword = company.CompanyPassword;

            serviceCompany.Country = Helpers.TranslateToServiceCountry(company.Country);
           

            foreach (CompanyLoyaltyAccount coLoyaltyAccount in company.LoyaltyAccounts)
            {
                var account = Helpers.TranslateToServiceLoyaltyAccount(coLoyaltyAccount.LoyaltyAccount);

                if(account != null)
                    serviceCompany.CompanyLoyaltyAccounts.Add(account);
            }

            foreach (Site site in company.Sites)
            {
                serviceCompany.Sites.Add(Helpers.TranslateToServiceSite(site));
            }

            return serviceCompany;
        }


        public static Status GetLoyaltyCardStatus(LoyaltyCard loyaltyCard)
        {
            foreach(LoyaltyCardStatus ls in loyaltyCard.LoyaltyCardStatus)
            {
                return ls.Status;
            }

            return null;
        }

        /// <summary>
        /// Lists all cards used at a RamesesAddress
        /// </summary>
        /// <param name="ramesesAddress"></param>
        /// <returns></returns>
        public static List<serviceLoyaltyCard> ListCardsByAddress(RamesesAddress ramesesAddress)
        {
            var cardByAddress = new List<serviceLoyaltyCard>();

            //translate for the serviceCard
            var card = new serviceLoyaltyCard();
            
            foreach (RamesesAddressLoyaltyCard loyaltyCard in ramesesAddress.RamesesAddressLoyaltyCard)
            {
                card = TranslateToServiceLoyaltyCard(loyaltyCard.LoyaltyCard);
             
                var loyaltyCardStatus = (LoyaltyCardStatus)loyaltyCard.LoyaltyCard.LoyaltyCardStatus[0];

                //make sure returning only returning live
                if (loyaltyCardStatus.Status.Id == 1)
                    cardByAddress.Add(card);
            }

            cardByAddress.OrderByDescending(c => c.LoyaltyAccount.Id);

            return cardByAddress;
        }

        /// <summary>
        /// Since Rameses Passes in Pence, we must divide by 100
        /// </summary>
        /// <param name="transactionHistory"></param>
        /// <returns></returns>
        public static serviceTransactionHistory ConvertFromRamesesPence(ref serviceTransactionHistory transactionHistory)
        {
            transactionHistory.OrderTotal = transactionHistory.OrderTotal/100;
           
            foreach(var orderItemHistory  in transactionHistory.OrderItemHistory)
            {
                orderItemHistory.ItemPrice = orderItemHistory.ItemPrice/100;
            }

            return transactionHistory;
        }


        /// <summary>
        /// Creates/Adds the Transaction History of a card when it is used.
        /// </summary>
        /// <param name="card"></param>
        /// <param name="points"></param>
        /// <param name="transaction"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public static TransactionHistory AddTransactionHistory(LoyaltyCard card, int points, serviceTransactionHistory transaction, Site site)
        {

            //bug: we don't need int points.
            var transHistory = new TransactionHistory
                               {
                                   LoyaltyCard = card,
                                   OrderId = transaction.OrderId,
                                   OrderType = new OrderType(),
                                   OrderTotal = transaction.OrderTotal,
                                   LoyaltyPointsAdded = 0,
                                   LoyaltyPointsRedeemed = transaction.LoyaltyPointsRedeemed,
                                   LoyaltyPointsValue = site.Company.RedemptionRatio
                               };

            transHistory.OrderType.Id = transaction.OrderTypeId;

            var loyatyPointsAdded = 0;

            foreach (serviceOrderItemHistory item in transaction.OrderItemHistory)
            {
                var orderHistory = new OrderItemHistory {Id = item.Id, ItemLoyaltyPoints = item.ItemLoyaltyPoints};

                loyatyPointsAdded = loyatyPointsAdded + item.ItemLoyaltyPoints;

                orderHistory.ItemPrice = item.ItemPrice;
                orderHistory.Name = item.Name;
                orderHistory.TransactionHistory = transHistory;
                transHistory.OrderItemHistory.Add(orderHistory);
            }

            //03 - June 2009
            //note: BC change, pass total points in regardless if they are in the order
            //eg.  what if they discount the total price in points.
            // expect this to be buggy as it breaks encapsulation!!!
            if (loyatyPointsAdded != transaction.LoyaltyPointsAdded)
            {
                transHistory.LoyaltyPointsAdded = transaction.LoyaltyPointsAdded;
            }
            else
            {
                transHistory.LoyaltyPointsAdded = loyatyPointsAdded;
            }

            transHistory.Site = site;

            return transHistory;
        }


        public static AccountAddress TranslateToAccountAddress(serviceAccountAddress serviceAccountAddress)
        {
            var accountAddress = new AccountAddress
                                     {
                                         Id = serviceAccountAddress.Id,
                                         AddressLineOne = serviceAccountAddress.AddressLineOne,
                                         AddressLineTwo = serviceAccountAddress.AddressLineTwo,
                                         AddressLineThree = serviceAccountAddress.AddressLineThree,
                                         AddressLineFour = serviceAccountAddress.AddressLineFour,
                                         CountyProvince = serviceAccountAddress.CountyProvince,
                                         TownCity = serviceAccountAddress.TownCity,
                                         PostCode = serviceAccountAddress.PostCode,
                                         Country = Helpers.TranslateToCountry(serviceAccountAddress.Country)
                                     };

            if (serviceAccountAddress.serviceLoyaltyAccount != null)
            {
                accountAddress.LoyaltyAccount = new LoyaltyAccount();
                accountAddress.LoyaltyAccount.Id = serviceAccountAddress.serviceLoyaltyAccount.Id;
            }

            return accountAddress;
        }

        public static serviceLoyaltyCard TranslateToServiceLoyaltyCard(LoyaltyCard loyaltyCard)
        {
            var serviceLoyaltyCard = new serviceLoyaltyCard();
            var serviceLoyaltyAccount = new serviceLoyaltyAccount();

            //note: needed? only used in SiteService for Rameses
            //var serviceAccountAddress = new serviceAccountAddress();

            serviceLoyaltyCard.LoyaltyAccount = serviceLoyaltyAccount;

            serviceLoyaltyCard.LoyaltyAccount.LoyaltyUser = new serviceLoyaltyUser();

            serviceLoyaltyCard.Id = loyaltyCard.Id;
            serviceLoyaltyCard.CardNumber = loyaltyCard.CardNumber;
            serviceLoyaltyCard.Pin = loyaltyCard.Pin;

            serviceLoyaltyCard.LoyaltyAccount = serviceLoyaltyAccount;
            serviceLoyaltyCard.LoyaltyAccount.Id = loyaltyCard.LoyaltyAccount.Id;
            serviceLoyaltyCard.LoyaltyAccount.Points = loyaltyCard.LoyaltyAccount.Points;

            foreach (LoyaltyUser loyaltyUser in loyaltyCard.LoyaltyAccount.LoyaltyUser)
            {
                serviceLoyaltyCard.LoyaltyAccount.LoyaltyUser = TranslateToServiceLoyaltyUser(loyaltyUser);
            }

            return serviceLoyaltyCard;
        }

        public static serviceLoyaltyUser TranslateToServiceLoyaltyUser(LoyaltyUser loyaltyUser)
        {
            var serviceLoyaltyUser = new serviceLoyaltyUser();

            serviceLoyaltyUser.Id = loyaltyUser.Id;
            serviceLoyaltyUser.FirstName = loyaltyUser.FirstName;
            serviceLoyaltyUser.MiddleInitial = loyaltyUser.MiddleInitial;
            serviceLoyaltyUser.SurName = loyaltyUser.SurName;
            serviceLoyaltyUser.EmailAddress = loyaltyUser.EmailAddress;
            serviceLoyaltyUser.Password = loyaltyUser.Password;
            serviceLoyaltyUser.UserTitle = loyaltyUser.UserTitle;

            return serviceLoyaltyUser;
        }

        public static serviceAccountAddress TranslateToServiceAccountAddress(AccountAddress accountAddress)
        {
            var address = new serviceAccountAddress();
            address.Id = accountAddress.Id;
            address.AddressLineOne = accountAddress.AddressLineOne;
            address.AddressLineTwo = accountAddress.AddressLineTwo;
            address.AddressLineThree = accountAddress.AddressLineThree;
            address.AddressLineFour = accountAddress.AddressLineFour;
            
            address.PostCode = accountAddress.PostCode;
            address.TownCity = accountAddress.TownCity;
            
            address.CountyProvince = accountAddress.CountyProvince;
            address.Country = Helpers.TranslateToServiceCountry(accountAddress.Country);

            //note: infiniteloop
            //address.serviceLoyaltyAccount = TranslateToServiceLoyaltyAccount(accountAddress.LoyaltyAccount);

            return address;
        }

        public static serviceLoyaltyAccount TranslateToServiceLoyaltyAccount(LoyaltyAccount loyaltyAccount)
        {
            var serviceLoyaltyAccount = new serviceLoyaltyAccount();
            serviceLoyaltyAccount.LoyaltyCards = new List<serviceLoyaltyCard>();
            serviceLoyaltyAccount.AccountAddress = new serviceAccountAddress();

            foreach (LoyaltyAccountStatus loyaltyAccountStatus in  loyaltyAccount.AccountStatus)
            {
                //only live accounts returned!
                if (loyaltyAccountStatus.AccountStatus.Id != 1)
                    return null;
            }

            serviceLoyaltyAccount.Id = loyaltyAccount.Id;
            serviceLoyaltyAccount.Points = loyaltyAccount.Points;

            if (loyaltyAccount.AccountAddress.Count != 0)
            {               //add the account address
                serviceLoyaltyAccount.AccountAddress =
                    TranslateToServiceAccountAddress((AccountAddress) loyaltyAccount.AccountAddress[0]);
            }
            foreach (LoyaltyCard loyaltyCard in loyaltyAccount.LoyaltyCards)
            {
                foreach(LoyaltyCardStatus lcs in loyaltyCard.LoyaltyCardStatus)
                {
                    if (lcs.Status.Id == 1)
                    {
                        //todo:  we should use TRANSLATE_TO_SERVICE_LOYALTYCARD?
                        var card = new serviceLoyaltyCard
                                       {
                                           Id = loyaltyCard.Id,
                                           CardNumber = loyaltyCard.CardNumber,
                                           Pin = loyaltyCard.Pin
                                       };

                        serviceLoyaltyAccount.LoyaltyCards.Add(card);
                    }
                }
            }

            serviceLoyaltyAccount.LoyaltyUser = new serviceLoyaltyUser();

            foreach(LoyaltyUser loyaltyUser in loyaltyAccount.LoyaltyUser)
            {
                serviceLoyaltyAccount.LoyaltyUser = TranslateToServiceLoyaltyUser(loyaltyUser);
            }

            return serviceLoyaltyAccount;
        }


        public static bool validEmailAddress(string emailAddress)
        {
            const string pattern = "(\\b[A-Z0-9._%-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}\\b)";

            Regex exp = new Regex(pattern, RegexOptions.IgnoreCase);

            MatchCollection MatchList = exp.Matches(emailAddress);
            
            if(MatchList.Count == 1)
                return true;

            return false;
        }

        //public static bool validEmailAddress()
        //{
        //    var result = new List<string>();
        //    var _in = "%00000A123001178?";

        //   // var pattern = "(?<loyaltyCard>[a-b]\\d\\d\\d)";

        //    var pattern = "(\\b[A-Z0-9._%-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}\\b)";


        //    Regex exp = new Regex(pattern, RegexOptions.IgnoreCase);

        //    //if (_in == null) return false;

        //    MatchCollection MatchList = exp.Matches(_in);

        //    var x = exp.GroupNumberFromName("loyaltyCard");


        //    foreach (Match match in MatchList)
        //    {
        //        result.Add(match.Value);
        //    }


        //    var gx = exp.GetGroupNames();
            

        //    return true;

        //}

    }
}
