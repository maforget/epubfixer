using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace ePubFixer
{
    public partial class frmTocEdit : BaseForm
    {
        #region Constructor
        public frmTocEdit(XElement xml)
            : base(xml)
        {
            InitializeComponent();
            ns = TOC == null ? "\"http://www.daisy.org/z3986/2005/ncx/\" version=\"2005-1\" xml:lang=\"en\"" : TOC.Name.Namespace;
            nodeTextBox.DrawText += new EventHandler<DrawEventArgs>(nodeTextBox_DrawText);
        }
        #endregion

        #region Form Events

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            renameToolStripMenuItem.Text = tree.SelectedNodes.Count > 1 ? "Mass Rename..." : "Rename...";
        }

        void DuplicateToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            foreach (var item in tree.SelectedNodes)
            {
                Node n = item.Tag as Node;
                NavDetails nav = n.Tag as NavDetails;
                int index = n.Index + tree.SelectedNodes.Count - 1;

                Node newNode = new Node(n.Text);
                newNode.IsChecked = true;
                NavDetails Newnav = new NavDetails(Guid.NewGuid().ToString(), nav.ContentSrc);
                newNode.Tag = Newnav;

                n.Parent.Nodes.Insert(index + 1, newNode);
            }



        }

        void nodeTextBox_DrawText(object sender, DrawEventArgs e)
        {
            if (String.IsNullOrEmpty(((e.Node.Tag as Node).Tag as NavDetails).ContentSrc)
                || ((e.Node.Tag as Node).Tag as NavDetails).ContentSrc.StartsWith("#"))
            {
                e.TextColor = Color.Red;
            }
        }

        #endregion

        #region Load TOC

        protected override void LoadBaseNodes()
        {
            if (TOC != null)
            {
                Node ChildNode;
                foreach (var item in TOC.Elements(ns + "navPoint"))
                {
                    if (item.Name.LocalName == "navPoint")
                    {
                        string s = item.Element(ns + "navLabel").Element(ns + "text").Value.Trim();
                        Model.Nodes.Add(new Node(s));
                        ChildNode = Model.Nodes.Last();
                        ChildNode.IsChecked = true;
                        SaveInfoIntoNode(item, ChildNode);
                        LoadChildNode(item, ChildNode);
                    }
                }
            }
        }

        private void LoadChildNode(XElement x, Node parentNode)
        {
            Node TempNode = null;
            foreach (var item in x.Elements(ns + "navPoint"))
            {
                if (item.Name.LocalName == "navPoint")
                {
                    string s = item.Element(ns + "navLabel").Element(ns + "text").Value.Trim();
                    parentNode.Nodes.Add(new Node(s));
                    TempNode = parentNode.Nodes.Last();
                    TempNode.IsChecked = true;
                    SaveInfoIntoNode(item, TempNode);
                    LoadChildNode(item, TempNode);
                }
            }
        }

        private void SaveInfoIntoNode(XElement item, Node childNode)
        {
            XAttribute attr = item.Element(ns + "content").Attribute("src");
            string src = attr == null ? "deleteME" : attr.Value;
            XAttribute attrid = item.Attribute("id");
            string id = attrid == null ? "deleteME" : attrid.Value;

            NavDetails det = new NavDetails(id, src);

            childNode.Tag = det;
        }
        #endregion

        #region TOC Creator
        private void SetNewSource(Dictionary<string, string> newSrc, System.Collections.ObjectModel.Collection<Node> nodes)
        {
            if (newSrc != null)
            {
                foreach (var item in nodes)
                {
                    (item.Tag as NavDetails).ContentSrc = newSrc[(item.Tag as NavDetails).ContentSrc];
                    SetNewSource(newSrc, item.Nodes);
                }
            }
        }

        protected override XElement ExportNewTOC()
        {
            Dictionary<string, string> newSrc = SplitChapters();
            List<XElement> list = new List<XElement>();
            SetNewSource(newSrc, Model.Nodes);

            foreach (Node item in Model.Nodes)
            {
                XElement navPoint = ConvertToXElement(item);
                if (navPoint != null)
                {
                    list.Add(navPoint);
                }
            }

            XElement navMap = new XElement(ns + "navMap", list);
            OnSave(new ExportTocEventArgs(navMap));

            //_NewTOC = navMap;
            return navMap;
        }

        private List<XElement> CreateNavPoint(Node node)
        {
            List<XElement> list = new List<XElement>();

            foreach (Node item in node.Nodes)
            {
                XElement navPoint = ConvertToXElement(item);
                if (navPoint != null)
                {
                    list.Add(navPoint);
                }
            }

            return list;
        }

        private XElement ConvertToXElement(Node item)
        {
            if (item.IsChecked != true)
                return null;

            playOrder++;

            XElement el =
                new XElement(ns + "navPoint",
                    new XAttribute("id", ((NavDetails)item.Tag).navPointid),
                    new XAttribute("playOrder", playOrder),
                    new XElement(ns + "navLabel",
                        new XElement(ns + "text", item.Text)),
                        new XElement(ns + "content",
                            new XAttribute("src", ((NavDetails)item.Tag).ContentSrc)),
                            CreateNavPoint(item));

            return el;
        }

        #endregion

        #region Shift

        private void ShiftChapter(ArrowDirection direction)
        {
            List<string> CleanedFileName = GetFilenames(Model.Nodes, false);

            for (int i = tree.SelectedNodes.Count - 1; i >= 0; i--)
            {
                Node n = tree.SelectedNodes[i].Tag as Node;
                NavDetails nav = n.Tag as NavDetails;
                int index = CleanedFileName.IndexOf(nav.ContentSrc);

                List<string> Backup = new List<string>();
                int newSourceIndex = 0;
                switch (direction)
                {
                    //In shift Down the First Item is ignored
                    //And the 2 First selected item will be the same
                    case ArrowDirection.Down:
                        if (index > 0)
                        {
                            newSourceIndex = index - 1;
                            nav.ShiftChapter(CleanedFileName, newSourceIndex);
                        }
                        break;
                    //In shift Up the Last Item is ignored
                    //And the 2 last selected item will be the same
                    case ArrowDirection.Up:
                        if (index < CleanedFileName.Count - 1)
                        {
                            newSourceIndex = index + 1;
                            nav.ShiftChapter(CleanedFileName, newSourceIndex);
                        }
                        break;
                    default:
                        break;
                }



            }

        }

        private void shiftChapterUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShiftChapter(ArrowDirection.Up);
        }

        private void shiftChapterDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShiftChapter(ArrowDirection.Down);
        }
        #endregion

        #region Add

        protected override void AddFilesToTree(List<NavDetails> filesToAdd)
        {
            if (filesToAdd.Count > 0)
            {
                foreach (NavDetails nav in filesToAdd)
                {
                    Node n = new Node();
                    nav.Text = String.IsNullOrEmpty(nav.Text) ? nav.ContentSrc : nav.Text;
                    n.Tag = nav;
                    n.Text = nav.Text;
                    n.IsChecked = true;
                    if (AddSelectedNode == null)
                    {
                        Model.Nodes.Add(n);
                    } else
                    {
                        AddSelectedNode.Nodes.Add(n);
                    }
                }

                tree.ExpandAll();
            }

        }

        protected override void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add(AddWindowType.TOCEdit);
        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            AddSelectedNode = null;
            Add(AddWindowType.TOCEdit);
        }

        #endregion

        #region Rename
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int qtySelected = tree.SelectedNodes.Count;
            if (qtySelected == 0)
            {
                nodeTextBox.BeginEdit();
            } else
            {
                List<TreeNodeAdv> NodeToRename = tree.SelectedNodes.OrderBy(x => x.Index).ToList();
                List<string> TextToSend = NodeToRename.Select(x => (x.Tag as Node).Text).ToList();

                frmMassRename frmRename = new frmMassRename(TextToSend);
                frmRename.ShowDialog();
                List<string> s = frmRename.RenamedText;

                if (s.Count == 0)
                    return;

                for (int i = 0; i < qtySelected; i++)
                {
                    var item = NodeToRename[i];
                    string text = s[i];
                    Node n = item.Tag as Node;
                    n.Text = text;
                }
            }
        }
        #endregion

        #region Split Chapter
        private Dictionary<string, string> SplitChapters()
        {
            if (cbSplit.Checked)
            {
                Utils.NewFilename(Variables.BackupDone);
                Utils.CloseOpenedForms();

                SplitChapters doc = new SplitChapters(GetFilenames(Model.Nodes, false));
                doc.UpdateFiles();
                return doc.list;
            } else
            {
                return null;
            }
        }
        #endregion


    }

}
