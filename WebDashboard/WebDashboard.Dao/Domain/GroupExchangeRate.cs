using System;
using System.Collections;
using System.Collections.Generic;

namespace WebDashboard.Dao.Domain
{
    public class GroupExchangeRate : Entity.Entity
    {
        protected int _groupId;
        public int GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }

        protected string _currencyCode;
        public string CurrencyCode
        {
            get { return _currencyCode; }
            set { _currencyCode = value; }
        }

        protected float _exchangeRate;
        public float ExchangeRate
        {
            get { return _exchangeRate; }
            set { _exchangeRate = value; }
        }

        public GroupExchangeRate() 
        { 
        }

        public GroupExchangeRate(int groupId, string currencyCode, float exchangeRate)
        {
            this._groupId = groupId;
            this._currencyCode = currencyCode;
            this._exchangeRate = exchangeRate;
        }
    }
}



