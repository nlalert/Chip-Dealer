using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Player : GameObject
{
    private const float HAND_SLIDE_DURATION = 2f;
    private const float SHOOT_SPEED = 900f;
    private const float ROTATION_SPEED = 90f;
    private const float MAX_ROTATION = (float)(80 * (Math.PI / 180)); //80 Degree

    private const float DOT_LINE_LENGTH = 100f;
    private const float DOT_SIZE = 4f;
    private const float DOT_GAP = 8f;

    private const float HAND_MOVE_UP = 50f;


    private float _handSlideTimer;
    private bool _isSliding;
    private Vector2 _initialPosition;

    public SoundEffect SlidingSound;
    public Chip Chip, LastShotChip;
    public Keys Left, Right, Fire;
    
    public Player(Texture2D texture) : base(texture)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 Origin = new Vector2(0, 0);

        Vector2 PositionOffset = new Vector2(ViewportManager.Get("Player_Hand").Width/2, 0);

        DrawDottedLine(spriteBatch, _initialPosition + PositionOffset + Origin - new Vector2(DOT_SIZE/2, 0), Rotation, Color.White);

        if (!Chip.IsShot){
            Chip.Draw(spriteBatch);
        }

        // Draw the sprite with rotation around its center
        spriteBatch.Draw(
            _texture,
            Position + PositionOffset + Origin, // Position adjusted to account for origin
            Viewport,
            Color.White,
            Rotation, 
            Origin,
            Scale,
            SpriteEffects.None,
            0f
        );

        base.Draw(spriteBatch);
    }

    public override void Reset()
    {
        _handSlideTimer = 0;
        _isSliding = false;
        _initialPosition = new Vector2((Singleton.SCREEN_WIDTH - Rectangle.Width) / 2, Singleton.CHIP_SHOOTING_HEIGHT);
        Position = _initialPosition;
        LastShotChip = Chip;
        base.Reset();
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        HandleSlidingMotion(gameTime);
        HandleRotation(gameTime);
        HandleShooting(gameObjects);
        base.Update(gameTime, gameObjects);
    }

    protected void HandleShooting(List<GameObject> gameObjects)
    {
        if (LastShotChip.Speed == 0 && Singleton.Instance.CurrentKey.IsKeyDown(Fire) &&
            Singleton.Instance.PreviousKey != Singleton.Instance.CurrentKey)
        {
            StartSliding();
            ShootChip(gameObjects);
            Singleton.Instance.waitForPlayer = false;
            Singleton.Instance.Money -= 1;
        }
    }

    protected void ShootChip(List<GameObject> gameObjects)
    {
        var newChip = Chip.Clone() as Chip; 

        newChip.Position = new Vector2(Rectangle.Width / 2 + Position.X - newChip.Rectangle.Width / 2,
                                        Singleton.CHIP_SHOOTING_HEIGHT - Singleton.CHIP_SIZE/2);
        newChip.IsShot = true;
        newChip.Rotation = Rotation;
        newChip.Angle = Rotation + (float)(3 * Math.PI / 2);
        newChip.ChipType = Singleton.Instance.CurrentChip;
        newChip.Reset();
        newChip.Speed = SHOOT_SPEED;

        gameObjects.Add(newChip);
        LastShotChip = newChip;

        Singleton.Instance.CurrentChip = Singleton.Instance.NextChip;
        Singleton.Instance.ChipShotAmount++;
    }

    protected void StartSliding()
    {
        _isSliding = true;
        _handSlideTimer = 0f;
        Position -= new Vector2(0, HAND_MOVE_UP); // Move up by 50 pixels

        SlidingSound.Play();
    }

    protected void HandleRotation(GameTime gameTime)
    {
        if (Singleton.Instance.CurrentKey.IsKeyDown(Left))
        {
            Rotation -= MathHelper.ToRadians(ROTATION_SPEED) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        if (Singleton.Instance.CurrentKey.IsKeyDown(Right))
        {
            Rotation += MathHelper.ToRadians(ROTATION_SPEED) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        Rotation = MathHelper.Clamp(Rotation, -MAX_ROTATION, MAX_ROTATION);
    }

    protected void HandleSlidingMotion(GameTime gameTime)
    {
        if (!_isSliding) return;

        _handSlideTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (_handSlideTimer >= HAND_SLIDE_DURATION)
        {
            Position = _initialPosition;
            _isSliding = false;
        }
        else
        {
            Position = Vector2.Lerp(Position, _initialPosition, _handSlideTimer / HAND_SLIDE_DURATION);
        }   
    }

    private void DrawDottedLine(SpriteBatch spriteBatch, Vector2 start, float rotation, Color color)
    {
        // 1x1 pixel texture 
        Texture2D dotTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        dotTexture.SetData(new[] { Color.White });

        // rotate to straight with player
        float adjustedRotation = rotation - MathHelper.PiOver2;
        // cal direction
        Vector2 direction = new Vector2(
            (float)Math.Cos(adjustedRotation),
            (float)Math.Sin(adjustedRotation)
        );

        Vector2 currentPos = start;
        float totalDistance = 0;
        while (totalDistance < DOT_LINE_LENGTH)
        {
            // Draw a dot
            spriteBatch.Draw(
                dotTexture,
                currentPos,
                null,
                color,
                0f,
                Vector2.Zero,
                new Vector2(DOT_SIZE, DOT_SIZE), // Scale the dot size
                SpriteEffects.None,
                0
            );

            currentPos += direction * (DOT_SIZE + DOT_GAP);
            totalDistance += DOT_SIZE + DOT_GAP;
        }
    }
}