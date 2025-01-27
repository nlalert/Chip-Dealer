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
        Console.WriteLine(mouseState);
        return Viewport.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed;
    }
    
}
