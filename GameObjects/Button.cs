using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Button : GameObject
{
    public string Text { get; set; }
    public Button(Texture2D texture) : base(texture)
    {

    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        if(Singleton.Instance.CurrentGameState ==Singleton.GameState.Pause){
            spriteBatch.Draw(_texture,Position,Viewport,Color.White);
        }
        base.Draw(spriteBatch);
    }

    public bool IsClicked(MouseState mouseState)
    {
        //have to add obj pos to make it know where it have to check for click
        // if not will origin at 0,0
        Rectangle buttonBounds = new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height);
        return buttonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed;
    }
    
}
