using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using man_dont_get_angry.Models;

namespace man_dont_get_angry.Utils
{
    internal static class OptionsChecker
    {
        private static List<int> checkPlayerPositions(Player player, Field[] fieldArray)
        {
            List<int> playerPositions = new List<int>();
            for (int i = 0; i < fieldArray.Length; i++)
                if (fieldArray[i].ThePiece != null)
                {
                    if (player.ThePieces.Contains(fieldArray[i].ThePiece))
                    {
                        playerPositions.Add(i);
                    }
                }
            return playerPositions;
        }

        public static PlayerState GenerateStateAfterRolling(Player player, Dice dice, Field[] gameBoardFields, Field[] startFields)
        {
            //TODO: change so that dependent on possible movements dice is activated or not
            if (checkPlayerPositions(player, gameBoardFields).Count == 0)
            {
                if (dice.Value != 6 && dice.DiceThrownNumber != 3)
                    return PlayerState.ThrowDice;
                else if (dice.Value == 6 && checkPlayerPositions(player, startFields).Count != 0)
                    return PlayerState.MovePieces;
                else
                    return PlayerState.MoveDone;
            }
            else // precalculation of possible movements is needed because when coming into end fields   if (dice.Value !=
            {
                return PlayerState.MoveDone;
            }
        }

        public static List<Tuple<int, int>> checkMovements(Player player, Dice dice, Field[] gameBoardFields, Field[] startFields, Field[] endFields)
        {
            List<int> startPositions = checkPlayerPositions(player, startFields);
            List<int> gameBoardPositions = checkPlayerPositions(player, gameBoardFields);
            List<int> endPositions = checkPlayerPositions(player, endFields);

            List<Tuple<int, int>> movementOptions = new List<Tuple<int, int>>();

            if (dice.Value == 6 && startPositions.Count > 0)
            {
                switch (player.TheColor)
                {
                    case Color.Red:
                        movementOptions.Add(Tuple.Create(startPositions.ElementAt(0), 10));
                        break;
                    case Color.Green:
                        movementOptions.Add(Tuple.Create(startPositions.ElementAt(0), 0));
                        break;
                    case Color.Blue:
                        movementOptions.Add(Tuple.Create(startPositions.ElementAt(0), 30));
                        break;
                    case Color.Yellow:
                        movementOptions.Add(Tuple.Create(startPositions.ElementAt(0), 20));
                        break;
                }
            }
            else
            {
                movementOptions.AddRange(COpt(startPositions, gameBoardPositions, endPositions, player, dice));
            }

            return movementOptions;
        }
        private static List<Tuple<int, int>> COpt(List<int> startPositions, List<int> gameBoardPositions, List<int> endPositions, Player player, Dice dice)
        {
            // TODO: Implement that movement cannot be done when Piece is already in the position to move to, implement end positions
            List<Tuple<int, int>> movementOptions = new List<Tuple<int, int>>();
            if (gameBoardPositions.Count > 0)
            {
                foreach (int position in gameBoardPositions)
                {
                    switch (player.TheColor)
                    {
                        case Color.Red:
                            if ((position + dice.Value) < 10)
                            {
                                movementOptions.Add(Tuple.Create(position, position + dice.Value));
                                break;
                            }
                            else if ((position + dice.Value) - 10 < 4)
                            {
                                movementOptions.Add(Tuple.Create(position, 60 + (position + dice.Value) - 10));
                                break;
                            }
                            else
                                break;
                        case Color.Green:
                            if ((position + dice.Value) < 40)
                            {
                                movementOptions.Add(Tuple.Create(position, position + dice.Value));
                                break;
                            }
                            else if ((position + dice.Value) - 40 < 4)
                            {
                                movementOptions.Add(Tuple.Create(position, 56 + (position + dice.Value) - 10));
                                break;
                            }
                            else
                                break;
                        case Color.Blue:
                            if ((position + dice.Value) < 30)
                            {
                                movementOptions.Add(Tuple.Create(position, position + dice.Value));
                                break;
                            }
                            else if ((position + dice.Value) - 30 < 4)
                            {
                                movementOptions.Add(Tuple.Create(position, 68 + (position + dice.Value) - 10));
                                break;
                            }
                            else
                                break;
                        case Color.Yellow:
                            if ((position + dice.Value) < 20)
                            {
                                movementOptions.Add(Tuple.Create(position, position + dice.Value));
                                break;
                            }
                            else if ((position + dice.Value) - 20 < 4)
                            {
                                movementOptions.Add(Tuple.Create(position, 64 + (position + dice.Value) - 10));
                                break;
                            }
                            else
                                break;
                    }
                }
            }
            return movementOptions;
        }
    }

    
}
