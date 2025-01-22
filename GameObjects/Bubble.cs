using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Bubble : GameObject
{   
    public float Angle;
    public float Speed;

    public float Radius;

    public Vector2 BoardCoord;

    public BubbleType BubbleType;
    Color BallColor;

    public Bubble(Texture2D texture) : base(texture)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Viewport, BallColor);
        base.Draw(spriteBatch);
    }

    public override void Reset()
    {
        Speed = 1000f;
        Radius = Singleton.BUBBLE_SIZE / 2;
        BoardCoord = new Vector2(Singleton.BUBBLE_GRID_WIDTH, Singleton.BUBBLE_GRID_HEIGHT);
        BallColor = Singleton.GetBubbleColor(BubbleType);
        base.Reset();
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        Velocity.X = (float) Math.Cos(Angle) * Speed;
        Velocity.Y = (float) Math.Sin(Angle) * Speed;

        Position += Velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);

        //Snap
        if (Position.Y < Singleton.PLAY_AREA_END_Y){
            Position.Y = Singleton.PLAY_AREA_END_Y;
            SnapToGrid();
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
            if (IsTouching(s) && IsTouchingAsCircle(s) && s.Name.Contains("Bubble"))
            {
                SnapToGrid();
                CheckAndDestroySameTypeBubble(gameObjects);
            }
        }

        Velocity = Vector2.Zero;

        base.Update(gameTime, gameObjects);
    }

    private void CheckAndDestroySameTypeBubble(List<GameObject> gameObjects)
    {
        List<Vector2> sameTypeBubbles = new List<Vector2>();

        CheckSameTypeBubbles(BoardCoord, sameTypeBubbles);

        Console.WriteLine("Visited : "+ sameTypeBubbles.Count);
        if(sameTypeBubbles.Count >= 4)
            DestroySameTypeBubbles(sameTypeBubbles, gameObjects);
    }

    protected void SnapToGrid()
    {
        Speed = 0;

        List<(float Distance, Vector2 GridPosition)> closestPositions = new List<(float, Vector2)>();

        // Find all grid positions and calculate their distances
        for (int j = 0; j < Singleton.BUBBLE_GRID_HEIGHT; j++)
        {
            int Xoffset = (j % 2 == 0) ? 0 : (Singleton.BUBBLE_SIZE / 2);

            for (int i = 0; i < Singleton.BUBBLE_GRID_WIDTH; i++)
            {
                //skip last column
                if (Xoffset != 0 && i == Singleton.BUBBLE_GRID_WIDTH - 1)
                    continue;

                float targetX = i * Singleton.BUBBLE_SIZE + Singleton.PLAY_AREA_START_X + Xoffset;
                float targetY = j * Singleton.BUBBLE_SIZE; // Add Y offset if needed

                float distance = Vector2.Distance(new Vector2(targetX, targetY), Position);

                closestPositions.Add((distance, new Vector2(i, j)));
            }
        }

        closestPositions = closestPositions.OrderBy(p => p.Distance).ToList();

        for (int i = 0; i < closestPositions.Count; i++)
        {
            int X = (int) closestPositions[i].GridPosition.X;
            int Y = (int) closestPositions[i].GridPosition.Y;
            int Xoffset = (Y % 2 == 0) ? 0 : (Singleton.BUBBLE_SIZE / 2);

            //skip last column
            if (Xoffset != 0 && X == Singleton.BUBBLE_GRID_WIDTH - 1)
                continue;

            float targetX = X * Singleton.BUBBLE_SIZE + Singleton.PLAY_AREA_START_X + Xoffset;
            float targetY = Y * Singleton.BUBBLE_SIZE;

            if(Singleton.Instance.GameBoard[Y, X] == BubbleType.None)
            {
                Singleton.Instance.GameBoard[Y, X] = BubbleType;
                Position = new Vector2(targetX, targetY);

                BoardCoord = new Vector2(X, Y);
                break;
            }
        }
    }

    protected void CheckSameTypeBubbles(Vector2 gridCoord, List<Vector2> visitedCoord)
    {
        if(visitedCoord.Contains(gridCoord))
            return;

        int X = (int)gridCoord.X;
        int Y = (int)gridCoord.Y;

        Vector2 coord = new Vector2(X, Y);
        visitedCoord.Add(coord);

        bool isOddRow = (Y % 2 == 1);

        if(IsValidAndSame(X, Y, X-1, Y)) CheckSameTypeBubbles(new Vector2(X-1, Y), visitedCoord);
        if(IsValidAndSame(X, Y, X+1, Y)) CheckSameTypeBubbles(new Vector2(X+1, Y), visitedCoord);
        if(IsValidAndSame(X, Y, X, Y-1)) CheckSameTypeBubbles(new Vector2(X, Y-1), visitedCoord);
        if(IsValidAndSame(X, Y, X, Y+1)) CheckSameTypeBubbles(new Vector2(X, Y+1), visitedCoord);
        
        if (isOddRow)
        {
            if(IsValidAndSame(X, Y, X+1, Y-1)) CheckSameTypeBubbles(new Vector2(X+1, Y-1), visitedCoord);
            if(IsValidAndSame(X, Y, X+1, Y+1)) CheckSameTypeBubbles(new Vector2(X+1, Y+1), visitedCoord);
        }
        else
        {
            if(IsValidAndSame(X, Y, X-1, Y-1)) CheckSameTypeBubbles(new Vector2(X-1, Y-1), visitedCoord);
            if(IsValidAndSame(X, Y, X-1, Y+1)) CheckSameTypeBubbles(new Vector2(X-1, Y+1), visitedCoord);
        }
    }

    protected void DestroySameTypeBubbles(List<Vector2> visitedCoord, List<GameObject> gameObjects)
    {
        for (int i = 0; i < visitedCoord.Count; i++)
        {
            Singleton.Instance.GameBoard[(int)visitedCoord[i].Y, (int)visitedCoord[i].X] = BubbleType.None;
            foreach (GameObject s in gameObjects)
            {
                if(s is Bubble && (s as Bubble).BoardCoord == visitedCoord[i])
                {
                    s.IsActive = false;
                }
            }
        }
    }

    protected bool IsValidAndSame(int x, int y, int refX, int refY)
    {
        if (refX >= 0 && refX < Singleton.BUBBLE_GRID_WIDTH && 
            refY >= 0 && refY < Singleton.BUBBLE_GRID_HEIGHT)
        {
            return Singleton.Instance.GameBoard[y, x] == Singleton.Instance.GameBoard[refY, refX];
        }
        return false;
    }
    
    protected bool IsTouchingAsCircle(GameObject g)
    {
        
        if (g is Bubble otherBubble && this is Bubble)
        {
            float distance = Vector2.Distance(this.Position, otherBubble.Position);
            // Console.WriteLine($"distace : {distance}");
            return distance < this.Radius + otherBubble.Radius;
        }
        return false;
    }

}
