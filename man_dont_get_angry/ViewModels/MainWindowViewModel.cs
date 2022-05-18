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
            return this.ManDontGetAngryGame.DiceRollable();
        }

        public void OnFieldClicked(Object arg)
        { 

        }

        public bool OnFieldClickAllowed(Object arg)
        {
            return this.ManDontGetAngryGame.positionSettable(int.Parse(arg.ToString()));
        }

        public Field[] GameBoardFields => this.ManDontGetAngryGame.TheGameBoard.GameBoardFields;
        public Field[] StartFields => this.ManDontGetAngryGame.TheGameBoard.StartFields;
        public Field[] EndFields => this.ManDontGetAngryGame.TheGameBoard.EndFields;
    }
}
