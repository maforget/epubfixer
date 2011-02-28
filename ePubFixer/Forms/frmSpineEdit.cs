using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Aga.Controls.Tree;


namespace ePubFixer
{
    public partial class frmSpineEdit : BaseForm
    {
        #region Constructor
        public frmSpineEdit(XElement xml)
            : base(xml)
        {
            InitializeComponent();
            base.cbSplit.Visible = false;
            ns = TOC == null ? "\"http://www.idpf.org/2007/opf\"" : TOC.Name.Namespace;

        }
        #endregion

        #region Form Events

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (tree.SelectedNodes.Count == 0)
            {
                e.Cancel = true;
            }
        }



        #endregion

        #region Load TOC

        protected override void LoadBaseNodes()
        {
            if (TOC != null)
            {
                Node ChildNode;
                foreach (var item in TOC.Elements())
                {
                    if (item.Name.LocalName == "itemref")
                    {
                        string idRef = item.Attribute("idref").Value;
                        Model.Nodes.Add(new Node(idRef));
                        ChildNode = Model.Nodes.Last();
                        ChildNode.IsChecked = true;
                        SaveInfoIntoNode(idRef, ChildNode);
                    }
                }
            }
        }


        private void SaveInfoIntoNode(string idRef, Node childNode)
        {
            NavDetails det = new NavDetails(idRef, Utils.GetSrc(idRef));
            childNode.Tag = det;
        }
        #endregion

        #region TOC Creator
        protected override XElement ExportNewTOC()
        {
            List<XElement> list = new List<XElement>();

            foreach (Node item in Model.Nodes)
            {
                XElement navPoint = ConvertToXElement(item);
                if (navPoint != null)
                {
                    list.Add(navPoint);
                }
            }
            IEnumerable<XAttribute> spine = TOC.Attributes();
            XElement navMap = null;

            if (spine.Count()>0)
            {
                navMap = new XElement(ns + "spine", spine, list);
            } else
            {
                //There is no spine elements
                OpfDocument doc = new OpfDocument();
                XAttribute TocRef = new XAttribute("toc", doc.GetNCXid());

                navMap = new XElement(ns + "spine", TocRef, list);
            }


            OnSave(new ExportTocEventArgs(navMap));

            //_NewTOC = navMap;
            return navMap;
        }

        private XElement ConvertToXElement(Node item)
        {
            if (item.IsChecked != true)
                return null;


            XElement el =
                    new XElement(ns + "itemref",
                        new XAttribute("idref", ((NavDetails)item.Tag).navPointid));

            return el;
        }

        #endregion

        #region DragDrop
        protected override void tree_DragDrop(object sender, DragEventArgs e)
        {
            tree.BeginUpdate();

            TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
            Node dropNode = tree.DropPosition.Node.Tag as Node;

            Node parent = dropNode.Parent;
            Node nextItem = dropNode;

            if (tree.DropPosition.Position == NodePosition.After ||
                tree.DropPosition.Position == NodePosition.Inside)
            {
                nextItem = dropNode.NextNode;
            }

            foreach (TreeNodeAdv node in nodes)
                (node.Tag as Node).Parent = null;

            int index = -1;
            index = parent.Nodes.IndexOf(nextItem);
            foreach (TreeNodeAdv node in nodes)
            {
                Node item = node.Tag as Node;
                (node.Tag as Node).Text = ((node.Tag as Node).Tag as NavDetails).navPointid;
                item.IsChecked = true;
                if (index == -1)
                    parent.Nodes.Add(item);
                else
                {
                    parent.Nodes.Insert(index, item);
                    index++;
                }
            }

            tree.EndUpdate();
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
                    //nav.Text = String.IsNullOrEmpty(nav.Text) ? nav.ContentSrc : nav.Text;
                    string Id = Utils.GetId(nav.ContentSrc);
                    n.Tag = nav;
                    n.Text = Id;
                    n.IsChecked = true;
                    Model.Nodes.Add(n);
                }
            }

        }

        protected override void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add(AddWindowType.SpineEdit);

        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            Add(AddWindowType.SpineEdit);
        }
        #endregion

    }
}
