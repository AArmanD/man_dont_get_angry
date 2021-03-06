using man_dont_get_angry.ModelUtils;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace man_dont_get_angry.Models
{
    /// <summary>
    /// Represents a player in the game
    /// </summary>
    public class Player : INotifyPropertyChanged
    {
        /// <summary>
        /// For saving the name of the player
        /// </summary>
        private string? _name;

        /// <summary>
        /// For saving the actual state of the player
        /// </summary>
        private PlayerState? _playerState;

        /// <summary>
        /// For saving the color of the player
        /// </summary>
        private Color? _color;

        /// <summary>
        /// For saving whether the player should be played automatically or manually
        /// </summary>
        private bool? _isAutomatic;

        /// <summary>
        /// For starting auto playing
        /// </summary>
        private AutoPlayerManager? _autoPlayerManager;

        /// <summary>
        /// Event which is to be raised when a property changes from which the value should be updated
        /// in the main window
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Constructor needed for serializing/deserializing
        /// </summary>
        private Player()
        { }

        /// <summary>
        /// Constructor for creating a player object
        /// </summary>
        /// <param name="Name">Name of the player</param>
        /// <param name="color">Color of the player, are defined in ModelUtils.Constants</param>
        /// <param name="gameManager">Instance of the game manager</param>
        /// <param name="first">Optional bool parameter which specifies whether the player which is created is the first one</param>
        public Player(string Name, Color color, AutoPlayerManager autoPlayerThreadManager, bool first = false)
        {
            this._name = Name;
            this._color = color;
            this._isAutomatic = false;
            this._autoPlayerManager = autoPlayerThreadManager;

            if (first)
            {
                this._playerState = ModelUtils.PlayerState.ThrowDice;
            }
            else
            {
                this._playerState = ModelUtils.PlayerState.MoveDone;
            }
        }

        /// <summary>
        /// Get/Set the name of the player
        /// </summary>
        public string? Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Get/Set the state of the player
        /// </summary>
        public PlayerState? PlayerState
        {
            get { return _playerState; }
            set
            {
                this._playerState = value;
            }
        }

        /// <summary>
        /// Get/Set the color of the player
        /// </summary>
        public Color? Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Get/Set whether the player is automatic
        /// </summary>
        public bool? IsAutomatic
        {
            get { return _isAutomatic; }
            set
            {
                this._isAutomatic = value;
                OnPropertyChanged("IsAutomatic");

                // start auto playing thread in the game manager
                switch (this._color)
                {
                    case ModelUtils.Color.Green:
                        this._autoPlayerManager?.StartAutoPlayer(0);
                        break;
                    case ModelUtils.Color.Red:
                        this._autoPlayerManager?.StartAutoPlayer(1);
                        break;
                    case ModelUtils.Color.Yellow:
                        this._autoPlayerManager?.StartAutoPlayer(2);
                        break;
                    case ModelUtils.Color.Blue:
                        this._autoPlayerManager?.StartAutoPlayer(3);
                        break;
                }

            }
        }

        /// <summary>
        /// Invokes the PropertyChanged event, so that the value of a specific property is updated
        /// in the gui
        /// </summary>
        /// <param name="prop">Name of the property of which the value should be updated in the gui</param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
