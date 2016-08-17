using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrosswordGenerator
{
    class CrosswordBoard : GameBoard
    {
        
        private Dictionary<string, short> _wordsOnBoard;
        private Dictionary<string, char> _wordDirection;
        private Dictionary<string, Point> _wordToPinchPoint;
        private short[,,] _mapCellTOWordId;
        private short _width;
        private short _height;

        public short Width {
            get { return this._width;  }
        }

        public short Height
        {
            get { return this._height; }
        }

        public CrosswordBoard(int h, int w) : base(w, h)
        {
            // check for the width and height
            this._width = (short)w;
            this._height = (short)h;
            this._wordDirection = new Dictionary<string, char>();
            this._wordsOnBoard = new Dictionary<string, short>();
            this._wordToPinchPoint = new Dictionary<string, Point>();
            this._mapCellTOWordId = new short[h, w, 2];

        }

        public List<string> WordsOnBoard {
            get { return this._wordsOnBoard.Keys.ToList<string>();  }
        }

        public short GetWordId(string word) {
            if (this._wordsOnBoard.ContainsKey(word)) return (short)this._wordsOnBoard[word];
            else return 0;
        }

        public char GetWordDirection(string word) {
            if (!this.ContainsWord(word)) { } // throw exception
            return this._wordDirection[word];
        }

        public short GetWordIdOnCell(short row, short col, char dir) {
            if (row < 0 || row >= this._height || col < 0 || col >= this._width)
            {
                // throw exception
            }
            if (dir == WordDirection.HORIZONTAL)
            {
                return this._mapCellTOWordId[row, col, 0];
            }
            else if (dir == WordDirection.VERTICAL)
            {
                return this._mapCellTOWordId[row, col, 1];
            }
            else
                return 0;

        }

        public Point[] GetAllPointsOnWord(string word) {

            if (!this.ContainsWord(word)) {
                return null;
            }

            short wordId = this._wordsOnBoard[word];
            Point start = this._wordToPinchPoint[word];
            char direction = this._wordDirection[word];

            Point[] result = new Point[word.Length];
            for (short c = 0; c < word.Length; c++)
            {
                if (direction == WordDirection.HORIZONTAL)
                {
                    result[c] = new Point(start.Row, (short)(start.Col + c));
                }
                else {
                    result[c] = new Point((short)(start.Row + c), start.Col);
                }
            }
            return result;
        }

        public bool AddWord(string word, short row, short col, short index, char direction) {
            if (row < 0 || row >= this._height || col < 0 || col >= this._width)
            {
                // throw exception
            }
            if (direction == WordDirection.VERTICAL) row = (short)(row - index);
            else col = (short)(col - index);

            // check the bounds, throw exceptions
            if (direction == WordDirection.HORIZONTAL)
            {
                if (col < 0 || (col + word.Length) > this._height) {
                    return false;
                }
            }
            else {
                if (row < 0 || (row + word.Length) > this._width) {
                    return false;
                }
            }
            if (this.ContainsWord(word)) return false;

            // generate the wordId
            short wordId = (short)word.GetHashCode();

            // all operations should be in try/ catch block
            // update the 
            this._wordDirection.Add(word, direction);
            this._wordsOnBoard.Add(word, wordId);
            this._wordToPinchPoint.Add(word, new Point(row, col));
            
            
            // add the wordId to the map
            // add the letters to the board
            if (direction == WordDirection.VERTICAL)
            {
                this.AddWordVertical(col, row, row + word.Length - 1, word);
                for (short c = 0; c < word.Length; c++)
                {
                    this._mapCellTOWordId[row + c, col, 1] = wordId;
                }

            }
            else
            {
                this.AddWordHorizontal(row, col, col + word.Length - 1, word);
                for (short c = 0; c < word.Length; c++)
                {
                    this._mapCellTOWordId[row, col + c, 0] = wordId;
                }
            }

            return true;
        }
        public bool RemoveWord(string word) {
            if (!this.ContainsWord(word))
            {
                return false;
            }

            short wordId = this._wordsOnBoard[word];
            Point start = this._wordToPinchPoint[word];
            char direction = this._wordDirection[word];
            string deleteWordPath = "";
            // update the wordId grid
            for (short c = 0; c < word.Length; c++)
            {
                if (direction == WordDirection.HORIZONTAL)
                {
                    this._mapCellTOWordId[start.Row, start.Col + c, 0] = 0;
                    if (this._mapCellTOWordId[start.Row, start.Col + c, 1] != 0) deleteWordPath += "x";
                    else deleteWordPath += char.ToUpper(word[c]);
                }
                else
                {
                    this._mapCellTOWordId[start.Row +c, start.Col, 1] = 0;
                    if (this._mapCellTOWordId[start.Row + c, start.Col, 0] != 0) deleteWordPath += "x";
                    else deleteWordPath += char.ToUpper(word[c]);
                }
            }

            // remove the word from the board
            if (direction == WordDirection.HORIZONTAL)
                this.RemoveWordHorizontal(deleteWordPath, start.Row, start.Col, start.Col + word.Length - 1);
            else
                this.RemoveWordVertical(deleteWordPath, start.Col, start.Row, start.Row + word.Length - 1);

            // clear the objects
            this._wordDirection.Remove(word);
            this._wordsOnBoard.Remove(word);
            this._wordToPinchPoint.Remove(word);

            return true;
        }
        public short GetInsertIndex(string word, short row, short col, char letter, char direction) {

           
            foreach (Match match in Regex.Matches(word, letter.ToString()))
            {
                // check with the index if 
                int index = match.Index;
                int right = word.Length - index - 1;

                if (direction == WordDirection.HORIZONTAL)
                {
                     if ((col - index) >= 0 && (col + right) < this._width) return (short)index; 
                }
                else {
                    if ((row - index) >= 0 && (row + right) < this._height) return (short)index;
                }
            }
            return -1;
        }

        public char GetLetterAt(short row, short col) {
            if (row < 0 || row >= this._height || col < 0 || col >= this._width) {
                // throw exception
            }
            return this.Grid[row, col];
        }


        public bool ContainsWord(string word) {
            if (word == null) return false;
            else return this._wordsOnBoard.ContainsKey(word);
        }

    }
}
