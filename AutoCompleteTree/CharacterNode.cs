using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public class CharacterNode
    {
        /// <summary>
        /// If this is intended for a regular auto complete (with a limited number of options displayed),
        /// There is no need to append every word to each node,
        /// The algorithm can accumulate the first 10, but then as the user adds more letters the sugestions will get closer.
        /// </summary>
        public const int MAX_WORDS_REFERENCED_PER_NODE = 20;

        private int WordCounter = 0;
        public CharacterNode(char parent)
        {
            WordsReferences = new List<string>();
            Childs = new Dictionary<char, CharacterNode>();
            Value = parent;
        }

        /// <summary>
        /// This assumes the index is not the last character.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="index"></param>
        public CharacterNode(string word, int index)
        {
            WordsReferences = new List<string>();
            Childs = new Dictionary<char, CharacterNode>();
            Value = word[index];
            Childs[word[index + 1]] = new CharacterNode(word[index + 1]);
        }
        public char Value { get; set; }

        public void AddWordReference(string toAdd)
        {
            if (WordCounter++ < MAX_WORDS_REFERENCED_PER_NODE)
                WordsReferences.Add(toAdd);
        }
        public List<string> WordsReferences { get; set; }
        public Dictionary<char, CharacterNode> Childs { get; set; }
    }
}
