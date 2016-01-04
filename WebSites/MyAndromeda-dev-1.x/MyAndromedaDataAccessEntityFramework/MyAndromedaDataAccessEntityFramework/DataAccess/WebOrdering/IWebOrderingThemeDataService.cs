using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.WebOrdering
{
    public interface IWebOrderingThemeDataService : IDataProvider<AndroWebOrderingTheme>, IDependency
    {
    }
}
