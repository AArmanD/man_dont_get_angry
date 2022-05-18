using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using man_dont_get_angry.Utils;

namespace man_dont_get_angry.Models
{
    internal class Piece
    {
        private Color _pieceColor;
        private int _id;

        public Piece(int id, Color color)
        {
            this._id = id;
            this._pieceColor = color;
        }

        public Color TheColor 
        {
            get { return _pieceColor; }
            set { _pieceColor = value; }
        }

        public int Id
        { 
            get { return _id; }
            set { _id = value; }
        }
    }
}
