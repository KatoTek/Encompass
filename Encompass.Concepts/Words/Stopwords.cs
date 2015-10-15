using System;
using System.Collections.Generic;
using System.Linq;
using Encompass.Simple.Extensions;

namespace Encompass.Concepts.Words
{
    /// <summary>
    /// Provides a list of stop words and ways to work with them. Stop words are words that are useful to filter out when performing text searches based on individual words.
    /// </summary>
    public static class Stopwords
    {
        /// <summary>
        /// Filters out words that are designated as stop words
        /// </summary>
        /// <param name="text">A string to split using the specified <paramref name="separators"/> and remove any stop words</param>
        /// <param name="separators">The separators to use to split the string. The default value is null, which uses a space as the separator when splitting the string</param>
        /// <returns>A collection of <see cref="string"/> within the <paramref name="text"/> string that are not stop words</returns>
        public static IQueryable<string> FilterOut(string text, string[] separators = null)
        {
            if (text.IsNullOrWhiteSpace())
                return new string[0].AsQueryable();

            separators = separators ?? new[] { " " };

            return FilterOut(text.Split(separators, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Filters out words that are designated as stop words
        /// </summary>
        /// <param name="terms">The collection of strings to remove stop words from</param>
        /// <returns>A collection of <see cref="string"/> within the <paramref name="terms"/> collection that are not stop words</returns>
        public static IQueryable<string> FilterOut(IEnumerable<string> terms)
        {
            return terms?.Distinct().Except(Get()).AsQueryable() ?? new string[0].AsQueryable();
        }

        /// <summary>
        /// Gets the words designated as stop words
        /// </summary>
        /// <returns>A collection of stop words</returns>
        /// <remarks>The original list of stop words was taken from <see href="http://www.ranks.nl/stopwords"/></remarks>
        public static IQueryable<string> Get()
        {
            return
                (new[]
                 {
                     "a",
                     "about",
                     "above",
                     "after",
                     "again",
                     "against",
                     "all",
                     "am",
                     "an",
                     "and",
                     "any",
                     "are",
                     "aren't",
                     "as",
                     "at",
                     "be",
                     "because",
                     "been",
                     "before",
                     "being",
                     "below",
                     "between",
                     "both",
                     "but",
                     "by",
                     "can't",
                     "cannot",
                     "could",
                     "couldn't",
                     "did",
                     "didn't",
                     "do",
                     "does",
                     "doesn't",
                     "doing",
                     "don't",
                     "down",
                     "during",
                     "each",
                     "few",
                     "for",
                     "from",
                     "further",
                     "had",
                     "hadn't",
                     "has",
                     "hasn't",
                     "have",
                     "haven't",
                     "having",
                     "he",
                     "he'd",
                     "he'll",
                     "he's",
                     "her",
                     "here",
                     "here's",
                     "hers",
                     "herself",
                     "him",
                     "himself",
                     "his",
                     "how",
                     "how's",
                     "i",
                     "i'd",
                     "i'll",
                     "i'm",
                     "i've",
                     "if",
                     "in",
                     "into",
                     "is",
                     "isn't",
                     "it",
                     "it's",
                     "its",
                     "itself",
                     "let's",
                     "me",
                     "more",
                     "most",
                     "mustn't",
                     "my",
                     "myself",
                     "no",
                     "nor",
                     "not",
                     "of",
                     "off",
                     "on",
                     "once",
                     "only",
                     "or",
                     "other",
                     "ought",
                     "our",
                     "ours",
                     "ourselves",
                     "out",
                     "over",
                     "own",
                     "same",
                     "shan't",
                     "she",
                     "she'd",
                     "she'll",
                     "she's",
                     "should",
                     "shouldn't",
                     "so",
                     "some",
                     "such",
                     "than",
                     "that",
                     "that's",
                     "the",
                     "their",
                     "theirs",
                     "them",
                     "themselves",
                     "then",
                     "there",
                     "there's",
                     "these",
                     "they",
                     "they'd",
                     "they'll",
                     "they're",
                     "they've",
                     "this",
                     "those",
                     "through",
                     "to",
                     "too",
                     "under",
                     "until",
                     "up",
                     "very",
                     "was",
                     "wasn't",
                     "we",
                     "we'd",
                     "we'll",
                     "we're",
                     "we've",
                     "were",
                     "weren't",
                     "what",
                     "what's",
                     "when",
                     "when's",
                     "where",
                     "where's",
                     "which",
                     "while",
                     "who",
                     "who's",
                     "whom",
                     "why",
                     "why's",
                     "with",
                     "won't",
                     "would",
                     "wouldn't",
                     "you",
                     "you'd",
                     "you'll",
                     "you're",
                     "you've",
                     "your",
                     "yours",
                     "yourself",
                     "yourselves"
                 }).AsQueryable();
        }
    }
}
