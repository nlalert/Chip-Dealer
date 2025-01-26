using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
class ExplosiveChip : Chip
{
    public ExplosiveChip(Texture2D texture) : base(texture)
    {  
        ChipType = ChipType.Explosive;
    }

    public override void CheckAndDestroySameTypeChip(List<GameObject> gameObjects)
    {   
        GameBoard gameBoard = Singleton.Instance.GameBoard;

        // Get current position on the board
        int row = (int)BoardCoord.Y;
        int col = (int)BoardCoord.X;

        // Find all six neighbors
        List<Vector2> neighbors = GetSurroundingPositions(row, col);

        // Destroy the neighbors
        foreach (var neighbor in neighbors)
        {
            int neighborRow = (int)neighbor.Y;
            int neighborCol = (int)neighbor.X;

            if (gameBoard.IsInsideBounds(neighborRow, neighborCol) && gameBoard[neighborRow, neighborCol] != ChipType.None)
            {
                gameBoard[neighborRow, neighborCol] = ChipType.None;
                foreach (GameObject gameObject in gameObjects)
                {
                    if (gameObject is Chip chip && chip.BoardCoord == neighbor)
                    {
                        chip.IsActive = false;
                        Singleton.Instance.Score += chip.Score; 
                        break;
                    }
                }
            }
        }

        // Also deactivate the explosive chip itself
        Singleton.Instance.GameBoard[row, col] = ChipType.None;
        this.IsActive = false;
    }

    private List<Vector2> GetSurroundingPositions(int row, int col)
    {

        List<(int row,int col)> offsets;

        //determine is odd or even
        if(row % 2 == 1){
            offsets = new List<(int, int)> { (-1, 0), (-1, 1), (0, -1), (0, 1), (1, 0), (1, 1)};
        }else{
            offsets = new List<(int, int)> { (-1, -1), (-1, 0), (0, -1), (0, 1), (1, -1), (1, 0) };
        }

        // Compute the surrounding positions
        List<Vector2> neighbors = new List<Vector2>();
        foreach (var offset in offsets)
        {
            int neighborRow = row + offset.row;
            int neighborCol = col + offset.col;
            neighbors.Add(new Vector2(neighborCol, neighborRow));
        }
        return neighbors;
    }
}
