# Fast Autocomplete C# for Jquery

Autocomplete made on C#, when set up on a search field it allows the user to search inbetween millions of words and search for autocomplete options based on the input, it is very fast, it has a time complexity of O(n) where n is the length of the word to be autocompleted.

It has to be preloaded first with the list of terms.

The unit test contains an example where it search words in a list of +15,000,000 unordered elements.
It also contains a site example with 250,000 names where it takes a few milliseconds seconds to search the name.

The returned autocomplete list is limited to 20 elements but it can be configured to more or less.
