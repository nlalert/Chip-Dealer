using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Bubble : GameObject
{   
    public float Angle;
    public float Speed;

    // public float DistantMoved;
    public Bubble(Texture2D texture) : base(texture)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Viewport, Color.White);
        base.Draw(spriteBatch);
    }

    public override void Reset()
    {
        // DistantMoved = 0;
        Angle = (float)(3.2*Math.PI/3);

        Speed = 300;
        base.Reset();
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        Velocity.X = (float) Math.Cos(Angle) * Speed;
        Velocity.Y = (float) Math.Sin(Angle) * Speed;

        Position += Velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);

        if (Position.Y < Singleton.PlayAreaEndY) 
            Position.Y = Singleton.PlayAreaEndY;

        if (Position.X < Singleton.PlayAreaStartX || Position.X > Singleton.PlayAreaEndX) 
            Angle = (float)Math.PI - Angle;

        foreach (GameObject s in gameObjects)
        {
            if (IsTouching(s))
            {
                // Handle collision logic here (if required)
            }
        }

        Velocity = Vector2.Zero;

    base.Update(gameTime, gameObjects);
    }
}
