using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace ePubFixer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Resolver);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Utils.Upgrade();
            GetArgument(args);
                
            Application.Run(new frmMain());
        }

        #region Get Filenames from Command-Line Arguments
        private static void GetArgument(string[] args)
        {
            foreach (string item in args)
            {
                //Argument is a directory search for epub
                if (Directory.Exists(item))
                {
                    Variables.Filenames.AddRange(Directory.GetFiles(item, "*.epub", SearchOption.AllDirectories));
                } else
                {

                    if (Path.GetExtension(item) == ".epub")
                    {
                        Variables.Filenames.Add(item);
                    }
                }
            }
        } 
        #endregion

        #region Load Librairies from embedded files
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
        #endregion
    }



}
