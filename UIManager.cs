namespace ConsoleMemoryGame
{
    public class UIManager
    {
        public void PlayMemoryGame()
        {
            int o_firstRowChoice;
            int o_firstColumnChoice;
            int o_secondRowChoice;
            int o_secondColumnChoice;
            Card o_firstCard;
            Card o_secondCard;
            Player currentPlayer;
            GameManager gameManager = InitializeGame();

            ConsoleRenderer.DisplayBoard(gameManager.Board);
            while (!gameManager.GameOver())
            {
                currentPlayer = gameManager.currentPlayer;

                if (currentPlayer.IsComputer)
                {
                    ComputerTurn(gameManager, out o_firstCard, out o_secondCard, out o_firstRowChoice, out o_firstColumnChoice, out o_secondRowChoice, out o_secondColumnChoice);
                }
                else
                {
                    HumanTurn(gameManager, out o_firstCard, currentPlayer, out o_firstRowChoice, out o_firstColumnChoice);
                    HumanTurn(gameManager, out o_secondCard, currentPlayer, out o_secondRowChoice, out o_secondColumnChoice);
                }

                if (o_firstCard.Value == o_secondCard.Value)
                {
                    MatchFound(gameManager, currentPlayer, o_firstCard.Value);
                }
                else
                {
                    MatchNotFound(gameManager, o_firstRowChoice, o_firstColumnChoice, o_secondRowChoice, o_secondColumnChoice);
                }
            }

            FinishGame(gameManager);
        }

        public GameManager InitializeGame()
        {
            int io_boardHeight;
            int io_boardWidth;
            string player1Name = ConsoleRenderer.GetName(1);
            string player2Name = "ComputerPlayer";
            int numOfPlayers = ConsoleRenderer.GetValidNumOfPlayers();
            bool playWithComputer = true;

            if (numOfPlayers == 2)
            {
                player2Name = ConsoleRenderer.GetName(2);
                playWithComputer = false;
            }

            ConsoleRenderer.GetValidBoardSize(out io_boardHeight, out io_boardWidth);

            return new GameManager(io_boardWidth, io_boardHeight, player1Name, player2Name, playWithComputer);
        }

        public void HumanTurn(GameManager i_GameManager, out Card o_ChosenCard, Player i_CurrentPlayer, out int o_RowIndex, out int o_ColumnIndex)
        {
            ConsoleRenderer.GetValidPlayerChoice(i_GameManager.Board, i_CurrentPlayer, out o_RowIndex, out o_ColumnIndex);
            o_ChosenCard = i_GameManager.Board.ChooseCard(o_RowIndex, o_ColumnIndex);
            ConsoleRenderer.DisplayBoard(i_GameManager.Board);
            i_GameManager.AddCardToMemory(o_ChosenCard.Value, o_RowIndex, o_ColumnIndex);
        }

        public void ComputerTurn(GameManager i_GameManager, out Card firstCard, out Card secondCard, out int o_firstRowChoice, out int o_firstColumnChoice, out int o_secondRowChoice, out int o_secondColumnChoice)
        {
            i_GameManager.GetValidComputerChoice(out o_firstRowChoice, out o_firstColumnChoice, out o_secondRowChoice, out o_secondColumnChoice);
            firstCard = i_GameManager.Board.ChooseCard(o_firstRowChoice, o_firstColumnChoice);
            ConsoleRenderer.DisplayBoard(i_GameManager.Board);
            ConsoleRenderer.ComputerIsPlayingMessage();
            System.Threading.Thread.Sleep(1000);
            secondCard = i_GameManager.Board.ChooseCard(o_secondRowChoice, o_secondColumnChoice);
            ConsoleRenderer.DisplayBoard(i_GameManager.Board);
            ConsoleRenderer.ComputerIsPlayingMessage();
            System.Threading.Thread.Sleep(1000);
        }

        public void MatchFound(GameManager i_GameManager, Player i_CurrentPlayer, int i_MatchedCardValue)
        {
            i_GameManager.Board.NumOfMatches++;
            i_CurrentPlayer.Points++;
            i_GameManager.Memory.Remove(i_MatchedCardValue);
            ConsoleRenderer.ItsAMatchMessage();
        }

        public void MatchNotFound(GameManager i_GameManager, int i_firstRowChoice, int i_firstColumnChoice, int i_secondRowChoice, int i_secondColumnChoice)
        {
            ConsoleRenderer.ItsNotAMatchMessage();
            System.Threading.Thread.Sleep(2000);
            i_GameManager.Board.HideCard(i_firstRowChoice, i_firstColumnChoice);
            i_GameManager.Board.HideCard(i_secondRowChoice, i_secondColumnChoice);
            i_GameManager.ChangeTurns();
            ConsoleRenderer.DisplayBoard(i_GameManager.Board);
        }

        public void FinishGame(GameManager i_GameManager)
        {
            string winnerName = i_GameManager.GetWinner().Name;
            string loserName = i_GameManager.GetLoser().Name;
            int winnerScore = i_GameManager.GetWinner().Points;
            int loserScore = i_GameManager.GetLoser().Points;

            ConsoleRenderer.GameEndingSummary(winnerName, loserName, winnerScore, loserScore);
            bool resetGame = ConsoleRenderer.ResetOrQuitGame();
            if (resetGame)
            {
                ConsoleRenderer.ClearScreen();
                PlayMemoryGame();
            }
        }
    }
}
