using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using man_dont_get_angry.ModelUtils;

namespace man_dont_get_angry.Models
{
    internal class Player
    {
        private string _name;
        private Piece[] _pieces;
        private PlayerState _playerState;
        private Color _color;
        private bool _isAutomatic;

        public Player(string Name, Color color, bool first = false)
        {
            this._name = Name;
            this._pieces = new Piece[4];
            this._color = color;
            this._isAutomatic = true;

            if (first)
                this._playerState = PlayerState.ThrowDice;

            else
                this._playerState = PlayerState.MoveDone;

            for (int i = 0; i < _pieces.Length; i++)
            {
                _pieces[i] = new Piece(i, color);
            }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Piece[] ThePieces
        {
            get { return _pieces; }
        }

        public PlayerState ThePlayerState
        {
            get { return _playerState; }
            set
            {
                int dice_thrown;
                this._playerState = value;
            }
        }

        public Color TheColor
        {
            get { return _color; }
            set { _color = value; }
        }

        public bool IsAutomatic
        {
            get { return _isAutomatic; }
        }
    }
}
