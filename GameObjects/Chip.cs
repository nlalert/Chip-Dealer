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
    public bool IsShot;

    public bool IsFalling;
    private float FallSpeed;

    public int Score;
    private float _explosiveTimer;
    private bool _explosiveFrameToggle;

    public Vector2 BoardCoord;

    public ChipType ChipType;

    public SoundEffect ChipHitSound;


    public Chip(Texture2D texture) : base(texture)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsShot)
        {
            DrawCurrentChipOnHand();
        }
        
        spriteBatch.Draw(_texture, Position, Viewport, Color.White);
        base.Draw(spriteBatch);
    }

    public override void Reset()
    {
        IsFalling = false;
        Speed = 0;
        FallSpeed = 1000f;
        Radius = Singleton.CHIP_SIZE / 2;
        BoardCoord = new Vector2(Singleton.CHIP_GRID_WIDTH, Singleton.CHIP_GRID_HEIGHT);
        _explosiveTimer = 0f;
        _explosiveFrameToggle = false;
        ResetChipTexture();

        base.Reset();
    }


    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        if (IsFalling)
        {
            ApplyGravity(gameTime);
            HandleChipOutOfBounds(gameObjects);
        }
        else
        {
            MoveChip(gameTime);
            CheckCollisions(gameTime, gameObjects);
        }

       // if(!_isShot){
            HandleExplosiveAnimation(gameTime);
            // Console.WriteLine(_explosiveTimer);
        //}

        Velocity = Vector2.Zero;

        base.Update(gameTime, gameObjects);
    }

    public void ResetChipTexture()
    {
        Viewport = Singleton.GetChipViewPort(ChipType);
    }

    protected void DrawCurrentChipOnHand()
    {
        Position = new Vector2((Singleton.SCREEN_WIDTH / 2) - 16, Singleton.CHIP_SHOOTING_HEIGHT - Singleton.CHIP_SIZE / 2);
        Viewport = Singleton.GetChipViewPort(Singleton.Instance.CurrentChip);

        if (Singleton.Instance.CurrentChip == ChipType.Explosive)
        {
            Viewport = _explosiveFrameToggle
                ? Singleton.GetViewPortFromSpriteSheet("Explosive_Chip0")
                : Singleton.GetViewPortFromSpriteSheet("Explosive_Chip1");
        }
    }
    
    protected void CheckCollisions(GameTime gameTime, List<GameObject> gameObjects)
    {
        HandleCeilingCollision(gameObjects);
        HandleWallCollisions();
        HandleChipCollisions(gameTime, gameObjects);
    }

    protected void HandleCeilingCollision(List<GameObject> gameObjects)
    {
        if (Position.Y >= Singleton.Instance.CeilingPosition) return;

        Position.Y = Singleton.Instance.CeilingPosition;
        SnapToGrid();
        DestroyAroundChips(gameObjects);
        Singleton.Instance.CurrentGameState = Singleton.GameState.CheckCeiling;
    }

    protected void HandleWallCollisions()
    {
        if (Position.X < Singleton.PLAY_AREA_START_X)
        {
            Position.X = Singleton.PLAY_AREA_START_X;
            Angle = (float)Math.PI - Angle;
        }
        
        if(Position.X > Singleton.PLAY_AREA_END_X - Rectangle.Width) 
        {
            Position.X = Singleton.PLAY_AREA_END_X - Rectangle.Width;
            Angle = (float)Math.PI - Angle;
        }
    }

    protected void HandleChipCollisions(GameTime gameTime, List<GameObject> gameObjects)
    {
        foreach (var obj in gameObjects)
        {
            if (obj == this || !(obj is Chip otherChip) || otherChip.IsFalling || !IsTouchingAsCircle(obj))
                continue;

            MoveBackUntilNoCollision(gameTime, obj);
            SnapToGrid();
            DestroyAroundChips(gameObjects);
            Singleton.Instance.CurrentGameState = Singleton.GameState.CheckCeiling;
        }
    }

    protected void DestroyAroundChips(List<GameObject> gameObjects)
    {
        switch (ChipType)
        {
            case ChipType.Explosive:
                Singleton.Instance.GameBoard.DestroyAdjacentChips(BoardCoord, gameObjects);
                break;
            default:
                Singleton.Instance.GameBoard.DestroyConnectedSameTypeChips(BoardCoord, gameObjects);
                break;
        }
    }

    protected void HandleChipOutOfBounds(List<GameObject> gameObjects)
    {
        if (Position.Y <= Singleton.SCREEN_HEIGHT) return;

        IsActive = false;
        Singleton.Instance.GameBoard.DestroySingleChip((int)BoardCoord.Y, (int)BoardCoord.X, gameObjects);
        Singleton.Instance.CurrentGameState = Singleton.GameState.CheckGameBoard;
    }

    protected void ApplyGravity(GameTime gameTime)
    {
        Position.Y += FallSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    protected void MoveBackUntilNoCollision(GameTime gameTime, GameObject otherChip)
    {
        Angle = Angle - (float)Math.PI;
        do
        {
            Speed = 10;
            MoveChip(gameTime);

        } while (IsTouchingAsCircle(otherChip));
    }

    protected void MoveChip(GameTime gameTime)
    {
        Velocity.X = (float) Math.Cos(Angle) * Speed;
        Velocity.Y = (float) Math.Sin(Angle) * Speed;

        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            int xOffset = (j % 2 == 0) ? 0 : (Singleton.CHIP_SIZE / 2);
            for (int i = x - 1; i <= x + 1; i++)
            {
                if (!Singleton.Instance.GameBoard.IsInsideBounds(j, i))
                    continue;
                if (Singleton.Instance.GameBoard.IsUnUseSpot(j, i))
                    continue;
                if (Singleton.Instance.GameBoard.HaveChip(j, i))
                    continue;

                float targetX = i * Singleton.CHIP_SIZE + Singleton.PLAY_AREA_START_X + xOffset;
                float targetY = j * Singleton.CHIP_SIZE + Singleton.Instance.CeilingPosition;
                float distance = Vector2.Distance(new Vector2(targetX, targetY), Position);

                if (distance <= closestDistance)
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

    protected void HandleExplosiveAnimation(GameTime gameTime)
    {
        _explosiveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_explosiveTimer >= 1f)
        {
            _explosiveTimer = 0f;
            _explosiveFrameToggle = !_explosiveFrameToggle;
        }
    }
}
