using man_dont_get_angry.ModelUtils;

namespace man_dont_get_angry.Models
{
    /// <summary>
    /// Represents the gameboard of the game
    /// </summary>
    public class GameBoard
    {
        /// <summary>
        /// For saving the game board fields
        /// </summary>
        private Field[] _gameBoardFields;

        /// <summary>
        /// For saving the start fields
        /// </summary>
        private Field[] _startFields;

        /// <summary>
        /// For saving the end fields
        /// </summary>
        private Field[] _endFields;

        /// <summary>
        /// Constructor for creating a game board object
        /// </summary>
        public GameBoard()
        {
            // create field arrays
            this._startFields = new Field[16];
            this._endFields = new Field[16];
            this._gameBoardFields = new Field[40];

            // initialize game board field array, consider arrow fields
            for (int i = 0; i < _gameBoardFields.Length; i++)
            {
                if (i == 0)
                {
                    this._gameBoardFields[i] = new Field(FieldType.GreenArrowField);
                }
                else if (i == 10)
                {
                    this._gameBoardFields[i] = new Field(FieldType.RedArrowField);
                }
                else if (i == 20)
                {
                    this._gameBoardFields[i] = new Field(FieldType.YellowArrowField);
                }
                else if (i == 30)
                {
                    this._gameBoardFields[i] = new Field(FieldType.BlueArrowField);
                }
                else
                {
                    this._gameBoardFields[i] = new Field(FieldType.NormalField);
                }
            }

            // initialize start field array, set pieces at the same time
            for (int count = 0; count < _startFields.Length; count++)
            {
                if (-1 < count && count < 4)
                {
                    this._startFields[count] = new Field(FieldType.GreenStartField, new Piece(Color.Green));
                }
                else if (3 < count && count < 8)
                {
                    this._startFields[count] = new Field(FieldType.RedStartField, new Piece(Color.Red));
                }
                else if (7 < count && count < 12)
                {
                    this._startFields[count] = new Field(FieldType.YellowStartField, new Piece(Color.Yellow));
                }
                else if (11 < count && count < 16)
                {
                    this._startFields[count] = new Field(FieldType.BlueStartField, new Piece(Color.Blue));
                }
            }

            // initialize end field array
            for (int i = 0; i < _endFields.Length; i++)
            {
                if (-1 < i && i < 4)
                {
                    this._endFields[i] = new Field(FieldType.GreenEndField);
                }
                else if (3 < i && i < 8)
                {
                    this._endFields[i] = new Field(FieldType.RedEndField);
                }
                else if (7 < i && i < 12)
                {
                    this._endFields[i] = new Field(FieldType.YellowEndField);
                }
                else if (11 < i && i < 16)
                {
                    this._endFields[i] = new Field(FieldType.BlueEndField);
                }
            }
        }

        /// <summary>
        /// Get/Set the gameboard fields
        /// </summary>
        public Field[] GameBoardFields
        {
            get { return _gameBoardFields; }
            set { _gameBoardFields = value; }
        }

        /// <summary>
        /// Get/Set the start fields
        /// </summary>
        public Field[] StartFields
        {
            get { return _startFields; }
            set { _startFields = value; }
        }

        /// <summary>
        /// Get/Set the end fields
        /// </summary>
        public Field[] EndFields
        {
            get { return _endFields; }
            set { _endFields = value; }
        }

        /// <summary>
        /// Sets a piece to a specified position
        /// </summary>
        /// <param name="movementOption">Object of type MovementOption in which stands from where to where should be moved</param>
        public void SetPiece(MovementOption movementOption)
        {
            // remove the piece from its initial position, save it into a temporary variable
            Piece? pieceToSet = null;
            if (movementOption.StartPosition < 40)
            {
                pieceToSet = this._gameBoardFields[movementOption.StartPosition].Piece;
                this._gameBoardFields[movementOption.StartPosition].Piece = null;
            }
            else if (movementOption.StartPosition > 39 && movementOption.StartPosition < 56)
            {
                pieceToSet = this._startFields[movementOption.StartPosition - 40].Piece;
                this._startFields[movementOption.StartPosition - 40].Piece = null;
            }
            else if (movementOption.StartPosition > 55 && movementOption.StartPosition < 72)
            {
                pieceToSet = this._endFields[movementOption.StartPosition - 56].Piece;
                this._endFields[movementOption.StartPosition - 56].Piece = null;
            }

            // check whether the position to set the piece to is on the gameboard fields
            if (movementOption.EndPosition < 40)
            {

                // check whether at the position to set the piece to already piece stands
                if (this._gameBoardFields[movementOption.EndPosition].Piece != null)
                {

                    // move piece on field back to start
                    int count = 3;
                    switch (this._gameBoardFields[movementOption.EndPosition].Piece?.Color)
                    {
                        case Color.Green:
                            break;
                        case Color.Red:
                            count = 7;
                            break;
                        case Color.Yellow:
                            count = 11;
                            break;
                        case Color.Blue:
                            count = 15;
                            break;
                    }

                    for (int i = count; i > -1; i--)
                    {
                        if (this._startFields[i].Piece == null)
                        {
                            this._startFields[i].Piece = this._gameBoardFields[movementOption.EndPosition].Piece;
                            break;
                        }
                    }
                }
                this._gameBoardFields[movementOption.EndPosition].Piece = pieceToSet;
            }

            // check whether the position to set the piece to is on the start fields
            else if (movementOption.EndPosition > 39 && movementOption.EndPosition < 56)
            {
                this._startFields[movementOption.EndPosition - 40].Piece = pieceToSet;
            }

            // check whether the position to set the piece to is on the end fields
            else if (movementOption.EndPosition > 55 && movementOption.EndPosition < 72)
            {
                this._endFields[movementOption.EndPosition - 56].Piece = pieceToSet;
            }
        }

        /// <summary>
        /// Sets up the initial start positions, is used when the game is resetted
        /// </summary>
        public void SetupStartPostitions()
        {
            // set all pieces to the start fields
            for (int count = 0; count < _startFields.Length; count++)
            {
                if (-1 < count && count < 4)
                {
                    this._startFields[count].Piece = new Piece(Color.Green);
                }
                else if (3 < count && count < 8)
                {
                    this._startFields[count].Piece = new Piece(Color.Red);
                }
                else if (7 < count && count < 12)
                {
                    this._startFields[count].Piece = new Piece(Color.Yellow);
                }
                else if (11 < count && count < 16)
                {
                    this._startFields[count].Piece = new Piece(Color.Blue);
                }
            }

            // clear out end fields from pieces
            for (int i = 0; i < _endFields.Length; i++)
            {
                _endFields[i].Piece = null;
            }

            // clear out game board fields from pieces
            for (int i = 0; i < this._gameBoardFields.Length; i++)
            {
                this._gameBoardFields[i].Piece = null;
            }
        }

        /// <summary>
        /// Sets up positions from a gameManager instance, is used when a game state is imported from an xml file
        /// </summary>
        /// <param name="gameManager"></param>
        public void SetupPositions(GameManager gameManager)
        {
            // set piece positions in the start fields
            for (int i = 0; i < this._startFields.Length; i++)
            {
                this._startFields[i].Piece = gameManager.GameBoard._startFields[i].Piece;
            }

            // set piece positions in the end fields
            for (int i = 0; i < this._endFields.Length; i++)
            {
                this._endFields[i].Piece = gameManager.GameBoard._endFields[i].Piece;
            }

            // set piece positions in the game board fields
            for (int i = 0; i < this._gameBoardFields.Length; i++)
            {
                this._gameBoardFields[i].Piece = gameManager.GameBoard._gameBoardFields[i].Piece;
            }
        }
    }
}
