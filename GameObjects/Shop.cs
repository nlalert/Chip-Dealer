using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Shop : GameObject
{   
    private List<ShopChip> _shopItems;
    // private int _rows = 3; // Number of rows
    private int _columns = 2; // Number of columns
    private int _itemSpacing = 10; // Spacing between items
    private int _itemSize = 50; 
    public SpriteFont font;

    public Shop(Texture2D texture) : base(texture)
    {
        _shopItems = new List<ShopChip>();
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        //Display Shop
        spriteBatch.Draw(_texture, Position,Viewport, Color.White);
        for (int i = 0; i < _shopItems.Count; i++)
        {
            int row = i / _columns; 
            int col = i % _columns; 
            Vector2 SpaceOffSet = new Vector2(0,10);
            Vector2 itemPosition = new Vector2(
                Position.X + col * (_itemSize + _itemSpacing),
                Position.Y + ViewportManager.Get("Pause_Button").Height + row * (_itemSize + _itemSpacing)
            ) + SpaceOffSet;

            _shopItems[i].Position = itemPosition;

            // Draw the item
            _shopItems[i].Draw(spriteBatch);
            //draw keys font
            Vector2 KeyOffSetPos = _shopItems[i].Position + new Vector2(Singleton.CHIP_SIZE,0);
            Vector2 fontSize = font.MeasureString(_shopItems[i].Price.ToString()+"$");
            Vector2 PriceOffSetPos = _shopItems[i].Position + new Vector2((Singleton.CHIP_SIZE- fontSize.X)/2  ,Singleton.CHIP_SIZE + 4) ;
            spriteBatch.DrawString(font,_shopItems[i].BuyKey.ToString(),KeyOffSetPos,Color.White);
            //draw prices font
            spriteBatch.DrawString(font,_shopItems[i].Price.ToString()+"$",PriceOffSetPos,Color.White);
            
        }
        base.Draw(spriteBatch);
    }

    public override void Reset()
    {
        base.Reset();
    }


    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        foreach (ShopChip item in _shopItems)
        {
            if (Singleton.Instance.CurrentKey.IsKeyDown(item.BuyKey) && Singleton.Instance.PreviousKey.IsKeyUp(item.BuyKey))
            {
                BuyItem(item,gameObjects);
            }
            if(item.IsClicked(Singleton.Instance.CurrentMouseState)
            &&Singleton.Instance.CurrentMouseState.LeftButton != Singleton.Instance.PreviousMouseState.LeftButton){
                BuyItem(item,gameObjects);
            }
        }
        base.Update(gameTime, gameObjects);
    }

    protected void BuyItem(ShopChip item,List<GameObject> gameObjects){
        if (Singleton.Instance.Score >= item.Price)
        {
            Singleton.Instance.Score -= item.Price;
            item.OnBuy(gameObjects);
            Console.WriteLine($"Item bought for {item.Price}!");
        }
        else
        {
            Console.WriteLine("Not enough currency to buy this item!");
        }
    }
    public void AddShopItem(ShopChip item){
        _shopItems.Add(item);
    }
}
