using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MyAndromeda.Framework.Helpers
{
    public enum Area 
    {
        Head, 
        Foot
    }
    
    public static class HtmlScriptHelperExtensions
    {
        private class ScriptBlockArea 
        {
            public ScriptBlockArea() { this.Area = Helpers.Area.Foot; }

            public Area Area { get; set; }
            public string Content { get; set; }
            public string Key { get; set; }
        }

        private class ScriptBlock : IDisposable
        {
            private const string ScriptLocationKey = "scripts.locations";
            private const string ScriptsKey = "scripts";

            private readonly Area area;

            private string key;

            public static List<ScriptBlockArea> PageScripts
            {
                get
                {
                    if (HttpContext.Current.Items[ScriptsKey] == null)
                        HttpContext.Current.Items[ScriptsKey] = new List<ScriptBlockArea>();

                    return (List<ScriptBlockArea>)HttpContext.Current.Items[ScriptsKey];
                }
            }

            public static List<ScriptBlockArea> PageScriptLocations 
            {
                get 
                {
                    if (HttpContext.Current.Items[ScriptLocationKey] == null)
                        HttpContext.Current.Items[ScriptLocationKey] = new List<ScriptBlockArea>();

                    return (List<ScriptBlockArea>)HttpContext.Current.Items[ScriptLocationKey];
                }
            } 

            readonly WebViewPage webPageBase;

            public ScriptBlock(WebViewPage webPageBase, string key, Area area)
            {
                this.key = key;
                this.area = area;
                this.webPageBase = webPageBase;
                this.webPageBase.OutputStack.Push(new StringWriter());
            }

            public void Dispose()
            {
                var scriptBlockArea = new ScriptBlockArea() 
                {
                    Key = key,
                    Content = ((StringWriter)this.webPageBase.OutputStack.Pop()).ToString(),
                    Area = this.area
                };

                if(!PageScripts.Any(e=> e.Key.Equals(this.key)))
                    PageScripts.Add(scriptBlockArea);
                //pageScripts.Add((StringWriter)this.webPageBase.OutputStack.Pop());
            }
        }

        

        public static IDisposable BeginScripts(this HtmlHelper helper, string key, Area area = Area.Foot)
        {
            if (string.IsNullOrWhiteSpace(key))
                key = Guid.NewGuid().ToString();

            var content = (WebViewPage)helper.ViewDataContainer;

            return new ScriptBlock(content, key, area);
        }

        public static IDisposable BeginScripts(this HtmlHelper helper, Area area = Area.Foot)
        {
            var content = (WebViewPage)helper.ViewDataContainer;

            return new ScriptBlock(content, Guid.NewGuid().ToString(), area);
        }


        public static MvcHtmlString PageScriptFiles(this HtmlHelper helper, Area area = Area.Head) 
        {
            if (!ScriptBlock.PageScriptLocations.Any(e => e.Area == area)) 
            {
                return MvcHtmlString.Empty;
            }

            var sb = new System.Text.StringBuilder();

            using (var sw = new System.IO.StringWriter(sb))
            {
                using (var tw = new HtmlTextWriter(sw))
                {
                    foreach (var script in ScriptBlock.PageScriptLocations.Where(e => e.Area == area))
                    {
                        tw.WriteLine();
                        tw.AddAttribute(HtmlTextWriterAttribute.Src, script.Content);
                        tw.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                        tw.RenderBeginTag(HtmlTextWriterTag.Script);
                        tw.RenderEndTag();
                    }
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString PageHeadScripts(this HtmlHelper helper) 
        {
            var headerScripts = ScriptBlock.PageScripts
                .Where(s => s.Area == Area.Head);

            StringBuilder sb = new StringBuilder();
            foreach (var item in headerScripts) {
                sb.AppendLine(item.Content);        
            }

            return MvcHtmlString.Create(sb.ToString());
        }
        
        public static MvcHtmlString PageFooterScripts(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();
            var footerScripts = ScriptBlock.PageScripts
                .Where(s => s.Area == Area.Foot);

            foreach (var item in footerScripts) {
                sb.AppendLine(item.Content);
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static void RequireScript(this HtmlHelper helper, string url, Area area = Area.Foot) 
        {
            if(url.StartsWith("~/"))
                url = UrlHelper.GenerateContentUrl(url, helper.ViewContext.HttpContext);

            var previousEntry = ScriptBlock.PageScriptLocations.FirstOrDefault(e => e.Content.Equals(url, StringComparison.InvariantCultureIgnoreCase));

            if (previousEntry != null) 
            {
                if (area == Area.Head) { previousEntry.Area = Area.Head; }
                return;
            }

            ScriptBlock.PageScriptLocations.Add(new ScriptBlockArea(){
                Content = url,
                Area = area
            });
        }

        
    }
}