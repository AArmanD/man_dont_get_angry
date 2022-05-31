using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using man_dont_get_angry.Models;

namespace man_dont_get_angry.ModelUtils
{
    internal static class OptionsChecker
    {
        private static List<int> checkPlayerPositions(Player player, Field[] fieldArray)
        {
            List<int> playerPositions = new List<int>();
            for (int i = 0; i < fieldArray.Length; i++)
                if (fieldArray[i].ThePiece != null)
                {
                    if (player.TheColor == fieldArray[i].ThePiece.TheColor)
                    {
                        playerPositions.Add(i);
                    }
                }
            return playerPositions;
        }

        public static PlayerState GenerateStateAfterRolling(List<MovementOption> movementOptions, Dice dice, Player player, Field[] endFields)
        {

            if (dice.Value == 6 && movementOptions.Count > 0)
            {
                return PlayerState.MovePiecesRepeadetly;
            }
            else if (dice.Value == 6 && movementOptions.Count == 0)
            { 
                return PlayerState.ThrowDice;
            }

            if (movementOptions.Count == 0)
            {
                // TODO: Check whether all players are after each other in the end fields --> check if algorithm is correct
                if (dice.DiceThrownNumber < 3 && checkPlayersAreAfterEachOtherInEnd(player, endFields))
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
                return PlayerState.MovePieces;
            }
        }

        public static List<MovementOption> checkMovements(Player player, Dice dice, Field[] gameBoardFields, Field[] startFields, Field[] endFields)
        {
            List<int> startPositions = checkPlayerPositions(player, startFields);
            List<int> gameBoardPositions = checkPlayerPositions(player, gameBoardFields);
            List<int> endPositions = checkPlayerPositions(player, endFields);

            List<MovementOption> movementOptions = new List<MovementOption>();

            if (dice.Value == 6 && startPositions.Count > 0)
            {
                switch (player.TheColor)
                {
                    case Color.Green:
                        if (gameBoardFields[0].ThePiece == null || gameBoardFields[0].ThePiece.TheColor != Color.Green)
                        {
                            movementOptions.Add(new MovementOption(startPositions.ElementAt(0) + 40, 0));
                        }
                        else
                        {
                            movementOptions.AddRange(COpt(gameBoardPositions, player, dice, gameBoardFields, endFields));
                        }
                        break;
                    case Color.Red:
                        if (gameBoardFields[10].ThePiece == null || gameBoardFields[10].ThePiece.TheColor != Color.Red)
                        {
                            movementOptions.Add(new MovementOption(startPositions.ElementAt(0) + 40, 10));
                        }
                        else
                        {
                            movementOptions.AddRange(COpt(gameBoardPositions, player, dice, gameBoardFields, endFields));
                        }
                        break;
                    case Color.Blue:
                        if (gameBoardFields[30].ThePiece == null || gameBoardFields[30].ThePiece.TheColor != Color.Blue)
                        {
                            movementOptions.Add(new MovementOption(startPositions.ElementAt(0) + 40, 30));
                        }
                        else
                        {
                            movementOptions.AddRange(COpt(gameBoardPositions, player, dice, gameBoardFields, endFields));
                        }
                        break;
                    case Color.Yellow:
                        if (gameBoardFields[20].ThePiece == null || gameBoardFields[20].ThePiece.TheColor != Color.Yellow)
                        {
                            movementOptions.Add(new MovementOption(startPositions.ElementAt(0) + 40, 20));
                        }
                        else
                        {
                            movementOptions.AddRange(COpt(gameBoardPositions, player, dice, gameBoardFields, endFields));
                        }
                        break;
                }
            }
            else
            {
                movementOptions.AddRange(COpt(gameBoardPositions, player, dice, gameBoardFields, endFields));
                movementOptions.AddRange(COptEndPositions(player, endPositions, endFields, dice));
            }

            return movementOptions;
        }
        private static List<MovementOption> COpt(List<int> gameBoardPositions, Player player, Dice dice, Field[] gameBoardFields, Field[] endFields)
        {
            List<MovementOption> movementOptions = new List<MovementOption>();
            if (gameBoardPositions.Count > 0)
            {
                foreach (int position in gameBoardPositions)
                {
                    switch (player.TheColor)
                    {
                        case Color.Green:
                            if ((position + dice.Value) < 40)
                            {
                                if (gameBoardFields[(position + dice.Value)].ThePiece == null || gameBoardFields[(position + dice.Value)].ThePiece.TheColor != Color.Green)
                                {
                                    movementOptions.Add(new MovementOption(position, position + dice.Value));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 40 < 4)
                            {
                                if (endFields[(position + dice.Value) - 40].ThePiece == null)
                                {
                                    movementOptions.Add(new MovementOption(position, 56 + (position + dice.Value) - 40));
                                }
                                break;
                            }
                            else
                                break;
                        case Color.Red:
                            if ((position + dice.Value) < 10 && position < 10 || position > 9)
                            {
                                // check whether an own figure is already standing on the field
                                int nextPos = (position + dice.Value);
                                if (nextPos > 39)
                                    nextPos = (position + dice.Value) - 40;

                                if (gameBoardFields[nextPos].ThePiece == null || gameBoardFields[nextPos].ThePiece.TheColor != Color.Red)
                                {
                                    movementOptions.Add(new MovementOption(position, nextPos));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 10 < 4)
                            {
                                if (endFields[(position + dice.Value) - 6].ThePiece == null)
                                {
                                    movementOptions.Add(new MovementOption(position, 60 + (position + dice.Value) - 10));
                                }
                                break;
                            }
                            else
                                break;
                        case Color.Yellow:
                            if ((position + dice.Value) < 20 && position < 20 || position > 19)
                            {
                                int nextPos = (position + dice.Value);
                                if (nextPos > 39)
                                    nextPos = (position + dice.Value) - 40;

                                if (gameBoardFields[nextPos].ThePiece == null || gameBoardFields[nextPos].ThePiece.TheColor != Color.Yellow)
                                {
                                    movementOptions.Add(new MovementOption(position, nextPos));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 20 < 4)
                            {
                                if (endFields[(position + dice.Value) - 12].ThePiece == null)
                                {
                                    movementOptions.Add(new MovementOption(position, 64 + (position + dice.Value) - 20));
                                }
                                break;
                            }
                            else
                                break;
                        case Color.Blue:
                            if ((position + dice.Value) < 30 && position < 30 || position > 29)
                            {
                                int nextPos = (position + dice.Value);
                                if (nextPos > 39)
                                    nextPos = (position + dice.Value) - 40;

                                if (gameBoardFields[nextPos].ThePiece == null || gameBoardFields[nextPos].ThePiece.TheColor != Color.Blue)
                                {
                                    movementOptions.Add(new MovementOption(position, nextPos));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 30 < 4)
                            {
                                if (endFields[(position + dice.Value) - 18].ThePiece == null)
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


        private static List<MovementOption> COptEndPositions(Player player, List<int> endPositions, Field[] endFields, Dice dice)
        {
            List<MovementOption> movementOptions = new List<MovementOption>();

            foreach (int position in endPositions)
            {
                switch (player.TheColor)
                {
                    case Color.Green:
                        if ((position + dice.Value) < 4)
                        {
                            if (endFields[(position + dice.Value)].ThePiece == null)
                            {
                                movementOptions.Add(new MovementOption(position + 56, position + dice.Value + 56));
                            }
                            break;
                        }
                        break;
                    case Color.Red:
                        if (position < 7 && position > 3 && (position + dice.Value) < 8)
                        {
                            if (endFields[(position + dice.Value)].ThePiece == null)
                            {
                                movementOptions.Add(new MovementOption(position + 56, position + dice.Value + 56));
                            }
                            break;
                        }
                        break;
                    case Color.Yellow:
                        if (position < 11 && position > 7 && (position + dice.Value) < 12)
                        {
                            if (endFields[(position + dice.Value)].ThePiece == null)
                            {
                                movementOptions.Add(new MovementOption(position + 56, position + dice.Value + 56));
                            }
                            break;
                        }
                        break;
                    case Color.Blue:
                        if (position < 15 && position > 11 && (position + dice.Value) < 16)
                        {
                            if (endFields[(position + dice.Value)].ThePiece == null)
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

        public static bool checkGameWon(Player player, Field[] endFields)
        {
            List<int> playerPositions = checkPlayerPositions(player, endFields);

            if (playerPositions.Count == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool checkPlayersAreAfterEachOtherInEnd(Player player, Field[] endFields)
        {
            List<int> playerPositions = checkPlayerPositions(player, endFields);


            switch (player.TheColor)
            {
                case Color.Green:
                    for(int i = 3; i > 3-playerPositions.Count; i--)
                    {
                        if(endFields[i].ThePiece == null)
                        {
                            return false;
                        }
                    }
                    break;
                case Color.Red:
                    for (int i = 7; i > 7 - playerPositions.Count; i--)
                    {
                        if (endFields[i].ThePiece == null)
                        {
                            return false;
                        }
                    }
                    break;
                case Color.Blue:
                    for (int i = 11; i > 11 - playerPositions.Count; i--)
                    {
                        if (endFields[i].ThePiece == null)
                        {
                            return false;
                        }
                    }
                    break;
                case Color.Yellow:
                    for (int i = 15; i > 15 - playerPositions.Count; i--)
                    {
                        if (endFields[i].ThePiece == null)
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
