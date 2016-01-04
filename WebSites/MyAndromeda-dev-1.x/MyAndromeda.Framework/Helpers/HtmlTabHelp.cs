using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MyAndromeda.Framework.Helpers
{
    public enum TabArea
    {
        /// <summary>
        /// 
        /// </summary>
        Help,
        /// <summary>
        /// 
        /// </summary>
        Debug,
        /// <summary>
        /// 
        /// </summary>
        Support,
        /// <summary>
        /// 
        /// </summary>
        AdminHelp,
        /// <summary>
        /// 
        /// </summary>
        Glossary ,
        /// <summary>
        /// Inline content
        /// </summary>
        Content
    }

    public class TabHelpSctionData
    {
        public string Name { get; set; }
        public bool Selected { get; set; }

        public MvcHtmlString Content { get; set; }

        public TabArea Area { get; set; }
    }

    public class TemplateContent 
    {
        public MvcHtmlString Content {get;set;}
    }

    public class TabHelpSection : IDisposable
    {
        private const string GeneralTabHelpKey = "MyAndromeda.TabStrip.Tabs";
        //private const string DebugTabHelpKey = "MyAndromeda.TabStrip.DebugTabs";
        //private const string SupportTabKey = "MyAndromeda.Tabsrip.SupportTabs";
        //private const string AdminHelpKey = "MyAndomeda.Tabsrip.AdminHelpTabs";

        private readonly string name;
        private readonly bool selected;
        private readonly TabArea area;
        private readonly WebViewPage content;

        public TabHelpSection(string name, bool selected, TabArea area, WebViewPage content)
        {
            this.name = name;
            this.selected = selected;
            this.area = area;
            this.content = content;
            this.content.OutputStack.Push(new StringWriter());
        }

        public static List<TabHelpSctionData> GeneralHelpItems
        {
            get
            {
                if (HttpContext.Current.Items[GeneralTabHelpKey] == null)
                    HttpContext.Current.Items[GeneralTabHelpKey] = new List<TabHelpSctionData>();
                return (List<TabHelpSctionData>)HttpContext.Current.Items[GeneralTabHelpKey];
            }
        }

        public void Dispose()
        {
            GeneralHelpItems.Add(new TabHelpSctionData()
            {
                Name = this.name,
                Area = this.area,
                Selected = this.selected,
                Content = MvcHtmlString.Create(((StringWriter)this.content.OutputStack.Pop()).ToString())
            });
        }
    }

    public static class HtmlTabHelperExtensions 
    {
        public static IDisposable BeginHelpSection(this HtmlHelper helper, string name, TabArea area = TabArea.Help, bool selected = false)
        {
            var content = (WebViewPage)helper.ViewDataContainer;

            return new TabHelpSection(name, selected, area, content);
        }

        public static IEnumerable<TabHelpSctionData> HelpSections(this HtmlHelper helper, TabArea area)
        {
            return TabHelpSection.GeneralHelpItems.Where(e=> e.Area == area);
        }
    }

    public static class HtmlContentHelperExtensions 
    {
        /// <summary>
        /// Creates the HTML template.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns></returns>
        public static CompositeResult CreateHtmlTemplate(this HtmlHelper helper) 
        {
            var content = (WebViewPage)helper.ViewDataContainer;
            var id = Guid.NewGuid().ToString();

            return new CompositeResult(content);
        }

        /// <summary>
        /// escapes the kendo template directive x levels. 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="levels"></param>
        /// <returns>\\# statement #</returns>
        public static MvcHtmlString EscapeKendoHashTemplate(this string text,  int levels = 1) 
        {
            string indent = string.Empty;
            for(var i = levels; i > 0; i--)
            {
                indent = string.Concat(indent, "\\");
            }
            
            return new MvcHtmlString(string.Format("{0}{1}", indent, text));
        } 

        public static MvcHtmlString KendoMember<TModel, TProperty>(this HtmlHelper helper, Expression<Func<TModel, TProperty>> predicate) 
            where TModel : class
        {
            var name = helper.MemberName(predicate);
            return new MvcHtmlString(string.Format("#= {0}#", name));
        }

        public static string MemberName<TModel, TProperty>(this HtmlHelper helper, Expression<Func<TModel, TProperty>> predicate)
            where TModel: class
        {
            var memberExpression = predicate.Body as MemberExpression;
            
            return memberExpression.Member.Name;
        }
    }

    public class CompositeResult 
    {
        private readonly WebViewPage content;

        public CompositeResult(WebViewPage content)
        {
            this.content = content;
        }
        
        public IDisposable CreateTemplate 
        {
            get 
            {
                var holder = new CompositeResultHolder(content);
                holder.DisposingCallback = (c) =>
                {
                    this.Result = Regex.Replace(c ?? string.Empty, @"\t|\n|\r", "");

                    var encode= System.Web.HttpUtility.UrlEncode("+");
                    this.Result = Regex.Replace(this.Result, @"[+]", encode);
                };
                return holder;
            } 
        }
        
        public string Result { get; private set; }
        
        public MvcHtmlString GetHtmlString() 
        {
            return new MvcHtmlString(this.Result);
        }
    }

    public class CompositeResultHolder : IDisposable
    { 
        private WebViewPage content;

        public CompositeResultHolder(WebViewPage content)
        {
            this.content = content;
            this.content.OutputStack.Push(new StringWriter());
        }

        public Action<string> DisposingCallback { get; set; }

        public void Dispose()
        {
            if(DisposingCallback == null)
            {
                this.content.OutputStack.Pop();
                return;
            }
            
            string outputcontent = ((StringWriter)this.content.OutputStack.Pop()).ToString();
            
            DisposingCallback(outputcontent);
        }
    }
}
