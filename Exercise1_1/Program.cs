using System;
using System.Collections.Generic;
using System.IO;

namespace Exercise1_1
{
    class LineWords
    {
        public int Id { get; set; }
        public List<string> Words { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**************************************");
            Console.WriteLine("Solution to C# crash course exercise 1");
            Console.WriteLine("**************************************");

            var lines = File.ReadAllLines(@"C:\Users\bulskov\Downloads\c# crash course exercises data.csv");
            var maxLen = 0;
            var longstTitle = "";
            foreach (var line in lines)
            {
                var data = line.Split(",");
                var len = data[1].Length;
                if (len > maxLen)
                {
                    maxLen = len;
                    longstTitle = data[1];
                }
            }

            Console.WriteLine($"a) The longst title ({maxLen}):\n{longstTitle}\n");

            var wordList = new List<LineWords>();

            Console.WriteLine("b) Id and number of words:");
            foreach (var line in lines)
            {
                // split the line into an array with ["id", "title"] 
                var data = line.Split(",");
                
                var id = int.Parse(data[0]); // convert the id to an integer
                var title = data[1].Trim(); // remove trailing whitespaces
                // remove the " first [0] and last [^1], by getting everything from position 1
                // upto, but not including the last [^1] character.
                title = title[1..^1];

                var words = title.Split(); // default split on space

                Console.WriteLine($"{id}: {words.Length}");

                wordList.Add(new LineWords { Id = id, Words = new List<string>(words)});
            }

            var writer = File.AppendText("words.csv");
            foreach (var element in wordList)
            {
                foreach (var word in element.Words)
                {
                    writer.WriteLine($"{element.Id},\"{word}\"");
                }
            }
            Console.WriteLine($"c) Words written to file words.csv");
        }
    }
}
