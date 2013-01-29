using System.Reflection;
using System.Runtime.InteropServices;

#if(DEBUG)
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("Vagif Abilov")]
[assembly: AssemblyProduct("PCL Compliance Analyzer")]
[assembly: AssemblyCopyright("Copyright © 2013 Vagif Abilov")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
