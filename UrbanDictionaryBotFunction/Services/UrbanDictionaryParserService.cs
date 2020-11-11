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

            return ParseToTermDefinition(defHeaderNode);
        }

        public List<TermDefinition> GetTermDefinitions(string term)
        {
            var url = "https://www.urbandictionary.com/define.php?term=" + term;
            var htmlDoc = GetHtmlPage(url);

            var defPanelNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'def-panel ']");

            var termDefinitions = new List<TermDefinition>();

            foreach(var node in defPanelNodes)
            {
                termDefinitions.Add(ParseToTermDefinition(node));
            }

            return termDefinitions;
        }

        public List<TermDefinition> GetTop10Words()
        {
            var url = "https://www.urbandictionary.com/";
            var htmlDoc = GetHtmlPage(url);

            var liNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='panel trending-words-panel']/ul/li").Take(10);

            var termDefinitions = new List<TermDefinition>();

            foreach (var node in liNodes)
            {
                termDefinitions.Add(ParseToTermDefinition(node));
            }

            return termDefinitions;
        }

        private HtmlDocument GetHtmlPage(string url)
        {
            var web = new HtmlWeb();
            var htmlDoc = web.Load(url);

            return htmlDoc;
        }

        private TermDefinition ParseToTermDefinition(HtmlNode defHeaderNode)
        {
            TermDefinition termDefinition = null;

            if (defHeaderNode == null)
                return termDefinition;

            var word = WebUtility.HtmlDecode(defHeaderNode.SelectSingleNode("//div[@class = 'def-header']/a[@class = 'word']").InnerText);
            var meaning = WebUtility.HtmlDecode(defHeaderNode.SelectSingleNode("//div[@class = 'meaning']").InnerText);
            var example = WebUtility.HtmlDecode(defHeaderNode.SelectSingleNode("//div[@class = 'example']").InnerText);
            var author = WebUtility.HtmlDecode(defHeaderNode.SelectSingleNode("//div[@class = 'contributor']/a").InnerText);

            termDefinition = new TermDefinition() { Word = word, Meaning = meaning, Example = example, Author = author };

            return termDefinition;
        }
    }
}
