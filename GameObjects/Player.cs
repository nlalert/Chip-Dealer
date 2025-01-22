using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Player : GameObject
{
    public Bubble Bubble, ShootedBubble;
    public Keys Left, Right, Fire;
    
    public Player(Texture2D texture) : base(texture)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 Origin = new Vector2(Rectangle.Width / 2, Rectangle.Height / 2);
        
        //draw aim line
        float DotLinelength = 100f;
        float DotSize = 4f;
        float DotGap = 8f;
        DrawDottedLine(spriteBatch, Position + Origin, Rotation, DotLinelength, Color.White, DotSize, DotGap);

        // Draw the sprite with rotation around its center
        spriteBatch.Draw(
            _texture,
            Position + Origin, // Position adjusted to account for origin
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
        Position = new Vector2((Singleton.SCREEN_WIDTH - Rectangle.Width) / 2, 400);
        ShootedBubble = Bubble;
        base.Reset();
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        if(Singleton.Instance.CurrentKey.IsKeyDown(Left))
        {
            Rotation -= MathHelper.ToRadians(90f) * (float)gameTime.ElapsedGameTime.TotalSeconds; // Rotate counterclockwise
        }
        if(Singleton.Instance.CurrentKey.IsKeyDown(Right))
        {
            Rotation += MathHelper.ToRadians(90f) * (float)gameTime.ElapsedGameTime.TotalSeconds; // Rotate clockwise
        }

        Rotation = MathHelper.Clamp(Rotation, -Singleton.MAX_PLAYER_ROTATION, Singleton.MAX_PLAYER_ROTATION);

        if( Singleton.Instance.CurrentKey.IsKeyDown(Fire) &&
            ShootedBubble.Speed == 0 &&
            Singleton.Instance.PreviousKey != Singleton.Instance.CurrentKey)
        {
            var newBubble = Bubble.Clone() as Bubble; 
            newBubble.Position = new Vector2(Rectangle.Width / 2 + Position.X - newBubble.Rectangle.Width / 2,
                                            Position.Y);
            newBubble.Angle = Rotation + (float)(3 * Math.PI / 2);
            newBubble.Reset();
            gameObjects.Add(newBubble);
            
            ShootedBubble = newBubble;
        }

        Velocity = Vector2.Zero;

        base.Update(gameTime, gameObjects);
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