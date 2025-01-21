using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Player : GameObject
{
    public Bubble Bubble;
    public Keys Left, Right, Fire;
    
    public Player(Texture2D texture) : base(texture)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 Origin = new Vector2(Rectangle.Width / 2, Rectangle.Height / 2);

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
        Position = new Vector2((Singleton.SCREENWIDTH - Rectangle.Width) / 2, 400);
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
        if( Singleton.Instance.CurrentKey.IsKeyDown(Fire) &&
            Singleton.Instance.PreviousKey != Singleton.Instance.CurrentKey)
        {
            var newBubble = Bubble.Clone() as Bubble;
            newBubble.Position = new Vector2(Rectangle.Width / 2 + Position.X - newBubble.Rectangle.Width / 2,
                                            Position.Y);
            newBubble.Reset();
            gameObjects.Add(newBubble);
        }

        Velocity = Vector2.Zero;

        base.Update(gameTime, gameObjects);
    }
}