using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for SiteLinkBuilder
/// </summary>
public static class SiteLinkBuilder
{
    public static string BuildUrlFromExpression<T>(Expression<Action<T>> action) where T : LoyaltyLibrary
	{
        var call = action.Body as MethodCallExpression;
        if (call == null)
        {
            throw new InvalidOperationException("Expression must be a method call"); 
        }

        string actionName = call.Method.Name;

        //not the best, easier in MVC as we have a ViewContext
        return "../Shared/jQ.aspx?method=" + actionName;
	}
}
