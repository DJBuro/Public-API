﻿using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.WebOrdering
{
    public interface IWebOrderingThemeDataService : IDataProvider<AndroWebOrderingTheme>, IDependency
    {
        
    }
}
