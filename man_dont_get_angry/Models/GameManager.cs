using man_dont_get_angry.Utils;
using System.Collections.Generic;
using System;

namespace man_dont_get_angry.Models
{
    /// <summary>
    /// This class manages the Game flow in a loop, uses class OptionsChecker to check options 
    /// which can be made, sets the wanted pieces
    /// </summary>
    internal class GameManager
    {
        private GameBoard _gameBoard;
        private Dice _dice;
        private Player[] _players;
        private int _actualPlayerID;
        private List<Tuple<int, int>> _movementOptions;

        public GameManager()
        {
            this._dice = new Dice();

            this._players = new Player[]
            {
                // Reihenfolge ist "noch" wichtig
                new Player("Player 1", Utils.Color.Green, true),
                new Player("Player 2", Utils.Color.Red),
                new Player("Player 3", Utils.Color.Yellow),
                new Player("Player 4", Utils.Color.Blue),
            };

            this._gameBoard = new GameBoard(_players);
            this._actualPlayerID = 0;
        }

        public void RollDice()
        {
            this._dice.roll();
            this._movementOptions = OptionsChecker.checkMovements(_players[this._actualPlayerID], this._dice, this._gameBoard.GameBoardFields, this._gameBoard.StartFields, this._gameBoard.EndFields);
            this._players[_actualPlayerID].ThePlayerState = OptionsChecker.GenerateStateAfterRolling(this._movementOptions, this._dice);
        }

        public GameBoard TheGameBoard
        {
            get { return this._gameBoard; }
        }

        public Dice TheDice
        {
            get { return this._dice; }
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

        }

        public bool positionSettable(int a)
        {
            if (this._players[_actualPlayerID].ThePlayerState == PlayerState.MovePieces)
            {
                foreach(Tuple<int,int> tuple in this._movementOptions)
                {
                    if (tuple.Item1 == a)
                        return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
