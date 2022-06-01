using System;
using man_dont_get_angry.ViewModelUtils;
using man_dont_get_angry.Models;

namespace man_dont_get_angry.ViewModels
{
    internal class MainWindowViewModel
    {
        public GameManager ManDontGetAngryGame { get; set; }

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

        public DelegateCommand ResetClickedCommand
        {
            get;
            private set;
        }

        public DelegateCommand SaveAsXMLClickedCommand
        {
            get;
            private set;
        }

        public DelegateCommand LoadXMLClickedCommand
        {
            get;
            private set;
        }

        public MainWindowViewModel()
        {
            ManDontGetAngryGame = new GameManager();

            DiceClickedCommand = new DelegateCommand(OnDiceButtonClicked, OnDiceButtonClickAllowed);
            FieldClickedCommand = new DelegateCommand(OnFieldClicked, OnFieldClickAllowed);
            ResetClickedCommand = new DelegateCommand(OnResetClicked, OnResetClickAllowed);
            SaveAsXMLClickedCommand = new DelegateCommand(OnSaveAsXMLClicked, OnResetClickAllowed);
            LoadXMLClickedCommand = new DelegateCommand(OnLoadXMLClicked, OnLoadXMLClickeAllowed);
        }

        public void OnDiceButtonClicked(Object arg)
        {
            this.ManDontGetAngryGame.RollDice();
        }

        public bool OnDiceButtonClickAllowed(Object arg)
        {
            return true;
        }

        public void OnFieldClicked(Object arg)
        {
            string? fieldNumber = (string?)arg;
            if (!string.IsNullOrEmpty(fieldNumber))
                this.ManDontGetAngryGame.setPosition(int.Parse(fieldNumber));
        }

        public bool OnFieldClickAllowed(Object arg)
        {
            return true;
            //return this.ManDontGetAngryGame.positionSettable(int.Parse(arg.ToString()));
        }

        public void OnResetClicked(Object arg)
        {
            this.ManDontGetAngryGame.ResetGame();
        }

        public bool OnResetClickAllowed(Object arg)
        {
            // make only possible when game is already started
            return true;
        }

        public void OnSaveAsXMLClicked(Object arg)
        {
            PopupWindowHandler.HandleSaveFile(this.ManDontGetAngryGame);
        }

        public bool OnSaveAsXMLClickAllowed(Object arg)
        {
            return true;
        }

        public void OnLoadXMLClicked(Object arg)
        {
            PopupWindowHandler.HandleOpenFile(this.ManDontGetAngryGame);
        }

        public bool OnLoadXMLClickeAllowed(Object arg)
        {
            return true;
        }

        public Dice TheDice => this.ManDontGetAngryGame.TheDice;

        public Field[] GameBoardFields => this.ManDontGetAngryGame.TheGameBoard.GameBoardFields;
        public Field[] StartFields => this.ManDontGetAngryGame.TheGameBoard.StartFields;
        public Field[] EndFields => this.ManDontGetAngryGame.TheGameBoard.EndFields;

        public Player[] Players => this.ManDontGetAngryGame.Players;
    }
}
