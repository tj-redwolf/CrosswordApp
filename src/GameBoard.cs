using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordGenerator
{
    class GameBoard
    {
       
        private int _width;
        private int _height;
        private char[,] _grid;

        public char[,] Grid {
            get {
                return this._grid;
            }
        }

        public GameBoard(int w, int h)
        {
            this._grid = new char[w,h];
            this._width = w;
            this._height = h;
        }

        public bool AddWordHorizontal(int row, int start, int end, string word) {
            if (start < 0 || end >= _width) return false;
            if (row < 0 || row >= _height) return false;
            if (word.Length < end - start + 1) return false;
            for (int i = 0; i + start <= end; i++) {
                this._grid[row, i + start] = word[i];
            }
            return true;
        }

        public bool AddWordVertical(int col, int start, int end, string word)
        {
            if (start < 0 || end >= _height) return false;
            if (col < 0 || col >= _width) return false;
            if (word.Length < end - start + 1) return false;
            for (int i = 0; i + start <= end; i++)
            {
                this._grid[i + start, col] = word[i];
            }
            return true;

        }

        public bool RemoveWordHorizontal(string deletePath, int row, int start, int end) {
            if (start < 0 || end >= _width) return false;
            if (row < 0 || row >= _height) return false;
  
            for (int i = 0; i + start <= end; i++)
            {
                if(deletePath[i] != 'x')
                    this._grid[row, i + start] = '\0';
            }
            return true;
        }

        public bool RemoveWordVertical(string deletePath, int col, int start, int end) {
            if (start < 0 || end >= _height) return false;
            if (col < 0 || col >= _width) return false;
            for (int i = 0; i + start <= end; i++)
            {
                if (deletePath[i] != 'x')
                    this._grid[i + start, col] = '\0';
            }
            return true;
        }
    }
}
