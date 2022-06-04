using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using man_dont_get_angry.ModelUtils;
using System.Threading;

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
        private AutoPlayerThreadManager _autoPlayerThreadManager;

        /// <summary>
        /// Event which is to be raised when a property changes from which the value should be updated
        /// in the main window
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Get/Set the game board
        /// </summary>
        public GameBoard TheGameBoard
        {
            get { return this._gameBoard; }
            set { this._gameBoard = value; }
        }

        /// <summary>
        /// Get/Set the dice
        /// </summary>
        public Dice TheDice
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
                switch (this.ActualPlayer.ThePlayerState)
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
        /// Get the saved AutoPlayerThreadManager
        /// </summary>
        public AutoPlayerThreadManager TheAutoPlayerThreadManager
        {
            get { return _autoPlayerThreadManager; }
        }

        /// <summary>
        /// Constructor for creating a GameManager object
        /// </summary>
        public GameManager()
        {

            this._dice = new Dice();
            this._movementOptions = new List<MovementOption>();

            this._autoPlayerThreadManager = new AutoPlayerThreadManager(this);

            this._players = new Player[]
            {
                new Player("Green", Color.Green, this._autoPlayerThreadManager, true),
                new Player("Red", Color.Red, this._autoPlayerThreadManager),
                new Player("Yellow", Color.Yellow, this._autoPlayerThreadManager),
                new Player("Blue", Color.Blue, this._autoPlayerThreadManager),
            };

            this._gameBoard = new GameBoard();
            this._actualPlayerID = 0;
        }

        /// <summary>
        /// Rolls dice, checks movement options, sets player state and optionally changes player
        /// </summary>
        public void RollDice()
        {
            // check if win checking algorithm is correct
            if (this._players[this._actualPlayerID].ThePlayerState == PlayerState.ThrowDice && !OptionsChecker.checkGameWon(this._players[this._lastPlayerID], this._gameBoard.EndFields))
            {
                this._dice.roll();
                this._movementOptions = OptionsChecker.checkMovements(_players[this._actualPlayerID], this._dice, this._gameBoard.GameBoardFields, this._gameBoard.StartFields, this._gameBoard.EndFields);
                this._players[_actualPlayerID].ThePlayerState = OptionsChecker.GenerateStateAfterRolling(this._movementOptions, this._dice, this._players[this._actualPlayerID], this._gameBoard.EndFields);
                OnPropertyChanged("ActualMove");

                if (this._players[_actualPlayerID].ThePlayerState == PlayerState.MoveDone)
                {
                    changePlayer();
                }
            }
        }

        /// <summary>
        /// Sets a piece to a specified position if that is in the movementOptions list
        /// </summary>
        /// <param name="a">Position to set a Piece of the actual player to</param>
        public void setPosition(int a)
        {
            if ((this._players[_actualPlayerID].ThePlayerState == PlayerState.MovePieces || this._players[_actualPlayerID].ThePlayerState == PlayerState.MovePiecesRepeadetly))
            {
                foreach (MovementOption movementOption in this._movementOptions)
                {
                    if (movementOption.EndPosition == a)
                    {
                        this._gameBoard.setPiece(movementOption);


                        if (this._players[_actualPlayerID].ThePlayerState == PlayerState.MovePiecesRepeadetly)
                        {
                            this._players[_actualPlayerID].ThePlayerState = PlayerState.ThrowDice;
                            OnPropertyChanged("ActualMove");
                        }
                        else
                        {
                            this._players[_actualPlayerID].ThePlayerState = PlayerState.MoveDone;
                            changePlayer();
                            OnPropertyChanged("ActualMove");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Changes the player, so the next player can make his move
        /// </summary>
        private void changePlayer()
        {
            // als enumerator implementieren
            this._lastPlayerID = this._actualPlayerID;
            if (this._actualPlayerID < 3)
            {
                this._actualPlayerID++;
            }
            else
                this._actualPlayerID = 0;

            OnPropertyChanged("ActualPlayer");
            this._dice.resetDice();
            this._players[_actualPlayerID].ThePlayerState = PlayerState.ThrowDice;

            // Use coalescing operator
            if ((this.ActualPlayer.IsAutomatic ?? false) && !this._autoPlayerThreadManager.AutoPlayerThreadRunning())
            {
                this._autoPlayerThreadManager.StartAutoThread();
            }
        }

        /// <summary>
        /// Resets the game so a new game can be started
        /// </summary>
        public void ResetGame()
        {
            this.TheGameBoard.SetupStartPostitions();
            for (int i = 0; i < this._players.Length; i++)
            {
                this._players[i].IsAutomatic = false;
            }

            this._players[0].ThePlayerState = PlayerState.ThrowDice;
            OnPropertyChanged("ActualMove");
            this._actualPlayerID = 0;
            OnPropertyChanged("ActualPlayer");
            this._dice.resetDice();
        }

        /// <summary>
        /// Loads a game from a GameManager instance
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
                this._players[i].ThePlayerState = gameManager.Players[i].ThePlayerState;
            }

            this._gameBoard.SetupPositions(gameManager);
        }

        /// <summary>
        /// Checks whether a field is in the movement options
        /// </summary>
        /// <param name="pos">Position to check wheter it is in the movement options</param>
        /// <returns>true if field is in the movement options, otherwise false</returns>
        public bool CheckFieldIsInMovementOptions(int pos)
        {
            foreach (MovementOption option in this._movementOptions)
            {
                if (option.EndPosition == pos)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Handler for The PropertyChangedEvent, which is for updating the value of the property 
        /// in the gui
        /// </summary>
        /// <param name="prop">Name of the property of which the value should be updated in the gui</param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
