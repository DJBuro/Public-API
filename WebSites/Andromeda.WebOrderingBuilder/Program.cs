using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.WebOrderingBuilder
{
    class Program
    {
        private const string VersionFilename = @"Version.txt";
        private const string TemplateIndexFilename = @"Source";
        private const string ContentFolder = @"Content";
        private const string ViewsFolder = @"Source\Views";
        private const string InjectionPoint = "<!-- INSERT_VIEWS_HERE -->";
        private const string ScriptsInjectionPoint = "// >> INJECT SCRIPTS HERE <<";

        static void Main(string[] args)
        {
            Console.WriteLine("\r\n>>> Andromeda.WebOrderingBuilder <<<");

            foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }

            string projectFolder = args[0];

            // Remove trailing slash - screws Path.Combine up
            if (projectFolder.EndsWith("\"")) projectFolder = projectFolder.Substring(0, projectFolder.Length - 1);

            string templateIndexFilename = Path.Combine(projectFolder, Path.Combine(Program.TemplateIndexFilename, args[1]));
            string indexFilename = Path.Combine(projectFolder, args[1]);
            bool excludeScriptsFile = (args.Length == 3 && args[2] == "excludeScriptsFile");

            // Read in the contents of the index.html file
            string templateIndexHtml = "";
            using (StreamReader templateHtmlStreamReader = new StreamReader(templateIndexFilename))
            {
                templateIndexHtml = templateHtmlStreamReader.ReadToEnd();
            }

            // Inject the scripts file into the index html
            string outputIndexHtml = templateIndexHtml;

            if (!excludeScriptsFile)
            {
                outputIndexHtml = Program.InjectScript(projectFolder, templateIndexHtml);
            }

            // Inject the view html into the index html
            outputIndexHtml = Program.InjectHtml(projectFolder, outputIndexHtml);

            // Inject version number
            string version = "";
            string versionFilename = Path.Combine(projectFolder, VersionFilename);
            using (StreamReader versionStreamReader = new StreamReader(versionFilename))
            {
                version = versionStreamReader.ReadToEnd();
            }

            outputIndexHtml = outputIndexHtml.Replace("<!--VERSION-->", version);

            // Start writing out the new index.html to a temporary file
            using (StreamWriter indexHtmlStreamWriter = new StreamWriter(indexFilename))
            {
                // Write out the index html
                indexHtmlStreamWriter.Write(outputIndexHtml);
            }

            Console.WriteLine(" >>> Done <<<\r\n");
        }

        private static string InjectScript(string projectFolder, string indexHtml)
        {
            // Find the script insertion point
            int scriptInjectionPoint = indexHtml.IndexOf(ScriptsInjectionPoint);

            if (scriptInjectionPoint == -1) throw new Exception("No script injection point");

            string indexHtmlStart = indexHtml.Substring(0, scriptInjectionPoint);
            int scriptInjectionEndIndex = scriptInjectionPoint + ScriptsInjectionPoint.Length;
            string indextHtmlEnd = indexHtml.Substring(scriptInjectionEndIndex, indexHtml.Length - scriptInjectionEndIndex);

            string scripts = "";
            string scriptsFilename = Path.Combine(Path.Combine(projectFolder, ContentFolder) + "Scripts.js");
            using (StreamReader scriptsStreamReader = new StreamReader(scriptsFilename))
            {
                scripts = scriptsStreamReader.ReadToEnd();
            }

            return indexHtmlStart + scripts + indextHtmlEnd;
        }

        private static string InjectHtml(string projectFolder, string indexHtml)
        {
            // Find the views insertion point
            int injectionIndex = indexHtml.IndexOf(Program.InjectionPoint);

            if (injectionIndex == -1) throw new Exception("No html injection point");

            string indexHtmlStart = indexHtml.Substring(0, injectionIndex);
            int injectionEndIndex = injectionIndex + InjectionPoint.Length;
            string indextHtmlEnd = indexHtml.Substring(injectionEndIndex, indexHtml.Length - injectionEndIndex);

            StringBuilder outputIndexHtml = new StringBuilder(indexHtmlStart);

            // Write out views to the index html file in this and all sub folders
            Program.ProcessViewsFolder(outputIndexHtml, Path.Combine(projectFolder, Program.ViewsFolder));

            return outputIndexHtml.Append(indextHtmlEnd).ToString(); 
        }

        private static void ProcessViewsFolder(StringBuilder outputIndexHtml, string folder)
        {
            // Read in each view html file
            foreach (string viewFileName in Directory.GetFiles(folder))
            {
                Console.WriteLine(" > Merging view " + viewFileName);

                // Read in the view html
                using (StreamReader viewStreamReader = new StreamReader(viewFileName))
                {
                    outputIndexHtml.Append(viewStreamReader.ReadToEnd());
                }
            }

            // Process all child folders
            foreach (string childFolder in Directory.GetDirectories(folder))
            {
                Program.ProcessViewsFolder(outputIndexHtml, childFolder);
            }
        }
    }
}
