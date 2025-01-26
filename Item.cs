using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Item : GameObject{
    public int Price;
    public Keys BuyKey;
    public Item(Texture2D texture) : base(texture)
    {
        
    }
    public virtual void OnBuy(List<GameObject> gameObject){
        if (Singleton.Instance.Score >= Price)
        {
            Singleton.Instance.Score -= Price;
            // Singleton.Instance.CurrentChip = this.ChipType;
            Console.WriteLine($"Item bought for {Price}!");
        }
        else
        {
            Console.WriteLine("Not enough currency to buy this item!");
        }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Viewport, Color.White);
        base.Draw(spriteBatch);
    }
    private void ReplaceCurrentChip(List<GameObject> gameObjects){

    }
}