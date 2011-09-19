using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Aga.Controls.Tree;

namespace ePubFixer
{
    public partial class frmAdd : Form
    {
        #region Fields

        private List<string> PresentFileList = new List<string>();
        private TreeModel Model;
        internal event EventHandler<FilesAddedArgs> FilesAdded;
        private List<NavDetails> FilesToAdd;
        private Dictionary<string, string> PresentAnchors = new Dictionary<string, string>();
        Dictionary<string, bool> IsNodeExpanded = new Dictionary<string, bool>();
        Dictionary<string, string> TextSelected = new Dictionary<string, string>();
        private IEnumerable<string> presentFile;
        private AddWindowType AddType;
        #endregion

        #region Constructor
        public frmAdd(IEnumerable<string> presentFilelist, AddWindowType add)
        {
            InitializeComponent();

            WindowSave.RestoreWindows(Properties.Settings.Default.frmAdd, this);
            RestoreColumns();

            this.Icon = Utils.GetIcon();

            AddType = add;
            presentFile = presentFilelist;
            SetPresentFiles();

            Model = new Aga.Controls.Tree.TreeModel();
            treeView1.Model = Model;
            this.Text = Variables.BookName;
            nodeDetectedTexts.EditorShowing += new CancelEventHandler(nodeDetectedTexts_EditorShowing);
            nodeDetectedTexts.ChangesApplied += new EventHandler(nodeDetectedTexts_ChangesApplied);
            nodeFile.DrawText += new EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(node_DrawText);
            nodeDetectedTexts.DrawText += new EventHandler<Aga.Controls.Tree.NodeControls.DrawEventArgs>(node_DrawText);
            treeView1.Expanded += new EventHandler<TreeViewAdvEventArgs>(treeView1_Expanded);
            treeView1.Collapsed += new EventHandler<TreeViewAdvEventArgs>(treeView1_Expanded);

            if (AddType == AddWindowType.SpineEdit)
            {
                cbShowAll.Visible = false;
                cbShowAnchors.Visible = false;
                btnSearchNet.Visible = false;
            }
        }

        #endregion

        #region Set Files Present in TOC
        private void SetPresentFiles()
        {
            foreach (var item in presentFile)
            {
                AddPresentFiles(item);
            }
        }

        private void SetFilesBasedOnAnchors(string file)
        {
            if (AddType == AddWindowType.TOCEdit)
            {
                MyHtmlDocument doc = new MyHtmlDocument();
                List<string> Anchors = doc.FindAnchorsInFile(file);

                foreach (var anch in Anchors)
                {
                    string str = file + '#' + anch;

                    if (!PresentAnchors.ContainsKey(str) && PresentFileList.Contains(file))
                    {
                        PresentFileList.Remove(file);
                    }
                }
            }
        }

        private void AddPresentFiles(string source)
        {
            string[] split = source.Split('#');
            string path = split[0];
            string Anchor = split[split.Length - 1];

            if (!PresentFileList.Contains(path))
                PresentFileList.Add(path);

            if (!PresentAnchors.ContainsKey(source))
                PresentAnchors.Add(source, Anchor);

            SetFilesBasedOnAnchors(path);
        }

        #endregion

        #region Detected TExt
        private void AddToTextSelected(NavDetails nav)
        {
            if (!TextSelected.ContainsKey(nav.ContentSrc))
            {
                TextSelected.Add(nav.ContentSrc, nav.Text);
            } else
            {
                TextSelected[nav.ContentSrc] = nav.Text;
            }
        }

        void nodeDetectedTexts_ChangesApplied(object sender, EventArgs e)
        {
            MyNode n = treeView1.CurrentNode.Tag as MyNode;
            NavDetails nav = n.Tag as NavDetails;
            if (nav.DetectedTexts != null && nav.DetectedTexts.Count > 0)
            {
                nav.Text = nodeDetectedTexts.GetValue(treeView1.CurrentNode).ToString();
                AddToTextSelected(nav);
            }
        }

        void nodeDetectedTexts_EditorShowing(object sender, CancelEventArgs e)
        {
            nodeDetectedTexts.DropDownItems.Clear();
            MyNode n = treeView1.CurrentNode.Tag as MyNode;
            NavDetails nav = n.Tag as NavDetails;
            if (nav.DetectedTexts != null && nav.DetectedTexts.Count > 0)
            {
                nodeDetectedTexts.DropDownItems.AddRange(nav.DetectedTexts.ToArray());
            }
        }
        #endregion

        #region Select Next TExt
        private void selectNext_Click(object sender, EventArgs e)
        {
            ChooseTextInDropDown(SelectDirection.Next, false);
        }

        private void selectNextDownload_Click(object sender, EventArgs e)
        {
            ChooseTextInDropDown(SelectDirection.Next, true);
        }

        private void selectPrevious_Click(object sender, EventArgs e)
        {
            ChooseTextInDropDown(SelectDirection.Previous, false);
        }

        enum SelectDirection
        {
            Previous, Next
        }

        private void ChooseTextInDropDown(SelectDirection dir, bool Incremental)
        {
            using (new HourGlass())
            {
                treeView1.BeginUpdate();
                int Increment = 1;
                foreach (var item in treeView1.SelectedNodes)
                {
                    MyNode n = item.Tag as MyNode;
                    NavDetails nav = n.Tag as NavDetails;
                    int Base = Incremental ? n.OriginalCount - 1 : 0;
                    if (nav.DetectedTexts == null)
                        continue;

                    int QtyMax = n.DetectedCombo.Count - 1;
                    int index = nav.DetectedTexts.IndexOf(n.DetectedText);
                    if (nav.DetectedTexts.Count > 1)
                    {
                        if (index >= 0 && dir == SelectDirection.Next)
                        {
                            int NewIndex = Incremental ? index + Increment + Base : index + Increment;
                            if (NewIndex > QtyMax)
                            {
                                NewIndex = QtyMax;
                            }

                            nav.Text = nav.DetectedTexts[NewIndex];
                            n.DetectedText = nav.DetectedTexts[NewIndex];
                        } else
                        {
                            if (index > 0 && index <= QtyMax)
                            {
                                nav.Text = nav.DetectedTexts[index - Increment];
                                n.DetectedText = nav.DetectedTexts[index - Increment];
                            }
                        }


                        if (Incremental)
                            Increment++;

                    }
                    AddToTextSelected(nav);
                }
                treeView1.EndUpdate();
            }
        }
        #endregion

        #region Load
        private void frmAdd_Load(object sender, EventArgs e)
        {
            LoadFiles();
        }

        private void LoadFiles()
        {
            try
            {

                using (new HourGlass())
                {
                    treeView1.BeginUpdate();

                    if (Model != null && Model.Nodes.Count > 0)
                        Model.Nodes.Clear();

                    OpfDocument OpfDoc = new OpfDocument();
                    List<string> htmlFileLIst = OpfDoc.GetFilesList("html");
                    List<string> t = htmlFileLIst;
                    if (!cbShowAll.Checked)
                    {
                        t = (from i in htmlFileLIst
                             where !PresentFileList.Contains(i)
                             select i).ToList();
                    }

                    MyHtmlDocument htmlDoc = new MyHtmlDocument();
                    Dictionary<string, DetectedHeaders> DetectText = htmlDoc.FindHeaderTextInFile(t);

                    foreach (string item in t)
                    {
                        DetectedHeaders det = new DetectedHeaders();
                        DetectText.TryGetValue(item, out det);
                        List<string> text = det != null ? det.Result : null;
                        MyNode n = new MyNode(item, text);
                        n.OriginalCount = det != null ? det.OriginalCount : 0;
                        Model.Nodes.Add(n);
                    }

                    SortList();

                    Dictionary<string, string> SrcTag = OpfDoc.GetFilesList();
                    foreach (MyNode item in Model.Nodes)
                    {
                        item.Tag = new NavDetails(Utils.GetId(item.Text, SrcTag), item.Text, item.DetectedCombo);
                        NavDetails nav = item.Tag as NavDetails;

                        if (AddType == AddWindowType.TOCEdit && cbShowAnchors.Checked)
                        {
                            List<string> Anchors = htmlDoc.FindAnchorsInFile(item.Text);

                            if (!cbShowAll.Checked)
                            {
                                Anchors = (from i in Anchors
                                           where !PresentAnchors.ContainsKey(nav.File + "#" + i)
                                           select i).ToList();
                            }

                            Dictionary<string, DetectedHeaders> DetectAnchorText = htmlDoc.FindAchorTextInFile(item.Text, Anchors);
                            item.AddAnchors(Anchors, DetectAnchorText);
                        }
                    }

                    RemoveEmptyNodes();
                    treeView1.EndUpdate();

                    //Utils.RemoveNonExistantNode(Model.Nodes);
                }
            } catch (Exception)
            {

            }
        }

        /// <summary>
        /// Removes Nodes when there is only an anchor that has no detected text
        /// </summary>
        private void RemoveEmptyNodes()
        {
            if (!cbShowAll.Checked && AddType == AddWindowType.TOCEdit)
            {
                for (int i = Model.Nodes.Count - 1; i >= 0; i--)
                {
                    MyNode node = Model.Nodes[i] as MyNode;
                    node.RemoveAnchors(x => x.DetectedCombo.Count == 0, PresentFileList.Contains(node.Text));
                    node.RemoveAnchors(x => x.DetectedText == node.DetectedText && PresentAnchors.ContainsKey(node.Text), true);

                    if (!PresentFileList.Contains(node.Text) && PresentAnchors.ContainsKey(node.Text) && node.Nodes.Count == 0)
                        Model.Nodes.Remove(node);
                }
            }
        }

        private void node_DrawText(object sender, Aga.Controls.Tree.NodeControls.DrawEventArgs e)
        {
            NavDetails nav = ((e.Node.Tag as MyNode).Tag as NavDetails);
            if (nav != null)
            {
                string anch = "";
                if (PresentAnchors.TryGetValue(nav.ContentSrc, out anch))
                {
                    Font f = new System.Drawing.Font(e.Font, FontStyle.Bold);
                    e.TextColor = Color.DarkGreen;
                    e.Font = f;
                }

                //Keep expanded status
                bool expand = false;
                IsNodeExpanded.TryGetValue(nav.File, out expand);
                if (expand)
                {
                    SaveExpand = false;
                    e.Node.Expand();
                    SaveExpand = true;
                } else
                {
                    e.Node.Collapse();
                }

                //Keep SelectedText
                string text = "";
                if (TextSelected.TryGetValue(nav.ContentSrc, out text))
                {
                    ((MyNode)e.Node.Tag).DetectedText = text;
                }
            }
        }

        #endregion

        #region Form Events
        bool SaveExpand = true;
        void treeView1_Expanded(object sender, TreeViewAdvEventArgs e)
        {
            MyNode node = e.Node.Tag as MyNode;

            if (SaveExpand)
            {
                if (IsNodeExpanded.ContainsKey(node.Text))
                {
                    bool exp = IsNodeExpanded[node.Text];
                    IsNodeExpanded[node.Text] = exp ? false : true;
                } else
                {
                    IsNodeExpanded.Add(node.Text, true);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            FilesToAdd = null;
            this.Close();
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            ShowPreview();
        }

        private void frmAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.frmAdd = WindowSave.SaveWindow(this);
            SaveColumns();
            Properties.Settings.Default.Save();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (treeView1.SelectedNodes.Count == 0)
            {
                e.Cancel = true;
            } else
            {
                int AnchorSelected = treeView1.SelectedNodes.Count(x => x.Level > 1);
                int FileSelected = treeView1.SelectedNodes.Count(x => x.Level == 1);
                int QtyChecked = treeView1.SelectedNodes.Count(x => (x.Tag as Node).IsChecked == true);
                int QtyUnChecked = treeView1.SelectedNodes.Count(x => (x.Tag as Node).IsChecked == false);
                int QtyCollapsed = treeView1.AllNodes.Count(x => x.IsExpanded == false && x.Level == 1);
                int QtyExpanded = treeView1.AllNodes.Count(x => x.IsExpanded == true && x.Level == 1);

                deleteFilesToolStripMenuItem.Visible = FileSelected == 0 && AnchorSelected >= 1 ? false : true;
                selectNextTextIncremental.Visible = Variables.TOCDownloadedFromNet ? true : false;

                if (QtyChecked == treeView1.SelectedNodes.Count)
                {
                    check.Visible = false;
                    unCheck.Visible = true;
                } else if (QtyUnChecked == treeView1.SelectedNodes.Count)
                {
                    unCheck.Visible = false;
                    check.Visible = true;
                } else
                {
                    check.Visible = true;
                    unCheck.Visible = true;
                }

                expandAllToolStripMenuItem.Visible = QtyCollapsed > 0 ? cbShowAnchors.Checked : false;
                collapseAllToolStripMenuItem.Visible = QtyExpanded > 0 ? cbShowAnchors.Checked : false;

                check.Text = treeView1.SelectedNodes.Count > 1 ? "Check All Selected" : "Check Selected";
                unCheck.Text = treeView1.SelectedNodes.Count > 1 ? "Uncheck All Selected" : "Uncheck Selected";
            }

        }
        #endregion

        #region Preview
        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPreview();
        }

        private void ShowPreview()
        {
            foreach (var item in treeView1.SelectedNodes)
            {
                Node n = item.Tag as Node;
                string file = (n.Tag as NavDetails).ContentSrc;
                this.ShowPreview(file);
            }
        }
        #endregion

        #region Check All
        private void check_Click(object sender, EventArgs e)
        {
            checkAll(true);
        }

        private void unCheck_Click(object sender, EventArgs e)
        {
            checkAll(false);
        }

        private void checkAll(System.Collections.ObjectModel.Collection<Node> Nodes, bool IsChecked)
        {
            foreach (var n in Nodes)
            {
                n.IsChecked = IsChecked;
                checkAll(n.Nodes, IsChecked);
            }
        }

        private void checkAll(bool IsChecked)
        {
            foreach (var item in treeView1.SelectedNodes)
            {
                Node n = item.Tag as Node;
                n.IsChecked = IsChecked;
            }
        }
        #endregion

        #region DragDrop
        bool ModelSaved = false;
        private void tree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            treeView1.DoDragDropSelectedNodes(DragDropEffects.Copy);
            if (cbShowAll.Checked)
            {
                TreeNodeAdv[] items = e.Item as TreeNodeAdv[];

                foreach (var item in items)
                {
                    MyNode nav = item.Tag as MyNode;
                    AddPresentFiles(nav.ContentSrc);
                }
            }
        }

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            if (!ModelSaved)
            {
                Model.NodesRemoved += new EventHandler<TreeModelEventArgs>(Model_NodesRemoved);
                ModelSaved = true;
            }
        }

        void Model_NodesRemoved(object sender, TreeModelEventArgs e)
        {
            if (ModelSaved)
            {
                Model.NodesRemoved -= Model_NodesRemoved;
                ModelSaved = false;

                if (cbShowAll.Checked)
                {
                    //TODO Reload TOC if Show All is true and using drag drop
                    LoadFiles();
                }
            }
        }
        #endregion

        #region Show All & Show Anchors
        private void cbShowAll_CheckedChanged(object sender, EventArgs e)
        {
            if (Model != null)
            {
                //this.IsNodeExpanded = new Dictionary<string, bool>();
                LoadFiles();
            }
        }

        private void cbShowAnchors_CheckedChanged(object sender, EventArgs e)
        {
            if (Model != null)
            {
                SetPresentFiles();
                LoadFiles();
            }
        }
        #endregion

        #region Add
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (FilesAdded != null)
            {
                FilesToAdd = new List<NavDetails>();
                SelectFilesToAdd(Model.Nodes);
                FilesAdded(this, new FilesAddedArgs(FilesToAdd));
            }

            LoadFiles();
        }

        private void SelectFilesToAdd(System.Collections.ObjectModel.Collection<Node> node)
        {
            foreach (var item in node)
            {
                if (item.IsChecked)
                {
                    NavDetails nav = item.Tag as NavDetails;
                    AddPresentFiles(nav.ContentSrc);

                    FilesToAdd.Add(nav);
                }
                SelectFilesToAdd(item.Nodes);
            }
        }
        #endregion

        #region Form Save
        private void RestoreColumns()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Columns) == true)
            {
                return;
            }
            string[] columns = Properties.Settings.Default.Columns.Split('|');
            int FileCol = colFile.Width;
            int TextCol = colDetectedText.Width;

            if (int.TryParse(columns[0], out FileCol))
            {
                colFile.Width = FileCol;
            }

            if (int.TryParse(columns[1], out TextCol))
            {
                colDetectedText.Width = TextCol;
            }

        }

        private void SaveColumns()
        {
            Properties.Settings.Default.Columns =
                colFile.Width.ToString() + "|" +
                colDetectedText.Width.ToString();
        }
        #endregion

        #region Expand All
        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }
        #endregion

        #region Sorting
        private void treeView1_ColumnClicked(object sender, TreeColumnEventArgs e)
        {
            if (e.Column.SortOrder != SortOrder.Ascending)
            {
                e.Column.SortOrder = SortOrder.Ascending;
            } else
            {
                e.Column.SortOrder = SortOrder.Descending;
            }

            SortList();
        }

        private void OrderNodes(TreeColumn column)
        {
            List<Node> Ordered = new List<Node>();

            if (column.SortOrder == SortOrder.Ascending)
            {
                Ordered = Model.Nodes.OrderBy(x => column.Index == 0 ? (x as MyNode).Text : (x as MyNode).DetectedText).ToList();
            } else
            {
                Ordered = Model.Nodes.OrderByDescending(x => column.Index == 0 ? (x as MyNode).Text : (x as MyNode).DetectedText).ToList();
            }

            treeView1.BeginUpdate();
            Model.Nodes.Clear();
            foreach (MyNode item in Ordered)
            {
                Model.Nodes.Add(item);
            }
            treeView1.EndUpdate();

        }

        private TreeColumn GetSortingColumn()
        {
            TreeColumn column = null;
            if (colFile.SortOrder != SortOrder.None)
            {
                column = colFile;
            } else if (colDetectedText.SortOrder != SortOrder.None)
            {
                column = colDetectedText;
            }

            return column;
        }

        private void SortList()
        {
            TreeColumn column = GetSortingColumn();

            if (column != null)
            {
                OrderNodes(column);
            }
        }
        #endregion

        #region Delete Files
        private void deleteFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Remove these files?\nThis can't be undone. You should make sure to backup the file first",
                "Delete Files", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (new HourGlass())
                {
                    var FilesToDelete = from i in treeView1.SelectedNodes
                                        where i.Level == 1
                                        select (i.Tag as MyNode).ContentSrc;

                    Utils.DeleteFiles(FilesToDelete);
                }

                LoadFiles();
            }

        }
        #endregion

        #region Download From Kobo
        private void btnSearchNet_Click(object sender, EventArgs e)
        {
            SiteGraber site = new SiteGraber();
            List<string> Name = site.Parse();

            if (Name != null)
            {
                //Process the names (Add the found TOC to Varaiables.Header)
                foreach (var item in Variables.HeaderTextInFile)
                {
                    item.Value.Result.AddRange(Name);
                }

                //Reload
                LoadFiles();
                Variables.TOCDownloadedFromNet = true;
            }
        }
        #endregion

        #region Select All
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectAll();
        }

        private void selectAll()
        {
            treeView1.SelectAllNodes();
        } 
        #endregion

        private void tableOfContentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNode node = treeView1.SelectedNode.Tag as MyNode;
            NavDetails nav = node.Tag as NavDetails;

            OpfDocument doc = new OpfDocument();
            doc.AddTOCContentRef(nav.File);
        }

        private void coverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyNode node = treeView1.SelectedNode.Tag as MyNode;
            NavDetails nav = node.Tag as NavDetails;

            OpfDocument doc = new OpfDocument();
            doc.AddCoverRef(nav.File);
        }


        //TODO Look for links in html files and add a option to see only those (ex : Content with LInks or Footnote)


    }

}
