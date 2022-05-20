﻿using man_dont_get_angry.Utils;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace man_dont_get_angry.Models
{
    /// <summary>
    /// This class manages the Game flow in a loop, uses class OptionsChecker to check options 
    /// which can be made, sets the wanted pieces
    /// </summary>
    internal class GameManager : INotifyPropertyChanged
    {
        private GameBoard _gameBoard;
        private Dice _dice;
        private Player[] _players;
        private int _actualPlayerID;
        private List<Tuple<int, int>> _movementOptions;

        // error moving to last endfield green
        public GameManager()
        {
            this._dice = new Dice();

            this._players = new Player[]
            {
                // Reihenfolge ist "noch" wichtig
                new Player("Green", Utils.Color.Green, true),
                new Player("Red", Utils.Color.Red),
                new Player("Yellow", Utils.Color.Yellow),
                new Player("Blue", Utils.Color.Blue),
            };

            this._gameBoard = new GameBoard(_players);
            this._actualPlayerID = 0;
        }

        public void RollDice()
        {

            if (this._players[this._actualPlayerID].ThePlayerState == PlayerState.ThrowDice)
            {
                this._dice.roll();
                this._movementOptions = OptionsChecker.checkMovements(_players[this._actualPlayerID], this._dice, this._gameBoard.GameBoardFields, this._gameBoard.StartFields, this._gameBoard.EndFields);
                this._players[_actualPlayerID].ThePlayerState = OptionsChecker.GenerateStateAfterRolling(this._movementOptions, this._dice);

                if (this._players[_actualPlayerID].ThePlayerState == PlayerState.MoveDone)
                {
                    changePlayer();
                }
            }
        }

        public GameBoard TheGameBoard
        {
            get { return this._gameBoard; }
        }

        public Dice TheDice
        {
            get { return this._dice; }
        }

        public Player ActualPlayer
        {
            get { return this._players[this._actualPlayerID]; }
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
            if (this._movementOptions.Count > 0)
            {
                foreach (Tuple<int, int> movementOption in this._movementOptions)
                {
                    if (movementOption.Item2 == a)
                    {
                        this._gameBoard.setPiece(movementOption);

                        if (!OptionsChecker.checkGameWon(this._players[this._actualPlayerID], this._gameBoard.EndFields))
                        {

                            if (this._players[_actualPlayerID].ThePlayerState == PlayerState.MovePiecesRepeadetly)
                            {
                                this._players[_actualPlayerID].ThePlayerState = PlayerState.ThrowDice;
                            }
                            else
                            {
                                this._players[_actualPlayerID].ThePlayerState = PlayerState.MoveDone;
                                changePlayer();
                            }
                        }
                    }
                }
            }
            else
            {
                if (this._players[this._actualPlayerID].ThePlayerState == PlayerState.MovePiecesRepeadetly)
                { 
                    this._players[this._actualPlayerID].ThePlayerState = PlayerState.ThrowDice;
                }
            }
        }

        private void changePlayer()
        {
            if (this._actualPlayerID < 3)
            {
                this._actualPlayerID++;
            }
            else
                this._actualPlayerID = 0;

            OnPropertyChanged("ActualPlayer");
            this._dice.resetDice();
            this._players[_actualPlayerID].ThePlayerState = PlayerState.ThrowDice;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
