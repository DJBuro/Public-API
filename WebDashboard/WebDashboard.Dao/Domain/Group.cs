using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WebDashboard.Dao.Domain
{
    public class Group : Entity.Entity
    {
        protected string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
                _name = value;
            }
        }

        protected int? _groupHeadOfficeId;
        public int? GroupHeadOfficeId
        {
            get { return _groupHeadOfficeId; }
            set { _groupHeadOfficeId = value; }
        }

        protected IList _companies;
        public IList Companies
        {
            get
            {
                if (_companies == null)
                {
                    _companies = new ArrayList();
                }
                return _companies;
            }
            set { _companies = value; }
        }
    }
}
