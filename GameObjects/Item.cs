// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Audio;
// using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Input;

// public class Item : GameObject{
//     public int Price;
//     public Keys BuyKey;
//     public Item(Texture2D texture) : base(texture)
//     {
        
//     }
//     public virtual void OnBuy(List<GameObject> gameObject){

//     }
//     public override void Draw(SpriteBatch spriteBatch)
//     {
//         spriteBatch.Draw(_texture, Position, Viewport, Color.White);
//         base.Draw(spriteBatch);
//     }
//     public bool IsClicked(MouseState mouseState)
//     {
//         Rectangle buttonBounds = new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height);
//         return buttonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed;
//     }
// }