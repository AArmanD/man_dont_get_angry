using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using man_dont_get_angry.ModelUtils;

namespace man_dont_get_angry.Models
{
    internal class Player : INotifyPropertyChanged
    {
        private string _name;
        private PlayerState _playerState;
        private Color _color;
        private bool _isAutomatic;
        private GameManager _gameManager;

        public Player(string Name, Color color, GameManager gameManager, bool first = false)
        {
            this._name = Name;
            this._color = color;
            this._isAutomatic = false;
            this._gameManager = gameManager;

            if (first)
                this._playerState = PlayerState.ThrowDice;

            else
                this._playerState = PlayerState.MoveDone;

            //for (int i = 0; i < _pieces.Length; i++)
            //{
            //    _pieces[i] = new Piece(i, color);
            //}
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
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
            set { this._isAutomatic = value;
                OnPropertyChanged("IsAutomatic");
                switch (this._color)
                {
                    case Color.Green:
                        this._gameManager.StartAutoThread(0);
                        break;
                    case Color.Red:
                        this._gameManager.StartAutoThread(1);
                        break;
                    case Color.Yellow:
                        this._gameManager.StartAutoThread(2);
                        break;
                    case Color.Blue:
                        this._gameManager.StartAutoThread(3);
                        break;
                }
                
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
