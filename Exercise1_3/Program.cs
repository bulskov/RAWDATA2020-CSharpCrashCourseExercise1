using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Exercise1_3
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**************************************");
            Console.WriteLine("Solution to C# crash course exercise 1");
            Console.WriteLine("**************************************");

            var lines = ReadFile(@"..\..\..\data.csv");

            var longstTitle = LongestTitle(lines);

            Console.WriteLine($"a) The longest title:\n{longstTitle}\n");

            int wordCount = 0;
            var words = ExtractWords(lines, out wordCount);

            Console.WriteLine("b) Id and number of words:");
            foreach (var line in words)
            {
                Console.WriteLine($"{line.id}: {line.words.Count}");
            }

            Console.WriteLine($"Total number of words: {wordCount}\n");

            var outputFile = "words.csv";
            WriteWordsToFile(outputFile, words);
            Console.WriteLine($"c) Words written to file {outputFile}");

        }

        public static string LongestTitle(List<(int id, string title)> lines)
        {
            string longestTitle = "";
            foreach (var line in lines)
            {
                if (longestTitle.Length < line.title.Length)
                    longestTitle = line.title;
            }
            return longestTitle;
        }

        public static List<(int, string)> ReadFile(string filename)
        {
            var result = new List<(int id, string title)>();
            try
            {
                using (var reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = Parse(reader.ReadLine());
                        if (line.HasValue)
                        {
                            result.Add(line.Value);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading file:");
                Console.WriteLine(e.Message);
            }
            return result;
        }

        public static List<(int id, List<string> words)> ExtractWords(IEnumerable<(int id, string title)> lines, out int wordCount)
        {
            var words = new List<(int, List<string>)>();
            // prepare a list of possible word delimiters, thus refine from just using spaces
            char[] delimiterChars = { ' ', ',', '.', ':', '\t', '(', ')', '-', '?', '/', '<', '>', '!', '=' };
            wordCount = 0;
            foreach (var line in lines)
            {
                var wordsInLine = line.title.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                wordCount += wordsInLine.Length;
                words.Add((line.id, new List<string>(wordsInLine)));
            }
            return words;
        }

        public static void WriteWordsToFile(string outputFile, List<(int id, List<string> words)> words)
        {
            using (var writer = new StreamWriter(outputFile))
            {
                foreach (var line in words)
                {
                    foreach (var word in line.words)
                    {
                        writer.WriteLine($"{line.id},\"{word.ToLower()}\"");
                    }
                }
            }
        }

        // using a nullable tuple - thus the ? after the tuple in the return type
        public static (int id, string title)? Parse(string line)
        {
            // specify a regular expression to match the lines
            // see https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expressions
            var rgx = new Regex("([0-9]+),\"([^\"]+)\"");
            var matches = rgx.Matches(line);
            if (matches.Count > 0)
            {
                return (int.Parse(matches[0].Groups[1].Value), matches[0].Groups[2].Value);
            }
            return null;
        }
    }
}
