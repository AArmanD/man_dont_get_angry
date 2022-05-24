using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using man_dont_get_angry.ViewModelUtils;
using man_dont_get_angry.Models;
using System.Threading;
using man_dont_get_angry.ModelUtils;

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

        public DelegateCommand CheckBoxPlayerGreenAutoCommand
        {
            get;
            private set;
        }
        public DelegateCommand CheckBoxPlayerRedAutoCommand
        {
            get;
            private set;
        }
        public DelegateCommand CheckBoxPlayerYellowAutoCommand
        {
            get;
            private set;
        }
        public DelegateCommand CheckBoxPlayerBlueAutoCommand
        {
            get;
            private set;
        }

        public MainWindowViewModel()
        {
            ManDontGetAngryGame = new GameManager();

            DiceClickedCommand = new DelegateCommand(OnDiceButtonClicked, OnDiceButtonClickAllowed);
            FieldClickedCommand = new DelegateCommand(OnFieldClicked, OnFieldClickAllowed);
            CheckBoxPlayerGreenAutoCommand = new DelegateCommand(OnCheckBoxPlayerGreenAutoClicked, OnCheckBoxPlayerGreenAutoClickAllowed);
            CheckBoxPlayerRedAutoCommand = new DelegateCommand(OnCheckBoxPlayerRedAutoClicked, OnCheckBoxPlayerRedAutoClickAllowed);
            CheckBoxPlayerYellowAutoCommand = new DelegateCommand(OnCheckBoxPlayerYellowAutoClicked, OnCheckBoxPlayerYellowAutoClickAllowed);
            CheckBoxPlayerBlueAutoCommand = new DelegateCommand(OnCheckBoxPlayerBlueAutoClicked, OnCheckBoxPlayerBlueAutoClickAllowed);
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

        public void OnCheckBoxPlayerGreenAutoClicked(Object arg)
        {
            this.ManDontGetAngryGame.Players[0].IsAutomatic = (bool)arg;
            this.ManDontGetAngryGame.StartAutoThread();
        }

        public bool OnCheckBoxPlayerGreenAutoClickAllowed(Object arg)
        {
            return true;
        }

        public void OnCheckBoxPlayerRedAutoClicked(Object arg)
        {
            this.ManDontGetAngryGame.Players[1].IsAutomatic = (bool)arg;
            this.ManDontGetAngryGame.StartAutoThread();
        }

        public bool OnCheckBoxPlayerRedAutoClickAllowed(Object arg)
        {
            return true;
        }

        public void OnCheckBoxPlayerYellowAutoClicked(Object arg)
        {
            this.ManDontGetAngryGame.Players[2].IsAutomatic = (bool)arg;
            this.ManDontGetAngryGame.StartAutoThread();
        }

        public bool OnCheckBoxPlayerYellowAutoClickAllowed(Object arg)
        {
            return true;
        }

        public void OnCheckBoxPlayerBlueAutoClicked(Object arg)
        {
            this.ManDontGetAngryGame.Players[3].IsAutomatic = (bool)arg;
            this.ManDontGetAngryGame.StartAutoThread();
        }

        public bool OnCheckBoxPlayerBlueAutoClickAllowed(Object arg)
        {
            return true;
        }

        public Dice TheDice => this.ManDontGetAngryGame.TheDice;

        public Field[] GameBoardFields => this.ManDontGetAngryGame.TheGameBoard.GameBoardFields;
        public Field[] StartFields => this.ManDontGetAngryGame.TheGameBoard.StartFields;
        public Field[] EndFields => this.ManDontGetAngryGame.TheGameBoard.EndFields;

        public string ActualPlayer => this.ManDontGetAngryGame.ActualPlayer.Name;
        public string ActualMove => this.ManDontGetAngryGame.ActualMove;

        public Player[] Players => this.ManDontGetAngryGame.Players;
    }
}
