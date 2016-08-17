using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input: Size of the grid and words for crossword
            Console.WriteLine("Enter the board size and words count:");
            string input1 = Console.ReadLine();
            int boardSize = Int32.Parse(input1.Split(' ')[0]);
            int wordsCount = Int32.Parse(input1.Split(' ')[1]);
            string[] words = new string[wordsCount];
            Console.WriteLine("Enter the words:");
            for (var c = 0; c < wordsCount; c++) {
                words[c] = Console.ReadLine();
            }

            MaskFabricator maskHandler = new MaskFabricator();
            CrosswordBoard cb = new CrosswordBoard(boardSize, boardSize);
            CrosswordGenerator crossWord = new CrosswordGenerator(cb, maskHandler);
            // add all the words into the system
            crossWord.AddWordsToBoard(words);
            // Walk the board to assign cross words
            crossWord.DoCrossWalk();


            Console.WriteLine("*********Words Not used for the crossword************");
            foreach (string w in crossWord.GetUnusedWords()) {
                Console.WriteLine(w);
            }
            Console.WriteLine("*********** Board with crosswords *******************");
            ConsoleUtil.PrintBoard(crossWord.OutputBoard, 5, 5);
            Console.ReadLine();
        }
    }
}
 