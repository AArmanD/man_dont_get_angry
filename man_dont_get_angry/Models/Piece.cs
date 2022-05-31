using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using man_dont_get_angry.ModelUtils;

namespace man_dont_get_angry.Models
{
    public class Piece
    {
        private Color _pieceColor;

        public Piece(Color color)
        {
            this._pieceColor = color;
        }

        private Piece()
        { }

        public Color TheColor
        {
            get { return _pieceColor; }
            set { _pieceColor = value; }
        }
    }
}
