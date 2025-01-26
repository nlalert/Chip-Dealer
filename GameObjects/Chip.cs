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
                if (s is Chip && IsTouching(s) && IsTouchingAsCircle(s))
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
                float targetY = j * Singleton.CHIP_SIZE + Singleton.Instance.CeilingPosition;

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
            float targetY = Y * Singleton.CHIP_SIZE + Singleton.Instance.CeilingPosition;

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

    public virtual void CheckAndDestroySameTypeChip(List<GameObject> gameObjects)
    {
        List<Vector2> sameTypeChips = new List<Vector2>();

        Singleton.Instance.GameBoard.GetAllConnectedSameTypeChips(BoardCoord, sameTypeChips);

        if(sameTypeChips.Count >= Singleton.CHIP_BREAK_AMOUNT)
            Singleton.Instance.GameBoard.DestroyChips(sameTypeChips, gameObjects);
    }
}
