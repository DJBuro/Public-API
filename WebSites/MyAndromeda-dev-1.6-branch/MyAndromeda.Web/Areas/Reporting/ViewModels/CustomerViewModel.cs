using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Reporting.ViewModels
{
    public class CustomerViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? RegisteredDateTime { get; set; }

        public CustomerAddressViewModel Address { get; set; }
    }

    public class CustomerValueViewModel : CustomerViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AvgOrderValue { get; set; }
        public DateTime? LastOrderTime { get; set; }

        public string Phone { get; set; }
    }

    public class CustomerAddressViewModel
    {
        public string Country { get; set; }
        public string Directions { get; set; }
        public string DPS { get; set; }

        public string Town { get; set; }

        public string State { get; set; }

        public string RoadNum { get; set; }

        public string RoadName { get; set; }

        public string Prem6 { get; set; }

        public string Prem5 { get; set; }

        public string Prem4 { get; set; }

        public string Prem3 { get; set; }

        public string Prem2 { get; set; }

        public string Prem1 { get; set; }

        public string PostCode { get; set; }

        public string Org3 { get; set; }

        public string Org2 { get; set; }

        public string Org1 { get; set; }

        public string Long { get; set; }

        public string Lat { get; set; }
    }

    public static class CustomerViewModelExtensions
    {
        public static CustomerAddressViewModel ToViewModel(this Address address)
        {
            if (address == null) { return new CustomerAddressViewModel() { }; }

            return new CustomerAddressViewModel()
            {
                Country = address.Country.CountryName,
                Directions = address.Directions,
                DPS = address.DPS,
                Lat = address.Lat,
                Long = address.Long,
                Org1 = address.Org1,
                Org2 = address.Org2,
                Org3 = address.Org3,
                PostCode = address.PostCode,
                Prem1 = address.Prem1,
                Prem2 = address.Prem2,
                Prem3 = address.Prem3,
                Prem4 = address.Prem4,
                Prem5 = address.Prem5,
                Prem6 = address.Prem6,
                RoadName = address.RoadName,
                RoadNum = address.RoadNum,
                State = address.State,
                Town = address.Town,
            };
        }

        public static CustomerViewModel ToViewModel(this Customer customer)
        {
            return new CustomerViewModel()
            {
                Id = customer.ID,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                RegisteredDateTime = customer.RegisteredDateTime,
                Title = customer.Title,
                Address = customer.Address.ToViewModel()
            };
        }
    }
}