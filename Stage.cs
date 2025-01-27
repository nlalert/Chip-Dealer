public class Stage
{
    public static void SetUpBoard()
    {
        switch (Singleton.Instance.Stage)
        {
            case 1:
                Singleton.Instance.GameBoard[0, 0] = ChipType.Red;
                Singleton.Instance.GameBoard[0, 1] = ChipType.Red;
                Singleton.Instance.GameBoard[0, 2] = ChipType.Yellow;
                Singleton.Instance.GameBoard[0, 3] = ChipType.Yellow;
                Singleton.Instance.GameBoard[0, 4] = ChipType.Blue;
                Singleton.Instance.GameBoard[0, 5] = ChipType.Blue;
                Singleton.Instance.GameBoard[0, 6] = ChipType.Green;
                Singleton.Instance.GameBoard[0, 7] = ChipType.Green;

                Singleton.Instance.GameBoard[1, 0] = ChipType.Red;
                Singleton.Instance.GameBoard[1, 1] = ChipType.Red;
                Singleton.Instance.GameBoard[1, 2] = ChipType.Yellow;
                Singleton.Instance.GameBoard[1, 3] = ChipType.Yellow;
                Singleton.Instance.GameBoard[1, 4] = ChipType.Blue;
                Singleton.Instance.GameBoard[1, 5] = ChipType.Blue;
                Singleton.Instance.GameBoard[1, 6] = ChipType.Green;

                Singleton.Instance.GameBoard[2, 0] = ChipType.Blue;
                Singleton.Instance.GameBoard[2, 1] = ChipType.Blue;
                Singleton.Instance.GameBoard[2, 2] = ChipType.Green;
                Singleton.Instance.GameBoard[2, 3] = ChipType.Green;
                Singleton.Instance.GameBoard[2, 4] = ChipType.Red;
                Singleton.Instance.GameBoard[2, 5] = ChipType.Red;
                Singleton.Instance.GameBoard[2, 6] = ChipType.Yellow;
                Singleton.Instance.GameBoard[2, 7] = ChipType.Yellow;

                Singleton.Instance.GameBoard[3, 0] = ChipType.Blue;
                Singleton.Instance.GameBoard[3, 1] = ChipType.Green;
                Singleton.Instance.GameBoard[3, 2] = ChipType.Green;
                Singleton.Instance.GameBoard[3, 3] = ChipType.Red;
                Singleton.Instance.GameBoard[3, 4] = ChipType.Red;
                Singleton.Instance.GameBoard[3, 5] = ChipType.Yellow;
                Singleton.Instance.GameBoard[3, 6] = ChipType.Yellow;
                break;
            case 2:
                Singleton.Instance.GameBoard[0, 0] = ChipType.Red;
                Singleton.Instance.GameBoard[0, 1] = ChipType.Red;
                Singleton.Instance.GameBoard[0, 2] = ChipType.Yellow;
                Singleton.Instance.GameBoard[0, 3] = ChipType.Yellow;
                Singleton.Instance.GameBoard[0, 4] = ChipType.Blue;
                Singleton.Instance.GameBoard[0, 5] = ChipType.Blue;
                Singleton.Instance.GameBoard[0, 6] = ChipType.Green;
                Singleton.Instance.GameBoard[0, 7] = ChipType.Green;

                Singleton.Instance.GameBoard[1, 0] = ChipType.Red;
                Singleton.Instance.GameBoard[1, 1] = ChipType.Red;
                Singleton.Instance.GameBoard[1, 2] = ChipType.Yellow;
                Singleton.Instance.GameBoard[1, 3] = ChipType.Yellow;
                Singleton.Instance.GameBoard[1, 4] = ChipType.Blue;
                Singleton.Instance.GameBoard[1, 5] = ChipType.Blue;
                Singleton.Instance.GameBoard[1, 6] = ChipType.Green;

                Singleton.Instance.GameBoard[2, 0] = ChipType.Blue;
                Singleton.Instance.GameBoard[2, 1] = ChipType.Blue;
                Singleton.Instance.GameBoard[2, 2] = ChipType.Green;
                Singleton.Instance.GameBoard[2, 3] = ChipType.Green;
                Singleton.Instance.GameBoard[2, 4] = ChipType.Red;
                Singleton.Instance.GameBoard[2, 5] = ChipType.Red;
                Singleton.Instance.GameBoard[2, 6] = ChipType.Yellow;
                Singleton.Instance.GameBoard[2, 7] = ChipType.Yellow;

                Singleton.Instance.GameBoard[3, 0] = ChipType.Blue;
                Singleton.Instance.GameBoard[3, 1] = ChipType.Green;
                Singleton.Instance.GameBoard[3, 2] = ChipType.Green;
                Singleton.Instance.GameBoard[3, 3] = ChipType.Red;
                Singleton.Instance.GameBoard[3, 4] = ChipType.Red;
                Singleton.Instance.GameBoard[3, 5] = ChipType.Yellow;
                Singleton.Instance.GameBoard[3, 6] = ChipType.Yellow;
                break;
        }
    }
}