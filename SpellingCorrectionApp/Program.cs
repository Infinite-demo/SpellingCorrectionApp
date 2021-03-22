using System;
using System.Collections.Generic;

namespace SpellingCorrectionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var symSpell = new SymSpell(maxDictionaryEditDistance: 3);
            //load dictionary
            string dictionaryPath = "frequency_dictionary_en_82_765.txt";
            int termIndex = 0; //column of the term in the dictionary text file
            int countIndex = 1; //column of the term frequency in the dictionary text file
            if (!symSpell.LoadDictionary(dictionaryPath, termIndex, countIndex))
            {
                Console.WriteLine("File not found!");
                //press any key to exit program
                Console.ReadKey();
                return;
            }

            string input;
            //leplativ
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nType a word and hit enter key to get spelling suggestions:");
            Console.Write("> ");
            //lookup suggestions for single-word input strings
            while (!string.IsNullOrEmpty(input = (Console.ReadLine() ?? "").Trim()))
            {
                List<SymSpell.SuggestItem> suggestions;
                const SymSpell.Verbosity verbosity = SymSpell.Verbosity.Closest;
                suggestions = symSpell.Lookup(input, verbosity);
                foreach (var suggestion in suggestions)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(suggestion.term + " " + suggestion.distance.ToString());
                    //Console.ResetColor();

                }
                if (verbosity != SymSpell.Verbosity.Top) Console.WriteLine(suggestions.Count.ToString() + " suggestions");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n> ");
            }

            dictionaryPath = "frequency_bigramdictionary_en_243_342.txt";
            termIndex = 0; //column of the term in the dictionary text file
            countIndex = 2; //column of the term frequency in the dictionary text file
            if (!symSpell.LoadBigramDictionary(dictionaryPath, termIndex, countIndex))
            {
                Console.WriteLine("File not found!");
                //press any key to exit program
                Console.ReadKey();
                return;
            }

            //whereis th elove hehad dated forImuch of thepast who couqdn'tread in sixtgrade and ins pired him
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nType multi-word mixed without space in a sentence and hit enter key to get correct possible sentence:");
            Console.Write("> ");
            // lookup suggestions for multi-word input strings (supports compound splitting & merging)
            while (!string.IsNullOrEmpty(input = (Console.ReadLine() ?? "").Trim()))
            {
                var suggestions2 = symSpell.LookupCompound(input);
                //display suggestions, edit distance and term frequency
                foreach (var suggestion in suggestions2)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(suggestion.term + " " + suggestion.distance.ToString());
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n> ");
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nType sentence without space and hit enter key to get correct possible sentence:");
            //thequickbrownfoxjumpsoverthelazydog
            //whereisthelovehehaddatedformuchofthepastwhocouqdn'treadinsixtgradeandinspiredhim
            Console.Write("> ");
            //word segmentation and correction for multi-word input strings with/without spaces
            while (!string.IsNullOrEmpty(input = (Console.ReadLine() ?? "").Trim()))
            {
                if (input.Contains(" "))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Please enter again without space");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("\n> ");
                    continue;
                }
                var suggestion1 = symSpell.WordSegmentation(input);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(suggestion1.correctedString + " " + suggestion1.distanceSum.ToString("N0"));
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n> ");
            }
            Console.ReadKey();
        }
    }
}
