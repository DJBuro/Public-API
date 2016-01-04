using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Web.Areas.DeliveryZone.Models
{
    public static class DeliveryZoneNameViewModelExtensions
    {
        public static Models.DeliveryZoneNameViewModel ToViewModel(this DeliveryZoneName model)
        {
            var vm = new Models.DeliveryZoneNameViewModel();

            if (model != null)
            {
                vm.Id = model.Id;
                vm.IsCustom = model.IsCustom;
                vm.Name = model.Name;
                vm.OriginPostCode = model.OriginPostCode;
                vm.RadiusCovered = model.RadiusCovered;
                vm.StoreId = model.StoreId;
                vm.PostCodeSectors = model.PostCodeSectors
                                          .OrderBy(e => e.PostCodeSector1)
                                          .Select(e => e.ToViewModel())
                                          .ToList();
            }

            return vm;
        }

        public static PostCodeSectorViewModel ToViewModel(this PostCodeSector model)
        {
            var vm =
                new PostCodeSectorViewModel
                {
                    DeliveryZoneId = model.DeliveryZoneId,
                    Id = model.Id,
                    IsSelected = model.IsSelected,
                    PostCodeSector = model.PostCodeSector1,
                    DataVersion = model.DataVersion
                };

            return vm;
        }

        public static PostCodeSector CreateOrUpdateDataModel(this PostCodeSectorViewModel viewModel, PostCodeSector dbModel = null)
        {
            if (dbModel == null)
            {
                dbModel = new PostCodeSector();
            }

            //dataversion will already be known 
            //model.DataVersion = viewModel.DataVersion;
            dbModel.DeliveryZoneId = viewModel.DeliveryZoneId;
            //id will already be known 
            //model.Id = viewModel.Id;
            dbModel.IsSelected = viewModel.IsSelected;
            dbModel.PostCodeSector1 = viewModel.PostCodeSector;

            return dbModel;
        }

        public static void CreateOrUpdateDataModel(this DeliveryZoneNameViewModel deliveryZoneViewModel, DeliveryZoneName dbModel)
        {
            if (deliveryZoneViewModel == null)
            {
                return;
            }

            dbModel.Id = deliveryZoneViewModel.Id;
            dbModel.IsCustom = deliveryZoneViewModel.IsCustom;
            dbModel.Name = deliveryZoneViewModel.Name;
            dbModel.OriginPostCode = deliveryZoneViewModel.OriginPostCode;
            dbModel.RadiusCovered = deliveryZoneViewModel.RadiusCovered;
            dbModel.StoreId = deliveryZoneViewModel.StoreId;

            if (dbModel.PostCodeSectors == null)
            {
                dbModel.PostCodeSectors = new List<PostCodeSector>();
            }

            var currentDbPostcodes = dbModel.PostCodeSectors.ToList();
            var currentViewModelPostcodes = deliveryZoneViewModel.PostCodeSectors.ToList();

            //remove any postcodes that don't belong here anymore
            var removeItemsFromDbModel = dbModel.PostCodeSectors
                                                .Where(e => !currentViewModelPostcodes.Any(c => c.PostCodeSector == e.PostCodeSector1))
                                                .ToArray();

            foreach (var dbItem in removeItemsFromDbModel)
            {
                //check if each still exist in the view model
                dbModel.PostCodeSectors.Remove(dbItem);
            }

            //only add the the collection records that currently exist on the view model
            foreach (var item in currentViewModelPostcodes)
            {
                var model = currentDbPostcodes
                                              .Where(dbRow => dbRow.PostCodeSector1.Equals(item.PostCodeSector, StringComparison.InvariantCultureIgnoreCase))
                                              .FirstOrDefault();

                //updates to model are done here 
                model = item.CreateOrUpdateDataModel(model);

                //add to the collection
                if (!dbModel.PostCodeSectors.Contains(model))
                {
                    dbModel.PostCodeSectors.Add(model);
                }
            }
        }
    }
}