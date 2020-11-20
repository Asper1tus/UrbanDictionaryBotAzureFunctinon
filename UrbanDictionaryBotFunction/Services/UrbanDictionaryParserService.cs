using HtmlAgilityPack;
using System.Net;
using UrbanDictionaryBotFunction.Models;

namespace UrbanDictionaryBotFunction.Services
{
    class UrbanDictionaryParserService
    {
        public TermDefinition GetTermDefinition(string term)
        {
            var url = "https://www.urbandictionary.com/define.php?term=" + term;
            var htmlDoc = GetHtmlPage(url);

            var defHeaderNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'def-panel ']");

            return ParseToTermDefinition(defHeaderNode, url);
        }

        private HtmlDocument GetHtmlPage(string url)
        {
            var web = new HtmlWeb();
            var htmlDoc = web.Load(url);

            return htmlDoc;
        }

        private TermDefinition ParseToTermDefinition(HtmlNode defHeaderNode, string url)
        {
            TermDefinition termDefinition = null;

            if (defHeaderNode == null)
                return termDefinition;

            var word = WebUtility.HtmlDecode(defHeaderNode.SelectSingleNode("//div[@class = 'def-header']/a[@class = 'word']").InnerText);
            var meaning = WebUtility.HtmlDecode(defHeaderNode.SelectSingleNode("//div[@class = 'meaning']").InnerText);
            var example = WebUtility.HtmlDecode(defHeaderNode.SelectSingleNode("//div[@class = 'example']").InnerText);
            var author = WebUtility.HtmlDecode(defHeaderNode.SelectSingleNode("//div[@class = 'contributor']/a").InnerText);
            var authorUrl = "https://www.urbandictionary.com" + WebUtility.HtmlDecode(defHeaderNode.SelectSingleNode("//div[@class = 'contributor']/a").Attributes["href"].Value);

            termDefinition = new TermDefinition() { Word = word, Meaning = meaning, Example = example, Author = author, AuthorUrl = authorUrl, SourceUrl = url };

            return termDefinition;
        }
    }
}
