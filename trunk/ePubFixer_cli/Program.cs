using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;
using System.ComponentModel;
using System.Reflection;
using System.IO;

namespace ePubFixer_cli
{
    class Program
    {
        private sealed class Options : CommandLineOptionsBase
        {
            #region Standard Option Attribute
            [Option("f", "file", Required = true, HelpText = "ePub file to process")]
            public string InputFile { get; set; }

            [Option("t", "toc", HelpText = "Create a Table of Content")]
            public bool CreateTOC { get; set; }

            [Option("h", "html", DefaultValue = false, HelpText = "Add a HTML Table of Content to the file (Requires -t)")]
            public bool CreateHtmlTOC { get; set; }

            [Option("c", "chapter", DefaultValue = false, HelpText = "Retrieve title of Chapters automatically (Requires -t, will use the filename if not set)")]
            public bool RetrieveChapterText { get; set; }
            #endregion

            #region Help Screen
            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
            #endregion
        }


        static void Main(string[] args)
        {
            Console.Title = ePubFixer.Utils.GetTitle();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Resolver);
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(false, true, Console.Error));
            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            DoCoreTask(options);

            Environment.Exit(0);
        }

        private static void DoCoreTask(Options options)
        {
            Console.WriteLine(Environment.NewLine);

            if (!File.Exists(options.InputFile))
            {
                Console.WriteLine("Error : File Not Found, Exiting");
                Environment.Exit(1);
            }

            ePubFixer.Variables.Filename = options.InputFile;
            ePubFixer.Variables.CreateHtmlTOC = options.CreateHtmlTOC;
            ePubFixer.Variables.TextInChapters = options.RetrieveChapterText;
            int optCount = 0;

            if (options.CreateTOC)
            {
                optCount++;
                TOC toc = new TOC();
                toc.UpdateFile();
                Console.WriteLine("TOC has been created");
            }

            if (optCount == 0)
                Console.WriteLine("No Options Selected, see help\n\n\n" + options.GetUsage());
        }

        static System.Reflection.Assembly Resolver(object sender, ResolveEventArgs args)
        {
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.

            //Load required DLL form a embeded Ressources
            Assembly a1 = Assembly.GetExecutingAssembly(); // Get Executing Assembly 

            //Find the Name of the missing Dll to Load
            string Name = a1.GetName().Name + ".DLL." + args.Name.Split(',')[0];

            //Get a List of Embeded Ressources
            string[] str = a1.GetManifestResourceNames();
            byte[] block = null;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].StartsWith(Name))//Does not check for the Extension, SO multiple extension are a not OK. Or if the File Name does not match the Namespace
                {
                    Stream s = a1.GetManifestResourceStream(str[i]);
                    block = new byte[s.Length];
                    s.Read(block, 0, block.Length);
                    break;
                }
            }

            Assembly a2 = Assembly.Load(block);
            return a2;
        }
    }
}
