using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aga.Controls;
using Aga.Controls.Tree;
using ePubFixer;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace ePubFixer
{
    /// <summary>
    /// Inherits the node class to show how the class can be extended.
    /// </summary>
    public class MyNode : Node
    {

        public string ContentSrc
        {
            get
            {
                NavDetails nav = Tag as NavDetails;
                return nav.ContentSrc;
            }
        }

        public string File
        {
            get
            {
                NavDetails nav = Tag as NavDetails;
                return nav.File;
            }
        }

        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException();

                base.Text = value;
            }
        }

        /// <summary>
        /// Whether the box is checked or not.
        /// </summary>
        public new bool IsChecked
        {
            get
            {
                return base.IsChecked;
            }
            set
            {
                base.IsChecked = value;
            }
        }

        private string _DetectedText;

        /// <summary>
        /// Text Detected beside Anchor or at The beginning of a Page
        /// </summary>
        public string DetectedText
        {
            get
            {
                return _DetectedText;
            }
            set
            {
                _DetectedText = value;
            }
        }

        public int OriginalCount { get; set; }

        private List<string> _DetectedCombo;
        public List<string> DetectedCombo
        {
            get
            {
                return _DetectedCombo;
            }
            set
            {
                _DetectedCombo = value;
            }
        }

        /// <summary>
        /// Initializes a new MyNode class with a given Text property.
        /// </summary>
        /// <param name="text">String to set the text property with.</param>
        public MyNode(string text)
            : base(text)
        {
        }

        /// <summary>
        /// Initializes a new MyNode class with a given Text property and A Detected Text
        /// </summary>
        /// <param name="text">String For the Main Node</param>
        /// <param name="detectedText">String For the text Detected that will be used for the Chapter header</param>
        public MyNode(string text, string detectedText)
            : this(text)
        {
            _DetectedText = detectedText;

        }

        /// <summary>
        /// Initializes a new MyNode class with a given Text property.
        /// </summary>
        public MyNode(string text, List<string> detectedCombo)
            : this(text)
        {
            this._DetectedCombo = detectedCombo;
            this.DetectedText = detectedCombo == null ? "" : detectedCombo.FirstOrDefault();
        }



        public void AddAnchors(List<string> Anchors, Dictionary<string, DetectedHeaders> DetectAnchorText)
        {
            foreach (string anch in Anchors)
            {
                DetectedHeaders det = new DetectedHeaders();
                string source = Text + "#" + anch;
                DetectAnchorText.TryGetValue(source, out det);
                List<string> text = det != null ? det.Result : null;
                MyNode n = new MyNode(anch, text);
                n.Tag = new NavDetails(Guid.NewGuid().ToString(), source, n.DetectedCombo);
                n.OriginalCount = det != null ? det.OriginalCount : 0;
                Nodes.Add(n);
            }
        }

        public bool RemoveAnchors(Predicate<MyNode> Check, bool RemoveNodeCheck)
        {
            bool RemovedAnchor = false;

            for (int j = Nodes.Count - 1; j >= 0; j--)
            {
                MyNode anch = Nodes[j] as MyNode;

                //Remove all nodes that have the same text as the top & those that do not have any text
                if(Check.Invoke(anch))
                {
                    Nodes.Remove(anch);
                    RemovedAnchor = true;
                }
            }

            if (Nodes.Count == 0 & RemovedAnchor & RemoveNodeCheck)
                Parent.Nodes.Remove(this);

            return RemovedAnchor;
        }

    }

}
