using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public class AutoCompleteTree
    {
        public AutoCompleteTree()
        {
            Trees = new Dictionary<char, CharacterNode>();
            WordList = new HashSet<string>();
        }
        public AutoCompleteTree(List<string> list)
        {
            Trees = new Dictionary<char, CharacterNode>();
            WordList = new HashSet<string>();

            //maybe used only for testing but may be better 
            //if initialized when app started.
            for (int i = 0; i < list.Count(); i++)
                AddLowerCaseWord(list[i]);
        }
        public Dictionary<char, CharacterNode> Trees { get; set; }
        /// <summary>
        /// This is for duplicate checks porpuses.
        /// </summary>
        public HashSet<string> WordList { get; set; }


        /// <summary>
        /// This adds a word to the tree. This method is slower since it will try to lowercase the word
        /// Assumptions:
        /// - The word may contain uppercase characters.
        /// - English language lower/uppercase letters (special letters are not considered, ei: ñ )
        /// - The word to be added contains more than 2 characters.
        /// </summary>
        /// <param name="word"></param>
        public void AddWord(string word)
        {
            var original = word;
            word = word.ToLower();
            if (WordList.Contains(word)) //prevent duplicated terms/words
                return;

            var navigator = Trees;
            //assume text is lowercase.
            //assume: email is longer than 2 charcters.
            for (int i = 0; i < word.Length - 1; i++)
            {
                if (!navigator.ContainsKey(word[i]))
                    navigator[word[i]] = new CharacterNode(original, i);

                navigator[word[i]].AddWordReference(original);
                WordList.Add(word);
                navigator = navigator[word[i]].Childs;
            }
        }

        /// <summary>
        /// This adds a word to the tree.
        /// Assumptions:
        /// - The word to be added is lowercased.
        /// - The word to be added contains more than 2 characters.
        /// </summary>
        /// <param name="word"></param>
        public void AddLowerCaseWord(string word)
        {
            // IMPORTAN ASSUMPTION: all the words being entered in the tree are lower case.
            // Examples like "apple" and "Apple" would pass as non duplicate words.
            if (WordList.Contains(word)) //prevent duplicated terms/words
                return;

            var navigator = Trees;
            //it stops at Length-1 because the last character was already added.
            for (int i = 0; i < word.Length - 1; i++)
            {
                if (!navigator.ContainsKey(word[i]))
                    navigator[word[i]] = new CharacterNode(word, i);

                navigator[word[i]].AddWordReference(word);
                WordList.Add(word);
                navigator = navigator[word[i]].Childs;
            }
        }

        /// <summary>
        /// Counter of nodes explored during the search
        /// Important assumption:
        /// - This function is slower since assumes that the word may contain upper case characters.
        /// </summary>
        public int Ocounter = 0;
        public List<string> Search(string toFind)
        {
            Ocounter = 0;
            if (toFind.Length < 2) //do not search if not more than # chars.
                return new List<string>();

            var navigator = Trees;
            List<string> result = new List<string>();
            toFind = toFind.ToLower();
            for (int i = 0; i < toFind.Length; i++)
            {
                Ocounter++;
                var kToFind = toFind[i];

                if (navigator.ContainsKey(kToFind)
                    &&
                    navigator[kToFind].WordsReferences.Any())
                {
                    result = navigator[kToFind].WordsReferences;
                    navigator = navigator[kToFind].Childs;
                }
                else
                    break;
            }

            return result;
        }


        /// <summary>
        /// Counter of nodes explored during the search
        /// Important assumption:
        /// - The word was lower cased in client side and the toFind is completely lowercase.
        /// </summary>
        public List<string> SearchLoweredCased(string toFind)
        {
            Ocounter = 0;
            if (toFind.Length < 2) //do not search if not more than # chars.
                return new List<string>();

            var navigator = Trees;
            List<string> result = new List<string>();

            int i = 0;
            for (; i < toFind.Length; i++)
            {
                if (navigator.ContainsKey(toFind[i])
                    &&
                    navigator[toFind[i]].WordsReferences.Any())
                {
                    result = navigator[toFind[i]].WordsReferences;
                    navigator = navigator[toFind[i]].Childs;
                    Ocounter++;
                }
                else
                    break;
            }

            return result;
        }
    }
}
