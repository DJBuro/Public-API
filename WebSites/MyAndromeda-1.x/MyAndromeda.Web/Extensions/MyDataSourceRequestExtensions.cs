using System.Collections.Specialized;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Extensions
{
    public static class MyDataSourceRequestExtensions
    {
        public static void ReMap(this DataSourceRequest request, StringDictionary mappings)
        {
            var filters = request.Filters;
            var sorts = request.Sorts;
            var aggregates = request.Aggregates;
            var groups = request.Groups;

            MapFilterDescriptors(mappings, filters);
            MapSortDescriptors(mappings, request.Sorts);
            MapGroupDescriptors(mappings, request.Groups);
        }

        private static void MapGroupDescriptors(StringDictionary mappings, IList<GroupDescriptor> groups)
        {
            MapSortDescriptors(mappings, groups);
        }

        private static void MapFilterDescriptors(StringDictionary mappings, IEnumerable<IFilterDescriptor> filters)
        {
            foreach (var filter in filters)
            {
                if (filter is FilterDescriptor)
                {
                    var normalFilter = (FilterDescriptor)filter;
                    if (mappings.ContainsKey(normalFilter.Member))
                    {
                        normalFilter.Member = mappings[normalFilter.Member];
                    }
                }
                else if (filter is CompositeFilterDescriptor)
                {
                    var compositeFilter = (CompositeFilterDescriptor)filter;
                    MapFilterDescriptors(mappings, compositeFilter.FilterDescriptors);
                }
            }
        }

        private static void MapSortDescriptors(StringDictionary mappings, IEnumerable<SortDescriptor> sortDescriptors)
        {
            foreach (var sortDescriptor in sortDescriptors)
            {
                string sourceMemberName = sortDescriptor.Member;
                if (mappings.ContainsKey(sourceMemberName))
                {
                    sortDescriptor.Member = mappings[sourceMemberName];
                }
            }
        }
    }
}