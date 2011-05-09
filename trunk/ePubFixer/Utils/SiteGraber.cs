using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

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
                    SearchAddress += input.Value;
                    HtmlWeb Site = new HtmlWeb();
                    HtmlDocument Page = Site.Load(SearchAddress);
                    HtmlNode Link = Page.DocumentNode.SelectSingleNode("//div[@class='SCItemHeader']/h1/a");
                    string ret = string.Empty;

                    if (Link != null)
                    {
                        ret = Link.GetAttributeValue("href", string.Empty);
                        if (!string.IsNullOrEmpty(ret))
                        {
                            ret = site + ret;
                            ret = ret.Replace(@"/mix-", @"/book-");
                        }
                    }

                    return ret; 
                }
            } else
            {
                return string.Empty;
            }
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

        //TODO Return the name of the download book
    }
}
