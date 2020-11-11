using Microsoft.VisualBasic;
using System;

namespace UrbanDictionaryBotFunction.Models
{
    class TermDefinition
    {
        public string Word { get; set; }
        public string Meaning { get; set; }
        public string Example { get; set; }
        public string Author { get; set; }
        public string AuthorUrl { get; set; }
        public string SourceUrl { get; set; }
    }
}
