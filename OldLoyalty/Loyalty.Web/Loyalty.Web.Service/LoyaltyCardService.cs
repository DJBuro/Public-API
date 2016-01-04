using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loyalty.Dao;
using Loyalty.Dao.Domain;
using Loyalty.Dao.Domain.Service;
using Loyalty.Web.Service.Dao;
using Loyalty.Web.Service.Domain;
using Loyalty.Core;

namespace Loyalty.Web.Service
{
    public class LoyaltyCardService : ILoyaltyCardServiceDao
    {
        #region IDAOs

        public ILoyaltyCardDao LoyaltyCardDao { get; set; }
        public ILoyaltyUserDao LoyaltyUserDao { get; set; }
        public IRamesesAddressDao RamesesAddressDao { get; set; }
        public ISiteDao SiteDao { get; set; }
        public ICompanyDao CompanyDao { get; set; }
        public IStatusDao StatusDao { get; set; }

        #endregion

        #region ILoyaltyCardServiceDao Members

        /// <summary>
        /// The ratio at what points are redeemed for money
        /// </summary>
        /// <param name="site"></param>
        /// <returns>int</returns>
        public serviceRatio GetRatios(serviceSite site)
        {
            Site thisSite = SiteDao.ValidateSite(site);

            if (thisSite == null)//invalid site details passed
            {
               System.Reflection.MethodBase.GetCurrentMethod().Log(thisSite, Helpers.Reason.Invalid_site_details, "SiteKey: "+ site.SiteKey + " SitePass: " + site.SitePassword);
                return null;
            }

            var ratio = new serviceRatio
                            {
                                RedemptionRatio = thisSite.Company.RedemptionRatio,
                                GainRatio = thisSite.Company.GainRatio,
                                MaxReedemablePoints = thisSite.Company.MaxRedeemablePoints,
                                OnlyWholeNumbers = thisSite.Company.OnlyWholeNumber
                            };

            return ratio;
        }

        public bool AddUpdateLoyaltySite(serviceSite site, int ramesesCompanyId)
        {
            Site thisSite = SiteDao.ValidateSite(site);

            if(thisSite == null)
            {
                thisSite = SiteDao.FindByName(site.Name);
            }

            bool sameSite = true;

            if(thisSite != null)
            {
                sameSite = Helpers.CompareSites(site,thisSite);
            }


            var company = CompanyDao.FindByRamesesCompanyId(ramesesCompanyId);

            if(company == null)
            {
                System.Reflection.MethodBase.GetCurrentMethod().Log(thisSite, Helpers.Reason.Company_does_not_exist, "RamesesCompanyId: " + ramesesCompanyId + " does not exist");
                return false;
            }

            if(thisSite == null) // add new site
            {
                thisSite = new Site();

                thisSite.SiteKey = site.SiteKey;
                thisSite.SitePassword = site.SitePassword;
                thisSite.RamesesSiteId = site.RamesesSiteId;
                thisSite.Name = site.Name;

                thisSite.Company = new Company();
                thisSite.Company.Id = company.Id;

                thisSite.Country = new Country();

                if (site.Country != null)
                {
                    thisSite.Country.Id = site.Country.Id;
                }
                else
                {
                    thisSite.Country.Id = company.Country.Id;
                }

                SiteDao.Create(thisSite);

                return true;
            }

            if (sameSite == false)
            {
                thisSite.SitePassword = site.SitePassword;
                thisSite.RamesesSiteId = site.RamesesSiteId;
                thisSite.Name = site.Name;

                thisSite.Company = new Company();
                thisSite.Company.Id = company.Id;

                thisSite.Country = new Country();

                if (site.Country != null)
                {
                    thisSite.Country.Id = site.Country.Id;
                }
                else
                {
                    thisSite.Country.Id = company.Country.Id;
                }

                SiteDao.Update(thisSite);
            }

            return true;
        }

        /// <summary>
        /// Gets account points available on a loyalty card
        /// </summary>
        /// <param name="loyaltyCardNumber"></param>
        /// <param name="site"></param>
        /// <returns>int</returns>
        public int GetLoyaltyCardPoints(string loyaltyCardNumber, serviceSite site)
        {
            Site thisSite = SiteDao.ValidateSite(site);

            if (thisSite == null)//invalid site details passed
            {
                System.Reflection.MethodBase.GetCurrentMethod().Log(thisSite, Helpers.Reason.Invalid_site_details, "SiteKey: "+ site.SiteKey + " SitePass: " + site.SitePassword);
                return -1;
            }

            return LoyaltyCardDao.GetLoyaltyCardPoints(loyaltyCardNumber, thisSite);
        }

        //note: same method in LoyaltyCardSiteService
        /// <summary>
        /// Add points to a loyalty account by a loyalty card number
        /// </summary>
        /// <param name="loyaltyCardNumber"></param>
        /// <param name="points"></param>
        /// <param name="transaction"></param>
        /// <param name="ramesesAddress"></param>
        /// <param name="site"></param>
        public void AddPointsToLoyaltyCard(string loyaltyCardNumber, int points, serviceTransactionHistory transaction, RamesesAddress ramesesAddress, serviceSite site)
        {
            Site thisSite = SiteDao.ValidateSite(site);

            if (thisSite == null)//invalid site details passed
            {
                System.Reflection.MethodBase.GetCurrentMethod().Log(thisSite, Helpers.Reason.Invalid_site_details, "SiteKey: "+ site.SiteKey + " SitePass: " + site.SitePassword);
                return;
            }

            //We are assuming the card number has passed all validity checks
            var loyaltyCard = LoyaltyCardDao.FindLoyaltyCardByNumber(loyaltyCardNumber, thisSite);

            if (loyaltyCard == null) //new card
            {
                loyaltyCard = new LoyaltyCard();
                loyaltyCard.CardNumber = loyaltyCardNumber; 
                
                //add the site/company
                loyaltyCard.LoyaltyAccount = new LoyaltyAccount(points,thisSite);
                loyaltyCard.LoyaltyAccount.CompanyLoyaltyAccount.Add(thisSite.Company);
            }
            else
            {
                var status = Helpers.GetLoyaltyCardStatus(loyaltyCard);

                if (status.Id != 1)
                {
                    System.Reflection.MethodBase.GetCurrentMethod().Log(thisSite, Helpers.Reason.Card_no_longer_active, "Card: " + loyaltyCard.CardNumber + " SiteKey: " + site.SiteKey + " SitePass: " + site.SitePassword);
                    return;
                }
            }

            if(ramesesAddress != null)
            {
                var address = RamesesAddressDao.FindRamesesAddress(ramesesAddress); //find the address

                //since Rameses sends in pence, we must convert to decimal
                Helpers.ConvertFromRamesesPence(ref transaction);

               //populate the transaction history
                loyaltyCard.TransactionHistory.Add(Helpers.AddTransactionHistory(loyaltyCard, points, transaction, thisSite));

                if (address != null) //address exists
                {
                    LoyaltyCardDao.AddPoints(loyaltyCard, points, address);
                    return;
                }
                
                LoyaltyCardDao.AddPoints(loyaltyCard, points, ramesesAddress);
                return;
            }
            
            LoyaltyCardDao.AddPoints(loyaltyCard, points, ramesesAddress);
            return;
        }

        //note: same method in LoyaltyCardSiteService
        /// <summary>
        /// Removes points from a loyalty account by a card number
        /// </summary>
        /// <param name="loyaltyCardNumber"></param>
        /// <param name="points"></param>
        /// <param name="site"></param>
        public void RemovePointsFromLoyaltyCard(string loyaltyCardNumber, int points, serviceSite site)
        {
            Site thisSite = SiteDao.ValidateSite(site);

            if (thisSite == null)//invalid site details passed
            {
                System.Reflection.MethodBase.GetCurrentMethod().Log(thisSite, Helpers.Reason.Invalid_site_details, "SiteKey: "+ site.SiteKey + " SitePass: " + site.SitePassword);
                return;
            }

            var loyaltyCard = LoyaltyCardDao.FindLoyaltyCardByNumber(loyaltyCardNumber, thisSite);


            if (loyaltyCard == null)
            {
                System.Reflection.MethodBase.GetCurrentMethod().Log(thisSite,Helpers.Reason.Card_cannot_be_found);
                return;
            }
            
            var status = Helpers.GetLoyaltyCardStatus(loyaltyCard);

            if (status.Id != 1)
            {
                System.Reflection.MethodBase.GetCurrentMethod().Log(thisSite, Helpers.Reason.Card_no_longer_active, "Card: " + loyaltyCard.CardNumber + "SiteKey: " + site.SiteKey + " SitePass: " + site.SitePassword);
                return;
            }

            LoyaltyCardDao.RemovePoints(loyaltyCard,points);
            return;
        }

        /// <summary>
        /// Gets the loyalty card details by its number
        /// </summary>
        /// <param name="loyaltyCardNumber"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public serviceLoyaltyCard GetLoyaltyCardDetails(string loyaltyCardNumber, serviceSite site)
        {
            Site thisSite = SiteDao.ValidateSite(site);

            if (thisSite == null)//invalid site details passed
            {
                System.Reflection.MethodBase.GetCurrentMethod().Log(thisSite, Helpers.Reason.Invalid_site_details, "SiteKey: "+ site.SiteKey + " SitePass: " + site.SitePassword);
                return null;
            }

            var loyaltyCard = LoyaltyCardDao.FindLoyaltyCardByNumber(loyaltyCardNumber, thisSite);

            if (loyaltyCard == null)
                return null;

            var status = Helpers.GetLoyaltyCardStatus(loyaltyCard);

            if (status.Id != 1)
            {
                System.Reflection.MethodBase.GetCurrentMethod().Log(thisSite, Helpers.Reason.Card_no_longer_active, "Card: " + loyaltyCard.CardNumber + "SiteKey: " + site.SiteKey + " SitePass: " + site.SitePassword);
                return null;
            }

            return Helpers.TranslateToServiceLoyaltyCard(loyaltyCard);

        }
 
        /// <summary>
        /// Finds which cards have been used at an address
        /// </summary>
        /// <param name="ramesesAddress"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<serviceLoyaltyCard> FindLoyaltyCardByAddress(RamesesAddress ramesesAddress, serviceSite site)
        {
            Site thisSite = SiteDao.ValidateSite(site);

            if (thisSite == null)//invalid site details passed
            {
                System.Reflection.MethodBase.GetCurrentMethod().Log(thisSite, Helpers.Reason.Invalid_site_details, "SiteKey: "+ site.SiteKey + " SitePass: " + site.SitePassword);
                return null;
            }

            var address = RamesesAddressDao.FindRamesesAddress(ramesesAddress);

            if(address == null)
                return new List<serviceLoyaltyCard>();

            return Helpers.ListCardsByAddress(address);
        }



        //public List<serviceLoyaltyCard> FindLoyaltyCardById()
        public serviceLoyaltyCard FindLoyaltyCardById()
        { 
          var testSite = SiteDao.FindById(1);

            var tempSite = Helpers.TranslateToServiceSite(testSite);
            tempSite.Name = "Reigate";
            //tempSite.SitePassword = "kidky";


            //this.AddUpdateLoyaltySite(tempSite, 163);

            return null;

        }

        #endregion

    }
}
