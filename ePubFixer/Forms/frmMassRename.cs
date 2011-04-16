using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;

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
            //cbUpperCase.Enabled = cbConvertToWords.Checked ? true : false;
            cbRoman.Enabled = cbConvertToWords.Checked ? false : true;
        }

        private void cbRoman_CheckedChanged(object sender, EventArgs e)
        {
            //cbUpperCase.Enabled = cbRoman.Checked ? false : true;
            cbConvertToWords.Enabled = cbRoman.Checked ? false : true;

        }

        private void cbTitleCase_CheckedChanged(object sender, EventArgs e)
        {
            cbUpperCase.Enabled = !cbTitleCase.Checked;
            cbUpperCase.Checked = false;
        }

        private void cbUpperCase_CheckedChanged(object sender, EventArgs e)
        {
            cbTitleCase.Enabled = !cbUpperCase.Checked;
            cbTitleCase.Checked = false;
        }
        #endregion

        #region Rename
        private void ConvertToText(string text)
        {
            string Prefix = String.Empty;
            string Number = String.Empty;
            string Sufix = String.Empty;
            string Prefix2 = String.Empty;
            string Number2 = String.Empty;
            string Sufix2 = String.Empty;

            Regex regexObj = new Regex(@"(?<Prefix>[^\d]*)(?<Number>\d*)(?<Sufix>.*$)");
            Prefix = regexObj.Match(text).Groups["Prefix"].Value;
            Number = regexObj.Match(text).Groups["Number"].Value;
            Sufix = regexObj.Match(text).Groups["Sufix"].Value;

            double num = 0;
            double.TryParse(Number, out num);

            if (num <= 0)
                Number = String.Empty;

            bool FirstNumberFound = false;
            for (int i = 0; i < InputText.Count; i++)
            {

                string TextToKeep = Regex.Replace(text, "%T", InputText[i], RegexOptions.IgnoreCase);
                if (Prefix.ToUpper().Contains("%T") || Sufix.ToUpper().Contains("%T"))
                {
                    Prefix2 = regexObj.Match(TextToKeep).Groups["Prefix"].Value;
                    Number2 = regexObj.Match(TextToKeep).Groups["Number"].Value;
                    Sufix2 = regexObj.Match(TextToKeep).Groups["Sufix"].Value;

                    if (!string.IsNullOrEmpty(Number2) && !FirstNumberFound)
                    {
                        double.TryParse(Number2, out num);
                        FirstNumberFound = true;
                    } else if (string.IsNullOrEmpty(Number2) && num > 0)
                    {
                        num = 0;
                        FirstNumberFound = false;
                    }
                } else
                {
                    Prefix2 = Prefix;
                    Sufix2 = Sufix;
                }

                if (num > 0)
                {
                    if (cbConvertToWords.Checked || cbRoman.Checked)
                    {
                        Number = cbRoman.Checked ? NumberToWordsConverter.NumberToRoman(num)
                    : NumberToWordsConverter.NumberToWords(num);
                        //Number = cbUpperCase.Checked ? Number.ToUpper() : Number;  
                    } else
                    {
                        if (Number != string.Empty)
                        {
                            Number = num.ToString();
                        } else if (Number2 != string.Empty)
                        {
                            Number = Number2;
                        }
                    }
                }

                _text.Add(Prefix2 + Number + Sufix2);
                if (!string.IsNullOrEmpty(Number))
                {
                    num++;
                }

                Number = string.Empty;
            }

            _text = ConvertCase(_text);
        }

        private List<string> ConvertCase(List<string> text)
        {
            for (int i = 0; i < text.Count; i++)
            {
                string item = text[i];
                CultureInfo cultureInfo = CultureInfo.InvariantCulture;
                TextInfo textInfo = cultureInfo.TextInfo;

                text[i] = cbUpperCase.Checked ? textInfo.ToUpper(item) :
                    cbTitleCase.Checked ? textInfo.ToTitleCase(item.ToLower()) : item;
            }

            return text;
        }
        #endregion

        void ShowHelp()
        {
            string message =
@"Enter the name for the top item selected. 
Any number will be automatically incremented. 
You can also check the number to Words box to 
Convert the number to Words or Roman Numerals 
(i.e. Chapter 1 =  Chapter One).
            
You can also use the special tag ""%T"",
that will keep the previous text. 
(ex : You have a Chapter Named : Atlantis, 
Typing 1 - %T will change the name to 1 - Atlantis). 
Also if you have a Chapter 1 entry you can just type %T
and Number To Words and it will change it to Chapter One.";

            //MessageBox.Show(message);
            tipHelp.Show(message, LabelHelp);


        }

        private void btnShowHelp_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }


        //TODO remove text tool
    }
}
