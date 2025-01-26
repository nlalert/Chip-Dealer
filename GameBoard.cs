using System;
using System.Collections.Generic;

public class GameBoard
{
    private ChipType[,] _board;

    public int Rows => _board.GetLength(0); // Number of rows
    public int Columns => _board.GetLength(1); // Number of columns

    public GameBoard(int rows, int columns)
    {
        _board = new ChipType[rows, columns];
    }

    // Indexer for 2D array access
    public ChipType this[int row, int col]
    {
        get
        {
            ValidateIndex(row, col);
            return _board[row, col];
        }
        set
        {
            ValidateIndex(row, col);
            _board[row, col] = value;
        }
    }

    // Add methods for additional functionality
    public bool IsInsideBounds(int row, int col)
    {
        return row >= 0 && row < Rows && col >= 0 && col < Columns;
    }

    public bool IsUnUseSpot(int row, int col)
    {
        return row % 2 == 1 && col == Columns - 1;
    }

    public void ClearBoard(ChipType defaultChip = ChipType.None)
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                _board[row, col] = defaultChip;
            }
        }
    }

    // Private method to validate index
    private void ValidateIndex(int row, int col)
    {
        if (!IsInsideBounds(row, col))
        {
            throw new IndexOutOfRangeException($"Index ({row}, {col}) is out of bounds.");
        }
    }

    public ChipType GetRandomChipColor(){
        // Create a list to store non-None chip types
        List<ChipType> nonNoneChipTypes = new List<ChipType>();

        // Iterate through the GameBoard and collect non-None chip types
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                if (_board[row, col] != ChipType.None)
                {
                    nonNoneChipTypes.Add(_board[row, col]);
                }
            }
        }

        // If no non-None chip types are found
        if (nonNoneChipTypes.Count == 0)
        {
            return (ChipType) Singleton.Instance.Random.Next(1, 5);
        }

        // Randomly select a chip type from the list
        int randomIndex = Singleton.Instance.Random.Next(nonNoneChipTypes.Count);
        return nonNoneChipTypes[randomIndex];
    }

}