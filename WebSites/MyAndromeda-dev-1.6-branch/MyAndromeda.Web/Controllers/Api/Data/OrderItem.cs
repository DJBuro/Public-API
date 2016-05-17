using System;

namespace MyAndromeda.Web.Controllers.Api.Data
{

    public class OrderItem 
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int? Qty { get; set; }

        public int? Price { get; set; }

        public string Person { get; set; }
    }

}