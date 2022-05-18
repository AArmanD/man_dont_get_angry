using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using man_dont_get_angry.ViewModelUtils;

namespace man_dont_get_angry.Models
{
    internal class Dice : INotifyPropertyChanged
    {
        private int _value;
        private System.Random _random;
        private int _diceThrownNumber;
        private string _imagePath;

        private int _testCounter;

        public Dice()
        {
            this._testCounter = 0;

            this._value = 1;
            this._random = new System.Random();
            this._diceThrownNumber = 0;
            this._imagePath = "\\images\\dice_1.png";
        }

        public void roll()
        {
            this._value = _random.Next(1, 7);
            
            //if (this._testCounter != 10)
            //{
            //    this._value = 3;
            //}
            //else
            //{
            //    this._value = 6;
            //}

            this._testCounter++;
            
            this._diceThrownNumber++;
            this._imagePath = "\\images\\dice_" + this._value + ".png";
            OnPropertyChanged("ImagePath");
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

        public string ImagePath
        {
            get { return this._imagePath; }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
