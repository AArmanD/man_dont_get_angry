using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace man_dont_get_angry.Models
{
    public class MovementOption
    {
        private int _startPosition;
        private int _endPosition;

        private MovementOption()
        { }

        public MovementOption(int startPosition, int endPosition)
        {
            this._startPosition = startPosition;
            this._endPosition = endPosition;
        }

        public int StartPosition
        { 
            get { return this._startPosition; }
            set { this._startPosition = value; }
        }

        public int EndPosition
        {
            get { return this._endPosition; }
            set { this._endPosition = value; }
        }
    }
}
