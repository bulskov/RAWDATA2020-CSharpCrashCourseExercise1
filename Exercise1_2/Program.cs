using System;
using System.Collections.Generic;
using System.IO;

namespace Exercise1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**************************************");
            Console.WriteLine("Solution to C# crash course exercise 1");
            Console.WriteLine("**************************************");

            var path = @"C:\Users\bulskov\Downloads\c# crash course exercises data.csv";
            var lines = ParseLines(path);
            
            FindLongstTitle(lines);
            SplitTitleIntoWords(lines, @"..\..\..\words.csv");
            Console.WriteLine($"c) Words written to file words.csv");
        }

        // the return type here is a list of tuples. A tuple is a way to create simple types
        // You specify a tuple with ( ) and the types inside, ex (int, string) and you then get 
        // a type with two elements Item1 and Item2. If you have an instance 
        // var x = (5, "Hello");
        // you can access the elements with x.Item1 or x.Item2 (and so on for all the elements in you tuple
        // Here we further use the possibility to name the elements (int id, string title) to "id" and "title"
        // and then we can access elements by their name, eg x.id or x.title :-)
        // see https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples
        private static List<(int id, string title)> ParseLines(string path)
        {
            var result = new List<(int id, string line)>();

            // the using statement ensures that the resource, here the file, are closed
            // when the scope ends.
            using (var reader = new StreamReader(path))
            {
                string line = null;
                while((line = reader.ReadLine()) != null)
                {
                    // split the line into an array with ["id", "title"] 
                    var data = line.Split(",");

                    var id = int.Parse(data[0]); // convert the id to an integer
                    var title = data[1].Trim(); // remove trailing whitespaces
                    // remove the " first [0] and last [^1], by getting everything from position 1
                    // upto, but not including the last [^1] character.
                    title = title[1..^1];
                    result.Add((id, title));
                }
            } // end of using scope - cleanup resource usage
            return result;
        }

        private static void FindLongstTitle(List<(int id, string title)> lines)
        {
            var maxLen = 0;
            var longstTitle = "";
            foreach (var line in lines)
            {
                var len = line.title.Length;
                if (len > maxLen)
                {
                    maxLen = len;
                    longstTitle = line.title;
                }
            }

            Console.WriteLine($"a) The longst title ({maxLen}):\n{longstTitle}\n");
        }

        private static void SplitTitleIntoWords(List<(int id, string title)> lines, string fileName)
        {
            Console.WriteLine("b) Id and number of words:");
            // the default output folder is <project-folder>\bin\Debug\netcoreapp3.1
            // this "..\..\..\" will add it to the project, 
            // first .. to get back to the Debug folder
            // next .. to get back to the bin folder
            // the final .. to get back to the <project-folder>
            var writer = File.AppendText(fileName);
            foreach (var line in lines)
            {
                var words = line.title.Split(); // default split on space

                Console.WriteLine($"{line.id}: {words.Length}");

                foreach (var word in words)
                {
                    writer.WriteLine($"{line.id},\"{word}\"");
                }
            }

            // NOTE:
            // if you add the data file "c# crash course exercises data.csv" to your project folder
            // you can open it with "..\..\..\c# crash course exercises data.csv", due to the fact that
            // "<project-folder>\bin\Debug\netcoreapp3.1" is the folder where the program is executed,
            // ie the so-called "working directory" of the program
        }
    }
}
