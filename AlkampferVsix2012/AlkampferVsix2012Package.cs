using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Company.AlkampferVsix.Commands;
using EnvDTE80;
using EnvDTE;

namespace Alkampfer.AlkampferVsix2012
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidAlkampferVsix2012PkgString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasMultipleProjects)]
    public sealed class AlkampferVsix2012Package : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public AlkampferVsix2012Package()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        EnvDTE80.DTE2 Dte;

        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        private StopBuildAtFirstError _stopBuildAtFirstErrorCommand;

        private AttachToIIS _attachToIIS;

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            Dte = Package.GetGlobalService(typeof(EnvDTE.DTE)) as DTE2;

            // Add our command handlers for menu (commands must exist in the .vsct file)
            _attachToIIS = new AttachToIIS(Dte);
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidAlkampferVsix2012CmdSet, (int)PkgCmdIDList.attachToIIS);
                MenuCommand menuItem = new MenuCommand(_attachToIIS.MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);
            }

            //Stop at first error command.
            _stopBuildAtFirstErrorCommand = new StopBuildAtFirstError(Dte);
            mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidAlkampferVsix2012CmdSet, (int) PkgCmdIDList.stopBuildAtFirstError);
                MenuCommand menuItem = new MenuCommand(_stopBuildAtFirstErrorCommand.MenuItemCallback, menuCommandID);
                _stopBuildAtFirstErrorCommand.ManageMenuItem(menuItem);
                mcs.AddCommand(menuItem);
            }


            
        }
        #endregion

        #region Command handlers

        

        #endregion



    }
}
