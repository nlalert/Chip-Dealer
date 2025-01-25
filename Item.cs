using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Item : Chip{
    public int Price;
    public Keys BuyKey;
    private bool _wasMousePressed = false;
    public Item(Texture2D texture) : base(texture)
    {
        
    }
    public virtual void OnBuy(){
        if (Singleton.Instance.Score >= Price)
        {
            Singleton.Instance.Score -= Price;
            // Add the item to the player's inventory
            Console.WriteLine($"Item bought for {Price}!");
        }
        else
        {
            Console.WriteLine("Not enough currency to buy this item!");
        }
    }
    public override void Reset()
    {
        ResetChipTexture();
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Viewport, Color.White);
        base.Draw(spriteBatch);
    }
}