using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Web;

namespace ePubFixer
{
    public class SiteGraber
    {
        static string site = @"http://www.kobobooks.com";
        public string SearchAddress = site + @"/search/search.html?q=";
        public string Address;
        List<string> TOC;

        public SiteGraber()
        {
            TOC = new List<string>();

            //Find URL for Book
            Address = FindBook();
        }

        private string FindBook()
        {
            InputBox input = new InputBox();
            //Put the default name in the box
            if (input.ShowDialog("Search Kobobooks for TOC","Please enter the Title and Author of the book to search for.") == System.Windows.Forms.DialogResult.OK)
            {
                using (new HourGlass())
                {
                    SearchAddress += HttpUtility.UrlEncode(input.Value);
                    HtmlWeb Site = new HtmlWeb();
                    HtmlDocument Page = Site.Load(SearchAddress);
                    HtmlNode Link = Page.DocumentNode.SelectSingleNode("//div[@class='SCItemHeader']/h1/a");
                    HtmlNode Detail = Page.DocumentNode.SelectSingleNode("//div[@class='SCItemSummary']/span");
                    string ret = string.Empty;
                    string Title = string.Empty;
                    string Author = string.Empty;

                    if (Link != null)
                    {
                        Author = GetAuthors(Detail);
                        Title = Link.InnerText.Trim();
                        ret = Link.GetAttributeValue("href", string.Empty);
                        if (!string.IsNullOrEmpty(ret))
                        {
                            ret = site + ret;
                            ret = ret.Replace(@"/mix-", @"/book-");
                        }
                    }

                    if (string.IsNullOrEmpty(Title))
                    {
                        System.Windows.Forms.MessageBox.Show("No Book Found","Error",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
                    } else
                    {
                        System.Windows.Forms.DialogResult BookIsOk = System.Windows.Forms.MessageBox.Show("Using Book\n" + Title + "\nby " + Author+"\n\nIs this ok?", "Book Found", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information);

                        if (BookIsOk==System.Windows.Forms.DialogResult.Cancel)
                        {
                            ret = string.Empty;
                        }
                    }

                    return ret; 
                }
            } else
            {
                return string.Empty;
            }
        }

        private string GetAuthors(HtmlNode Detail)
        {
            StringBuilder sb = new StringBuilder();
            List<string> list = new List<string>();

            List<string> Unique = (from d in Detail.Descendants("a")
                                   group d by d.InnerText into g
                                   select g.Key).ToList();


            foreach (string item in Unique)
            {
                if (list.Count >= 1)
                {
                    list.Add("& ");
                } 

                list.Add(item+" ");
            }

            list.ForEach(x => sb.Append(x));

            return sb.ToString().Trim();
        }

        public List<string> Parse()
        {
            using (new HourGlass())
            {
                if (!string.IsNullOrEmpty(Address))
                {
                    Parse(Address);
                    return TOC;
                }

                return null; 
            }
        }

        public void Parse(string address)
        {
            HtmlWeb Site = new HtmlWeb();
            HtmlDocument Page = Site.Load(address);
            HtmlNodeCollection PageContent = Page.DocumentNode.SelectNodes("//span[@class='SCShortCoverTitle']");

            TOC.AddRange(PageContent.Select(x => x.InnerText));
            string next = CheckForNextPage(Page, address);
            if (!string.IsNullOrEmpty(next))
                Parse(next);
        }

        private string CheckForNextPage(HtmlDocument Page, string address)
        {
            HtmlNode nbrPages = Page.DocumentNode.SelectSingleNode("//ul[@class='SCPaging FieldPager']");
            int TotalPages = 0;
            int CurrentPage = 0;

            if (nbrPages != null)
            {
                int.TryParse(nbrPages.GetAttributeValue("NumPages", "1"), out TotalPages);
                int.TryParse(nbrPages.GetAttributeValue("CurrentPage", "1"), out CurrentPage);

                if (TotalPages > 0 && CurrentPage > 0)
                {
                    if (CurrentPage < TotalPages)
                    {
                        string nextPage = "page" + (CurrentPage + 1).ToString() + ".html";
                        string currentPage = "page" + CurrentPage.ToString() + ".html";

                        return address.Replace(currentPage, nextPage);

                    } else
                    {
                        return string.Empty;
                    }
                } else
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }
    }
}
