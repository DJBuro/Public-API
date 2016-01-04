using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace AndroWebAdmin.Mvc.Utilities
{
    public class SiteLinkBuilder
    {
        public static string BuildUrlFromExpression<T>(ViewContext context, Expression<Action<T>> action, RouteValueDictionary persistentData) where T : SiteBaseController
        {
            RouteValueDictionary values = BuildParameterValuesFromExpression(action, persistentData);

            VirtualPathData vpd = RouteTable.Routes.GetVirtualPath(context.RequestContext, values);
            //return (vpd == null) ? null : HttpUtility.HtmlEncode(vpd.VirtualPath);
            //note: test if W3C validation change not working - HtmlEncode is incorrect, but UrlEncode does not work either!

            return (vpd == null) ? null : vpd.VirtualPath;
        }

        public static string BuildUrlFromExpression<T>(ViewContext context, Expression<Action<T>> action, string linkText) where T : SiteBaseController
        {
            //todo: reflector the old HTML.BuildUrlFromExpression
            //<%= SiteLinkBuilder.BuildUrlFromExpression<GlobalController>(this.ViewContext, slb => slb.EditCountry(Model.Countries[0].Id.Value),"tadaa" )%>
           
            RouteValueDictionary values = BuildParameterValuesFromExpression(action);

            // values = ReplaceUrlGenValues(values, data);

            VirtualPathData vpd = RouteTable.Routes.GetVirtualPath(context.RequestContext, values);
            //return (vpd == null) ? null : HttpUtility.HtmlEncode(vpd.VirtualPath);

            //TODO: clean this up... very messy!
            var link = HtmlHelper.GenerateLink(context.RequestContext, RouteTable.Routes, linkText, "Default", values["Action"].ToString(), values["Controller"].ToString(), values, null);

            //note: test if W3C validation change not working - HtmlEncode is incorrect, but UrlEncode does not work either!
            //return (vpd == null) ? null : vpd.VirtualPath;
            return link ?? null;
        }

        public static RouteValueDictionary BuildParameterValuesFromExpression<T>(Expression<Action<T>> action) where T : SiteBaseController
        {
            return BuildParameterValuesFromExpression(action, new RouteValueDictionary());
        }

        public static RouteValueDictionary BuildParameterValuesFromExpression<T>(Expression<Action<T>> action, RouteValueDictionary persistentData) where T : SiteBaseController
        {
            var call = action.Body as MethodCallExpression;

            if (call == null)
            {
                throw new InvalidOperationException("Expression must be a method call");
            }
            if (call.Object != action.Parameters[0])
            {
                throw new InvalidOperationException("Method call must target lambda argument");
            }

            string actionName = call.Method.Name;
            string controllerName = typeof(T).Name;
            if (controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
            {
                controllerName = controllerName.Remove(controllerName.Length - 10, 10);
            }




           // var p = new TagBuilder("");
           // p.

            var LinkBuilder = new TagBuilder(call.Method.Name);
            //LinkBuilder.

            //RouteValueDictionary values =  LinkBuilder.BuildParameterValuesFromExpression(call);
            RouteValueDictionary values = new RouteValueDictionary(); //LinkBuilder.MergeAttributes();

            

            values = RemoveEmptyValues(values);

            values = values ?? new RouteValueDictionary();
            values.Add("controller", controllerName);
            values.Add("action", actionName);

            foreach (var s in call.Arguments)
            {
                values.Add("id", s);
            }

            AddAllIfNotPresent(values, persistentData);

            return values;
        }

        private static RouteValueDictionary RemoveEmptyValues(RouteValueDictionary values)
        {
            var result = new RouteValueDictionary();
            foreach (var pair in values)
            {
                if (!Asserts.IsEmpty(pair.Value))
                {
                    result.Add(pair.Key, pair.Value);
                }
            }
            return result;
        }

        private static void AddAllIfNotPresent(RouteValueDictionary target, RouteValueDictionary source)
        {
            Asserts.ArgumentExists(target, "target");
            if (Asserts.IsEmpty(source)) return;
            foreach (var pair in source)
            {
                if (!target.ContainsKey(pair.Key))
                    target.Add(pair.Key, pair.Value);
            }
        }
    }
}
