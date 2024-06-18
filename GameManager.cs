using System;
using System.Collections.Generic;

namespace ConsoleMemoryGame
{
    public class GameManager
    {
        private readonly Board r_Board;
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private Player m_CurrentTurnPlayer;
        private Dictionary<int, List<(int, int)>> m_Memory;
        private readonly Random m_Random;

        public GameManager(int i_BoardWidth, int i_BoardHeight, string i_Player1Name, string i_Player2Name, bool i_IsComputer)
        {
            r_Board = new Board(i_BoardWidth, i_BoardHeight);
            r_Player1 = new Player(i_Player1Name, false);
            r_Player2 = new Player(i_Player2Name, i_IsComputer);
            m_CurrentTurnPlayer = setStartingPlayer();
            m_Memory = new Dictionary<int, List<(int, int)>>();
            m_Random = new Random();
        }

        public Board Board
        {
            get { return r_Board; }
        }

        public Player currentPlayer
        {
            get { return m_CurrentTurnPlayer; }
        }

        public Player Player1
        {
            get { return r_Player1; }
        }

        public Player Player2
        {
            get { return r_Player2; }
        }
        public Dictionary<int, List<(int, int)>> Memory
        {
            get { return m_Memory; }
            set { m_Memory = value; }
        }

        public Random Random
        {
            get { return m_Random; }
        }

        public static bool IsValidNumOfPlayers(int i_numOfPlayers, int i_MinNumOfPlayers, int i_MaxNumOfPlayers)
        {
            return i_numOfPlayers >= i_MinNumOfPlayers && i_numOfPlayers <= i_MaxNumOfPlayers;
        }

        private Player setStartingPlayer()
        {
            Random random = new Random();

            return random.Next(1, 3) == 1 ? r_Player1 : r_Player2;
        }

        public void ChangeTurns()
        {
            if (m_CurrentTurnPlayer == r_Player1)
            {
                m_CurrentTurnPlayer = r_Player2;
            }

            else
            {
                m_CurrentTurnPlayer = r_Player1;
            }

        }
        public bool GameOver()
        {
            return r_Board.IsFull();
        }

        public Player GetWinner()
        {
            return r_Player1.Points > r_Player2.Points ? r_Player1 : r_Player2;
        }

        public Player GetLoser()
        {
            return r_Player1.Points > r_Player2.Points ? r_Player2 : r_Player1;
        }

        public void GetValidComputerChoice(out int io_FirstRowChoice, out int io_FirstColumnChoice, out int io_SecondRowChoice, out int io_SecondColumnChoice)
        {
            foreach (var memoryEntry in Memory)
            {
                if (memoryEntry.Value.Count == 2)
                {
                    (io_FirstRowChoice, io_FirstColumnChoice) = memoryEntry.Value[0];
                    (io_SecondRowChoice, io_SecondColumnChoice) = memoryEntry.Value[1];
                    AddComputerChoiceToMemory(io_FirstRowChoice, io_FirstColumnChoice, io_SecondRowChoice, io_SecondColumnChoice);
                    return;
                }
            }

            GetRandomComputerChoice(out io_FirstRowChoice, out io_FirstColumnChoice, out io_SecondRowChoice, out io_SecondColumnChoice);
            AddComputerChoiceToMemory(io_FirstRowChoice, io_FirstColumnChoice, io_SecondRowChoice, io_SecondColumnChoice);
        }

        public void AddCardToMemory(int i_CardValue, int i_RowIndex, int i_ColumnIndex)
        {
            if (!Memory.ContainsKey(i_CardValue))
            {
                Memory[i_CardValue] = new List<(int, int)>();
                Memory[i_CardValue].Add((i_RowIndex, i_ColumnIndex));
            }
            else
            {
                bool coordinateAlreadyExists = false;
                foreach (var coordinate in Memory[i_CardValue])
                {
                    if (coordinate.Item1 == i_RowIndex && coordinate.Item2 == i_ColumnIndex)
                    {
                        coordinateAlreadyExists = true;
                        break;
                    }
                }

                if (!coordinateAlreadyExists)
                {
                    Memory[i_CardValue].Add((i_RowIndex, i_ColumnIndex));
                }
            }
        }

        public void GetRandomComputerChoice(out int o_FirstRowChoice, out int o_FirstColumnChoice, out int o_SecondRowChoice, out int o_SecondColumnChoice)
        {
            List<(int, int)> unrevealedCards = Board.GetUnrevealedCardPositions();

            int firstRandomIndex = Random.Next(unrevealedCards.Count);
            (o_FirstRowChoice, o_FirstColumnChoice) = unrevealedCards[firstRandomIndex];
            unrevealedCards.RemoveAt(firstRandomIndex);
            int secondRandomIndex = Random.Next(unrevealedCards.Count);
            (o_SecondRowChoice, o_SecondColumnChoice) = unrevealedCards[secondRandomIndex];
        }

        public void AddComputerChoiceToMemory(int i_FirstRowChoice, int i_FirstColumnChoice, int i_SecondRowChoice, int i_SecondColumnChoice)
        {
            int firstCardVal = Board.GetCardValue(i_FirstRowChoice, i_FirstColumnChoice);
            int secondCardVal = Board.GetCardValue(i_SecondRowChoice, i_SecondColumnChoice);
            AddCardToMemory(firstCardVal, i_FirstRowChoice, i_FirstColumnChoice);
            AddCardToMemory(secondCardVal, i_SecondRowChoice, i_SecondColumnChoice);
        }
    }
}

