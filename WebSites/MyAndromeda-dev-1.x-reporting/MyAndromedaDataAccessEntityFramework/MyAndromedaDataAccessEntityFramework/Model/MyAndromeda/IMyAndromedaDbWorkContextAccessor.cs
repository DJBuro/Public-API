using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.Model.MyAndromeda
{
    public interface IMyAndromedaDbWorkContextAccessor : IDependency
    {
        MyAndromedaDbContext DbContext { get; set; }
    }
}