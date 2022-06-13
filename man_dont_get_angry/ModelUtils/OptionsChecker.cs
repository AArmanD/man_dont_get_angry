using man_dont_get_angry.Models;
using System.Collections.Generic;
using System.Linq;

namespace man_dont_get_angry.ModelUtils
{
    /// <summary>
    /// Class for calculating logic for the GameManager
    /// </summary>
    internal static class OptionsChecker
    {
        /// <summary>
        /// Checks the Piece positions of a player in a specified field array and returns them in a list
        /// </summary>
        /// <param name="player">Player for which the piece positions should be found</param>
        /// <param name="fieldArray">Array of fields -> gameBoardfields, startFields, endfields</param>
        /// <returns>Lists with the positions at which the pieces of the player are</returns>
        private static List<int> CheckPlayerPositions(Player player, Field[] fieldArray)
        {
            List<int> playerPositions = new List<int>();
            for (int i = 0; i < fieldArray.Length; i++)
                if (fieldArray[i].Piece != null)
                {
                    if (player.Color == fieldArray[i].Piece?.Color)
                    {
                        playerPositions.Add(i);
                    }
                }
            return playerPositions;
        }

        /// <summary>
        /// Generates a player state depending on the game state
        /// </summary>
        /// <param name="movementOptions">Options of moving pieces of the specified player</param>
        /// <param name="dice">Dice for getting dice attributes</param>
        /// <param name="player">Player for which the state should be generated</param>
        /// <param name="endFields">End fields of the gameboard</param>
        /// <returns>Generated player state</returns>
        public static PlayerState GenerateStateAfterRolling(List<MovementOption> movementOptions, Dice dice, Player player, Field[] endFields)
        {

            // when the dice value is 6 and there is a movement option, let player move and dice again
            if (dice.Value == 6 && movementOptions.Count > 0)
            {
                return PlayerState.MovePiecesRepeadetly;
            }

            // when the dice value is 6 and there are no movement options, let player dice again
            else if (dice.Value == 6 && movementOptions.Count == 0)
            {
                return PlayerState.ThrowDice;
            }

            if (movementOptions.Count == 0)
            {

                // when there are no movement options, all pieces are aligned after each other in the end fields and the dice is not thrown three times, let player dice again, otherwise set the PlayerState to MoveDone
                // so that the player is changed
                if (dice.DiceThrownNumber < 3 && CheckPlayersAreAfterEachOtherInEndFields(player, endFields))
                {
                    return PlayerState.ThrowDice;
                }
                else
                {
                    return PlayerState.MoveDone;
                }
            }
            else
            {
                // otherwise let player move pieces
                return PlayerState.MovePieces;
            }
        }

        /// <summary>
        /// Calculates movement options for pieces of specified player
        /// </summary>
        /// <param name="player">Player for which movement options should be calculated</param>
        /// <param name="dice">Dice for getting dice attributes</param>
        /// <param name="gameBoardFields">Game board field array for getting positions of pieces</param>
        /// <param name="startFields">Start field array for getting positions of pieces</param>
        /// <param name="endFields">End field array for getting positions of pieces</param>
        /// <returns>List with movement options</returns>
        public static List<MovementOption> CalculateMovementOptions(Player player, Dice dice, Field[] gameBoardFields, Field[] startFields, Field[] endFields)
        {
            // find positions of the pieces of the player
            List<int> startPositions = CheckPlayerPositions(player, startFields);
            List<int> gameBoardPositions = CheckPlayerPositions(player, gameBoardFields);
            List<int> endPositions = CheckPlayerPositions(player, endFields);

            List<MovementOption> movementOptions = new List<MovementOption>();

            // check whether a player can be moved from the startFields on the gameBoardFields, otherwise calclulate movement options in the start- and endFields
            if (dice.Value == 6 && startPositions.Count > 0)
            {
                switch (player.Color)
                {

                    // when there is the option to move out a player from start fields add Movement option to move out next piece in the start fields,
                    // otherwise calculate movement options in the gameBoardFields
                    case Color.Green:
                        if (gameBoardFields[0].Piece == null || gameBoardFields[0].Piece?.Color != Color.Green)
                        {
                            movementOptions.Add(new MovementOption(startPositions.ElementAt(0) + 40, 0));
                        }
                        else
                        {
                            movementOptions.AddRange(CalculateMovementOptionsInGameBoardFields(gameBoardPositions, player, dice, gameBoardFields, endFields));
                        }
                        break;
                    case Color.Red:
                        if (gameBoardFields[10].Piece == null || gameBoardFields[10].Piece?.Color != Color.Red)
                        {
                            movementOptions.Add(new MovementOption(startPositions.ElementAt(0) + 40, 10));
                        }
                        else
                        {
                            movementOptions.AddRange(CalculateMovementOptionsInGameBoardFields(gameBoardPositions, player, dice, gameBoardFields, endFields));
                        }
                        break;
                    case Color.Blue:
                        if (gameBoardFields[30].Piece == null || gameBoardFields[30].Piece?.Color != Color.Blue)
                        {
                            movementOptions.Add(new MovementOption(startPositions.ElementAt(0) + 40, 30));
                        }
                        else
                        {
                            movementOptions.AddRange(CalculateMovementOptionsInGameBoardFields(gameBoardPositions, player, dice, gameBoardFields, endFields));
                        }
                        break;
                    case Color.Yellow:
                        if (gameBoardFields[20].Piece == null || gameBoardFields[20].Piece?.Color != Color.Yellow)
                        {
                            movementOptions.Add(new MovementOption(startPositions.ElementAt(0) + 40, 20));
                        }
                        else
                        {
                            movementOptions.AddRange(CalculateMovementOptionsInGameBoardFields(gameBoardPositions, player, dice, gameBoardFields, endFields));
                        }
                        break;
                }
            }
            else
            {
                movementOptions.AddRange(CalculateMovementOptionsInGameBoardFields(gameBoardPositions, player, dice, gameBoardFields, endFields));
                movementOptions.AddRange(CalculateMovementOptionsInEndFields(player, endPositions, endFields, dice));
            }

            return movementOptions;
        }

        /// <summary>
        /// Calculates movement options for pieces on the gameBoard
        /// </summary>
        /// <param name="gameBoardPositions">List with positions of pieces on the game board fields</param>
        /// <param name="player">Player for which the the movement options should be calculated</param>
        /// <param name="dice">Dice for getting dice attributes</param>
        /// <param name="gameBoardFields">Game board field array for getting positions of pieces</param>
        /// <param name="endFields">End field array for getting positions of pieces</param>
        /// <returns>Movement options for pieces on the gameboard</returns>
        private static List<MovementOption> CalculateMovementOptionsInGameBoardFields(List<int> gameBoardPositions, Player player, Dice dice, Field[] gameBoardFields, Field[] endFields)
        {
            List<MovementOption> movementOptions = new List<MovementOption>();

            if (gameBoardPositions.Count > 0)
            {

                // Go through every the positions of every found piece and calculate whether a movement is possible
                foreach (int position in gameBoardPositions)
                {
                    switch (player.Color)
                    {
                        case Color.Green:

                            // Check whether there has to be gone over the own start field for getting to the next position
                            // If no, check possible movements in gameboard fields, otherwise check possible movement into the end fields
                            if ((position + dice.Value) < 40)
                            {

                                // check whether an own figure is already standing on the field, if not add movement option
                                if (gameBoardFields[(position + dice.Value)].Piece == null || gameBoardFields[(position + dice.Value)]?.Piece?.Color != Color.Green)
                                {
                                    movementOptions.Add(new MovementOption(position, position + dice.Value));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 40 < 4)
                            {

                                // check whether an own figure standing on the possible end position to move to, if not, add movement option
                                if (endFields[(position + dice.Value) - 40].Piece == null)
                                {
                                    movementOptions.Add(new MovementOption(position, 56 + (position + dice.Value) - 40));
                                }
                                break;
                            }
                            else
                                break;
                        case Color.Red:

                            // Check whether there has to be gone over the own start field for getting to the next position
                            // If no, check possible movements in gameboard fields, otherwise check possible movement into the end fields
                            if ((position + dice.Value) < 10 && position < 10 || position > 9)
                            {
                                int nextPos = (position + dice.Value);

                                // when next position exceeds 39, recalculate it, so pieces can be moved around circularly in the gameBoardFields array
                                if (nextPos > 39)
                                    nextPos = (position + dice.Value) - 40;

                                // check whether an own figure is already standing on the field, if not add movement option
                                if (gameBoardFields[nextPos].Piece == null || gameBoardFields[nextPos].Piece?.Color != Color.Red)
                                {
                                    movementOptions.Add(new MovementOption(position, nextPos));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 10 < 4)
                            {

                                // check whether an own figure standing on the possible end position to move to, if not, add movement option
                                if (endFields[(position + dice.Value) - 6].Piece == null)
                                {
                                    movementOptions.Add(new MovementOption(position, 60 + (position + dice.Value) - 10));
                                }
                                break;
                            }
                            else
                                break;
                        case Color.Yellow:

                            // Check whether there has to be gone over the own start field for getting to the next position
                            // If no, check possible movements in gameboard fields, otherwise check possible movement into the end fields
                            if ((position + dice.Value) < 20 && position < 20 || position > 19)
                            {
                                int nextPos = (position + dice.Value);

                                // when next position exceeds 39, recalculate it, so pieces can be moved around circularly in the gameBoardFields array
                                if (nextPos > 39)
                                    nextPos = (position + dice.Value) - 40;

                                // check whether an own figure is already standing on the field, if not add movement option
                                if (gameBoardFields[nextPos].Piece == null || gameBoardFields[nextPos].Piece?.Color != Color.Yellow)
                                {
                                    movementOptions.Add(new MovementOption(position, nextPos));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 20 < 4)
                            {

                                // check whether an own figure standing on the possible end position to move to, if not, add movement option
                                if (endFields[(position + dice.Value) - 12].Piece == null)
                                {
                                    movementOptions.Add(new MovementOption(position, 64 + (position + dice.Value) - 20));
                                }
                                break;
                            }
                            else
                                break;
                        case Color.Blue:

                            // Check whether there has to be gone over the own start field for getting to the next position
                            // If no, check possible movements in gameboard fields, otherwise check possible movement into the end fields
                            if ((position + dice.Value) < 30 && position < 30 || position > 29)
                            {
                                int nextPos = (position + dice.Value);

                                // when next position exceeds 39, recalculate it, so pieces can be moved around circularly in the gameBoardFields array
                                if (nextPos > 39)
                                    nextPos = (position + dice.Value) - 40;

                                // check whether an own figure is already standing on the field, if not add movement option
                                if (gameBoardFields[nextPos].Piece == null || gameBoardFields[nextPos].Piece?.Color != Color.Blue)
                                {
                                    movementOptions.Add(new MovementOption(position, nextPos));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 30 < 4)
                            {

                                // check whether an own figure standing on the possible end position to move to, if not, add movement option
                                if (endFields[(position + dice.Value) - 18].Piece == null)
                                {
                                    movementOptions.Add(new MovementOption(position, 68 + (position + dice.Value) - 30));
                                }
                                break;
                            }
                            else
                                break;
                    }
                }
            }
            return movementOptions;
        }

        /// <summary>
        /// Calculates movement options for pieces in the end fields
        /// </summary>
        /// <param name="player">Player for which the the movement options should be calculated</param>
        /// <param name="endPositions">List with positions of the pieces in the end field array</param>
        /// <param name="endFields">End field array for getting positions of pieces</param>
        /// <param name="dice">Dice for getting dice attributes</param>
        /// <returns>List with movement options for pieces in the end fields</returns>
        private static List<MovementOption> CalculateMovementOptionsInEndFields(Player player, List<int> endPositions, Field[] endFields, Dice dice)
        {
            List<MovementOption> movementOptions = new List<MovementOption>();

            foreach (int position in endPositions)
            {
                switch (player.Color)
                {
                    case Color.Green:
                        if ((position + dice.Value) < 4)
                        {

                            // check whether an own figure standing on the possible end position to move to, if not, add movement option
                            if (endFields[(position + dice.Value)].Piece == null)
                            {
                                movementOptions.Add(new MovementOption(position + 56, position + dice.Value + 56));
                            }
                            break;
                        }
                        break;
                    case Color.Red:
                        if (position < 7 && position > 3 && (position + dice.Value) < 8)
                        {

                            // check whether an own figure standing on the possible end position to move to, if not, add movement option
                            if (endFields[(position + dice.Value)].Piece == null)
                            {
                                movementOptions.Add(new MovementOption(position + 56, position + dice.Value + 56));
                            }
                            break;
                        }
                        break;
                    case Color.Yellow:
                        if (position < 11 && position > 7 && (position + dice.Value) < 12)
                        {

                            // check whether an own figure standing on the possible end position to move to, if not, add movement option
                            if (endFields[(position + dice.Value)].Piece == null)
                            {
                                movementOptions.Add(new MovementOption(position + 56, position + dice.Value + 56));
                            }
                            break;
                        }
                        break;
                    case Color.Blue:
                        if (position < 15 && position > 11 && (position + dice.Value) < 16)
                        {

                            // check whether an own figure standing on the possible end position to move to, if not, add movement option
                            if (endFields[(position + dice.Value)].Piece == null)
                            {
                                movementOptions.Add(new MovementOption(position + 56, position + dice.Value + 56));
                            }
                            break;
                        }
                        break;
                }
            }
            return movementOptions;
        }

        /// <summary>
        /// Checks whether a game is won
        /// </summary>
        /// <param name="player">Player for which should be calculated whether game was won</param>
        /// <param name="endFields">End field array for getting positions of pieces</param>
        /// <returns>true when game is won, otherwise false</returns>
        public static bool CheckGameWon(Player player, Field[] endFields)
        {
            List<int> playerPositions = CheckPlayerPositions(player, endFields);

            if (playerPositions.Count == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks whether all players are after each other in the end fields, so it can be checked that 
        /// it is allowed to dice 3 times when no other player is on the field
        /// </summary>
        /// <param name="player">Player for which should be checked whether the players are after each other</param>
        /// <param name="endFields">End field array for getting positions of pieces</param>
        /// <returns></returns>
        private static bool CheckPlayersAreAfterEachOtherInEndFields(Player player, Field[] endFields)
        {
            List<int> playerPositions = CheckPlayerPositions(player, endFields);

            switch (player.Color)
            {
                case Color.Green:
                    for (int i = 3; i > 3 - playerPositions.Count; i--)
                    {
                        if (endFields[i].Piece == null)
                        {
                            return false;
                        }
                    }
                    break;
                case Color.Red:
                    for (int i = 7; i > 7 - playerPositions.Count; i--)
                    {
                        if (endFields[i].Piece == null)
                        {
                            return false;
                        }
                    }
                    break;
                case Color.Blue:
                    for (int i = 11; i > 11 - playerPositions.Count; i--)
                    {
                        if (endFields[i].Piece == null)
                        {
                            return false;
                        }
                    }
                    break;
                case Color.Yellow:
                    for (int i = 15; i > 15 - playerPositions.Count; i--)
                    {
                        if (endFields[i].Piece == null)
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }
    }
}
