using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loyalty.Dao.Domain;
using Loyalty.Web.Service.Domain;

namespace Loyalty.Dao
{       
    public interface ILoyaltyCardDao : IGenericDao<LoyaltyCard, int>
    {
        //IList<LoyaltyCard> FindLoyaltyCardByAddress(RamesesAddress ramesesAddress);
        LoyaltyCard FindLoyaltyCardByNumber(string cardNumber, Site site);
        IList<LoyaltyCard> FindLoyaltyCardsByCompany(Company company);
        LoyaltyCard FindLoyaltyCardByNumber(string cardNumber, Status status, Site site);

        //bool addCardToAccount(LoyaltyCard loyaltyCard, LoyaltyAccount loyaltyAccount);
        //bool removeCardFromAccount(LoyaltyCard loyaltyCard, LoyaltyAccount loyaltyAccount, Status status);
        bool registerCard(LoyaltyCard loyaltyCard, AccountAddress accountAddress);

        /// <summary>
        /// Used in the Webservices only
        /// </summary>
        int GetLoyaltyCardPoints(string loyaltyCardNumber, Site site);

        bool AddPoints(LoyaltyCard loyaltyCard, int points, RamesesAddress ramesesAddress);

        bool RemovePoints(LoyaltyCard loyaltyCard, int points);
    }
}