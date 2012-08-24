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
    using Microsoft.VisualStudio.Shell;
    using System.ComponentModel.Design;


    public class AttachToIIS
    {
        DTE2 _dte;

        public AttachToIIS( DTE2 dte)
        {
            _dte = dte;        
        }

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        public void MenuItemCallback(object sender, EventArgs e)
        {

            Execute();
        }

        public void Execute()
        {
            try
            {
                EnvDTE80.Debugger2 dbg2 = (EnvDTE80.Debugger2)_dte.Debugger;
                EnvDTE80.Transport trans = dbg2.Transports.Item("Default");
                EnvDTE.Processes processes = dbg2.GetProcesses(trans, "");
                ListView lvProcesses = new ListView();
                foreach (EnvDTE80.Process2 proc in processes)
                {
                    if ((proc.Name.EndsWith("w3wp.exe")))
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Tag = proc;
                        string engineList = null;
                        lvi.Text = proc.ProcessID.ToString();
                        lvi.SubItems.Add(proc.UserName);
                        lvProcesses.Items.Add(lvi);
                    }
                }

                if (lvProcesses.Items.Count == 0)
                {
                    return;
                }

                if (lvProcesses.Items.Count == 1)
                {
                    EnvDTE80.Process2 proc = (EnvDTE80.Process2)lvProcesses.Items[0].Tag;
                    proc.Attach2();
                    return;
                }

                Form frm = new Form();
                Button btn = new Button();
                btn.Text = "OK";
                btn.DialogResult = DialogResult.OK;
                frm.Controls.Add(btn);
                frm.Width = 700;
                frm.Text = "Choose IIS worker process to debug";
                btn.Dock = DockStyle.Bottom;
                frm.Controls.Add(lvProcesses);
                lvProcesses.Dock = DockStyle.Fill;
                lvProcesses.View = View.Details;
                lvProcesses.Columns.Add("ProcessId", 100, HorizontalAlignment.Left);
                lvProcesses.Columns.Add("User", 300, HorizontalAlignment.Left);
                //lvProcesses.Columns.Add("Type", 300, HorizontalAlignment.Left)
                lvProcesses.FullRowSelect = true;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    foreach (ListViewItem fitem in lvProcesses.SelectedItems)
                    {
                        EnvDTE80.Process2 proc = (EnvDTE80.Process2)fitem.Tag;
                        proc.Attach2();
                    }
                }
            }
            catch (COMException comEx) 
            {
                MessageBox.Show("ERROR:" + ExceptionUtils.GetMessageFromException(comEx.ErrorCode));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("ERROR:" + ex.Message);
            }
        }

    }
}
