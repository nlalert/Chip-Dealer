using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class ShopChip : GameObject
{
    public int Price;
    public Keys BuyKey;
    public ChipType ChipType;

    public ShopChip(Texture2D texture) : base(texture)
    {
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Viewport, Color.White);
        base.Draw(spriteBatch);
    }
    public void OnBuy(List<GameObject> gameObjects)
    {
        Singleton.Instance.CurrentChip = ChipType;
    }
    public bool IsClicked(MouseState mouseState)
    {
        Rectangle buttonBounds = new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height);
        return buttonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed;
    }
}