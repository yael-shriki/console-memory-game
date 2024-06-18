using System;
using System.Text;

namespace ConsoleMemoryGame
{
    internal class ConsoleRenderer
    {
        private static readonly string r_CardValueOptions = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int k_MinBoardDimension = 4;
        private const int k_MaxBoardDimension = 6;
        private const int k_MinNumOfPlayers = 1;
        private const int k_MaxNumOfPlayers = 2;

        public static void ClearScreen()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }

        public static void DisplayBoard(Board i_Board)
        {
            StringBuilder sb = new StringBuilder();
            string equalLine = new string('=', i_Board.Width * 4 + 1);

            ClearScreen();
            sb.Append("    ");
            for (int col = 0; col < i_Board.Width; col++)
            {
                sb.Append((char)('A' + col) + "   ");
            }

            sb.AppendLine();
            sb.AppendLine("  " + equalLine);
            for (int rowNum = 0; rowNum < i_Board.Height; rowNum++)
            {
                sb.Append(rowNum + 1 + " |");
                for (int colNum = 0; colNum < i_Board.Width; colNum++)
                {
                    sb.Append(' ');
                    Card chosenCard = i_Board.GameBoard[rowNum, colNum];
                    sb.Append(chosenCard.IsRevealed ? r_CardValueOptions[chosenCard.Value] : ' ');
                    sb.Append(" |");
                }

                sb.AppendLine();
                sb.AppendLine("  " + equalLine);
            }

            Console.WriteLine(sb.ToString());
        }

        public static string GetName(int i_PlayerNumber)
        {
            Console.WriteLine("Player {0}, please enter your name:", i_PlayerNumber);
            string name = Console.ReadLine();
            while (!Player.ValidateName(name))
            {
                Console.WriteLine("You may enter a name with a maximum length of 20 characters and no spaces.");
                name = Console.ReadLine();
            }

            return name;
        }

        public static int GetValidDimension(string i_Dimension, int i_MinDimension, int i_MaxDimension)
        {
            Console.WriteLine("Enter the {0} of your board. The {0} must be between 4 and 6:", i_Dimension);
            int dimension = ReadNumberFromUser();
            while (!Board.IsValidDimension(dimension, i_MinDimension, i_MaxDimension))
            {
                Console.WriteLine("Not a valid {0}, please enter a number between 4 and 6", i_Dimension);
                dimension = ReadNumberFromUser();
            }

            return dimension;
        }

        public static void GetValidBoardSize(out int io_InputHeight, out int io_InputWidth)
        {
            io_InputHeight = GetValidDimension("height", k_MinBoardDimension, k_MaxBoardDimension);
            io_InputWidth = GetValidDimension("width", k_MinBoardDimension, k_MaxBoardDimension);

            while (!Board.IsValidBoardSize(io_InputWidth, io_InputHeight))
            {
                Console.WriteLine("The board must have an even number of cards. Lets choose the size of your board again.");
                io_InputHeight = GetValidDimension("height", k_MinBoardDimension, k_MaxBoardDimension);
                io_InputWidth = GetValidDimension("width", k_MinBoardDimension, k_MaxBoardDimension);
            }
        }

        public static int ReadNumberFromUser()
        {
            int number;

            while (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine("Not a number, please enter a valid number:");
            }

            return number;
        }

        public static int GetValidNumOfPlayers()
        {
            Console.WriteLine("How many players want to play? Choose 1 to play against the computer or 2 to play against a friend:");
            int numOfPlayers = ReadNumberFromUser();
            while (!GameManager.IsValidNumOfPlayers(numOfPlayers, k_MinNumOfPlayers, k_MaxNumOfPlayers))
            {
                Console.WriteLine("You must choose a number of players between 1 and 2:");
                numOfPlayers = ReadNumberFromUser();
            }

            return numOfPlayers;
        }

        public static void GetValidPlayerChoice(Board i_Board, Player i_currentPlayer, out int o_Row, out int o_Column)
        {
            Console.WriteLine("{0}, please enter the coordinates of your choice:", i_currentPlayer.Name);
            string playerChoice = GetValidChoiceFormat();
            while (!IsValidCoordinate(playerChoice, i_Board, out o_Row, out o_Column))
            {
                playerChoice = GetValidChoiceFormat();
            }
        }

        public static string GetValidChoiceFormat()
        {
            string i_PlayerChoice = Console.ReadLine().Trim();

            if (i_PlayerChoice.ToUpper() == "Q")
            {
                Environment.Exit(0);
            }

            while (i_PlayerChoice.Length != 2 || !char.IsLetter(i_PlayerChoice[0]) || !char.IsDigit(i_PlayerChoice[1]))
            {
                Console.WriteLine("You must choose a letter followed by a number, please choose again:");
                i_PlayerChoice = Console.ReadLine().Trim();
                if (i_PlayerChoice.ToUpper() == "Q")
                {
                    Environment.Exit(0);
                }
            }

            return i_PlayerChoice.ToUpper();
        }

        public static bool IsValidCoordinate(string i_PlayerChoice, Board i_Board, out int o_row, out int o_column)
        {
            bool isValidCoordinate = true;
            o_column = char.ToUpper(i_PlayerChoice[0]) - 'A';
            o_row = int.Parse(i_PlayerChoice.Substring(1)) - 1;

            if (o_column < 0 || o_column >= i_Board.Width)
            {
                Console.WriteLine("Your column index is not in the range of your board.");
                isValidCoordinate = false;
            }

            if (o_row < 0 || o_row >= i_Board.Height)
            {
                Console.WriteLine("Your row index is not in the range of your board.");
                isValidCoordinate = false;
            }

            if (isValidCoordinate && i_Board.GameBoard[o_row, o_column].IsRevealed == true)
            {
                Console.WriteLine("This card has already been revealed.");
                isValidCoordinate = false;
            }

            return isValidCoordinate;
        }

        public static void GameEndingSummary(string i_WinnerName, string i_LoserName, int i_WinnerScore, int i_LoserScore)
        {
            if (i_WinnerScore == i_LoserScore)
            {
                Console.WriteLine("You've tied! {0} with {1} points and {2} with {3} points!", i_WinnerName, i_WinnerScore, i_LoserName, i_LoserScore);
            }
            else
            {
                Console.WriteLine("The winner is {0} with {1} points! Second place goes to {2} with {3} points!", i_WinnerName, i_WinnerScore, i_LoserName, i_LoserScore);
            }
        }

        public static bool ResetOrQuitGame()
        {
            bool resetGame = false;

            Console.WriteLine("Thanks for playing! Press R to reset game or Press Q to quit.");
            string playerAnswer = Console.ReadLine().ToUpper().Trim();
            while (playerAnswer != "Q" && playerAnswer != "R")
            {
                Console.WriteLine("Please choose between R for resetting and Q for quitting.");
                playerAnswer = Console.ReadLine().ToUpper().Trim();
            }

            if (playerAnswer == "Q")
            {
                Console.WriteLine("Bye Bye! See you next time!");
            }

            if (playerAnswer == "R")
            {
                resetGame = true;
            }

            return resetGame;
        }

        public static void ComputerIsPlayingMessage()
        {
            Console.WriteLine("Computer's turn.");
        }

        public static void ItsAMatchMessage()
        {
            Console.WriteLine("It's a match! Play again.");
        }

        public static void ItsNotAMatchMessage()
        {
            Console.WriteLine("No Match!");
        }

    }
}

