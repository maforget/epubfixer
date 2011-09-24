using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace ePubFixer
{
    public abstract partial class BaseForm : Form
    {
        #region Fields
        private bool FilesExtracted = false;
        private bool DoubleClicked;
        protected TreeModel Model;
        private int border = 150;
        private int textColWidth = 100;
        protected XNamespace ns;
        protected XElement TOC;
        protected int playOrder = 0;
        #endregion

        #region Constructor
        public BaseForm()
        {
            InitializeComponent();
            SetToolTips();
            WindowSave.RestoreWindows(Properties.Settings.Default.BaseForm, this);
        }

        public BaseForm(XElement xml)
            : this()
        {
            Model = new TreeModel();
            DoubleClicked = false;
            tree.Size = new Size(tree.Size.Width, this.Size.Height - border);
            TextCol.Width = this.Size.Width - textColWidth;
            this.Icon = Utils.GetIcon();

            tree.Model = Model;
            TOC = xml;

        }
        private void SetToolTips()
        {

            toolTip.SetToolTip(this.cbSplit, "Split your Html file into multiple files\n" +
                                        "when more than 1 chapter per file is found (No duplicate Entries)");
        }
        #endregion

        #region Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            playOrder = 0;
            Utils.RemoveNonExistantNode(Model.Nodes);
            TOC = ExportNewTOC();
            LoadTOC();
            ExtractFiles();
        }

        internal event EventHandler<ExportTocEventArgs> Save;

        //The event-invoking method that derived classes can override.
        protected virtual void OnSave(ExportTocEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<ExportTocEventArgs> handler = Save;
            if (handler != null)
            {
                handler(this, e);
                SetStatus(e.Message);
            }
        }
        #endregion

        #region Form Events

        private void btnClose_Click(object sender, EventArgs e)
        {
            //_NewTOC = null;
            this.Close();
        }

        private void BaseForm_Resize(object sender, EventArgs e)
        {
            tree.BeginUpdate();
            tree.Size = new Size(tree.Size.Width, this.Size.Height - border);
            TextCol.Width = this.Size.Width - textColWidth;
            tree.EndUpdate();
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            int QtyChecked = tree.SelectedNodes.Count(x => (x.Tag as Node).IsChecked == true);
            int QtyUnChecked = tree.SelectedNodes.Count(x => (x.Tag as Node).IsChecked == false);

            if (QtyChecked == tree.SelectedNodes.Count)
            {
                Check.Visible = false;
                unCheck.Visible = true;
            } else if (QtyUnChecked == tree.SelectedNodes.Count)
            {
                unCheck.Visible = false;
                Check.Visible = true;
            } else
            {
                Check.Visible = true;
                unCheck.Visible = true;
            }

            Check.Text = tree.SelectedNodes.Count > 1 ? "Check All Selected" : "Check Selected";
            unCheck.Text = tree.SelectedNodes.Count > 1 ? "Uncheck All Selected" : "Uncheck Selected";
        }

        private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.BaseForm = WindowSave.SaveWindow(this);
            Properties.Settings.Default.InsertAnInlineTOC = cbCreateHtmlTOC.Checked;
            Properties.Settings.Default.Save();
            Preview.CloseOpenedForms();
            Zip.DeleteTemp();
        }

        private void tree_Collapsed(object sender, TreeViewAdvEventArgs e)
        {
            if (DoubleClicked)
            {
                tree.ExpandAll();
                DoubleClicked = false;
            }
        }

        private void tree_MouseHover(object sender, EventArgs e)
        {
            TreeNodeAdv currentNode = tree.CurrentNode;
            TooltipProvider tool = new TooltipProvider();
            nodeTextBox.ToolTipProvider = tool;
            nodeCheckBox.ToolTipProvider = tool;

            if (currentNode != null)
            {
                nodeTextBox.GetToolTip(currentNode);
                nodeCheckBox.GetToolTip(currentNode);
                //SetSelectedStatusSrc();
            }
        }

        private void tree_NodeMouseClick(object sender, TreeNodeAdvMouseEventArgs e)
        {
            SetSelectedStatusSrc();
        }

        #endregion

        #region Load TOC
        private void frmTocEdit_Load(object sender, EventArgs e)
        {
            using (new HourGlass())
            {
                LoadTOC();
                this.Text += " - " + Variables.BookName;
            }

            if (!FilesExtracted)
            {
                ExtractFiles();
            }
        }
        private void LoadTOC()
        {
            tree.BeginUpdate();
            Model.Nodes.Clear();
            LoadBaseNodes();
            Utils.RemoveNonExistantNode(Model.Nodes);
            tree.ExpandAll();
            tree.EndUpdate();
        }

        protected virtual void LoadBaseNodes()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region TOC Creator
        protected virtual XElement ExportNewTOC()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DragDrop
        protected virtual void tree_DragDrop(object sender, DragEventArgs e)
        {
            tree.BeginUpdate();

            TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
            Node dropNode = tree.DropPosition.Node.Tag as Node;
            if (tree.DropPosition.Position == NodePosition.Inside)
            {
                foreach (TreeNodeAdv n in nodes)
                {
                    Node no = n.Tag as Node;
                    no.Parent = dropNode;
                }
                tree.DropPosition.Node.IsExpanded = true;
                tree.ExpandAll();
            } else
            {
                Node parent = dropNode.Parent;
                Node nextItem = dropNode;
                if (tree.DropPosition.Position == NodePosition.After)
                    nextItem = dropNode.NextNode;

                foreach (TreeNodeAdv node in nodes)
                    (node.Tag as Node).Parent = null;

                int index = -1;
                index = parent.Nodes.IndexOf(nextItem);
                foreach (TreeNodeAdv node in nodes)
                {
                    Node item = node.Tag as Node;

                    if (index == -1)
                        parent.Nodes.Add(item);
                    else
                    {
                        parent.Nodes.Insert(index, item);
                        index++;
                    }
                }
            }

            tree.EndUpdate();
        }

        private void tree_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodeAdv[])) && tree.DropPosition.Node != null)
            {
                TreeNodeAdv[] nodes = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
                TreeNodeAdv parent = tree.DropPosition.Node;
                if (tree.DropPosition.Position != NodePosition.Inside)
                    parent = parent.Parent;

                foreach (TreeNodeAdv node in nodes)
                {
                    if (node.Tag.GetType() == typeof(MyNode))
                    {

                        if (this.Name == "frmTocEdit")
                        {
                            MyNode item = node.Tag as MyNode;
                            item.Nodes.Clear();
                            NavDetails nav = item.Tag as NavDetails;
                            string str = String.IsNullOrEmpty(nav.Text) ? nav.ContentSrc : nav.Text;
                            item.Text = str;
                            item.IsChecked = true;
                        } else
                        {
                            Node item = node.Tag as Node;
                            NavDetails nav = item.Tag as NavDetails;
                            item.Text = Utils.GetId(nav.ContentSrc);
                            item.IsChecked = true;
                        }
                    }

                    if (!CheckNodeParent(parent, node))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }

                e.Effect = e.AllowedEffect;
            }
        }

        private bool CheckNodeParent(TreeNodeAdv parent, TreeNodeAdv node)
        {
            while (parent != null)
            {
                if (node == parent)
                    return false;
                else
                    parent = parent.Parent;
            }
            return true;
        }

        private void tree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            tree.DoDragDropSelectedNodes(DragDropEffects.Move);
        }
        #endregion

        #region Preview
        protected Dictionary<string, string> GetSelectedFileNames()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            foreach (var item in tree.SelectedNodes)
            {
                list.Add(((item.Tag as Node).Tag as NavDetails).ContentSrc, (item.Tag as Node).Text);
                //list.Add(((item.Tag as Node).Tag as NavDetails).ContentSrc.Split('#')[0]);
            }

            return list;
        }

        private void ShowPreview()
        {
            foreach (var item in GetSelectedFileNames())
            {
                this.ShowPreview(item.Key, item.Value);
                DoubleClicked = true;
            }
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPreview();
        }

        private void tree_DoubleClick(object sender, EventArgs e)
        {
            ShowPreview();
        }
        #endregion

        #region Check All
        private void checkAllSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkAll(true);
        }

        private void checkAll(bool IsChecked)
        {
            foreach (var item in tree.SelectedNodes)
            {
                Node n = item.Tag as Node;
                n.IsChecked = IsChecked;
            }
        }

        private void unCheckAllSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkAll(false);
        }
        #endregion

        #region Add
        protected Node AddSelectedNode = null;
        protected void Add(AddWindowType add)
        {
            AddSelectedNode = tree.SelectedNodes.Count > 0 ? tree.SelectedNodes.LastOrDefault().Tag as Node : null;

            List<string> CleanedFilename = GetFilenames(Model.Nodes, false);

            frmAdd frm = new frmAdd(CleanedFilename, add);
            Variables.OpenedForm.Add(frm);
            frm.Show();
            frm.FilesAdded += new EventHandler<FilesAddedArgs>(frm_FilesAdded);
        }

        protected virtual void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void frm_FilesAdded(object sender, FilesAddedArgs e)
        {
            AddFilesToTree(e.Files);
        }

        protected virtual void AddFilesToTree(List<NavDetails> filesToAdd)
        {
            throw new NotImplementedException();
        }

        protected virtual void btnAdd_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Status Strip Text
        private void SetSelectedStatusSrc()
        {
            try
            {
                Node n = (tree.SelectedNode.Tag as Node);
                statusLabel.Text = (n.Tag as NavDetails).ContentSrc;
                this.Update();
            } catch (Exception)
            {
                statusLabel.Text = string.Empty;
                this.Update();
            }
        }
        private void BaseForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                    SetSelectedStatusSrc();
                    break;

                default:
                    break;
            }
        }


        private void SetStatus(string Message)
        {
            tree.NodeMouseClick -= tree_NodeMouseClick;
            statusLabel.Text = Message;
            timer.Start();
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            tree.NodeMouseClick += new EventHandler<TreeNodeAdvMouseEventArgs>(tree_NodeMouseClick);
            SetSelectedStatusSrc();
        } 
        #endregion

        #region Get FileNames
        /// <summary>
        /// Get Cleaned FileNames
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Clean FileNames</returns>
        protected List<string> GetFilenames(System.Collections.ObjectModel.Collection<Node> node)
        {
            bool clean = true;
            return GetFilenames(node, clean);
        }

        protected List<string> GetFilenames(System.Collections.ObjectModel.Collection<Node> node, bool Clean)
        {
            List<string> CleanedFilename = new List<string>();

            foreach (Node item in node)
            {
                string name = "";
                name = Clean ? (item.Tag as NavDetails).ContentSrc.Split('#')[0] : (item.Tag as NavDetails).ContentSrc;

                if (!String.IsNullOrEmpty(name))
                    CleanedFilename.Add(name);

                CleanedFilename.AddRange(GetFilenames(item.Nodes, Clean).ToArray());
            }
            return CleanedFilename;
        }
        #endregion

        #region Delete
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Node> listNode = tree.SelectedNodes.Select(x => x.Tag as Node).ToList();
            RemoveNodes(Model.Nodes, listNode);
        }

        private void RemoveNodes(System.Collections.ObjectModel.Collection<Node> node, List<Node> NodesToDel)
        {
            for (int i = node.Count - 1; i >= 0; i--)
            {
                Node item = node[i];

                NodesToDel.ForEach(n =>
                {
                    if (item == n)
                        node.Remove(item);
                });

                RemoveNodes(item.Nodes, NodesToDel);
            }
        }
        #endregion

        #region Extract Temp Files
        private void ExtractFiles()
        {
            Zip.ExtractProgress += new EventHandler<ExtractProgressArgs>(Utils_ExtractProgress);
            Thread t = new Thread(new ThreadStart(Zip.ExtractZip));
            t.IsBackground = true;
            t.Start();
            FilesExtracted = true;
        }

        void Utils_ExtractProgress(object sender, ExtractProgressArgs e)
        {
            UpdateProgress(e.Pourcentage);
        }

        private delegate void UpdateProgressDelegate(int Pourc);
        private void UpdateProgress(int Pourc)
        {
            // Lock this block of code to one thread at a time because of multiple
            // threads accessing it and updating control.
            lock (this)
            {
                // Check to see if the thread that is trying to access this code
                // is the GUI thread or another thread.
                if (Progress.ProgressBar.InvokeRequired)
                {
                    // The thread that entered this code in not the thread that create
                    // the control. Create a deleagte and then execute this procedure
                    // from the GUI thread.
                    UpdateProgressDelegate del = new UpdateProgressDelegate(UpdateProgress);
                    Progress.ProgressBar.BeginInvoke(del, Pourc);
                } else
                {
                    // The thread in this section of code is the GUI thread
                    Progress.Value = Pourc;

                }
            }
        }


        #endregion









    }

    public class TooltipProvider : IToolTipProvider
    {
        #region IToolTipProvider Members

        public string GetToolTip(TreeNodeAdv node, NodeControl nodeControl)
        {
            return ((node.Tag as Node).Tag as NavDetails).ContentSrc;
        }

        #endregion
    }


}
