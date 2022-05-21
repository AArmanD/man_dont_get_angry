﻿using System;
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

        public static PlayerState GenerateStateAfterRolling(List<Tuple<int, int>> movementOptions, Dice dice)
        {

            if (dice.Value == 6 && movementOptions.Count > 0)
            {
                return PlayerState.MovePiecesRepeadetly;
            }
            else
            { 
                return PlayerState.ThrowDice;
            }

            if (movementOptions.Count == 0)
            {
                if (dice.DiceThrownNumber < 3)
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
                    case Color.Green:
                        if (gameBoardFields[0].ThePiece == null || gameBoardFields[0].ThePiece.TheColor != Color.Green)
                        {
                            movementOptions.Add(Tuple.Create(startPositions.ElementAt(0) + 40, 0));
                        }
                        else
                        {
                            movementOptions.AddRange(COpt(gameBoardPositions, player, dice, gameBoardFields, endFields));
                        }
                        break;
                    case Color.Red:
                        if (gameBoardFields[10].ThePiece == null || gameBoardFields[10].ThePiece.TheColor != Color.Red)
                        {
                            movementOptions.Add(Tuple.Create(startPositions.ElementAt(0) + 40, 10));
                        }
                        else
                        {
                            movementOptions.AddRange(COpt(gameBoardPositions, player, dice, gameBoardFields, endFields));
                        }
                        break;
                    case Color.Blue:
                        if (gameBoardFields[30].ThePiece == null || gameBoardFields[30].ThePiece.TheColor != Color.Blue)
                        {
                            movementOptions.Add(Tuple.Create(startPositions.ElementAt(0) + 40, 30));
                        }
                        else
                        {
                            movementOptions.AddRange(COpt(gameBoardPositions, player, dice, gameBoardFields, endFields));
                        }
                        break;
                    case Color.Yellow:
                        if (gameBoardFields[20].ThePiece == null || gameBoardFields[20].ThePiece.TheColor != Color.Yellow)
                        {
                            movementOptions.Add(Tuple.Create(startPositions.ElementAt(0) + 40, 20));
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
        private static List<Tuple<int, int>> COpt(List<int> gameBoardPositions, Player player, Dice dice, Field[] gameBoardFields, Field[] endFields)
        {
            List<Tuple<int, int>> movementOptions = new List<Tuple<int, int>>();
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
                                    movementOptions.Add(Tuple.Create(position, position + dice.Value));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 40 < 4)
                            {
                                if (endFields[(position + dice.Value) - 40].ThePiece == null)
                                {
                                    movementOptions.Add(Tuple.Create(position, 56 + (position + dice.Value) - 40));
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
                                    movementOptions.Add(Tuple.Create(position, nextPos));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 10 < 4)
                            {
                                if (endFields[(position + dice.Value) - 6].ThePiece == null)
                                {
                                    movementOptions.Add(Tuple.Create(position, 60 + (position + dice.Value) - 10));
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
                                    movementOptions.Add(Tuple.Create(position, nextPos));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 20 < 4)
                            {
                                if (endFields[(position + dice.Value) - 12].ThePiece == null)
                                {
                                    movementOptions.Add(Tuple.Create(position, 64 + (position + dice.Value) - 20));
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
                                    movementOptions.Add(Tuple.Create(position, nextPos));
                                }
                                break;
                            }
                            else if ((position + dice.Value) - 30 < 4)
                            {
                                if (endFields[(position + dice.Value) - 18].ThePiece == null)
                                {
                                    movementOptions.Add(Tuple.Create(position, 68 + (position + dice.Value) - 30));
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


        private static List<Tuple<int, int>> COptEndPositions(Player player, List<int> endPositions, Field[] endFields, Dice dice)
        {
            List<Tuple<int, int>> movementOptions = new List<Tuple<int, int>>();

            foreach (int position in endPositions)
            {
                switch (player.TheColor)
                {
                    case Color.Green:
                        if ((position + dice.Value) < 4)
                        {
                            if (endFields[(position + dice.Value)].ThePiece == null)
                            {
                                movementOptions.Add(Tuple.Create(position + 56, position + dice.Value + 56));
                            }
                            break;
                        }
                        break;
                    case Color.Red:
                        if (position < 7 && position > 3 && (position + dice.Value) < 8)
                        {
                            if (endFields[(position + dice.Value)].ThePiece == null)
                            {
                                movementOptions.Add(Tuple.Create(position + 56, position + dice.Value + 56));
                            }
                            break;
                        }
                        break;
                    case Color.Yellow:
                        if (position < 11 && position > 7 && (position + dice.Value) < 12)
                        {
                            if (endFields[(position + dice.Value)].ThePiece == null)
                            {
                                movementOptions.Add(Tuple.Create(position + 56, position + dice.Value + 56));
                            }
                            break;
                        }
                        break;
                    case Color.Blue:
                        if (position < 15 && position > 11 && (position + dice.Value) < 16)
                        {
                            if (endFields[(position + dice.Value)].ThePiece == null)
                            {
                                movementOptions.Add(Tuple.Create(position + 56, position + dice.Value + 56));
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
    }
}
