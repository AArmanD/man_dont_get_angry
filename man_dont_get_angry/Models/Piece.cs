using man_dont_get_angry.ModelUtils;

namespace man_dont_get_angry.Models
{
    /// <summary>
    /// Represents a piece in the game
    /// </summary>
    public class Piece
    {
        /// <summary>
        /// For saving the color of the piece
        /// </summary>
        private Color? _pieceColor;

        /// <summary>
        /// Constructor needed for serializing/deserializing
        /// </summary>
        private Piece()
        { }

        /// <summary>
        /// Constructor for creating a color object
        /// </summary>
        /// <param name="color"></param>
        public Piece(Color color)
        {
            this._pieceColor = color;
        }

        /// <summary>
        /// Get/Set the color of the piece
        /// </summary>
        public Color? TheColor
        {
            get { return _pieceColor; }
            set { _pieceColor = value; }
        }
    }
}
