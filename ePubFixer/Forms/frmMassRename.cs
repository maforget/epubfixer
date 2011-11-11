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
            txtInput.SelectAll();
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
        private void ConvertToText(string InText)
        {
            List<string> TextList = ReplaceSpecialTag(InText);
            List<ParsedText> Converted = ConvertToWords(TextList);
            ConvertCase(Converted);

        }

        private List<string> ReplaceSpecialTag(string InText)
        {
            List<string> text = new List<string>();

            for (int i = 0; i < InputText.Count; i++)
            {
                text.Add(Regex.Replace(InText, "%T", InputText[i], RegexOptions.IgnoreCase));
                //All lines should be the same SOme TExt - 1 
                //for example if I had put %t - 1 where Sometext was the original text
            }

            return text;
        }

        private List<ParsedText> ConvertToWords(List<string> TextToParse)
        {
            List<ParsedText> Parsed = ParseText(TextToParse);

            for (int i = 0; i < Parsed.Count; i++)
            {
                ParsedText p = Parsed[i];

                if (p.num > 0)
                {
                    if (i > 0 && p.OrigiNum == Parsed[i - 1].OrigiNum)
                    {
                        //The original number are all the same we should increment them
                        p.num = Parsed[i - 1].num + 1;
                        p.Number = p.num.ToString();
                    }

                    if (cbConvertToWords.Checked || cbRoman.Checked)
                    {
                        p.Number = cbRoman.Checked ? NumberToWordsConverter.NumberToRoman(p.num)
                    : NumberToWordsConverter.NumberToWords(p.num);
                        //Number = cbUpperCase.Checked ? Number.ToUpper() : Number;  
                    }
                }

                p.FinalText = p.Prefix + p.Number + p.Sufix;
            }

            return Parsed;
        }

        private List<ParsedText> ParseText(List<string> TextToParse)
        {
            List<ParsedText> Parsed = new List<ParsedText>();

            for (int i = 0; i < TextToParse.Count; i++)
            {
                string InText = TextToParse[i];
                ParsedText p = new ParsedText();

                Regex regexObj = new Regex(@"(?<Prefix>[^\d]*)(?<Number>\d*)(?<Sufix>.*$)");
                p.Prefix = regexObj.Match(InText).Groups["Prefix"].Value;
                p.Number = regexObj.Match(InText).Groups["Number"].Value;
                p.Sufix = regexObj.Match(InText).Groups["Sufix"].Value;

                int num = 0;
                int.TryParse(p.Number, out num);

                p.num = num;
                p.OrigiNum = num;

                Parsed.Add(p);
            }

            return Parsed;
        }

        private void ConvertCase(List<ParsedText> text)
        {
            for (int i = 0; i < text.Count; i++)
            {
                string item = text[i].FinalText;
                CultureInfo cultureInfo = CultureInfo.InvariantCulture;
                TextInfo textInfo = cultureInfo.TextInfo;

                string result = cbUpperCase.Checked ? textInfo.ToUpper(item) :
                    cbTitleCase.Checked ? textInfo.ToTitleCase(item.ToLower()) : item;

                UpperCaseRomanNumeral(item, ref result);

                _text.Add(result);
            }
        }

        private static void UpperCaseRomanNumeral(string item, ref string result)
        {
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            List<string> resultList = new List<string>();
            Regex regexRoman = new Regex(@"(\b(M{0,4}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})|[IDCXMLV])\b)");
            Match matchResult = regexRoman.Match(item);
            while (matchResult.Success)
            {
                resultList.Add(matchResult.Value);
                matchResult = matchResult.NextMatch();
            }
            resultList = resultList.Where(x => !string.IsNullOrEmpty(x)).ToList();

            foreach (string Roman in resultList)
            {
                if (!string.IsNullOrEmpty(Roman))
                {
                    string RomanResult = textInfo.ToTitleCase(Roman.ToLower());
                    result = result.Replace(RomanResult, Roman);
                }
            }
        }
        #endregion

        #region Show Help
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
        #endregion

        //TODO Bug when rename %t and covert to case all, first chapter is turned to number 2 (With Upper Level to Part 1 for ex:)

        //TODO remove text tool
    }


    class ParsedText
    {
        public string Prefix { get; set; }
        public string Number { get; set; }
        public string Sufix { get; set; }
        public int num { get; set; }
        public int OrigiNum { get; set; }
        public string FinalText { get; set; }
    }
}
