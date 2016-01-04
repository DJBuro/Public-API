using AndroAdminDataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroAdmin.ViewModels.ApiCredentials
{
    public class ApiParameters : List<ApiCredentialKeyPairViewModel> 
    {
    
    }


    public class ExernalApiViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string DefinitionParameters { get; set; }

        public string Parameters { get; set; }
    }

    public class ExternalApiSelectViewModel 
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }

    public class ApiCredentialDefinitionViewModel
    {
        public string Key { get; set; }
        //public string Placeholder {get;set;}
        public string Value { get; set; }
    }

    public class ApiCredentialKeyPairViewModel
    {
        public string Key { get; set; }
        public string Placeholder { get; set; }
        public string Value { get; set; }
    }

    

    public static class ApiCredentialViewModelExtensions
    {
        public static ExernalApiViewModel ToViewModel(this ExternalApi model) 
        {
            return new ExernalApiViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Parameters = model.Parameters,
                DefinitionParameters = model.DefinitionParameters
            };
        }

        public static ExternalApiSelectViewModel ToSelectViewModel(this ExternalApi model) 
        {
            return new ExternalApiSelectViewModel()
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}