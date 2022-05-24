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

        private string _imageSource;


        public GameBoard()
        {
            this._startFields = new Field[16];
            this._endFields = new Field[16];
            this._gameBoardFields = new Field[40];
            this._imageSource = "\\images\\square.png";

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
        }

        public Field[] StartFields
        {
            get { return _startFields; }
        }

        public Field[] EndFields
        {
            get { return _endFields; }
        }

        public string ImageSource
        {
            get { return _imageSource; }
        }

        public void setPiece(Tuple<int, int> setTuple)
        {
            Piece pieceToSet = null;
            if (setTuple.Item1 < 40)
            {
                pieceToSet = this._gameBoardFields[setTuple.Item1].ThePiece;
                this._gameBoardFields[setTuple.Item1].ThePiece = null;
            }
            else if (setTuple.Item1 > 39 && setTuple.Item1 < 56)
            {
                pieceToSet = this._startFields[setTuple.Item1 - 40].ThePiece;
                this._startFields[setTuple.Item1 - 40].ThePiece = null;
            }
            else if (setTuple.Item1 > 55 && setTuple.Item1 < 72)
            {
                pieceToSet = this.EndFields[setTuple.Item1 - 56].ThePiece;
                this.EndFields[setTuple.Item1 - 56].ThePiece = null;
            }

            if (setTuple.Item2 < 40)
            {
                if (this._gameBoardFields[setTuple.Item2].ThePiece != null)
                {
                    // move piece on field back to start
                    int count = 3;
                    switch (this._gameBoardFields[setTuple.Item2].ThePiece.TheColor)
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
                            this._startFields[i].ThePiece = this._gameBoardFields[setTuple.Item2].ThePiece;
                            break;
                        }
                    }
                }

                this._gameBoardFields[setTuple.Item2].ThePiece = pieceToSet;

            }
            else if (setTuple.Item2 > 39 && setTuple.Item2 < 56)
            {
                this._startFields[setTuple.Item2 - 40].ThePiece = pieceToSet;
            }
            else if (setTuple.Item2 > 55 && setTuple.Item2 < 72)
            {
                // here throw out check is not needed because cannot happen
                this.EndFields[setTuple.Item2 - 56].ThePiece = pieceToSet;
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
