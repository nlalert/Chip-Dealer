using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Button : GameObject
{
    public Rectangle HighlightedViewPort;

    public Vector2 LabelPosition;
    public Rectangle LabelViewPort;
    public Rectangle HighlightedLabelViewPort;

    public bool Dragging;

    public Button(Texture2D texture) : base(texture)
    {
    }

    public void ButtonUpdate()
    {
        Rectangle buttonBounds = new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height);

        if(buttonBounds.Contains(Singleton.Instance.CurrentMouseState.Position) && Singleton.Instance.PreviousMouseState.LeftButton == ButtonState.Pressed 
        && Singleton.Instance.CurrentMouseState.LeftButton != ButtonState.Released && !Dragging){
            Dragging = true;
            Console.WriteLine("Dragging");
        }

        if(Singleton.Instance.PreviousMouseState.LeftButton == ButtonState.Released && Dragging)  
            Dragging = false;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
           
        spriteBatch.Draw(_texture, Position, Viewport, Color.White);
        spriteBatch.Draw(_texture, LabelPosition, LabelViewPort, Color.White);
        
        if(IsMouseHovering()){
            spriteBatch.Draw(_texture, Position, HighlightedViewPort, Color.White);
            spriteBatch.Draw(_texture, LabelPosition, HighlightedLabelViewPort, Color.White);
        }
        base.Draw(spriteBatch);
    }

    public bool IsClicked()
    {
        //have to add obj pos to make it know where it have to check for click
        // if not will origin at 0,0
        Rectangle buttonBounds = new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height);
        return buttonBounds.Contains(Singleton.Instance.CurrentMouseState.Position) && Singleton.Instance.PreviousMouseState.LeftButton == ButtonState.Pressed 
        && Singleton.Instance.CurrentMouseState.LeftButton == ButtonState.Released;
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
