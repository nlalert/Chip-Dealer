using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Bubble : GameObject
{   
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
        base.Reset();
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        Position += Velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);

        if (Position.Y < Singleton.PlayAreaEndY) Position.Y = Singleton.PlayAreaEndY;
        if (Position.X < Singleton.PlayAreaStartX || Position.X < Singleton.PlayAreaEndX) Velocity.X *=-1;

        foreach (GameObject s in gameObjects)
        {
            if (IsTouching(s))
            {
                // Handle collision logic here (if required)
            }
        }

    base.Update(gameTime, gameObjects);
    }
}
