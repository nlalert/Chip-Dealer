using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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
                if (_board[row, col] != ChipType.None && (int)_board[row, col] < 5  )
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
    public bool HaveChip(int row, int col)
    {
        if (IsInsideBounds(row,col ))
        {
            return _board[row, col] != ChipType.None;
        }
        return false;
    }

    public bool IsSameChipType(int row1, int col1, int row2, int col2)
    {
        if (IsInsideBounds(row2, col2))
        {
            return _board[row1, col1] == _board[row2, col2];
        }
        return false;
    }

    public void GetAllConnectedChips(Vector2 chipCoord, List<Vector2> connectedChips)
    {
         if(connectedChips.Contains(chipCoord))
            return;

        int X = (int)chipCoord.X;
        int Y = (int)chipCoord.Y;

        connectedChips.Add(new Vector2(X, Y));

        if(HaveChip(Y, X-1)) GetAllConnectedChips(new Vector2(X-1, Y), connectedChips);
        if(HaveChip(Y, X+1)) GetAllConnectedChips(new Vector2(X+1, Y), connectedChips);
        if(HaveChip(Y-1, X)) GetAllConnectedChips(new Vector2(X, Y-1), connectedChips);
        if(HaveChip(Y+1,X)) GetAllConnectedChips(new Vector2(X, Y+1), connectedChips);

        bool isOddRow = (Y % 2 == 1);
        
        if (isOddRow)
        {
            if(HaveChip(Y-1, X+1)) GetAllConnectedChips(new Vector2(X+1, Y-1), connectedChips);
            if(HaveChip(Y+1, X+1)) GetAllConnectedChips(new Vector2(X+1, Y+1), connectedChips);
        }
        else
        {
            if(HaveChip(Y-1, X-1)) GetAllConnectedChips(new Vector2(X-1, Y-1), connectedChips);
            if(HaveChip(Y+1, X-1)) GetAllConnectedChips(new Vector2(X-1, Y+1), connectedChips);
        }
    }

    public void GetAllConnectedSameTypeChips(Vector2 chipCoord, List<Vector2> connectedChips)
    {
         if(connectedChips.Contains(chipCoord))
            return;

        int X = (int)chipCoord.X;
        int Y = (int)chipCoord.Y;

        connectedChips.Add(new Vector2(X, Y));

        if(IsSameChipType(Y, X, Y, X-1)) GetAllConnectedSameTypeChips(new Vector2(X-1, Y), connectedChips);
        if(IsSameChipType(Y, X, Y, X+1)) GetAllConnectedSameTypeChips(new Vector2(X+1, Y), connectedChips);
        if(IsSameChipType(Y, X, Y-1, X)) GetAllConnectedSameTypeChips(new Vector2(X, Y-1), connectedChips);
        if(IsSameChipType(Y, X, Y+1,X)) GetAllConnectedSameTypeChips(new Vector2(X, Y+1), connectedChips);

        bool isOddRow = (Y % 2 == 1);
        
        if (isOddRow)
        {
            if(IsSameChipType(Y, X, Y-1, X+1)) GetAllConnectedSameTypeChips(new Vector2(X+1, Y-1), connectedChips);
            if(IsSameChipType(Y, X, Y+1, X+1)) GetAllConnectedSameTypeChips(new Vector2(X+1, Y+1), connectedChips);
        }
        else
        {
            if(IsSameChipType(Y, X, Y-1, X-1)) GetAllConnectedSameTypeChips(new Vector2(X-1, Y-1), connectedChips);
            if(IsSameChipType(Y, X, Y+1, X-1)) GetAllConnectedSameTypeChips(new Vector2(X-1, Y+1), connectedChips);
        }
    }
}