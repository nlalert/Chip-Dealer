using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Bubble : GameObject
{   
    public float Angle;
    public float Speed;

    public float Radius;

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
        Speed = 1000;
        Radius = Singleton.BUBBLE_SIZE/2;
        base.Reset();
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        Velocity.X = (float) Math.Cos(Angle) * Speed;
        Velocity.Y = (float) Math.Sin(Angle) * Speed;

        Position += Velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);

        //Snap
        if (Position.Y < Singleton.PLAY_AREA_END_Y){
            Position.Y = Singleton.PLAY_AREA_END_Y;
            SnapToGrid();
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
            if (IsTouching(s) && IsTouchingAsCircle(s) && s.Name.Contains("Bubble"))
            {
                SnapToGrid();
            }
        }

        Velocity = Vector2.Zero;

        base.Update(gameTime, gameObjects);
    }

    protected void SnapToGrid()
    {
        Speed = 0;

        float closestDistance = float.MaxValue;
        Vector2 closestGridPosition = Vector2.Zero;
        
        for (int j = 0; j < Singleton.BUBBLE_GRID_HEIGHT; j++)
        {
            int Xoffset = (j % 2 == 0) ? 0 : (Singleton.BUBBLE_SIZE / 2);

            for (int i = 0; i < Singleton.BUBBLE_GRID_WIDTH; i++)
            {
                if (Xoffset != 0 && i == Singleton.BUBBLE_GRID_WIDTH - 1)
                    continue;

                float cellX = i * Singleton.BUBBLE_SIZE + Singleton.PLAY_AREA_START_X + Xoffset;
                float cellY = j * Singleton.BUBBLE_SIZE;// + Singleton.PLAY_AREA_START_Y;
  
                float distance = Vector2.Distance(new Vector2(cellX, cellY), Position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestGridPosition = new Vector2(cellX, cellY);
                }
            }
        }

        Position = closestGridPosition;
    }
    protected bool IsTouchingAsCircle(GameObject g)
    {
        
        if (g is Bubble otherBubble && this is Bubble)
        {
            float distance = Vector2.Distance(this.Position, otherBubble.Position);
            // Console.WriteLine($"distace : {distance}");
            return distance < this.Radius + otherBubble.Radius;
        }
        return false;
    }
}
