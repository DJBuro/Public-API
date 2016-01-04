using Microsoft.Web.Iis.Rewrite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AndroWebURLRewriter
{
    public class AndroWebOrderingURLRewriter : IRewriteProvider, IProviderDescriptor
    {
        StringBuilder logger = new StringBuilder();
        string Setting_IgnoreURLWithStrings, Setting_URLRewriteFormat, Setting_EnableLogging;
        //log4net.ILog LoggerObj = log4net.LogManager.GetLogger(@"c:\" + System.DateTime.Now.ToString("MM-dd-yyyy") + ".log");
        const string logDirectory = @"c:\URLRewriterLog\";
        string logPath = logDirectory + System.DateTime.Now.ToString("MM-dd-yyyy") + ".log";
        bool isLoggingEnabled = false;

        #region IRewriteProvider Members

        public void Initialize(IDictionary<string, string> settings, IRewriteContext rewriteContext)
        {
            try
            {
                File.AppendAllText(logDirectory + "iislog.txt", "\r\nIn initialise: ");
                if (!settings.TryGetValue("Setting_EnableLogging", out Setting_EnableLogging))
                {
                    throw new ArgumentException("Unable to read URLRewriteFormat provider setting is required");
                }
                else
                {
                    isLoggingEnabled = Convert.ToBoolean(Setting_EnableLogging);
                    if (isLoggingEnabled)
                    {
                        if (!Directory.Exists(logDirectory))
                        {
                            try
                            {
                                Directory.CreateDirectory(logDirectory);
                            }

                            catch (Exception ex)
                            {
                                throw new Exception("Unable to create log directory." + ex.Message);
                            }
                        }
                    }
                }

                if (!settings.TryGetValue("Setting_IgnoreURLWithStrings", out Setting_IgnoreURLWithStrings))
                {
                    if (isLoggingEnabled)
                    {
                        logger.AppendLine("=========================================================");
                        logger.AppendLine("Unable to read Ignore URLs Containing Strings");
                        File.AppendAllText(logPath, logger.ToString());
                    }
                    throw new ArgumentException("Unable to read Ignore URLs Containing Strings");
                }

                if (!settings.TryGetValue("Setting_URLRewriteFormat", out Setting_URLRewriteFormat))
                {
                    if (isLoggingEnabled)
                    {
                        logger.AppendLine("Unable to read URLRewriteFormat provider setting is required");
                        File.AppendAllText(logPath, logger.ToString());
                    }
                    throw new ArgumentException("Unable to read URLRewriteFormat provider setting is required");
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logDirectory + "iislog.txt", "\r\nException: " + ex.Message);
            }
        }

        public string Rewrite(string requestedURL)
        {
            try
            {
                File.AppendAllText(logDirectory + "iislog.txt", "\r\n---------------");
                File.AppendAllText(logDirectory + "iislog.txt", "\r\nFrom: " + requestedURL);
                if (isLoggingEnabled)
                {
                    logger.AppendLine("Settings");
                    logger.AppendLine("Setting_EnableLogging: " + Setting_IgnoreURLWithStrings);
                    logger.AppendLine("Setting_URLRewriteFormat: " + Setting_URLRewriteFormat);
                    logger.AppendLine("Setting_EnableLogging: " + Setting_EnableLogging);
                    logger.AppendLine("=========================================================");
                    File.AppendAllText(logPath, logger.ToString());
                    //LoggerObj.Info(logger.ToString());

                    logger.AppendLine("In Rewrite ");
                    logger.AppendLine(System.DateTime.Now.ToString("s"));
                }
                string server_name = requestedURL.Split(new char[] { '|' })[0];
                string urlPart = requestedURL.Split(new char[] { '|' })[1];
                if (isLoggingEnabled)
                {
                    logger.AppendLine("\t requestedURl (Complete): " + requestedURL);
                    logger.AppendLine("\t server name:" + server_name);
                    logger.AppendLine("\t URL Part:" + urlPart);
                }
                // regex for now - TBD: configure on the IIS provider, 
                if (Regex.IsMatch(urlPart, "^((?!.*(" + Setting_IgnoreURLWithStrings + ")).*)$", RegexOptions.IgnoreCase))
                {
                    string URLRewriterString = Setting_URLRewriteFormat.Replace("<host>", server_name.Replace("www.", "")).Replace("<relative-path>", urlPart.TrimStart(new char[] { '/' }));
                    if (isLoggingEnabled)
                    {
                        logger.AppendLine("URL  Rewritten to: " + URLRewriterString);
                        logger.AppendLine("------------------------------------------");
                        File.AppendAllText(logPath, logger.ToString());
                        File.AppendAllText(logDirectory + "iislog.txt", "\r\nto: " + URLRewriterString);
                        File.AppendAllText(logDirectory + "iislog.txt", "\r\n---------------");
                        //LoggerObj.Info(logger.ToString());
                    }
                    return URLRewriterString;
                }
                if (isLoggingEnabled)
                {
                    logger.AppendLine("URL not changed: " + urlPart);
                    logger.AppendLine("------------------------------------------");
                    File.AppendAllText(logPath, logger.ToString());
                    File.AppendAllText(logDirectory + "iislog.txt", "\r\nUnchanged: " + urlPart);
                    File.AppendAllText(logDirectory + "iislog.txt", "\r\n---------------");
                    //LoggerObj.Info(logger.ToString());
                }
                return urlPart;
            }
            catch (Exception ex)
            {
                File.AppendAllText(logDirectory + "iislog.txt", "\r\nException: " + ex.Message);
                File.AppendAllText(logDirectory + "iislog.txt", "\r\n---------------");
                return requestedURL.Split(new char[] { '|' })[1];
            }
        }

        #endregion


        #region IProviderDescriptor Members

        public IEnumerable<SettingDescriptor> GetSettings()
        {
            yield return new SettingDescriptor("Setting_IgnoreURLWithStrings", "Ignore URLs Containing Strings");
            yield return new SettingDescriptor("Setting_URLRewriteFormat", "URL Rewrite Format");
            yield return new SettingDescriptor("Setting_EnableLogging", "Enable Logging (true / false)");
        }

        #endregion
    }
}
