using man_dont_get_angry.Utils;

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
            this._players[_actualPlayerID].ThePlayerState = OptionsChecker.GenerateStateAfterRolling(this._players[_actualPlayerID], this._dice, this._gameBoard.GameBoardFields, this._gameBoard.StartFields);
        }

        public GameBoard TheGameBoard
        {
            get { return this._gameBoard; }
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

        public bool positionSettable()
        {
            return true;
        }
    }
}
