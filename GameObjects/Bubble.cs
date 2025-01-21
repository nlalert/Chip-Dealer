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
<<<<<<< HEAD
            Speed = 0;
        }

        if (Position.X < Singleton.PlayAreaStartX || Position.X > Singleton.PlayAreaEndX)
            Angle = (float)Math.PI - Angle;

        // foreach (GameObject s in gameObjects)
        // {
        //     if (IsTouching(s) && s is Bubble)
        //     {
                
        //         // Add the touching pair to the list for dev
        //         touchingPairs.Add((this.Position + new Vector2(_texture.Width/2,_texture.Height/2 ), s.Position+ new Vector2(_texture.Width/2,_texture.Height/2)));
        //         Vector2 direction = s.Position - this.Position;
        //         float collisionAngle = (float)Math.Atan2(direction.Y, direction.X);
        //         Console.WriteLine($"Touched at angle: {MathHelper.ToDegrees(collisionAngle):F2} degrees");

        //         Speed = 0;
        //         break;
        //     }
        // }
        for(int i = gameObjects.Count - 1; i >= 0; i--){
            if(IsTouching(gameObjects[i]) && gameObjects[i] is Bubble){
                //draw line for dev
                touchingPairs.Add((this.Position + new Vector2(_texture.Width/2,_texture.Height/2 ), gameObjects[i].Position+ new Vector2(_texture.Width/2,_texture.Height/2)));
                Vector2 direction = gameObjects[i].Position - this.Position;
                float collisionAngle = (float)Math.Atan2(direction.Y, direction.X);
                if(Speed !=0){
                    Console.WriteLine($"Touched at angle: {MathHelper.ToDegrees(collisionAngle):F2} degrees");
                }
                Speed = 0;
                break;
            }
        }
        
        
        base.Update(gameTime, gameObjects);
=======

        if (Position.X < Singleton.PLAY_AREA_START_X || Position.X > Singleton.PLAY_AREA_END_X - Rectangle.Width) 
            Angle = (float)Math.PI - Angle;

        foreach (GameObject s in gameObjects)
        {
            if (IsTouching(s))
            {
                // Handle collision logic here (if required)
            }
        }
>>>>>>> parent of d4ffaaa (Merge pull request #5 from nlalert/AngleDection)

        Velocity = Vector2.Zero;

    base.Update(gameTime, gameObjects);
    }
}
