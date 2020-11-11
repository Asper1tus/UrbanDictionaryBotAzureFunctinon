using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
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

        public List<TermDefinition> GetTermDefinitions(string term)
        {
            var url = "https://www.urbandictionary.com/define.php?term=" + term;
            var htmlDoc = GetHtmlPage(url);

            var defPanelNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'def-panel ']");

            var termDefinitions = new List<TermDefinition>();

            foreach(var node in defPanelNodes)
            {
                termDefinitions.Add(ParseToTermDefinition(node, url));
            }

            return termDefinitions;
        }

        public List<TermDefinition> GetTop10Words()
        {
            var url = "https://www.urbandictionary.com/";
            var htmlDoc = GetHtmlPage(url);

            var liNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='panel trending-words-panel']/ul/li/a").Take(10);
            var termDefinitions = new List<TermDefinition>();

            foreach (var node in liNodes)
            {
                var nodeUrl = "https://www.urbandictionary.com/" + node.Attributes["href"].Value;
                var nodeHtmlDoc = GetHtmlPage(nodeUrl);
                var defPanelNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'def-panel ']");
                termDefinitions.Add(ParseToTermDefinition(defPanelNode, nodeUrl));
            }

            return termDefinitions;
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
