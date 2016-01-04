using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using AndroAdmin.Models;
using AndroAdmin.Mvc;
using AndroAdmin.Mvc.Filters;

using System.Resources;

namespace AndroAdmin.Controllers
{
    public class TranslationController : SiteController
    {
        //
        // GET: /Transation/

        [RequiresAuthorisation]
        public ActionResult Index(string id)
        {
            
            var data = new AndroAdminViewData.IndexViewData();

            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

            data.AndroUser = AndroUserDao.FindById(userId);

            XDocument englishXml;
            XDocument frenchXml;
            XDocument polishXml;
            XDocument bulgarianXml;
            XDocument russianXml;
var resourcePath = "";
#if DEBUG
            resourcePath = "/Admin";
#endif




switch (id)
            {
                case "OrderTracking":

                    englishXml = XDocument.Load(Server.MapPath(resourcePath + "/OrderTrackingAdmin/App_GlobalResources/Master.resx"));
                    frenchXml = XDocument.Load(Server.MapPath(resourcePath + "/OrderTrackingAdmin/App_GlobalResources/Master.fr.resx"));
                    polishXml = XDocument.Load(Server.MapPath(resourcePath + "/OrderTrackingAdmin/App_GlobalResources/Master.pl.resx"));
                    bulgarianXml = XDocument.Load(Server.MapPath(resourcePath + "/OrderTrackingAdmin/App_GlobalResources/Master.bg.resx"));
                    russianXml = XDocument.Load(Server.MapPath(resourcePath + "/OrderTrackingAdmin/App_GlobalResources/Master.ru.resx"));

                    break;                
                
                case "Dashboard":

                    englishXml = XDocument.Load(Server.MapPath(resourcePath + "/DashboardAdmin/App_GlobalResources/Master.resx"));
                    frenchXml = XDocument.Load(Server.MapPath(resourcePath + "/DashboardAdmin/App_GlobalResources/Master.fr.resx"));
                    polishXml = XDocument.Load(Server.MapPath(resourcePath + "/DashboardAdmin/App_GlobalResources/Master.pl.resx"));
                    bulgarianXml = XDocument.Load(Server.MapPath(resourcePath + "/DashboardAdmin/App_GlobalResources/Master.bg.resx"));
                    russianXml = XDocument.Load(Server.MapPath(resourcePath + "/DashboardAdmin/App_GlobalResources/Master.ru.resx"));

                    break;    
            
                case "Loyalty":

                    englishXml = XDocument.Load(Server.MapPath(resourcePath + "/LoyaltyAdmin/App_GlobalResources/Master.resx"));
                    frenchXml = XDocument.Load(Server.MapPath(resourcePath + "/LoyaltyAdmin/App_GlobalResources/Master.fr.resx"));
                    polishXml = XDocument.Load(Server.MapPath(resourcePath + "/LoyaltyAdmin/App_GlobalResources/Master.pl.resx"));
                    bulgarianXml = XDocument.Load(Server.MapPath(resourcePath + "/LoyaltyAdmin/App_GlobalResources/Master.bg.resx"));
                    russianXml = XDocument.Load(Server.MapPath(resourcePath + "/LoyaltyAdmin/App_GlobalResources/Master.ru.resx"));

                    break;


                case "Andro":

                    englishXml = XDocument.Load(Server.MapPath(resourcePath + "/AndroAdmin/App_GlobalResources/Master.resx"));
                    frenchXml = XDocument.Load(Server.MapPath(resourcePath + "/AndroAdmin/App_GlobalResources/Master.fr.resx"));
                    polishXml = XDocument.Load(Server.MapPath(resourcePath + "/AndroAdmin/App_GlobalResources/Master.pl.resx"));
                    bulgarianXml = XDocument.Load(Server.MapPath(resourcePath + "/AndroAdmin/App_GlobalResources/Master.bg.resx"));
                    russianXml = XDocument.Load(Server.MapPath(resourcePath + "/AndroAdmin/App_GlobalResources/Master.ru.resx"));

                    break;

                default:

                    id = "Andro";

                    englishXml = XDocument.Load(Server.MapPath(resourcePath + "/AndroAdmin/App_GlobalResources/Master.resx"));
                    frenchXml = XDocument.Load(Server.MapPath(resourcePath + "/AndroAdmin/App_GlobalResources/Master.fr.resx"));
                    polishXml = XDocument.Load(Server.MapPath(resourcePath + "/AndroAdmin/App_GlobalResources/Master.pl.resx"));
                    bulgarianXml = XDocument.Load(Server.MapPath(resourcePath + "/AndroAdmin/App_GlobalResources/Master.bg.resx"));
                    russianXml = XDocument.Load(Server.MapPath(resourcePath + "/AndroAdmin/App_GlobalResources/Master.ru.resx"));

                    break;                    
            }


            data.TranslatingProject = id;

            data.English = new Dictionary<string,string>();
            data.French = new Dictionary<string,string>();
            data.Polish = new Dictionary<string,string>();
            data.Bulgarian = new Dictionary<string,string>();
            data.Russian = new Dictionary<string,string>();

            
           
            var englishData = from element in englishXml.Root.Descendants("data")
                           
                           select new
                           {
                               ResxName = element.Attribute("name").Value,
                               ResxValue = element.Element("value").Value
                             };            
            
            var frenchData = from element in frenchXml.Root.Descendants("data")
                           select new
                           {
                               ResxName = element.Attribute("name").Value,
                               ResxValue = element.Element("value").Value
                             };

            var polishData = polishXml.Root.Descendants("data").Select(element => new
                                                                                  {
                                                                                      ResxName =
                                                                                  element.Attribute("name").Value,
                                                                                      ResxValue =
                                                                                  element.Element("value").Value
                                                                                  });

            var bulgarianData = from element in bulgarianXml.Root.Descendants("data")
                             select new
                             {
                                 ResxName = element.Attribute("name").Value,
                                 ResxValue = element.Element("value").Value
                             };

            var russianData = from element in russianXml.Root.Descendants("data")
                             select new
                             {
                                 ResxName = element.Attribute("name").Value,
                                 ResxValue = element.Element("value").Value
                             };

            foreach (var enumerable in englishData)
            {
                data.English.Add(enumerable.ResxName,enumerable.ResxValue);
            }

            foreach (var enumerable in frenchData)
            {
                data.French.Add(enumerable.ResxName, enumerable.ResxValue);
            }

            foreach (var enumerable in polishData)
            {
                data.Polish.Add(enumerable.ResxName, enumerable.ResxValue);
            }            
            foreach (var enumerable in bulgarianData)
            {
                data.Bulgarian.Add(enumerable.ResxName, enumerable.ResxValue);
            }            
            foreach (var enumerable in russianData)
            {
                data.Russian.Add(enumerable.ResxName, enumerable.ResxValue);
            }

            return (View(TranslationControllerViews.Index, data));
        }

        //controller = "Translation", action = "Index", project = "", language = "", translate = "" 
        [RequiresAuthorisation]
        public ActionResult Translate(string project, string language, string translate)
        {


            var resourcePath = "";
#if DEBUG
            resourcePath = "/Admin";
#endif




            var data = new AndroAdminViewData.IndexViewData();

            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

            data.AndroUser = AndroUserDao.FindById(userId);

            var culture = CultureInfo.CreateSpecificCulture(language);

            data.TranslatingLanguage = culture.Parent.NativeName.Trim();
            data.TranslatingProject = project.Trim();


            var englishXml = XDocument.Load(Server.MapPath(resourcePath + "/" + project +"Admin/App_GlobalResources/Master.resx"));
            var translatingXml = XDocument.Load(Server.MapPath(resourcePath + "/" + project + "Admin/App_GlobalResources/Master." + language + ".resx"));

            data.EnglishWord = englishXml.Root.Elements("data")
                  .Single(x => x.FirstAttribute.Value == translate).Value.Trim();

            var translatingWord = translatingXml.Root.Elements("data")
                  .Single(x => x.FirstAttribute.Value == translate).Value.Trim();

            data.TranslatedWord = translatingWord;
            data.TranslatingWordId = translate;
            data.TranslatingLanguageId = language;

            return (View(TranslationControllerViews.Translate, data));
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult Save(string translatingWordId, string translatingProject, string translatingLanguageId, string TranslatedWord)
        {

            var resourcePath = "";
#if DEBUG
            resourcePath = "/Admin";
#endif

            if(translatingLanguageId == null)
                return (this.Index(translatingProject));

            var path =
                Server.MapPath(resourcePath + "/" + translatingProject + "Admin/App_GlobalResources/Master." +
                               translatingLanguageId + ".resx");

            UpdateResourceFile(translatingWordId, TranslatedWord, path);

            return (this.Index(translatingProject));
        }

        public static void UpdateResourceFile(string translatingWordId, String translatingWord, String path)
        {
            var resourceEntries = new Hashtable();

            //Get existing resources
            var reader = new ResXResourceReader(path);
 
            foreach (DictionaryEntry d in reader)
            {
                if (d.Value == null)
                    resourceEntries.Add(d.Key.ToString(), "");
                else
                    resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
            }
            reader.Close();

            //translate the word
            if(resourceEntries.ContainsKey(translatingWordId))
            {
                resourceEntries.Remove(translatingWordId);
                resourceEntries.Add(translatingWordId,translatingWord);

                //Write the combined resource file
                var resourceWriter = new ResXResourceWriter(path);

                foreach (String key in resourceEntries.Keys)
                {
                    resourceWriter.AddResource(key, resourceEntries[key]);
                }
                resourceWriter.Generate();
                resourceWriter.Close();
            }
        }


    }
}
