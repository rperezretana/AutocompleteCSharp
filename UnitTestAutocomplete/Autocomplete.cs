using System;
using System.Collections.Generic;
using System.Linq;
using AutoComplete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestAutocomplete
{

    [TestClass]
    public class Autocomplete
    {
        [TestMethod]
        public void TestAutoCompleteDictionaryIinitialization()
        {
            var exampleList = new List<string>() {
                "rrrrr@rrrrr.com",
                "123456789@wert.com",
                "dfgsdfg@sdasd",
                "1234567890123456789",
                "qwertyuiopasdfghjkl"
            };
            AutoCompleteTree aC = new AutoCompleteTree(exampleList);
            Assert.IsTrue(aC.Trees.ContainsKey('r'));
            Assert.IsTrue(aC.Trees.ContainsKey('1'));
            Assert.IsTrue(aC.Trees.ContainsKey('d'));
            Assert.IsFalse(aC.Trees.ContainsKey('x'));
            Assert.IsFalse(aC.Trees.ContainsKey(' '));
        }

        [TestMethod]
        public void TestAutoCompleteMinCharactersSearch()
        {
            var exampleList = new List<string>() {
                "rrrrr@rrrrr.com",
                "123456789@wert.com",
                "dfgsdfg@sdasd",
                "1234567890123456789",
                "qwertyuiopasdfghjkl"
            };
            AutoCompleteTree aC = new AutoCompleteTree(exampleList);
            var r = aC.Search("12");
            Assert.IsTrue(r.Count == 2);
            r = aC.Search("1");
            Assert.IsTrue(r.Count == 0);
        }

        [TestMethod]
        public void TestAutoCompleteIndexCreation()
        {
            var exampleList = new List<string>() {
                "123456789@wert.com"
            };
            AutoCompleteTree aC = new AutoCompleteTree(exampleList);
            Assert.IsTrue(aC.Trees.Keys.Count == 1, $"Index one failed with a count {aC.Trees.Keys.Count}");
            aC.AddLowerCaseWord("qwerty");
            Assert.IsTrue(aC.Trees.Keys.Count == 2, "Index two");
            aC.AddLowerCaseWord("qwerty2");
            Assert.IsTrue(aC.Trees.Keys.Count == 2, "Index three");
        }

        [TestMethod]
        public void TestAutoCompleteSearch()
        {
            var exampleList = new List<string>() {
                "rrrrr@rrrrr.com",
                "123456789@wert.com",
                "dfgsdfg@sdasd",
                "1234567890123456789",
                "qwertyuiopasdfghjkl"
            };
            AutoCompleteTree aC = new AutoCompleteTree(exampleList);
            var r = aC.Search("1234");
            Assert.IsTrue(r.Count == 2);
        }

        [TestMethod]
        public void TestAutoCompleteSearchCaseSensitive()
        {
            var exampleList = new List<string>() {
                "RRRrr@rrrrr.com",
                "123456789@wert.com",
                "dfgsdfg@sdasd",
                "rrrrdfg@sdasd",
                "rrrrdfy@sdasd",
                "1234567890123456789",
                "qwertyuiopasdfghjkl"
            };
            AutoCompleteTree aC = new AutoCompleteTree(exampleList);
            var r = aC.Search("rrr");
            Assert.IsTrue(r.Count == 2); //ignores the lower case
        }

        [TestMethod]
        public void TestAutoCompleteSearchCaseSensitive_2()
        {
            var exampleList = new List<string>() {
                "RRRrr@rrrrr.com",
                "123456789@wert.com",
                "dfgsdfg@sdasd",
                "rrrrdfg@sdasd",
                "rrrrdfy@sdasd",
                "1234567890123456789",
                "qwertyuiopasdfghjkl"
            };
            AutoCompleteTree aC = new AutoCompleteTree(exampleList);
            var r = aC.SearchLoweredCased("RRR"); //assumes it is lower case
            Assert.IsTrue(r.Count == 1, $"We found {r.Count} items.");
        }

        [TestMethod]
        public void TestAutoCompleteSearch_Add_ThenSearch()
        {
            var exampleList = new List<string>() {
                "rrrrr@rrrrr.com",
                "123456789@wert.com",
                "dfgsdfg@sdasd",
                "1234567890123456789",
                "qwertyuiopasdfghjkl"
            };
            AutoCompleteTree aC = new AutoCompleteTree(exampleList);
            var r = aC.Search("1234");
            Assert.IsTrue(r.Count == 2, $"Searching 1234 found {r.Count} results");
            aC.AddLowerCaseWord("enciclopedia");
            r = aC.Search("1234");
            Assert.IsTrue(r.Count == 2, $"Searching 1234 again found {r.Count} results");
            r = aC.Search("encic");
            Assert.IsTrue(r.Count == 1, $"Searching 'encic' found {r.Count} results");
        }

        [TestMethod]
        public void TestAutoCompleteSearch_SearchingTerms()
        {
            var exampleList = new List<string>() {
                "rrrrr@rrrrr.com",
                "123456789@wert.com",
                "dfgsdfg@sdasd",
                "1234567890123456789",
                "qwertyuiopasdfghjkl"
            };
            AutoCompleteTree aC = new AutoCompleteTree(exampleList);
            var enc = "enciclopedia";
            aC.AddLowerCaseWord(enc);
            var r = aC.Search("encic");
            Assert.IsTrue(r[0] == enc, $"Searching 'encic' found {r.Count} results");
            r = aC.Search("1234");
            Assert.IsTrue(r[0] == "123456789@wert.com", $"Searching 'encic' found {r.Count} results");
            Assert.IsTrue(r[1] == "1234567890123456789", $"Searching 'encic' found {r.Count} results");
        }

        [TestMethod]
        public void TestAutoCompleteSearch_HighVolumeSearch_WithSimilarRecords()
        {
            AutoCompleteTree aC = new AutoCompleteTree();

            //Add a huge number of random words
            for (int i = 0; i < 15000000; i++)
                aC.AddLowerCaseWord(System.IO.Path.GetRandomFileName().Substring(8));//Guid.NewGuid().ToString());

            var popularWord = System.IO.Path.GetRandomFileName();
            aC.AddLowerCaseWord(popularWord);
            int similarWordsCount = 250000;
            //add a big number of similar words
            for (int i = 0; i < similarWordsCount; i++)
                aC.AddLowerCaseWord(popularWord + "_" + i);

            var toSearch = popularWord;
            var expected = popularWord + "_0";
            var r = aC.Search(toSearch);
            Assert.IsTrue(r[0] == expected, $"Searching '{toSearch}' found as first '{r[0]}' and a total of {r.Count} results");
            Assert.IsTrue(r.Count == CharacterNode.MAX_WORDS_REFERENCED_PER_NODE, $"Searching '{toSearch}' found as a total of '{r.Count}' but it was expected {CharacterNode.MAX_WORDS_REFERENCED_PER_NODE} results");
            Assert.IsTrue(aC.Ocounter <= toSearch.Length, $"Search explore too many nodes ({aC.Ocounter} total)");
        }


    }
}
