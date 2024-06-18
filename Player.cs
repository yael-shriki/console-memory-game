namespace ConsoleMemoryGame
{
    public class Player
    {
        private readonly string r_Name;
        private readonly bool r_IsComputer;
        private int m_Points;

        public Player(string i_Name, bool i_IsComputer)
        {
            r_Name = i_Name;
            r_IsComputer = i_IsComputer;
            m_Points = 0;
        }

        public int Points
        {
            get { return m_Points; }
            set { m_Points = value; }
        }

        public string Name
        {
            get { return r_Name; }
        }

        public bool IsComputer
        {
            get { return r_IsComputer; }
        }

        public static bool ValidateName(string i_Name)
        {
            bool isValidName = true;

            if (i_Name.Length > 20 || i_Name.Contains(" "))
            {
                isValidName = false;
            }

            return isValidName;
        }
    }
}

