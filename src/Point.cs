using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordGenerator
{
    
    class Point
    {
        public short Row { get; set; }
        public short Col { get; set; }

        public Point(short row, short col)
        {
            this.Row = row;
            this.Col = col;
        }

    }
}
