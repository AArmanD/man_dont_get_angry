namespace man_dont_get_angry.Models
{
    /// <summary>
    /// Class for saving a movement option
    /// </summary>
    public class MovementOption
    {
        /// <summary>
        /// For saving field to move from
        /// </summary>
        private int _startPosition;

        /// <summary>
        /// For saving field to move to
        /// </summary>
        private int _endPosition;

        /// <summary>
        /// Constructor needed for serializing/deserializing
        /// </summary>
        private MovementOption()
        { }

        /// <summary>
        /// Constructor for creating a MovementOption object
        /// </summary>
        /// <param name="startPosition">Start position to move piece from</param>
        /// <param name="endPosition">End position to move piece to</param>
        public MovementOption(int startPosition, int endPosition)
        {
            this._startPosition = startPosition;
            this._endPosition = endPosition;
        }

        /// <summary>
        /// Get/Set start position
        /// </summary>
        public int StartPosition
        {
            get { return this._startPosition; }
            set { this._startPosition = value; }
        }

        /// <summary>
        /// Get/Set end position
        /// </summary>
        public int EndPosition
        {
            get { return this._endPosition; }
            set { this._endPosition = value; }
        }
    }
}
