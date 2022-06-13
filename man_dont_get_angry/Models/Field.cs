using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using man_dont_get_angry.ModelUtils;

namespace man_dont_get_angry.Models
{
    /// <summary>
    /// Represents a field on the gameboard
    /// </summary>
    public class Field : INotifyPropertyChanged
    {
        /// <summary>
        /// For saving the piece which is on the field
        /// </summary>
        private Piece? _piece;

        /// <summary>
        /// For saving the type of which the field is
        /// </summary>
        private FieldType? _fieldType;

        /// <summary>
        /// For saving the path of the image which should be shown on the dice button on the gameboard
        /// </summary>
        private string? _imagePath;

        /// <summary>
        /// Event which is to be raised when a property changes from which the value should be updated
        /// in the main window
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Constructor needed for serializing/deserializing
        /// </summary>
        private Field()
        { }

        /// <summary>
        /// Constructor which is used for creating a Field without containing a piece
        /// </summary>
        /// <param name="fieldType">Type of which the field is, which are defined in ModelUtils.Constants</param>
        public Field(FieldType fieldType)
        {
            this._fieldType = fieldType;
            this._piece = null;

            // set the correct image path up
            ImagePathSetter();
        }

        /// <summary>
        /// Constructor which is used for creating a Field containing a piece
        /// </summary>
        /// <param name="fieldType">Type of which the field is, which are defined in ModelUtils.Constants</param>
        /// <param name="piece">Piece which should be on the field</param>
        public Field(FieldType fieldType, Piece piece)
        {
            this._fieldType = fieldType;
            this._piece = piece;

            // set the correct image path up
            ImagePathSetter();
        }

        /// <summary>
        /// Gets/Sets a piece on the field
        /// </summary>
        public Piece? Piece
        {
            get { return this._piece; }
            set
            {
                this._piece = value;
                ImagePathSetter();
                OnPropertyChanged("ImagePath");
            }
        }

        /// <summary>
        /// Get/Set the image path of the image on the dice button
        /// </summary>
        public string? ImagePath
        {
            get { return this._imagePath; }
            set { this._imagePath = value; }
        }

        /// <summary>
        /// Get/Set the field type of the field
        /// </summary>
        public FieldType? FieldType
        {
            get { return this._fieldType; }
            set { this._fieldType = value; }
        }

        /// <summary>
        /// Sets up the correct image path for the field depending on the fieldType and whether
        /// a piece stands on the field or not
        /// </summary>
        private void ImagePathSetter()
        {
            switch (this._fieldType)
            {
                case ModelUtils.FieldType.NormalField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_" + PieceColorStringGenerator() + "_player.png";
                    }
                    break;
                case ModelUtils.FieldType.BlueArrowField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_blue_arrow.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_" + PieceColorStringGenerator() + "_player.png";
                    }
                    break;
                case ModelUtils.FieldType.GreenArrowField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_green_arrow.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_" + PieceColorStringGenerator() + "_player.png";
                    }
                    break;
                case ModelUtils.FieldType.RedArrowField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_red_arrow.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_" + PieceColorStringGenerator() + "_player.png";
                    }
                    break;
                case ModelUtils.FieldType.YellowArrowField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_yellow_arrow.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_" + PieceColorStringGenerator() + "_player.png";
                    }
                    break;
                case ModelUtils.FieldType.GreenStartField:
                case ModelUtils.FieldType.GreenEndField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_green.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_green_player_green.png";
                    }
                    break;
                case ModelUtils.FieldType.YellowStartField:
                case ModelUtils.FieldType.YellowEndField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_yellow.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_yellow_player_yellow.png";
                    }
                    break;
                case ModelUtils.FieldType.RedStartField:
                case ModelUtils.FieldType.RedEndField:
                    if (this._piece == null)
                    {
                        this._imagePath = "\\images\\square_red.png";
                    }
                    else
                    {
                        this._imagePath = "\\images\\square_red_player_red.png";
                    }
                    break;
                case ModelUtils.FieldType.BlueStartField:
                case ModelUtils.FieldType.BlueEndField:
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

        /// <summary>
        /// Generates a string which is used for inserting it into the image paths, so that code hasnt to be repeated as much
        /// </summary>
        /// <returns>Generated string for inserting into the image paths</returns>
        private string PieceColorStringGenerator()
        {
            switch (this._piece?.Color)
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
