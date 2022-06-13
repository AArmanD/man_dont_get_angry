using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using man_dont_get_angry.ModelUtils;
using System.Windows.Input;
using System.Windows;

namespace man_dont_get_angry.Models
{
    /// <summary>
    /// Manages the game flow
    /// </summary>
    public class GameManager : INotifyPropertyChanged
    {
        /// <summary>
        /// For saving the game board
        /// </summary>
        private GameBoard _gameBoard;

        /// <summary>
        /// For saving the dice
        /// </summary>
        private Dice _dice;

        /// <summary>
        /// For saving the players
        /// </summary>
        private Player[] _players;

        /// <summary>
        /// For saving the id of the actual player
        /// </summary>
        private int _actualPlayerID;

        /// <summary>
        /// For saving the id of the last player
        /// </summary>
        private int _lastPlayerID;

        /// <summary>
        /// For saving the movement options for the actual move
        /// </summary>
        private List<MovementOption> _movementOptions;

        /// <summary>
        /// For saving the Automatic player thread manager
        /// </summary>
        private AutoPlayerManager _autoPlayerManager;

        /// <summary>
        /// Event which is to be raised when properties changed for updating the ui
        /// in the main window
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Constructor for creating posToSet GameManager object
        /// </summary>
        public GameManager()
        {
            this._dice = new Dice();
            this._movementOptions = new List<MovementOption>();

            this._autoPlayerManager = new AutoPlayerManager(this);

            this._players = new Player[]
            {
                new Player("Green", Color.Green, this._autoPlayerManager, true),
                new Player("Red", Color.Red, this._autoPlayerManager),
                new Player("Yellow", Color.Yellow, this._autoPlayerManager),
                new Player("Blue", Color.Blue, this._autoPlayerManager),
            };

            this._gameBoard = new GameBoard();
            this._actualPlayerID = 0;
        }

        /// <summary>
        /// Get/Set the game board
        /// </summary>
        public GameBoard GameBoard
        {
            get { return this._gameBoard; }
            set { this._gameBoard = value; }
        }

        /// <summary>
        /// Get/Set the dice
        /// </summary>
        public Dice Dice
        {
            get { return this._dice; }
            set { this._dice = value; }
        }

        /// <summary>
        /// Get the actual player
        /// </summary>
        public Player ActualPlayer
        {
            get { return this._players[this._actualPlayerID]; }
        }

        /// <summary>
        /// Get/Set player array
        /// </summary>
        public Player[] Players
        {
            get { return this._players; }
            set { this._players = value; }
        }

        /// <summary>
        /// Get/Set ID of actual player
        /// </summary>
        public int PlayerID
        {
            get { return this._actualPlayerID; }
            set { this._actualPlayerID = value; }
        }

        /// <summary>
        /// Get/Set ID of last player
        /// </summary>
        public int LastPlayerID
        {
            get { return this._lastPlayerID; }
            set { this._lastPlayerID = value; }
        }

        /// <summary>
        /// Get string representation of the actual player state
        /// </summary>
        public string ActualMove
        {
            get
            {
                if (!OptionsChecker.CheckGameWon(this._players[this._actualPlayerID], this._gameBoard.EndFields))
                {
                    switch (this.ActualPlayer.PlayerState)
                    {
                        case PlayerState.ThrowDice:
                        case PlayerState.MoveDone:
                            return "Throw Dice";
                        case PlayerState.MovePieces:
                        case PlayerState.MovePiecesRepeadetly:
                            return "Move Pieces";
                        default:
                            return "";
                    }
                }
                else
                {
                    return "..";
                }
            }
        }

        /// <summary>
        /// Get/Set the list with the actual movement options
        /// </summary>
        public List<MovementOption> MovementOptions
        {
            get { return this._movementOptions; }
            set { this._movementOptions = value; }
        }

        /// <summary>
        /// Get the saved AutoPlayerManager
        /// </summary>
        public AutoPlayerManager AutoPlayerManager
        {
            get { return _autoPlayerManager; }
        }

        /// <summary>
        /// Rolls dice, checks movement options, sets player state and optionally changes player
        /// </summary>
        public void RollDice()
        {
            if (this._players[this._actualPlayerID].PlayerState == PlayerState.ThrowDice && !OptionsChecker.CheckGameWon(this._players[this._lastPlayerID], this._gameBoard.EndFields))
            {
                this._dice.Roll();
                this._movementOptions = OptionsChecker.CalculateMovementOptions(_players[this._actualPlayerID], this._dice, this._gameBoard.GameBoardFields, this._gameBoard.StartFields, this._gameBoard.EndFields);
                this._players[_actualPlayerID].PlayerState = OptionsChecker.GenerateStateAfterRolling(this._movementOptions, this._dice, this._players[this._actualPlayerID], this._gameBoard.EndFields);
                OnPropertyChanged("ActualMove");

                if (this._players[_actualPlayerID].PlayerState == PlayerState.MoveDone)
                {
                    ChangePlayer();
                }
            }
        }

        /// <summary>
        /// Sets posToSet piece to specified position if that is in the movementOptions list
        /// </summary>
        /// <param name="posToSet">Position to set posToSet Piece of the actual player to</param>
        public void SetPosition(int posToSet)
        {
            if ((this._players[_actualPlayerID].PlayerState == PlayerState.MovePieces || this._players[_actualPlayerID].PlayerState == PlayerState.MovePiecesRepeadetly))
            {
                foreach (MovementOption movementOption in this._movementOptions)
                {
                    if (movementOption.EndPosition == posToSet)
                    {
                        this._gameBoard.SetPiece(movementOption);

                        if (this._players[_actualPlayerID].PlayerState == PlayerState.MovePiecesRepeadetly)
                        {
                            this._players[_actualPlayerID].PlayerState = PlayerState.ThrowDice;
                            OnPropertyChanged("ActualMove");
                        }
                        else
                        {
                            this._players[_actualPlayerID].PlayerState = PlayerState.MoveDone;

                            if (!OptionsChecker.CheckGameWon(this._players[this._actualPlayerID], this._gameBoard.EndFields))
                            {
                                ChangePlayer();
                            }

                            OnPropertyChanged("ActualMove");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Changes the player, so the next player can make his move
        /// </summary>
        private void ChangePlayer()
        {
            // change id of actual player
            this._lastPlayerID = this._actualPlayerID;
            if (this._actualPlayerID < 3)
            {
                this._actualPlayerID++;
            }
            else
            {
                this._actualPlayerID = 0;
            }

            OnPropertyChanged("ActualPlayer");

            // reset dice count
            this._dice.Reset();

            // set player state of next player to ThrowDice so he is able to throw the dice
            this._players[_actualPlayerID].PlayerState = PlayerState.ThrowDice;

            // start auto player when next player is set to be automatic
            if ((this.ActualPlayer.IsAutomatic ?? false) && !this._autoPlayerManager.AutoPlayerRunning())
            {
                this._autoPlayerManager.StartAutoPlayer();
            }

            // Raise event to reevaluate whether buttons should be enabled/disabled
            Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
        }

        /// <summary>
        /// Resets the game so posToSet new game can be started
        /// </summary>
        public void ResetGame()
        {
            this.GameBoard.SetupStartPostitions();
            for (int i = 0; i < this._players.Length; i++)
            {
                this._players[i].IsAutomatic = false;
            }

            this._players[0].PlayerState = PlayerState.ThrowDice;
            OnPropertyChanged("ActualMove");
            this._actualPlayerID = 0;
            OnPropertyChanged("ActualPlayer");
            this._dice.Reset();
        }

        /// <summary>
        /// Loads posToSet game from posToSet GameManager instance
        /// </summary>
        /// <param name="gameManager">GameManager instance to load game from</param>
        public void LoadGame(GameManager gameManager)
        {
            // set actual player
            this._actualPlayerID = gameManager.PlayerID;
            this._lastPlayerID = gameManager.LastPlayerID;
            this._movementOptions = gameManager._movementOptions;

            OnPropertyChanged("ActualMove");
            OnPropertyChanged("ActualPlayer");

            // set data to players
            for (int i = 0; i < this._players.Length; i++)
            {
                this._players[i].IsAutomatic = false;
                this._players[i].PlayerState = gameManager.Players[i].PlayerState;
            }

            this._gameBoard.SetupPositions(gameManager);
        }

        /// <summary>
        /// Checks whether posToSet field is in the movement options
        /// </summary>
        /// <param name="pos">Position to check wheter it is in the movement options</param>
        /// <returns>true if field is in the movement options, otherwise false</returns>
        public bool CheckFieldIsInMovementOptions(int pos)
        {
            foreach (MovementOption option in this._movementOptions)
            {
                if (option.EndPosition == pos)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Handler for The PropertyChangedEvent, which is for updating the value of properties 
        /// in the gui
        /// </summary>
        /// <param name="prop">Name of the property of which the value should be updated in the gui</param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
