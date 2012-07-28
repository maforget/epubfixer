using System.Reflection;
using System;
using System.Runtime.InteropServices;
using CommandLine.Text;
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ePubFixer_cli")]
[assembly: AssemblyDescription("ePubFixer Command Line Client")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("ePubFixer_cli")]
[assembly: AssemblyCopyright("Copyright ©  2012")]
[assembly: AssemblyInformationalVersionAttribute("1.5.3")]
[assembly: AssemblyCulture("")]


// here we're using new CommandLine.Text attributes
//[assembly: AssemblyLicense(
//    "This is free software. You may redistribute copies of it under the terms of",
//    "the MIT License <http://www.opensource.org/licenses/mit-license.php>.")]
[assembly: AssemblyUsage(
    "Usage: ePubFixer_cli -fMyBook.epub -t -h -c",
    "       ePubFixer_cli -f MyBook.epub --toc --html -c",
    "       ePubFixer_cli --file=MyBook.epub --toc -c")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("cf5b3e05-187b-4421-a918-32105ff51f92")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.5.3.0")]
[assembly: AssemblyFileVersion("1.5.3.0")]
