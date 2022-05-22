using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using man_dont_get_angry.ModelUtils;

namespace man_dont_get_angry.Models
{
    internal class Field : INotifyPropertyChanged
    {
        private Piece? _piece;
        private string _imagePath;
        private FieldType _fieldType;

        public Field(FieldType fieldType)
        {
            this._fieldType = fieldType;
            this._piece = null;
            ImagePathSetter();
        }

        public Field(FieldType fieldType, Piece piece)
        {
            this._fieldType = fieldType;
            this._piece = piece;

            ImagePathSetter();
        }

        private void ImagePathSetter()
        {
            switch (this._fieldType)
            {
                case FieldType.NormalField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_" + PieceColorStringGenerator() + "_player.png";
                    }
                    break;
                case FieldType.BlueArrowField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_blue_arrow.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_" + PieceColorStringGenerator() + "_player.png";
                    }
                    break;
                case FieldType.GreenArrowField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_green_arrow.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_" + PieceColorStringGenerator() + "_player.png";
                    }
                    break;
                case FieldType.RedArrowField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_red_arrow.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_" + PieceColorStringGenerator() + "_player.png";
                    }
                    break;
                case FieldType.YellowArrowField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_yellow_arrow.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_" + PieceColorStringGenerator() + "_player.png";
                    }
                    break;
                case FieldType.GreenStartField:
                case FieldType.GreenEndField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_green.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_green_player_green.png";
                    }
                    break;
                case FieldType.YellowStartField:
                case FieldType.YellowEndField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_yellow.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_yellow_player_yellow.png";
                    }
                    break;
                case FieldType.RedStartField:
                case FieldType.RedEndField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_red.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_red_player_red.png";
                    }
                    break;
                case FieldType.BlueStartField:
                case FieldType.BlueEndField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_blue.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_blue_player_blue.png";
                    }
                    break;
            }
        }

        private string PieceColorStringGenerator()
        {
            switch (this._piece.TheColor)
            {
                case Color.Red:
                    return "red";
                case Color.Green:
                    return "green";
                case Color.Blue:
                    return "blue";
                case Color.Yellow:
                    return "yellow";
                default:
                    return "";
            }
        }

        public Piece ThePiece
        {
            get { return this._piece; }
            set 
            {
                this._piece = value;
                ImagePathSetter();
                OnPropertyChanged("ImagePath");
            }
        }

        public string ImagePath
        {
            get { return this._imagePath; }
        }

        public FieldType TheFieldType
        {
            get { return this._fieldType; }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
