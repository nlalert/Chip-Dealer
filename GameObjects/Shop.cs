using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Shop : GameObject
{   
    private List<Item> _shopItems;
    // private int _rows = 3; // Number of rows
    private int _columns = 2; // Number of columns
    private int _itemSpacing = 10; // Spacing between items
    private int _itemSize = 50; // Assume each item's size (width/height)

    public Shop(Texture2D texture) : base(texture)
    {
        _shopItems = new List<Item>();
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        //Display Shop
        spriteBatch.Draw(_texture, Position, Color.White);
        for (int i = 0; i < _shopItems.Count; i++)
        {
            int row = i / _columns; // Determine the row
            int col = i % _columns; // Determine the column

            // Calculate item's position
            Vector2 itemPosition = new Vector2(
                Position.X + col * (_itemSize + _itemSpacing),
                Position.Y + _texture.Height + row * (_itemSize + _itemSpacing)
            );

            // Set the position for the item
            _shopItems[i].Position = itemPosition;

            // Draw the item
            _shopItems[i].Draw(spriteBatch);
        }

        base.Draw(spriteBatch);
    }

    public override void Reset()
    {
        base.Reset();
    }


    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        foreach (var item in _shopItems)
        {
            if (Singleton.Instance.CurrentKey.IsKeyDown(item.BuyKey) && Singleton.Instance.PreviousKey.IsKeyUp(item.BuyKey))
            {
                if (Singleton.Instance.Score >= item.Price)
                {
                    Singleton.Instance.Score -= item.Price;
                    // Singleton.Instance.CurrentChip = this.ChipType;
                    Console.WriteLine($"Item bought for {item.Price}!");
                    item.OnBuy(gameObjects);
                }
                else
                {
                    Console.WriteLine("Not enough currency to buy this item!");
                }
            }
        }
        base.Update(gameTime, gameObjects);
    }
    public void AddShopItem(Item item){
        _shopItems.Add(item);
    }
}
