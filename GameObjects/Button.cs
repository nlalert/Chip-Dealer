using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Button : GameObject
{
    public Rectangle HighlightViewPort;
    public Button(Texture2D texture) : base(texture)
    {

    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        if(IsMouseHovering()){
            spriteBatch.Draw(_texture,Position,HighlightViewPort,Color.White);
        }
        else spriteBatch.Draw(_texture,Position,Viewport,Color.White);
        base.Draw(spriteBatch);
    }

    public bool IsClicked()
    {
        //have to add obj pos to make it know where it have to check for click
        // if not will origin at 0,0
        Rectangle buttonBounds = new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height);
        return buttonBounds.Contains(Singleton.Instance.CurrentMouseState.Position) && Singleton.Instance.CurrentMouseState.LeftButton == ButtonState.Pressed;
    }
    
    public bool IsMouseHovering()
    {
        // Rectangle reg = new Rectangle((int)BoxPos.X,(int)BoxPos.Y,boundingbox.Width,boundingbox.Height);
        Rectangle buttonBounds = new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height);
        return buttonBounds.Contains(Singleton.Instance.CurrentMouseState.Position);
        // return boundingbox.Contains(Singleton.Instance.CurrentMouseState.Position);
        // return reg.Contains(Singleton.Instance.CurrentMouseState.Position);
    }
}
