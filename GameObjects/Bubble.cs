using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Bubble : GameObject
{   
    public float Angle;
    public float Speed;

    private List<(Vector2, Vector2)> touchingPairs = new List<(Vector2, Vector2)>(); //for dev 
    // public float DistantMoved;
    public Bubble(Texture2D texture) : base(texture)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Viewport, Color.White);
        base.Draw(spriteBatch);
        Texture2D lineTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        lineTexture.SetData(new[] { Color.White });

        foreach (var pair in touchingPairs)
        {
            DrawLine(spriteBatch, lineTexture, pair.Item1, pair.Item2, Color.Red, 2);
        }
    }
    private void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end, Color color, float thickness)
    {
        //for dev purpose
        Vector2 direction = end - start;
        float length = direction.Length();
        float rotation = (float)Math.Atan2(direction.Y, direction.X);

        spriteBatch.Draw(texture, start, null, color, rotation, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);
      
    }

    public override void Reset()
    {
        Speed = 300;
        base.Reset();
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        Velocity.X = (float)Math.Cos(Angle) * Speed;
        Velocity.Y = (float)Math.Sin(Angle) * Speed;

        Position += Velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);

        if (Position.Y < Singleton.PlayAreaEndY)
        {
            Position.Y = Singleton.PlayAreaEndY;
            Speed = 0;
        }


        if (Position.X < Singleton.PLAY_AREA_START_X || Position.X > Singleton.PLAY_AREA_END_X - Rectangle.Width) 
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
                // touchingPairs.Add((this.Position + new Vector2(_texture.Width/2,_texture.Height/2 ), gameObjects[i].Position+ new Vector2(_texture.Width/2,_texture.Height/2)));
                Vector2 direction = gameObjects[i].Position - this.Position;
                float collisionAngle = (float)Math.Atan2(direction.Y, direction.X);
                float angleInDegrees = MathHelper.ToDegrees(collisionAngle);
                float snapNumber = MathF.Round(angleInDegrees / 60);
                float snappedDegrees = 60 * snapNumber;
                float snappedRadians = MathHelper.ToRadians(snappedDegrees);
                if(Speed !=0){
                    Console.WriteLine($"Touched at angle: {MathHelper.ToDegrees(collisionAngle):F2} degrees Snapped to : {snappedDegrees},{snapNumber}");
                    switch (snapNumber)
                    {
                        case 0:
                            this.Position = gameObjects[i].Position + new Vector2(-_texture.Width, 0);
                            break;
                        case 1:
                            this.Position = gameObjects[i].Position - new Vector2(
                                (float)(Math.Cos(MathHelper.ToRadians(60)) * _texture.Width), 
                                (float)(Math.Sin(MathHelper.ToRadians(60)) * _texture.Height) 
                            );
                            break;
                        case 2:
                            this.Position = gameObjects[i].Position - new Vector2(
                                (float)(Math.Cos(MathHelper.ToRadians(120)) * _texture.Width), 
                                (float)(Math.Sin(MathHelper.ToRadians(120)) * _texture.Height)  
                            );
                            break;
                        case -1:
                            this.Position = gameObjects[i].Position - new Vector2(
                                (float)(Math.Cos(MathHelper.ToRadians(-60)) * _texture.Width), 
                                (float)(Math.Sin(MathHelper.ToRadians(-60)) * _texture.Height)  
                            );
                            break;
                        case -2:
                            this.Position = gameObjects[i].Position - new Vector2(
                                (float)(Math.Cos(MathHelper.ToRadians(-120)) * _texture.Width), 
                                (float)(Math.Sin(MathHelper.ToRadians(-120)) * _texture.Height)  
                            );
                            break;
                        case 3:
                        case -3:
                            this.Position = gameObjects[i].Position + new Vector2(_texture.Width, 0); 
                            break;
                        default:
                            break;
                    }
                }
                //snap
                
                Speed = 0;
                break;
            }
        }
        base.Update(gameTime, gameObjects);

    }
}
