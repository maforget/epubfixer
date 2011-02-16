using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ePubFixer
{
    public partial class frmMassRename : Form
    {
        #region Fields & Properties
        private List<string> _text;
        private List<string> InputText;

        public List<string> RenamedText
        {
            get { return _text; }
            set { _text = value; }
        } 
        #endregion

        #region Constructor
        public frmMassRename(List<string> InputText)
        {
            InitializeComponent();
            this.Icon = Utils.GetIcon();
            _text = new List<string>();
            this.InputText = InputText;
        } 
        #endregion

        #region Form Events
        private void frmMassRename_Activated(object sender, EventArgs e)
        {
            txtInput.TabIndex = 0;
            txtInput.Focus();
            txtInput.Text = InputText.Count > 1 ? "" : InputText[0];
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ConvertToText(txtInput.Text);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbConvertToWords_CheckedChanged(object sender, EventArgs e)
        {
            cbUpperCase.Enabled = cbConvertToWords.Checked ? true : false;
            cbRoman.Enabled = cbConvertToWords.Checked ? false : true;
        }

        private void cbRoman_CheckedChanged(object sender, EventArgs e)
        {
            cbUpperCase.Enabled = cbRoman.Checked ? false : true;
            cbConvertToWords.Enabled = cbRoman.Checked ? false : true;

        }
        #endregion

        #region Rename
        private void ConvertToText(string text)
        {
            string Prefix = String.Empty;
            string Number = String.Empty;
            string Sufix = String.Empty;
            string Prefix2 = String.Empty;
            string Sufix2 = String.Empty;

            Regex regexObj = new Regex(@"(?<Prefix>[^\d]*)(?<Number>\d*)(?<Sufix>.*$)");
            Prefix = regexObj.Match(text).Groups["Prefix"].Value;
            Number = regexObj.Match(text).Groups["Number"].Value;
            Sufix = regexObj.Match(text).Groups["Sufix"].Value;

            double num = 0;
            double.TryParse(Number, out num);

            if (num <= 0)
                Number = String.Empty;


            for (int i = 0; i < InputText.Count; i++)
            {

                string TextToKeep = Regex.Replace(text, "%T", InputText[i], RegexOptions.IgnoreCase);
                if (Prefix.ToUpper().Contains("%T") || Sufix.ToUpper().Contains("%T"))
                {
                    Prefix2 = regexObj.Match(TextToKeep).Groups["Prefix"].Value;
                    string Number2 = regexObj.Match(TextToKeep).Groups["Number"].Value;
                    Sufix2 = regexObj.Match(TextToKeep).Groups["Sufix"].Value;

                    if (!string.IsNullOrEmpty(Number2) && i == 0)
                    {
                        double.TryParse(Number2, out num);
                    }
                } else
                {
                    Prefix2 = Prefix;
                    Sufix2 = Sufix;
                }


                if (cbConvertToWords.Checked || cbRoman.Checked)
                {
                    Number = cbRoman.Checked ? NumberToWordsConverter.NumberToRoman(num)
                        : NumberToWordsConverter.NumberToWords(num);
                    Number = cbUpperCase.Checked ? Number.ToUpper() : Number;
                } else
                {
                    if (num > 0)
                        Number = num.ToString();
                }

                _text.Add(Prefix2 + Number + Sufix2);
                if (!string.IsNullOrEmpty(Number))
                {
                    num++;
                }
            }
        } 
        #endregion

    }
}
