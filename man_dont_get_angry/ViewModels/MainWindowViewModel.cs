using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using man_dont_get_angry.ViewModelUtils;
using man_dont_get_angry.Models;
using System.IO;

namespace man_dont_get_angry.ViewModels
{
    // gelb ins ziel kommen error
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

        public MainWindowViewModel()
        {
            ManDontGetAngryGame = new GameManager();

            DiceClickedCommand = new DelegateCommand(OnDiceButtonClicked, OnDiceButtonClickAllowed);
            FieldClickedCommand = new DelegateCommand(OnFieldClicked, OnFieldClickAllowed);
            ResetClickedCommand = new DelegateCommand(OnResetClicked, OnResetClickAllowed);
            SaveAsXMLClickedCommand = new DelegateCommand(OnSaveAsXMLClicked, OnResetClickAllowed);
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
            this.ManDontGetAngryGame.SetAutoThread(false);
            TextWriter writer = null;
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(ManDontGetAngryGame.GetType());
                writer = new StreamWriter(@"C:\Temp\Lol.xml", false);
                x.Serialize(writer, ManDontGetAngryGame);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

            this.ManDontGetAngryGame.SetAutoThread(true);
        }

        public bool OnSaveAsXMLClickAllowed(Object arg)
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
