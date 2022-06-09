using man_dont_get_angry.Models;
using System;
using System.Threading;

namespace man_dont_get_angry.ModelUtils
{
    /// <summary>
    /// Implementation an auto player which uses a thread for automatic moving of players
    /// </summary>
    public class AutoPlayerManager
    {
        /// <summary>
        /// Saves gameManager for getting needed data
        /// </summary>
        private GameManager _gameManager;

        /// <summary>
        /// Controls the thread
        /// </summary>
        private bool _threadRunning;

        /// <summary>
        /// Saves the thread instance to be able to get data of its state
        /// </summary>
        private Thread _thread;

        /// <summary>
        /// Constructor for creating a thread manager object
        /// </summary>
        /// <param name="gameManager">Instance of the GameManager</param>
        public AutoPlayerManager(GameManager gameManager)
        {
            this._gameManager = gameManager;
            this._threadRunning = false;
            this._thread = new Thread(Run);
        }

        /// <summary>
        /// Is run when player should play automatically in a thread
        /// </summary>
        private void Run()
        {
            var random = new Random();
            while (this._threadRunning && (_gameManager.ActualPlayer.IsAutomatic ?? false) && !OptionsChecker.CheckGameWon(this._gameManager.Players[this._gameManager.LastPlayerID], this._gameManager.GameBoard.EndFields))
            {
                if (this._gameManager.ActualPlayer.PlayerState == ModelUtils.PlayerState.ThrowDice)
                {
                    this._gameManager.RollDice();
                    Thread.Sleep(1000);
                }
                else
                {
                    int index = random.Next(this._gameManager.MovementOptions.Count);
                    this._gameManager.SetPosition(this._gameManager.MovementOptions[index].EndPosition);
                    Thread.Sleep(1000);
                }
            }

            this._threadRunning = false;
        }

        /// <summary>
        /// Starts the automatic player thread
        /// </summary>
        /// <param name="playerID">specifies player which has initiated the start for avoiding starting double threads</param>
        public void StartAutoPlayer(int playerID = -1)
        {
            if (playerID == -1 || playerID == this._gameManager.PlayerID)
            {
                this._thread = new Thread(this.Run);
                this._threadRunning = true;
                this._thread.IsBackground = true;
                this._thread.Start();
            }
        }

        /// <summary>
        /// Stops the automatic player
        /// </summary>
        private void StopAutoPlayer()
        {
            this._threadRunning = false;
        }

        /// <summary>
        /// Returns whether the automatic player thread runs
        /// </summary>
        /// <returns>true if the auto player thread is alive, otherwise false</returns>
        public bool AutoPlayerRunning()
        {
            return this._thread.IsAlive;
        }

        /// <summary>
        /// Starts/Stops the auto player thread for saving/loading an actual game
        /// </summary>
        /// <param name="state">State to set the thread to: true-> running, false -> stopped</param>
        public void SetAutoPlayerState(bool state)
        {
            if (state)
            {
                if (!this._thread.IsAlive && (_gameManager.ActualPlayer.IsAutomatic ?? false))
                {
                    this.StartAutoPlayer();
                }
            }
            else
            {
                this.StopAutoPlayer();
            }
        }
    }
}
