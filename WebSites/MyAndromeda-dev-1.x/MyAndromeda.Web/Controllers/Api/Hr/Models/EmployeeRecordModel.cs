using System;
using System.Collections.Generic;
using MyAndromeda.Web.Controllers.Api.Hr;

namespace MyAndromeda.Web.Controllers.Api.Hr.Models
{
    public class EmployeeRecordModel
    {
        public Guid Id { get; set; }

        public bool Deleted { get; set; }

        public string Code { get; set; }

        public string ShortName { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        private DateTime dob;
        public DateTime DateOfBirth {

            get { return this.dob; }
            set { this.dob = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        public string Gender { get; set; }

        public string Department { get; set; }

        public string PrimaryRole { get; set; }

        //public string[] Roles { get; set; }
        //public string[] Skills { get; set; }

        public string DrivingLicense { get; set; }

        public string PayrollNumber { get; set; }

        public string NationalInsurance { get; set; }

        public List<EmployeeDocumentModel> Documents { get; set; }
        //readonly Dictionary<string, object> properties = new Dictionary<string, object>();
        //public override bool TryGetMember(GetMemberBinder binder, out object result)
        //{
        //    if (properties.ContainsKey(binder.Name))
        //    {
        //        result = properties[binder.Name];
        //        return true;
        //    }
        //    else
        //    {
        //        result = "Invalid Property!";
        //        return false;
        //    }
        //}
        //public override bool TrySetMember(SetMemberBinder binder, object value)
        //{
        //    properties[binder.Name] = value;
        //    return true;
        //}
        //public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        //{
        //    dynamic method = properties[binder.Name];
        //    result = method(args[0].ToString(), args[1].ToString());
        //    return true;
        //}
        //public override IEnumerable<string> GetDynamicMemberNames()
        //{
        //    return this.properties.Keys;
        //}
    }
}