using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

class Chip : GameObject
{   
    public float Angle;
    public float Speed;

    public float Radius;

    public Vector2 BoardCoord;

    public ChipType ChipType;

    public SoundEffect ChipHitSound;

    public Chip(Texture2D texture) : base(texture)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Viewport, Color.White);
        base.Draw(spriteBatch);
    }

    public override void Reset()
    {
        Speed = 0;
        Radius = Singleton.CHIP_SIZE / 2;
        BoardCoord = new Vector2(Singleton.CHIP_GRID_WIDTH, Singleton.CHIP_GRID_HEIGHT);

        ResetChipTexture();

        base.Reset();
    }

    public void ResetChipTexture()
    {
        switch (ChipType)
        {
            case ChipType.Red:
                Viewport = new Rectangle(0, 0, 32, 32);
                break;
            case ChipType.Blue:
                Viewport = new Rectangle(32, 0, 32, 32);
                break;
            case ChipType.Green:
                Viewport = new Rectangle(64, 0, 32, 32);
                break;
            case ChipType.Yellow:
                Viewport = new Rectangle(96, 0, 32, 32);
                break;
        }
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        Velocity.X = (float) Math.Cos(Angle) * Speed;
        Velocity.Y = (float) Math.Sin(Angle) * Speed;

        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if(Speed != 0)
        {
            if (Position.Y < Singleton.Instance.PlayAreaStartY){
                Position.Y = Singleton.Instance.PlayAreaStartY;
                SnapToGrid();
                CheckAndDestroySameTypeChip(gameObjects);
                Singleton.Instance.CurrentGameState = Singleton.GameState.CheckChipAndCeiling;
            }

            if (Position.X < Singleton.PLAY_AREA_START_X){
                Position.X = Singleton.PLAY_AREA_START_X;
                Angle = (float)Math.PI - Angle;
            }
            
            if(Position.X > Singleton.PLAY_AREA_END_X - Rectangle.Width) 
            {
                Position.X = Singleton.PLAY_AREA_END_X - Rectangle.Width;
                Angle = (float)Math.PI - Angle;
            }
            
            foreach (GameObject s in gameObjects)
            {
                if (IsTouching(s) && IsTouchingAsCircle(s) && s is Chip)
                {
                    SnapToGrid();
                    CheckAndDestroySameTypeChip(gameObjects);
                    Singleton.Instance.CurrentGameState = Singleton.GameState.CheckChipAndCeiling;
                }
            }
        }

        Velocity = Vector2.Zero;

        base.Update(gameTime, gameObjects);
    }

    protected void SnapToGrid()
    {
        Speed = 0;

        List<(float Distance, Vector2 GridPosition)> closestPositions = new List<(float, Vector2)>();

        // Find all grid positions and calculate their distances
        for (int j = 0; j < Singleton.CHIP_GRID_HEIGHT; j++)
        {
            int Xoffset = (j % 2 == 0) ? 0 : (Singleton.CHIP_SIZE / 2);

            for (int i = 0; i < Singleton.CHIP_GRID_WIDTH; i++)
            {
                //skip last column
                if (Xoffset != 0 && i == Singleton.CHIP_GRID_WIDTH - 1)
                    continue;

                float targetX = i * Singleton.CHIP_SIZE + Singleton.PLAY_AREA_START_X + Xoffset;
                float targetY = j * Singleton.CHIP_SIZE + Singleton.Instance.PlayAreaStartY;

                float distance = Vector2.Distance(new Vector2(targetX, targetY), Position);

                closestPositions.Add((distance, new Vector2(i, j)));
            }
        }

        closestPositions = closestPositions.OrderBy(p => p.Distance).ToList();

        for (int i = 0; i < closestPositions.Count; i++)
        {
            int X = (int) closestPositions[i].GridPosition.X;
            int Y = (int) closestPositions[i].GridPosition.Y;
            int Xoffset = (Y % 2 == 0) ? 0 : (Singleton.CHIP_SIZE / 2);

            //skip last column
            if (Xoffset != 0 && X == Singleton.CHIP_GRID_WIDTH - 1)
                continue;

            float targetX = X * Singleton.CHIP_SIZE + Singleton.PLAY_AREA_START_X + Xoffset;
            float targetY = Y * Singleton.CHIP_SIZE + Singleton.Instance.PlayAreaStartY;

            if(Singleton.Instance.GameBoard[Y, X] == ChipType.None)
            {
                Singleton.Instance.GameBoard[Y, X] = ChipType;
                Position = new Vector2(targetX, targetY);

                BoardCoord = new Vector2(X, Y);

                ChipHitSound.Play();
                break;
            }
        }
    }

    private void CheckAndDestroySameTypeChip(List<GameObject> gameObjects)
    {
        List<Vector2> sameTypeChips = new List<Vector2>();

        CheckSameTypeChips(BoardCoord, sameTypeChips);

        if(sameTypeChips.Count >= Singleton.CHIP_BREAK_AMOUNT)
            DestroySameTypeChips(sameTypeChips, gameObjects);
    }

    protected void CheckSameTypeChips(Vector2 gridCoord, List<Vector2> visitedCoord)
    {
        if(visitedCoord.Contains(gridCoord))
            return;

        int X = (int)gridCoord.X;
        int Y = (int)gridCoord.Y;

        visitedCoord.Add(new Vector2(X, Y));

        if(IsValidAndSame(X, Y, X-1, Y)) CheckSameTypeChips(new Vector2(X-1, Y), visitedCoord);
        if(IsValidAndSame(X, Y, X+1, Y)) CheckSameTypeChips(new Vector2(X+1, Y), visitedCoord);
        if(IsValidAndSame(X, Y, X, Y-1)) CheckSameTypeChips(new Vector2(X, Y-1), visitedCoord);
        if(IsValidAndSame(X, Y, X, Y+1)) CheckSameTypeChips(new Vector2(X, Y+1), visitedCoord);

        bool isOddRow = (Y % 2 == 1);
        
        if (isOddRow)
        {
            if(IsValidAndSame(X, Y, X+1, Y-1)) CheckSameTypeChips(new Vector2(X+1, Y-1), visitedCoord);
            if(IsValidAndSame(X, Y, X+1, Y+1)) CheckSameTypeChips(new Vector2(X+1, Y+1), visitedCoord);
        }
        else
        {
            if(IsValidAndSame(X, Y, X-1, Y-1)) CheckSameTypeChips(new Vector2(X-1, Y-1), visitedCoord);
            if(IsValidAndSame(X, Y, X-1, Y+1)) CheckSameTypeChips(new Vector2(X-1, Y+1), visitedCoord);
        }
    }

    protected void DestroySameTypeChips(List<Vector2> visitedCoord, List<GameObject> gameObjects)
    {
        for (int i = 0; i < visitedCoord.Count; i++)
        {
            Singleton.Instance.GameBoard[(int)visitedCoord[i].Y, (int)visitedCoord[i].X] = ChipType.None;
            foreach (GameObject s in gameObjects)
            {
                if(s is Chip && (s as Chip).BoardCoord == visitedCoord[i])
                {
                    s.IsActive = false;
                }
            }
        }
    }

    protected bool IsValidAndSame(int x, int y, int refX, int refY)
    {
        if (refX >= 0 && refX < Singleton.CHIP_GRID_WIDTH && 
            refY >= 0 && refY < Singleton.CHIP_GRID_HEIGHT)
        {
            return Singleton.Instance.GameBoard[y, x] == Singleton.Instance.GameBoard[refY, refX];
        }
        return false;
    }
    
    protected bool IsTouchingAsCircle(GameObject g)
    {
        
        if (g is Chip otherChip && this is Chip)
        {
            float distance = Vector2.Distance(this.Position, otherChip.Position);
            // Console.WriteLine($"distace : {distance}");
            return distance < this.Radius + otherChip.Radius;
        }
        return false;
    }

}
