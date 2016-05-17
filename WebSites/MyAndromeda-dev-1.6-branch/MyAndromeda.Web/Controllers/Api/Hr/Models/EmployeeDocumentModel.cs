using System.Collections.Generic;

namespace MyAndromeda.Web.Controllers.Api.Hr.Models
{
    public class EmployeeDocumentModel 
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the document URL.
        /// </summary>
        /// <value>The document URL.</value>
        public string DocumentUrl { get; set; }

        public List<EmployeeDocumentFileModel> Files { get; set; }

        //public string Content { get; set; }
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

    public class EmployeeDocumentFileModel 
    {
        public string FileName { get; set; }
    }
}