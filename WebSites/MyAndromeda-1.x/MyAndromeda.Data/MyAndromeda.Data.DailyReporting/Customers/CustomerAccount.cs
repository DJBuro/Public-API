//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAndromeda.Data.DailyReporting.Customers
{
    using System;
    using System.Collections.Generic;
    
    public partial class CustomerAccount
    {
        public CustomerAccount()
        {
            this.Customers = new HashSet<Customer>();
        }
    
        public System.Guid ID { get; set; }
        public string AccountNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int AccountTypeId { get; set; }
        public System.DateTime RegisteredDateTime { get; set; }
    
        public virtual AccountType AccountType { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
