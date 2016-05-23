using System.Web.Mvc;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Authorization
{
    public static class HtmlAuthorizerExtensions 
    {
        public static bool IsAuthorizedFor(this HtmlHelper html, Permission permission)
        {
            var authorizer = GetAuthorizer();

            return authorizer.Authorize(permission);
        }

        public static bool IsAuthorizedForAny(this HtmlHelper html, params Permission[] permissions) 
        {
            var authorizer = GetAuthorizer();

            return authorizer.AuthorizeAny(permissions);
        }

        public static bool IsAuthorizedForAll(this HtmlHelper html, params Permission[] permissions) 
        {
            var authorizer = GetAuthorizer();

            return authorizer.AuthorizeAll(permissions);
        }

        private static IAuthorizer GetAuthorizer()
        {
            return DependencyResolver.Current.GetService<IAuthorizer>();
        }
    }
}