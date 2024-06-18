using System;
using System.Collections.Generic;

namespace ConsoleMemoryGame
{
    public class Board
    {
        private readonly int r_BoardWidth;
        private readonly int r_BoardHeight;
        private readonly Card[,] r_GameBoard;
        private int m_NumOfMatches;

        public Board(int i_BoardWidth, int i_BoardHeight)
        {
            r_BoardWidth = i_BoardWidth;
            r_BoardHeight = i_BoardHeight;
            r_GameBoard = new Card[r_BoardHeight, r_BoardWidth];
            m_NumOfMatches = 0;
            InitializeBoard();
        }

        public int Width
        {
            get { return r_BoardWidth; }
        }

        public int Height
        {
            get { return r_BoardHeight; }
        }

        public Card[,] GameBoard
        {
            get { return r_GameBoard; }
        }

        public int NumOfMatches
        {
            get { return m_NumOfMatches; }
            set { m_NumOfMatches = value; }
        }

        public void InitializeBoard()
        {
            List<Card> cardValues = new List<Card>();
            Random random = new Random();
            int totalNumOfCards = r_BoardWidth * r_BoardHeight;

            for (int i = 0; i < totalNumOfCards / 2; i++)
            {
                cardValues.Add(new Card(i, false));
                cardValues.Add(new Card(i, false));
            }

            for (int rowNum = 0; rowNum < r_BoardHeight; rowNum++)
            {
                for (int colNum = 0; colNum < r_BoardWidth; colNum++)
                {
                    int index = random.Next(cardValues.Count);
                    r_GameBoard[rowNum, colNum] = cardValues[index];
                    cardValues.RemoveAt(index);
                }
            }
        }

        public Card ChooseCard(int i_RowIndex, int i_ColumnIndex)
        {
            ExposeCard(i_RowIndex, i_ColumnIndex);

            return r_GameBoard[i_RowIndex, i_ColumnIndex];
        }

        public int GetCardValue(int i_RowIndex, int i_ColumnIndex)
        {
            return r_GameBoard[i_RowIndex, i_ColumnIndex].Value;
        }

        public void ExposeCard(int i_RowIndex, int i_ColumnIndex)
        {
            r_GameBoard[i_RowIndex, i_ColumnIndex].IsRevealed = true;
        }

        public void HideCard(int i_RowIndex, int i_ColumnIndex)
        {
            r_GameBoard[i_RowIndex, i_ColumnIndex].IsRevealed = false;
        }

        public static bool IsValidDimension(int i_DimensionChosen, int i_MinDimension, int i_MaxDimension)
        {
            return i_DimensionChosen >= i_MinDimension && i_DimensionChosen <= i_MaxDimension;
        }

        public static bool IsValidBoardSize(int i_Width, int i_Height)
        {
            return (i_Width * i_Height) % 2 == 0;
        }

        public bool IsFull()
        {
            bool isFull = false;
            int totalNumOfCells = r_BoardWidth * r_BoardHeight;

            if (m_NumOfMatches >= totalNumOfCells / 2)
            {
                isFull = true;
            }

            return isFull;
        }

        public List<(int, int)> GetUnrevealedCardPositions()
        {
            List<(int, int)> unrevealedCards = new List<(int, int)>();

            for (int row = 0; row < r_BoardHeight; row++)
            {
                for (int col = 0; col < r_BoardWidth; col++)
                {
                    if (!r_GameBoard[row, col].IsRevealed)
                    {
                        unrevealedCards.Add((row, col));
                    }
                }
            }

            return unrevealedCards;
        }
    }
}

