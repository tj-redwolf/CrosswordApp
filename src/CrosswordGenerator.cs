using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordGenerator
{
    class CrosswordGenerator
    {
        private CrosswordBoard _myBoard;
        private MaskFabricator _maskUtil;
        private string[] _wordList;
        private char[,] _outBoard;
        private List<string> _outWordList;

        public CrosswordBoard Board { get { return this._myBoard;  } }
        public MaskFabricator MaskUtil { get { return this._maskUtil; } }
        public string[] WordList { get { return this._wordList; } }
        public List<string> WordsUsed { get { return this._outWordList; } }
        public char[,] OutputBoard { get { return _outBoard; } }


        public CrosswordGenerator(CrosswordBoard board, MaskFabricator maskUtil)
        {
            this._maskUtil = maskUtil;
            this._myBoard = board;

        }

        public void AddWordsToBoard(string[] words) {
            // generate the masks on the given lists
            this._wordList = words;
            foreach (string word in words) {
                this._maskUtil.AddAllMaskonWord(word);
            }
        }

        public void DoCrossWalk() {
            // by default starting with the first word in the list 
            if (this._wordList == null) return;

            // start the walk for generating the crossword
            for (int i = 0; i < this._myBoard.Height; i++) {
                bool isWordInserted = this._myBoard.AddWord(this._wordList[0], (short)i, 0, 0, WordDirection.HORIZONTAL);
                if (isWordInserted)
                {
                    this._outWordList = new List<string>(this._myBoard.WordsOnBoard);
                    this._outBoard = (char[,])this._myBoard.Grid.Clone();
                    this._traverse(this._wordList[0]);
                }
            }               
        }

        public List<string> GetUnusedWords() {
            if (this._wordList.Length == 0) return null;

            List<string> unused = new List<string>();

            foreach (string w in this._wordList) {
                if (this._outWordList.IndexOf(w) == -1) unused.Add(w);
            }

            return unused;
        }

        private void _traverse(string word) {
            char dir = this._myBoard.GetWordDirection(word);
            Point[] cords = this._myBoard.GetAllPointsOnWord(word);
            char crossDir = (dir == WordDirection.HORIZONTAL) ? WordDirection.VERTICAL : WordDirection.HORIZONTAL;

            // walk through all letters of the given word
            foreach (Point p in cords) {
                char letter = this._myBoard.GetLetterAt(p.Row, p.Col);
                string mask = this._maskUtil.GetMaskonGrid(this._myBoard.Grid,
                                                           this._myBoard.Width,
                                                           this._myBoard.Height,
                                                           p.Row,
                                                           p.Col,
                                                           crossDir);
                List<string> words = this._maskUtil.GetWordsOnMask(mask);

                // backtrack for all the possible words for the solution
                foreach (string w in words) {
                    int insertIndex = this._myBoard.GetInsertIndex(w, p.Row, p.Col, letter, crossDir);
                    if (insertIndex == -1 || this._myBoard.ContainsWord(w)) continue;
                    bool isWordInserted = this._myBoard.AddWord(w, p.Row, p.Col, (short)insertIndex, crossDir);
                    if (isWordInserted) {
                        // check the best solution
                        if (this._myBoard.WordsOnBoard.Count > this._outWordList.Count) {
                            this._outWordList = new List<string>(this._myBoard.WordsOnBoard);
                            this._outBoard = (char[,])this._myBoard.Grid.Clone();
                        }
                        this._traverse(w);
                        this._myBoard.RemoveWord(w);
                    }

                }
            }

        }

        
    }
}
