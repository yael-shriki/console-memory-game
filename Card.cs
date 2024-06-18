namespace ConsoleMemoryGame
{
    public struct Card
    {
        private readonly int r_CardValue;
        private bool m_IsRevealed;

        public Card(int i_CardValue, bool i_IsRevealed)
        {
            r_CardValue = i_CardValue;
            m_IsRevealed = i_IsRevealed;
        }

        public int Value
        {
            get { return r_CardValue; }
        }

        public bool IsRevealed
        {
            get { return m_IsRevealed; }

            set { m_IsRevealed = value; }
        }
    }
}

