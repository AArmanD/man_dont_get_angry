using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace man_dont_get_angry.Models
{
    /// <summary>
    /// Represents a Dice in the Game
    /// </summary>
    public class Dice : INotifyPropertyChanged
    {
        /// <summary>
        /// For saving the value of the dice
        /// </summary>
        private int _value;

        /// <summary>
        /// For saving instance of Random to generate random numbers
        /// </summary>
        private Random _random;

        /// <summary>
        /// For saving the number of times the dice has been thrown in one round
        /// </summary>
        private int _diceThrownNumber;

        /// <summary>
        /// For saving the path of the image which should be shown on the dice button on the gameboard
        /// </summary>
        private string _imagePath;

        /// <summary>
        /// Event which is to be raised when a property changes from which the value should be updated
        /// in the main window
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Constructor for creating a dice object
        /// </summary>
        public Dice()
        {
            this._value = 1;
            this._random = new Random();
            this._diceThrownNumber = 0;
            this._imagePath = "\\images\\dice_1.png";
        }

        /// <summary>
        /// Get/Set the dice value
        /// </summary>
        public int Value
        {
            get { return _value; }
            set { this._value = value; }
        }

        /// <summary>
        /// Get/Set the dice thrown number
        /// </summary>
        public int DiceThrownNumber
        {
            get { return this._diceThrownNumber; }
            set { this._diceThrownNumber = value; }
        }

        /// <summary>
        /// Get/Set the image path of the image on the dice button
        /// </summary>
        public string ImagePath
        {
            get { return this._imagePath; }
            set { this._imagePath = value; }
        }

        /// <summary>
        /// Rolls virtual dice
        /// </summary>
        public void Roll()
        {
            // create new random value
            this._value = _random.Next(1, 7);

            // increment thrown number for this round
            this._diceThrownNumber++;

            // change image which represents dice number on the game board
            this._imagePath = "\\images\\dice_" + this._value + ".png";
            OnPropertyChanged("ImagePath");
        }

        /// <summary>
        /// Resets the dice so it can be used again in the next round
        /// </summary>
        public void Reset()
        {
            this._diceThrownNumber = 0;
        }

        /// <summary>
        /// Handler for The PropertyChangedEvent, which is for updating the value of the property 
        /// in the gui
        /// </summary>
        /// <param name="prop">Name of the property of which the value should be updated in the gui</param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
