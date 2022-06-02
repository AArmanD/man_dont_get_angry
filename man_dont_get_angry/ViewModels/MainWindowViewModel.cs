using System;
using man_dont_get_angry.ViewModelUtils;
using man_dont_get_angry.Models;

namespace man_dont_get_angry.ViewModels
{
    /// <summary>
    /// ViewModel for the main window
    /// </summary>
    internal class MainWindowViewModel
    {
        /// <summary>
        /// Get/Set GameManager instance for controlling the game
        /// </summary>
        public GameManager ManDontGetAngryGame { get; set; }

        /// <summary>
        /// Get command which handles a clíck on the dice
        /// </summary>
        public DelegateCommand DiceClickedCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Get command which handles a click on one of the fields on the game board
        /// </summary>
        public DelegateCommand FieldClickedCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Get command which handles a click on the reset game button
        /// </summary>
        public DelegateCommand ResetClickedCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Get command which handles a click on the save as xml button
        /// </summary>
        public DelegateCommand SaveAsXMLClickedCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Get command which handles a click on the load from xml button
        /// </summary>
        public DelegateCommand LoadXMLClickedCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Shortcut for gui to get dice properties
        /// </summary>
        public Dice TheDice => this.ManDontGetAngryGame.TheDice;

        /// <summary>
        /// Shortcut for gui to get game board field properties
        /// </summary>
        public Field[] GameBoardFields => this.ManDontGetAngryGame.TheGameBoard.GameBoardFields;

        /// <summary>
        /// Shortcut for gui to get start field properties
        /// </summary>
        public Field[] StartFields => this.ManDontGetAngryGame.TheGameBoard.StartFields;

        /// <summary>
        /// Shortcut for gui to get end field properties
        /// </summary>
        public Field[] EndFields => this.ManDontGetAngryGame.TheGameBoard.EndFields;

        /// <summary>
        /// Shortcut for gui to get player properties
        /// </summary>
        public Player[] Players => this.ManDontGetAngryGame.Players;

        /// <summary>
        /// Constructor for creating a VievModel object
        /// </summary>
        public MainWindowViewModel()
        {
            ManDontGetAngryGame = new GameManager();

            DiceClickedCommand = new DelegateCommand(OnDiceButtonClicked, OnDiceButtonClickAllowed);
            FieldClickedCommand = new DelegateCommand(OnFieldClicked, OnFieldClickAllowed);
            ResetClickedCommand = new DelegateCommand(OnResetClicked, OnResetClickAllowed);
            SaveAsXMLClickedCommand = new DelegateCommand(OnSaveAsXMLClicked, OnResetClickAllowed);
            LoadXMLClickedCommand = new DelegateCommand(OnLoadXMLClicked, OnLoadXMLClickeAllowed);
        }


        /// <summary>
        /// Handler function which is called when the DicebuttonClickedCommand is run
        /// </summary>
        /// <param name="arg">not used</param>
        public void OnDiceButtonClicked(Object arg)
        {
            this.ManDontGetAngryGame.RollDice();
        }

        /// <summary>
        /// Handler function which is checks whether the command is runnable before running the handler function
        /// when the DicebuttonClickedCommand is run
        /// </summary>
        /// <param name="arg">not used</param>
        /// <returns>true, when handler function is allowed to run, otherwise false</returns>
        public bool OnDiceButtonClickAllowed(Object arg)
        {
            return true;
        }

        /// <summary>
        /// Handler function which is called when the FieldClickedCommand is run
        /// </summary>
        /// <param name="arg">Number of the field which is clicked</param>
        public void OnFieldClicked(Object arg)
        {
            string? fieldNumber = (string?)arg;
            if (!string.IsNullOrEmpty(fieldNumber))
                this.ManDontGetAngryGame.setPosition(int.Parse(fieldNumber));
        }

        /// <summary>
        /// Handler function which is checks whether the command is runnable before running the handler function
        /// when the FieldClickedCommand is run
        /// </summary>
        /// <param name="arg">number of the field which is clicked</param>
        /// <returns>true, when handler function is allowed to run, otherwise false</returns>
        public bool OnFieldClickAllowed(Object arg)
        {
            // TODO implement that
            return true;
            //return this.ManDontGetAngryGame.positionSettable(int.Parse(arg.ToString()));
        }

        /// <summary>
        /// Handler function which is called when the ResetClickedCommand is run
        /// </summary>
        /// <param name="arg">not used</param>
        public void OnResetClicked(Object arg)
        {
            this.ManDontGetAngryGame.ResetGame();
        }

        /// <summary>
        /// Handler function which is checks whether the command is runnable before running the handler function
        /// when the ResetClickedCommand is run
        /// </summary>
        /// <param name="arg">not used</param>
        /// <returns>true, when handler function is allowed to run, otherwise false</returns>
        public bool OnResetClickAllowed(Object arg)
        {
            // make only possible when game is already started
            return true;
        }

        /// <summary>
        /// Handler function which is called when the SaveAsXMLClickedCommand is run
        /// </summary>
        /// <param name="arg">not used</param>
        public void OnSaveAsXMLClicked(Object arg)
        {
            PopupWindowHandler.HandleSaveFile(this.ManDontGetAngryGame);
        }

        /// <summary>
        /// Handler function which is checks whether the command is runnable before running the handler function
        /// when the SaveAsXMLClickedCommand is run
        /// </summary>
        /// <param name="arg">not used</param>
        /// <returns>true, when handler function is allowed to run, otherwise false</returns>
        public bool OnSaveAsXMLClickAllowed(Object arg)
        {
            return true;
        }

        /// <summary>
        /// Handler function which is called when the LoadXMLClickedCommand is run
        /// </summary>
        /// <param name="arg">not used</param>
        public void OnLoadXMLClicked(Object arg)
        {
            PopupWindowHandler.HandleOpenFile(this.ManDontGetAngryGame);
        }

        /// <summary>
        /// Handler function which is checks whether the command is runnable before running the handler function
        /// when the LoadXMLClickedCommand is run
        /// </summary>
        /// <param name="arg">not used</param>
        /// <returns>true, when handler function is allowed to run, otherwise false</returns>
        public bool OnLoadXMLClickeAllowed(Object arg)
        {
            return true;
        }
    }
}
