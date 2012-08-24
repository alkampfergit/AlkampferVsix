using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.AlkampferVsix.Commands
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Utilities;
    using System.Windows.Media;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using AlkampferVsix2012.Utils;
    using EnvDTE80;
    using System.ComponentModel.Design;
    using Microsoft.VisualStudio.Shell;
    using EnvDTE;
    using System.IO;

    public class StopBuildAtFirstError
    {
        DTE2 _dte;
        MenuCommand _menuItem;
        Boolean _enabled = false;
        private Boolean _alreadyStopped = false;

        public StopBuildAtFirstError(DTE2 dte)
        {
            _dte = dte;
            dte.Events.BuildEvents.OnBuildProjConfigDone += OnBuildProjConfigDone;
            dte.Events.BuildEvents.OnBuildBegin += (sender, e) => _alreadyStopped = false;
        }


        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        public void MenuItemCallback(object sender, EventArgs e)
        {
            _menuItem.Checked = !_menuItem.Checked;
            _enabled = _menuItem.Checked;
        }

        private void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            if (_alreadyStopped || success || !_enabled) return;

            _alreadyStopped = true;

            _dte.ExecuteCommand("Build.Cancel");

            var pane = _dte.ToolWindows.OutputWindow.OutputWindowPanes
                                       .Cast<OutputWindowPane>()
                                       .SingleOrDefault(x => x.Guid.Equals(AlkampferVsix2012.Utils.Constants.BuildOutput, StringComparison.OrdinalIgnoreCase));

            if (pane != null)
            {
                Int32 lastSlashIndex = project.LastIndexOf('\\');
                String projectFileName = project.Substring(lastSlashIndex + 1, project.LastIndexOf('.') - lastSlashIndex - 1);
                var message = string.Format("INFO: Build stopped because project {0} failed to build.\n", projectFileName);
                pane.OutputString(message);
                pane.Activate();
            }
        }

        internal void ManageMenuItem(MenuCommand menuItem)
        {
            _menuItem = menuItem;
            _menuItem.Checked = false;
        }
    }
}
