using System;
using System.Linq;
using System.Collections.Generic;
using MyAndromeda.Core;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Bringg.Services.Checks
{

    public interface ICheckNewOrderIsActiveForBringgService : 
        ICheckOrdersForBringgDontDirectingInclude, 
        ISingletonDependency
    {

    }

}