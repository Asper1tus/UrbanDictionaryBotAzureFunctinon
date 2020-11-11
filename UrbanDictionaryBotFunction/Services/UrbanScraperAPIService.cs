using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UrbanDictionaryBotFunction.Models;

namespace UrbanDictionaryBotFunction.Services
{
    class UrbanScraperAPIService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string APIUrl = "https://www.urbandictionary.com/";

        public static async Task<TermDefinition> GetTermDefinitionAsync(string term)
        {
            TermDefinition termDefinition = null;
            HttpResponseMessage response = await client.GetAsync(APIUrl + "define.php?term=" + term);


            if (response.IsSuccessStatusCode)
            {
                termDefinition = await response.Content.ReadAsAsync<TermDefinition>();
            }

            return termDefinition;
        }

        public static async Task<List<TermDefinition>> GetTermDefinitionsAsync(string term)
        {
            List<TermDefinition> termDefinitions = null;
            HttpResponseMessage response = await client.GetAsync(APIUrl + "search/" + term);

            if (response.IsSuccessStatusCode)
            {
                termDefinitions = await response.Content.ReadAsAsync<List<TermDefinition>>();
            }

            return termDefinitions;

        }

    }
}
