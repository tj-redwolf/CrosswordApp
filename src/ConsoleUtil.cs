using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordGenerator
{
    class ConsoleUtil
    {
        /*
         * @param: output board
         * @param: board height
         * @param: board width
         */
        public static void PrintBoard(char[,] grid, short height, short width)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write("{0} ", (grid[i, j] != '\0') ? grid[i, j] : 'x');
                }
                Console.Write("\n");
            }
        }
    }
}
