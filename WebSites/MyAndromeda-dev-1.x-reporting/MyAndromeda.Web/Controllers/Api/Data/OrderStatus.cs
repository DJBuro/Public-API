using System;

namespace MyAndromeda.Web.Controllers.Api.Data
{

    public class OrderStatus 
    {
        public int Id { get; set; }

        public string Description { get; set; }
        public DateTime ChangeDateTime { get; set; }
    }

}