using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using man_dont_get_angry.ModelUtils;
using System.Threading;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

namespace man_dont_get_angry.Models
{
    /// <summary>
    /// This class manages the Game flow in a loop, uses class OptionsChecker to check options 
    /// which can be made, sets the wanted pieces
    /// </summary>
    public class GameManager : INotifyPropertyChanged
    {
        private GameBoard _gameBoard;
        private Dice _dice;
        private Player[] _players;
        private int _actualPlayerID;
        private int _lastPlayerID;
        private Thread _thread;
        private bool _threadRunning;
        private List<MovementOption> _movementOptions;


        public GameManager()
        {
            this._dice = new Dice();
            this._thread = new Thread(this.RollD);
            this._thread.IsBackground = true;

            this._players = new Player[]
            {
                new Player("Green", Color.Green, this, true),
                new Player("Red", Color.Red, this),
                new Player("Yellow", Color.Yellow, this),
                new Player("Blue", Color.Blue, this),
            };

            this._gameBoard = new GameBoard();
            this._actualPlayerID = 0;
            this._threadRunning = false;
        }

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

        public GameBoard TheGameBoard
        {
            get { return this._gameBoard; }
            set { this._gameBoard = value; }
        }

        public Dice TheDice
        {
            get { return this._dice; }
            set { this._dice = value; }
        }

        public Player ActualPlayer
        {
            get { return this._players[this._actualPlayerID]; }
        }

        public Player[] Players
        {
            get { return this._players; }
            set { this._players = value; }
        }

        public int PlayerID
        {
            get { return this._actualPlayerID; }
            set { this._actualPlayerID = value; }
        }

        public int LastPlayerID
        {
            get { return this._lastPlayerID; }
            set { this._lastPlayerID = value; }
        }

        public string ActualMove
        {
            get
            {
                switch (this.ActualPlayer.ThePlayerState)
                {
                    case PlayerState.ThrowDice:
                        return "Throw Dice";
                    case PlayerState.MovePieces:
                    case PlayerState.MovePiecesRepeadetly:
                        return "Move Pieces";
                    case PlayerState.MoveDone:
                        return "Move Done";
                    default:
                        return "";
                }
            }
        }

        // Ignore Movement Options, TODO allow to redice every time new when game is loaded
        public List<MovementOption> MovementOptions
        {
            get { return this._movementOptions; }
            set { this._movementOptions = value; }
        }

        public void SetAutoThread(bool state)
        {
            if (state)
            {
                if (this._thread.ThreadState != ThreadState.Running && this._players[this._actualPlayerID].IsAutomatic)
                {
                    this._thread = new Thread(this.RollD);
                    this._threadRunning = true;
                    this._thread.IsBackground = true;
                    this._thread.Start();
                }
            }
            else
            {
                this._threadRunning = false;
            }
        }

        public bool DiceRollable()
        {
            if (this._players[_actualPlayerID].ThePlayerState == PlayerState.ThrowDice)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void setPosition(int a)
        {
            // TODO: win check does not work
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

            if (this.ActualPlayer.IsAutomatic && !this._thread.IsAlive)
            {
                this._thread = new Thread(this.RollD);
                this._thread.IsBackground = true;
                this._threadRunning = true;
                this._thread.Start();
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void RollD()
        {
            var random = new Random();
            while (this._threadRunning && this.ActualPlayer.IsAutomatic && !OptionsChecker.checkGameWon(this._players[this._lastPlayerID], this._gameBoard.EndFields))
            {
                if (this.ActualPlayer.ThePlayerState == ModelUtils.PlayerState.ThrowDice)
                {
                    this.RollDice();
                    Thread.Sleep(1000);
                }
                else
                {
                    int index = random.Next(this.MovementOptions.Count);
                    this.setPosition(this.MovementOptions[index].EndPosition);
                    Thread.Sleep(1000);
                }
            }
        }

        public void StartAutoThread(int num)
        {
            if (num == this._actualPlayerID)
            {
                this._thread = new Thread(this.RollD);
                this._threadRunning = true;
                this._thread.IsBackground = true;
                this._thread.Start();
            }
        }

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
    }
}
