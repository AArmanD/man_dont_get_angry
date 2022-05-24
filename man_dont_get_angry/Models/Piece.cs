using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using man_dont_get_angry.ModelUtils;

namespace man_dont_get_angry.Models
{
    internal class Piece
    {
        private Color _pieceColor;

        public Piece(Color color)
        {
            this._pieceColor = color;
        }

        public Color TheColor 
        {
            get { return _pieceColor; }
            set { _pieceColor = value; }
        }
    }
}
