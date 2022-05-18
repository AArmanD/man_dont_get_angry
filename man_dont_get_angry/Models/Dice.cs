using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace man_dont_get_angry.Models
{
    internal class Dice
    {
        private int _value;
        private System.Random _random;
        private int _diceThrownNumber;

        public Dice()
        {
            this._value = 1;
            this._random = new System.Random();
            this._diceThrownNumber = 0;
        }

        public void roll()
        {
            this._value = _random.Next(1, 7);
            this._diceThrownNumber++;
        }

        public int Value
        {
            get { return _value; }
        }

        public int DiceThrownNumber
        {
            get { return this._diceThrownNumber; }
        }

        public void resetDice()
        {
            this._diceThrownNumber = 0;
        }
    }
}
