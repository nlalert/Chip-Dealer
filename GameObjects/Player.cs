using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Player : GameObject
{
    public float _handSlideTimer = 0f;
    public SoundEffect SlidingSound;
    public Vector2 _initialPosition;
    public bool _isSliding = false;
    public Chip Chip, LastShotChip;
    public Keys Left, Right, Fire;
    
    public Player(Texture2D texture) : base(texture)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 Origin = new Vector2(0, 0);
        
        //draw aim line
        float DotLinelength = 100f;
        float DotSize = 4f;
        float DotGap = 8f;

        Vector2 PositionOffset = new Vector2(Singleton.GetViewPortFromSpriteSheet("Player_Hand").Width/2, 0);

        DrawDottedLine(spriteBatch, _initialPosition + PositionOffset + Origin - new Vector2(DotSize/2, 0), Rotation, DotLinelength, Color.White, DotSize, DotGap);

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
        _initialPosition = new Vector2((Singleton.SCREEN_WIDTH - Rectangle.Width) / 2, Singleton.CHIP_SHOOTING_HEIGHT);
        Position = _initialPosition;
        LastShotChip = Chip;
        base.Reset();
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        if (_isSliding)
        {
            _handSlideTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_handSlideTimer >= 2f) // Return to original position after 2 second
            {
                Position = _initialPosition;
                _isSliding = false;
            }
            else
            {
                // Smoothly return to original position over 2 second
                Position = Vector2.Lerp(Position, _initialPosition, _handSlideTimer);
            }
        }

        if(Singleton.Instance.CurrentKey.IsKeyDown(Left))
        {
            Rotation -= MathHelper.ToRadians(90f) * (float)gameTime.ElapsedGameTime.TotalSeconds; // Rotate counterclockwise
        }
        if(Singleton.Instance.CurrentKey.IsKeyDown(Right))
        {
            Rotation += MathHelper.ToRadians(90f) * (float)gameTime.ElapsedGameTime.TotalSeconds; // Rotate clockwise
        }

        Rotation = MathHelper.Clamp(Rotation, -Singleton.MAX_PLAYER_ROTATION, Singleton.MAX_PLAYER_ROTATION);

        if(LastShotChip.Speed == 0)
        {
            if( Singleton.Instance.CurrentKey.IsKeyDown(Fire) &&
                Singleton.Instance.PreviousKey != Singleton.Instance.CurrentKey)
            {
                OnShoot(gameObjects);
                Singleton.Instance.CurrentChip = Singleton.Instance.NextChip;
                Singleton.Instance.ChipShotAmount++;
            }
        }

        base.Update(gameTime, gameObjects);
    }

    private void OnShoot(List<GameObject> gameObjects)
    {
        _isSliding = true;
        _handSlideTimer = 0f;
        Position -= new Vector2(0, 50); // Move up by 50 pixels

        SlidingSound.Play();

        var newChip = Chip.Clone() as Chip; 

        newChip.Position = new Vector2(Rectangle.Width / 2 + Position.X - newChip.Rectangle.Width / 2,
                                        Singleton.CHIP_SHOOTING_HEIGHT - Singleton.CHIP_SIZE/2);
        newChip.IsShot = true;
        newChip.Rotation = Rotation;
        newChip.Angle = Rotation + (float)(3 * Math.PI / 2);
        newChip.ChipType = Singleton.Instance.CurrentChip;
        newChip.Reset();
        newChip.Speed = 900f;
        gameObjects.Add(newChip);
        LastShotChip = newChip;
    }

    private void DrawDottedLine(SpriteBatch spriteBatch, Vector2 start, float rotation, float length, Color color, float dotSize, float gapSize)
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
        while (totalDistance < length)
        {
            // Draw a dot
            spriteBatch.Draw(
                dotTexture,
                currentPos,
                null,
                color,
                0f,
                Vector2.Zero,
                new Vector2(dotSize, dotSize), // Scale the dot size
                SpriteEffects.None,
                0
            );

            currentPos += direction * (dotSize + gapSize);
            totalDistance += dotSize + gapSize;
        }
    }
}