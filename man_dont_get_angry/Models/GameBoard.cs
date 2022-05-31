using System;
using System.Collections.Generic;
using System.Linq;
using man_dont_get_angry.ModelUtils;

namespace man_dont_get_angry.Models
{
    public class GameBoard
    {
        private Field[] _gameBoardFields;
        private Field[] _startFields;
        private Field[] _endFields;


        public GameBoard()
        {
            this._startFields = new Field[16];
            this._endFields = new Field[16];
            this._gameBoardFields = new Field[40];

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

        public Field[] GameBoardFields
        {
            get { return _gameBoardFields; }
            set { _gameBoardFields = value; }
        }

        public Field[] StartFields
        {
            get { return _startFields; }
            set { _startFields = value; }
        }

        public Field[] EndFields
        {
            get { return _endFields; }
            set { _endFields = value; }
        }

        public void setPiece(MovementOption setTuple)
        {
            Piece pieceToSet = null;
            if (setTuple.StartPosition < 40)
            {
                pieceToSet = this._gameBoardFields[setTuple.StartPosition].ThePiece;
                this._gameBoardFields[setTuple.StartPosition].ThePiece = null;
            }
            else if (setTuple.StartPosition > 39 && setTuple.StartPosition < 56)
            {
                pieceToSet = this._startFields[setTuple.StartPosition - 40].ThePiece;
                this._startFields[setTuple.StartPosition - 40].ThePiece = null;
            }
            else if (setTuple.StartPosition > 55 && setTuple.StartPosition < 72)
            {
                pieceToSet = this.EndFields[setTuple.StartPosition - 56].ThePiece;
                this.EndFields[setTuple.StartPosition - 56].ThePiece = null;
            }

            if (setTuple.EndPosition < 40)
            {
                if (this._gameBoardFields[setTuple.EndPosition].ThePiece != null)
                {
                    // move piece on field back to start
                    int count = 3;
                    switch (this._gameBoardFields[setTuple.EndPosition].ThePiece.TheColor)
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
                        if (this._startFields[i].ThePiece == null)
                        {
                            this._startFields[i].ThePiece = this._gameBoardFields[setTuple.EndPosition].ThePiece;
                            break;
                        }
                    }
                }

                this._gameBoardFields[setTuple.EndPosition].ThePiece = pieceToSet;

            }
            else if (setTuple.EndPosition > 39 && setTuple.EndPosition < 56)
            {
                this._startFields[setTuple.EndPosition - 40].ThePiece = pieceToSet;
            }
            else if (setTuple.EndPosition > 55 && setTuple.EndPosition < 72)
            {
                // here throw out check is not needed because cannot happen
                this.EndFields[setTuple.EndPosition - 56].ThePiece = pieceToSet;
            }
        }

        public void SetupStartPostitions()
        {
            for (int count = 0; count < _startFields.Length; count++)
            {
                if (-1 < count && count < 4)
                {
                    this._startFields[count].ThePiece = new Piece(Color.Green);
                }
                else if (3 < count && count < 8)
                {
                    this._startFields[count].ThePiece = new Piece(Color.Red);
                }
                else if (7 < count && count < 12)
                {
                    this._startFields[count].ThePiece = new Piece(Color.Yellow);
                }
                else if (11 < count && count < 16)
                {
                    this._startFields[count].ThePiece = new Piece(Color.Blue);
                }
            }

            for (int i = 0; i < _endFields.Length; i++)
            {
                _endFields[i].ThePiece = null;
            }

            for (int i = 0; i < this._gameBoardFields.Length; i++)
            {
                this._gameBoardFields[i].ThePiece = null;
            }
        }
    }
}
