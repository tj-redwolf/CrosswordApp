using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordGenerator
{
    class MaskFabricator
    {
 
        private Dictionary<string, List<string>> _maskToWords;

        public MaskFabricator()
        {
            _maskToWords = new Dictionary<string, List<string>>();
        }

        public List<string> Masks {
            get {
                return this._maskToWords.Keys.ToList<string>();
            }
        }

        public int AddAllMaskonWord(string word) {
            string inData = word;
            List<string> outMasks = new List<string>();
            // Generate all the masks on Word
            int len = (int)Math.Pow(2, inData.Length);
            for (int i = 1; i < len - 1; i++)
            {
                string r = inData.ToUpper();
                int d = i;
                int index = inData.Length - 1;
                while (d != 0)
                {
                    if ((d & 1) == 1)
                        r = StringUtil.ReplaceAt(r, index, 'x');
                    --index;
                    d = d >> 1;
                }
                outMasks.Add(r);
            }

            // update the masktoWord dataset;
            if (outMasks.Count != 0) {
                foreach (string mask in outMasks)
                {
                    if (!this._maskToWords.ContainsKey(mask))
                        this._maskToWords[mask] = new List<string>();
                    this._maskToWords[mask].Add(word);
                }
            }
            return this._maskToWords.Keys.Count;
        }

        public string GetMaskonGrid(char[,] grid, short width, short height, short row, short col, char Direction) {
            if (grid == null) {
                // throw exception
            }

            char[] mask = null;
            if (Direction == WordDirection.HORIZONTAL) {
                mask = new char[width];
                for (int i = 0; i < width; i++) {
                    mask[i] = (grid[row, i] != '\0') ? char.ToUpper(grid[row, i]) : 'x';
                }
            }
            else {
                mask = new char[height];
                for (int i = 0; i < height; i++)
                {
                    mask[i] = (grid[i, col] != '\0') ? char.ToUpper(grid[i, col]) : 'x';
                }
            }
            return new string(mask);
        }

        public List<string> GetWordsOnMask(string mask) {
            if (this._maskToWords.Count == 0 || mask.Equals("")) return null;
            // List<string> outWords = new List<string>();
            Dictionary<string, string> outWords = new Dictionary<string, string>();
            // add the words for the same mask

            if (this._maskToWords.ContainsKey(mask)) {
                foreach (string word in this._maskToWords[mask]) {
                    outWords.Add(word, "");
                }
            }

            // add all the relevant words on mask; reducing 'x' from left
            for (int idx = 0; (idx < mask.Length) && (mask[idx] == 'x'); idx++) {
                string m = mask.Substring(idx + 1);
                if (this._maskToWords.ContainsKey(m))
                {
                    foreach (string word in this._maskToWords[m])
                    {
                        outWords.Add(word, "");
                    }
                }
            }

            // add all the relevant words on mask; reducing 'x' from right
            for (int idx = mask.Length - 1; (idx >= 0) && (mask[idx] == 'x'); idx--)
            {
                string m = mask.Substring(0, idx);
                if (this._maskToWords.ContainsKey(m))
                {
                    foreach (string word in this._maskToWords[m])
                    {
                        outWords.Add(word, "");
                    }
                }
            }

            return outWords.Keys.ToList<string>();
        }

        
    }
}
