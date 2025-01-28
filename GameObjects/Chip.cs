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

    public bool _isShot;

    public int Score;

    public Vector2 BoardCoord;

    public ChipType ChipType;

    public SoundEffect ChipHitSound;

    public Chip(Texture2D texture) : base(texture)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!_isShot){

            Position = new Vector2((Singleton.SCREEN_WIDTH / 2) - 16, Singleton.CHIP_SHOOTING_HEIGHT - Singleton.CHIP_SIZE/2);
            //draw current chip on hand
            Viewport = Singleton.GetChipViewPort(Singleton.Instance.CurrentChip);

        }

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
        Viewport = Singleton.GetChipViewPort(ChipType);
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        Velocity.X = (float) Math.Cos(Angle) * Speed;
        Velocity.Y = (float) Math.Sin(Angle) * Speed;

        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if(Speed != 0)
        {
            if (Position.Y < Singleton.Instance.CeilingPosition){
                Position.Y = Singleton.Instance.CeilingPosition;
                SnapToGrid();
                Singleton.Instance.GameBoard.DestroyConnectedSameTypeChips(BoardCoord, gameObjects);
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
                if (s is Chip && IsTouching(s) && IsTouchingAsCircle(s))
                {
                    SnapToGrid();
                    Singleton.Instance.GameBoard.DestroyConnectedSameTypeChips(BoardCoord, gameObjects);
                    Singleton.Instance.CurrentGameState = Singleton.GameState.CheckChipAndCeiling;
                }
            }
        }
        else if(ChipType == ChipType.Explosive)
        {
            Singleton.Instance.GameBoard.DestroyAdjacentChips(BoardCoord, gameObjects);
            Singleton.Instance.CurrentGameState = Singleton.GameState.CheckChipAndCeiling;
        }

        Velocity = Vector2.Zero;

        base.Update(gameTime, gameObjects);
    }

    protected void SnapToGrid()
    {
        Speed = 0;

        Vector2 gridPosition = CalculateApproxGridPosition();

        Vector2 closestSpot = FindClosestEmptySpot(gridPosition);

        PlaceChipOnGrid(closestSpot);
    }

    protected Vector2 CalculateApproxGridPosition()
    {
        int approxY = (int)Math.Round((Position.Y - Singleton.Instance.CeilingPosition) / Singleton.CHIP_SIZE);
        int offset = (approxY % 2 == 0) ? 0 : (Singleton.CHIP_SIZE / 2);
        int approxX = (int)Math.Round((Position.X - offset - Singleton.PLAY_AREA_START_X) / Singleton.CHIP_SIZE);
        
        return new Vector2(approxX, approxY);
    }

    protected Vector2 FindClosestEmptySpot(Vector2 gridPosition)
    {
        int x = (int) gridPosition.X;
        int y = (int) gridPosition.Y;
        
        float closestDistance = float.MaxValue;
        Vector2 closestSpot = Vector2.Zero;

        for (int j = y - 1; j <= y + 1; j++)
        {
            int xOffset = (y % 2 == 0) ? 0 : (Singleton.CHIP_SIZE / 2);
            for (int i = x - 1; i <= x + 1; i++)
            {
                if (!Singleton.Instance.GameBoard.IsInsideBounds(j, i))
                    continue;
                if (Singleton.Instance.GameBoard.HaveChip(j, i))
                    continue;

                float targetX = i * Singleton.CHIP_SIZE + Singleton.PLAY_AREA_START_X + xOffset;
                float targetY = j * Singleton.CHIP_SIZE + Singleton.Instance.CeilingPosition;
                float distance = Vector2.Distance(new Vector2(targetX, targetY), Position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSpot = new Vector2(i, j);
                }
            }
        }

        return closestSpot;
    }

    protected void PlaceChipOnGrid(Vector2 closestSpot)
    {
        int approxX = (int)closestSpot.X;
        int approxY = (int)closestSpot.Y;


        Singleton.Instance.GameBoard[approxY, approxX] = ChipType;
        BoardCoord = new Vector2(approxX, approxY);

        int xOffset = (approxY % 2 == 0) ? 0 : (Singleton.CHIP_SIZE / 2);
        float targetX = approxX * Singleton.CHIP_SIZE + Singleton.PLAY_AREA_START_X + xOffset;
        float targetY = approxY * Singleton.CHIP_SIZE + Singleton.Instance.CeilingPosition;
        Position = new Vector2(targetX, targetY);

        ChipHitSound.Play();
    }
}
