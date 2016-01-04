using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loyalty.Core;
using Loyalty.Dao.Domain;
using Loyalty.Web.Service.Domain;
using Loyalty.Dao.NHibernate.Dao.Factory;
using NHibernate;
using Spring.Data.NHibernate.Support;

namespace Loyalty.Dao.NHibernate
{
    public class LoyaltyCardDao : HibernateDaoSupport,ILoyaltyCardDao
    {
        public LoyaltyHibernateDAOFactory LoyaltyHibernateDAOFactory { get; set; }

        #region IGenericDao<LoyaltyCard,int> Members

        public IList<LoyaltyCard> FindAll()
        {
            return this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.FindAll();
        }

        public LoyaltyCard FindById(int id)
        {
            return this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.FindById(id);
        }

        public LoyaltyCard Create(LoyaltyCard loyaltyCard)
        {
            return this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Create(loyaltyCard);
        }

        public LoyaltyCard Save(LoyaltyCard loyaltyCard)
        {
            return this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Save(loyaltyCard);
        }

        public void Update(LoyaltyCard loyaltyCard)
        {
            this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Update(loyaltyCard);
        }

        public void Delete(LoyaltyCard loyaltyCard)
        {
            this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Delete(loyaltyCard);
        }

        public void DeleteAll(LoyaltyCard loyaltyCardType)
        {
            this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.DeleteAll(loyaltyCardType);
        }

        public void DeleteAll(IList<LoyaltyCard> deleteSet)
        {
            this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.DeleteAll(deleteSet);
        }

        public void Close()
        {
            this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Close();
        }

        #endregion

        #region ILoyaltyCardDao Members

        public LoyaltyCard FindLoyaltyCardByNumber(string cardNumber, Site site)
        {
            //note: returns only live cards
            var hql = "select lc from LoyaltyCard as lc left join lc.LoyaltyCardStatus as LCS  where lc.CardNumber = :CARDNUMBER and lc.LoyaltyAccount.Site.Company.Id = :COMPANYID";

            var query = this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Session.CreateQuery(hql);

            query.SetString("CARDNUMBER", cardNumber);
            query.SetInt32("COMPANYID", site.Company.Id.Value);

            return this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.FindFirstElementByAdhocQuery(query);
        }

        public IList<LoyaltyCard> FindLoyaltyCardsByCompany(Company company)
        {
            //note: returns only live cards
            var hql = "select lc from LoyaltyCard as lc where lc.LoyaltyAccount.Site.Company.Id = :COMPANYID";

            var query = this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Session.CreateQuery(hql);

            query.SetInt32("COMPANYID", company.Id.Value);

            return this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.FindByAdhocQuery(query);
        }

        public LoyaltyCard FindLoyaltyCardByNumber(string cardNumber, Status status, Site site)
        {
            //note: returns only live cards
            var hql = "select lc from LoyaltyCard as lc left join lc.LoyaltyCardStatus as LCS  where lc.CardNumber = :CARDNUMBER and LCS.Status = 1  and lc.LoyaltyAccount.Site.Company.Id = :COMPANYID";

            var query = this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Session.CreateQuery(hql);

            query.SetString("CARDNUMBER", cardNumber);
            query.SetInt32("COMPANYID", site.Company.Id.Value);

            return this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.FindFirstElementByAdhocQuery(query);
        }

        /// <summary>
        /// Gets Available Points of a Loyalty Card
        /// </summary>
        /// <param name="loyaltyCardNumber"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public int GetLoyaltyCardPoints(string loyaltyCardNumber, Site site)
        {
            var hql = "select lc from LoyaltyCard as lc left join lc.LoyaltyCardStatus as LCS  where lc.CardNumber = :CARDNUMBER and LCS.Status = 1  and lc.LoyaltyAccount.Site.Company.Id = :COMPANYID";

            var query = this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Session.CreateQuery(hql);

            query.SetString("CARDNUMBER", loyaltyCardNumber);
            query.SetInt32("COMPANYID", site.Company.Id.Value);

            query.SetCacheable(true);

            var lc = this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.FindByAdhocQuery(query);

            if (lc.Count > 1)
            {
                System.Reflection.MethodBase.GetCurrentMethod().Log(site,Helpers.Reason.Multiple_cards_with_same_number);
                return -1;
            }


            int points = 0;

            //note: will allow the return of negative points
            foreach (var loyaltyCard in lc)
            {
                points = loyaltyCard.LoyaltyAccount.Points;
            }

            return points;

        }


        /// <summary>
        /// Web Service call to Add Points to a Loyalty Card
        /// </summary>
        /// <param name="loyaltyCard"></param>
        /// <param name="points"></param>
        /// <param name="ramesesAddress"></param>
        /// <returns></returns>
        public bool AddPoints(LoyaltyCard loyaltyCard, int points, RamesesAddress ramesesAddress)
        {
            //Convert any negative value points to positive
            points = Math.Abs(points);

            if (loyaltyCard.LoyaltyAccount.IsTransient)//first time using card, create an account to store points.
            {
                //create a new account and add points
                 this.LoyaltyHibernateDAOFactory.LoyaltyAccountDAO.Save(loyaltyCard.LoyaltyAccount);

                //assign the account to a company
                 foreach (Company company in loyaltyCard.LoyaltyAccount.CompanyLoyaltyAccount)
                 {
                     this.LoyaltyHibernateDAOFactory.CompanyLoyaltyAccountDAO.Save(
                       new CompanyLoyaltyAccount(loyaltyCard.LoyaltyAccount, company));

                     var accountStatus = this.LoyaltyHibernateDAOFactory.AccountStatusDAO.FindById(1);

                     //note: make the account active too!
                     this.LoyaltyHibernateDAOFactory.LoyaltyAccountStatusDAO.Save(new LoyaltyAccountStatus("new account added", loyaltyCard.LoyaltyAccount, accountStatus));
                 }

                //Save Card to get id
                this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Save(loyaltyCard);

                //make the card active
                loyaltyCard.LoyaltyCardStatus.Add(this.LoyaltyHibernateDAOFactory.LoyaltyCardStatusDAO.Save(new LoyaltyCardStatus(loyaltyCard,
                                                                                                this.LoyaltyHibernateDAOFactory
                                                                                                    .StatusDAO.FindById(
                                                                                                    1))));
                
            }
            else
            {
                var cardStatus = (LoyaltyCardStatus)loyaltyCard.LoyaltyCardStatus[0];

                if (cardStatus.Status.Id == 1)//only add to live cards
                    loyaltyCard.LoyaltyAccount.Points = loyaltyCard.LoyaltyAccount.Points + points;
            }

            if (ramesesAddress != null) // no address associated to card
            {
                var ramesesAddressLoyaltyCard = new RamesesAddressLoyaltyCard(loyaltyCard, ramesesAddress);

                if (!ramesesAddress.Id.HasValue | loyaltyCard.RamesesAddressLoyaltyCards.Count ==0)
                {
                    loyaltyCard.RamesesAddressLoyaltyCards.Add(ramesesAddress);
                    this.LoyaltyHibernateDAOFactory.RamesesAddressDAO.Save(ramesesAddressLoyaltyCard.RamesesAddress);
                    this.LoyaltyHibernateDAOFactory.RamesesAddressLoyaltyCardDAO.Save(ramesesAddressLoyaltyCard);
                }
            }

            this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Save(loyaltyCard);

            return true;
        }


        /// <summary>
        /// Web Service call to Remove Points to a Loyalty Card
        /// </summary>
        /// <param name="loyaltyCard"></param>
        /// <param name="points"></param>
        public bool RemovePoints(LoyaltyCard loyaltyCard, int points)
        {
            //Convert any negative value points to positive
            points = Math.Abs(points);

            if (loyaltyCard.LoyaltyAccount == null)
            {
                throw new AssertionFailure("Loyalty Card needs an account before points can be deducted");
            }

            var cardStatus = (LoyaltyCardStatus)loyaltyCard.LoyaltyCardStatus[0];

            if (cardStatus.Status.Id == 1 & loyaltyCard.LoyaltyAccount.Points > 0)//only remove from live cards
                loyaltyCard.LoyaltyAccount.Points = loyaltyCard.LoyaltyAccount.Points - points;

            this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Save(loyaltyCard);
            
            return true;
        }

        public bool registerCard(LoyaltyCard loyaltyCard, AccountAddress accountAddress)
        {
            accountAddress.LoyaltyAccount = loyaltyCard.LoyaltyAccount;

            if (loyaltyCard.LoyaltyCardStatus.Count > 0)
            {
                var cardStatus = (LoyaltyCardStatus) loyaltyCard.LoyaltyCardStatus[0];

                if (cardStatus.Status.Id == 1) //only add to live cards
                {
                    if (this.LoyaltyHibernateDAOFactory.AccountAddressDAO.Save(accountAddress).IsTransient)
                        return false;

                    this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Save(loyaltyCard);

                    return true;
                }
            }
            else //new card, no status yet.
            {
                this.LoyaltyHibernateDAOFactory.LoyaltyCardDAO.Save(loyaltyCard);
                
                if (this.LoyaltyHibernateDAOFactory.AccountAddressDAO.Save(accountAddress).IsTransient)
                    return false;
                
                //note: makes the card active
                loyaltyCard.LoyaltyCardStatus.Add(this.LoyaltyHibernateDAOFactory.LoyaltyCardStatusDAO.Save(new LoyaltyCardStatus(loyaltyCard,
                                                                                                this.LoyaltyHibernateDAOFactory
                                                                                                    .StatusDAO.FindById(
                                                                                                    1))));

                var accountStatus = this.LoyaltyHibernateDAOFactory.AccountStatusDAO.FindById(1);

                //note: makes the account active
                this.LoyaltyHibernateDAOFactory.LoyaltyAccountStatusDAO.Save(new LoyaltyAccountStatus("new account registered", loyaltyCard.LoyaltyAccount, accountStatus));

                return true;
            }
           
            return false;
        }



        #endregion
    }
}
