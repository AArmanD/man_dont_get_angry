using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using man_dont_get_angry.ViewModelUtils;
using man_dont_get_angry.Models;

namespace man_dont_get_angry.ViewModels
{
    // gelb ins ziel kommen error
    internal class MainWindowViewModel
    {
        public GameManager ManDontGetAngryGame { get; }

        public DelegateCommand DiceClickedCommand
        {
            get;
            private set;
        }

        public DelegateCommand FieldClickedCommand
        {
            get;
            private set;
        }

        public MainWindowViewModel()
        {
            ManDontGetAngryGame = new GameManager();

            DiceClickedCommand = new DelegateCommand(OnDiceButtonClicked, OnDiceButtonClickAllowed);
            FieldClickedCommand = new DelegateCommand(OnFieldClicked, OnFieldClickAllowed);
        }

        public void OnDiceButtonClicked(Object arg)
        {
            this.ManDontGetAngryGame.RollDice();
        }

        public bool OnDiceButtonClickAllowed(Object arg)
        {
            return true;
            //return this.ManDontGetAngryGame.DiceRollable();
        }

        public void OnFieldClicked(Object arg)
        {
            this.ManDontGetAngryGame.setPosition(int.Parse(arg.ToString()));
        }

        public bool OnFieldClickAllowed(Object arg)
        {
            return true;
            //return this.ManDontGetAngryGame.positionSettable(int.Parse(arg.ToString()));
        }

        public Dice TheDice => this.ManDontGetAngryGame.TheDice;

        public Field[] GameBoardFields => this.ManDontGetAngryGame.TheGameBoard.GameBoardFields;
        public Field[] StartFields => this.ManDontGetAngryGame.TheGameBoard.StartFields;
        public Field[] EndFields => this.ManDontGetAngryGame.TheGameBoard.EndFields;

        public string ActualPlayer => this.ManDontGetAngryGame.ActualPlayer.Name;
    }
}
